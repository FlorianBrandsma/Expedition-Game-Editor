using UnityEngine;

public class LayoutSection : MonoBehaviour
{
    public bool Active                                  { get; set; }
    
    public EditorForm EditorForm                        { get; set; }

    public EditorController TargetController            { get; set; }
    public EditorController PreviousTargetController    { get; set; }
    public EditorController DisplayTargetController     { get; set; }

    public IElementData PreviousTargetControllerData    { get; set; }
    
    public LayoutDependency TargetView                  { get; set; }

    private Enums.ExecuteType originalExecuteType;

    public bool Loaded
    {
        get
        {
            if (!EditorForm.loaded)
                return false;

            if (RenderManager.loadType == Enums.LoadType.Reload || RenderManager.loadType == Enums.LoadType.Return)
                return false;

            //Empty editor controllers are added to enforce loading (e.g. items, interactables)
            if (TargetController != PreviousTargetController)
                return false;

            if (PreviousTargetControllerData == null && TargetController.PathController.route.data != null)
                return false;

            if (PreviousTargetControllerData != null && !DataManager.Equals(TargetController.PathController.route.ElementData, PreviousTargetControllerData))
                return false;

            return true;
        }
    }

    public IEditor dataEditor;

    //Previous data editor
    public IEditor previousEditor;
    public IElementData previousDataSource;
    //
    
    public ButtonActionManager buttonActionManager;

    public void InitializeSection(EditorForm editorForm)
    {
        EditorForm = editorForm;

        if (buttonActionManager != null)
            buttonActionManager.InitializeButtons(this);
    }

    public void InitializeLayout()
    {
        if (TargetView == null) return;

        TargetView.InitializeDependency();       
    }

    public void SetLayout()
    {
        if (TargetView == null) return;

        //Adjust size of dependency content based on active headers and footers
        TargetView.SetDependency(); 
    }

    public void Activate()
    {
        Active = true;
    }

    public void OpenSegments()
    {
        if (TargetController == null) return;

        DisplayTargetController = TargetController;

        DisplayTargetController.OpenSegments();
    }

    public void SetActionButtons()
    {
        if (dataEditor == null) return;

        if (buttonActionManager != null && dataEditor != null)
        {
            buttonActionManager.SetButtons(dataEditor.EditData.ExecuteType, dataEditor.Changed());
        }
    }

    public void ResetEditor()
    {
        if (DisplayTargetController == null) return;

        DisplayTargetController.ResetEditor();

        SetActionButtons();
    }

    public void ClosePath()
    {
        if (TargetController == null) return;

        if (buttonActionManager != null)
            buttonActionManager.CloseButtons();

        PreviousTargetController = TargetController;
        PreviousTargetControllerData = TargetController.PathController.route.ElementData;
        
        TargetController = null;

        Active = false;
    }

    public void CloseLayoutDependencies()
    {
        if (TargetView == null) return;
        
        TargetView.CloseDependency();
        
        TargetView = null;
    }

    public void CloseEditorSegments()
    {
        if (DisplayTargetController == null) return;

        PreviousTargetController = DisplayTargetController;
        PreviousTargetControllerData = DisplayTargetController.PathController.route.ElementData;

        DisplayTargetController.CloseSegments();
    }

    public void ApplyChanges()
    {
        if (dataEditor == null) return;

        var dataRequest = new DataRequest();

        dataEditor.ApplyChanges(dataRequest);

        if (dataRequest.errorList.Count > 0)
        {
            dataRequest.errorList.ForEach(x => Debug.Log(x));
            return;
        }

        dataRequest.requestType = Enums.RequestType.Execute;

        //Apply changes when there are no combined errors 
        dataEditor.ApplyChanges(dataRequest);

        dataEditor.ElementDataList.ForEach(x =>
        {
            if (SelectionElementManager.SelectionActive(x.DataElement))
            {
                var editorElement = (EditorElement)x.DataElement.SelectionElement;

                x.DataElement.Id = x.Id;

                if (editorElement.child != null)
                    editorElement.child.DataElement.Id = x.Id;

                x.DataElement.UpdateElement();
            }
        });
        
        dataEditor.FinalizeChanges();

        Debug.Log(Fixtures.sceneShotList.Count);
    }

    private void ResetExecutionType()
    {
        dataEditor.ElementDataList.ForEach(x => x.ExecuteType = Enums.ExecuteType.Update);
    }

    public void CancelEdit()
    {
        if (dataEditor.Loaded)
            dataEditor.CancelEdit();

        if (!Active) dataEditor = null;
    }

    public void ToggleRemoval(bool toggled)
    {
        if (toggled)
        {
            originalExecuteType = dataEditor.EditData.ExecuteType;

            dataEditor.EditData.ExecuteType = Enums.ExecuteType.Remove;

            //dataEditor.ElementDataList.ForEach(x => x.ExecuteType = Enums.ExecuteType.Remove);

        } else {

            dataEditor.EditData.ExecuteType = originalExecuteType;

            //dataEditor.ElementDataList.ForEach(x => x.ExecuteType = originalExecuteType);
        }

        SetActionButtons();
    }

    public void CloseEditor()
    {
        RenderManager.PreviousPath();
    }
}
