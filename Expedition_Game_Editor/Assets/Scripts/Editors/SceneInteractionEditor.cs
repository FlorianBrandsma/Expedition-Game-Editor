using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SceneInteractionEditor : MonoBehaviour, IEditor
{
    //private List<InteractionDataElement> interactionDataList;

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get { return PathController.loaded; } }
    public Route.Data Data { get; set; }

    public List<IDataElement> DataElements
    {
        get
        {
            var list = new List<IDataElement>();

            //Temporary solution
            var interactionData = (InteractionDataElement)Data.dataElement;
            var interactionDataList = SelectionElementManager.FindDataElements((InteractionDataElement)Data.dataElement).Cast<InteractionDataElement>().ToList();
            //

            interactionDataList.ForEach(x => list.Add(x));

            return list;
        }
    }

    public void InitializeEditor()
    {
        if (Loaded) return;

        Data = PathController.route.data;

        var interactionData = (InteractionDataElement)Data.dataElement;

        //This didn't have the correct interaction data. Doesn't update or re-initialize editor when changing interactions in the action bar
        //The editor should be re-initialized when the selected interaction is changes

        //interactionDataList = SelectionElementManager.FindDataElements(interactionData).Cast<InteractionDataElement>().ToList();
        //interactionDataList.ForEach(x => Debug.Log(x.SelectionElement));

        DataElements.ForEach(x => x.ClearChanges());
    }

    public void UpdateEditor()
    {
        DataElements.ForEach(x => x.SelectionElement.UpdateElement());

        SetEditor();
    }

    public void UpdateIndex(int index) { }

    public void OpenEditor()
    {
        SetEditor();
    }

    public void SetEditor()
    {
        PathController.editorSection.SetActionButtons();
    }

    public bool Changed()
    {
        return DataElements.Any(x => x.Changed);
    }

    public void ApplyChanges()
    {
        DataElements.ForEach(x => x.Update());
        
        UpdateEditor();
    }

    public void CancelEdit()
    {
        
    }

    public void CloseEditor()
    {

    }
}
