using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AssetHeaderSegment : MonoBehaviour, ISegment
{
    public ModelDataController modelDataController;
    
    public ExIndexSwitch indexSwitch;
    public EditorElement iconEditorElement;
    public InputField inputField;
    public Text idText;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private ModelElementData ModelElementData   { get { return (ModelElementData)iconEditorElement.DataElement.ElementData; } }

    #region Data properties
    private int Id
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:
                    return ((ItemEditor)DataEditor).Id;

                case Enums.DataType.Interactable:
                    return ((InteractableEditor)DataEditor).Id;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
    }

    private int Index
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:
                    return ((ItemEditor)DataEditor).Index;

                case Enums.DataType.Interactable:
                    return ((InteractableEditor)DataEditor).Index;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
    }

    private string Name
    {
        get
        {
            switch(DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:
                    return ((ItemEditor)DataEditor).Name;

                case Enums.DataType.Interactable:
                    return ((InteractableEditor)DataEditor).Name;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return ""; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:

                    var itemEditor = (ItemEditor)DataEditor;
                    itemEditor.Name = value;

                    break;

                case Enums.DataType.Interactable:

                    var interactableEditor = (InteractableEditor)DataEditor;
                    interactableEditor.Name = value;

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
            }
        }
    }

    private int ModelId
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:
                    return ((ItemEditor)DataEditor).ModelId;

                case Enums.DataType.Interactable:
                    return ((InteractableEditor)DataEditor).ModelId;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:

                    var itemEditor = (ItemEditor)DataEditor;
                    itemEditor.ModelId = value;

                    break;

                case Enums.DataType.Interactable:

                    var interactableEditor = (InteractableEditor)DataEditor;
                    interactableEditor.ModelId = value;

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
            }
        }
    }

    private string ModelPath
    {
        get
        {
            switch(DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:
                    return ((ItemEditor)DataEditor).ModelPath;

                case Enums.DataType.Interactable:
                    return ((InteractableEditor)DataEditor).ModelPath;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return ""; }
            }
        }
        set
        {
            switch(DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:

                    var itemEditor = (ItemEditor)DataEditor;
                    itemEditor.ModelPath = value;

                    break;

                case Enums.DataType.Interactable:

                    var interactableEditor = (InteractableEditor)DataEditor;
                    interactableEditor.ModelPath = value;

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
            }
        }
    }

    private string ModelIconPath
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:
                    return ((ItemEditor)DataEditor).ModelIconPath;

                case Enums.DataType.Interactable:
                    return ((InteractableEditor)DataEditor).ModelIconPath;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return ""; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:

                    var itemEditor = (ItemEditor)DataEditor;
                    itemEditor.ModelIconPath = value;

                    break;

                case Enums.DataType.Interactable:

                    var interactableEditor = (InteractableEditor)DataEditor;
                    interactableEditor.ModelIconPath = value;

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
            }
        }
    }

    private float Height
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:
                    return ((ItemEditor)DataEditor).Height;

                case Enums.DataType.Interactable:
                    return ((InteractableEditor)DataEditor).Height;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:

                    var itemEditor = (ItemEditor)DataEditor;
                    itemEditor.Height = value;

                    break;

                case Enums.DataType.Interactable:

                    var interactableEditor = (InteractableEditor)DataEditor;
                    interactableEditor.Height = value;

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
            }
        }
    }

    private float Width
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:
                    return ((ItemEditor)DataEditor).Width;

                case Enums.DataType.Interactable:
                    return ((InteractableEditor)DataEditor).Width;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:

                    var itemEditor = (ItemEditor)DataEditor;
                    itemEditor.Width = value;

                    break;

                case Enums.DataType.Interactable:

                    var interactableEditor = (InteractableEditor)DataEditor;
                    interactableEditor.Width = value;

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
            }
        }
    }

    private float Depth
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:
                    return ((ItemEditor)DataEditor).Depth;

                case Enums.DataType.Interactable:
                    return ((InteractableEditor)DataEditor).Depth;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:

                    var itemEditor = (ItemEditor)DataEditor;
                    itemEditor.Depth = value;

                    break;

                case Enums.DataType.Interactable:

                    var interactableEditor = (InteractableEditor)DataEditor;
                    interactableEditor.Depth = value;

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
            }
        }
    }
    #endregion
    
    public void InitializeDependencies()
    {
        modelDataController.InitializeController();

        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData()
    {
        InitializeDependencies();

        if (DataEditor.Loaded) return;

        switch (DataEditor.Data.dataController.DataType)
        {
            case Enums.DataType.Item:           InitializeItemProperties();         break;
            case Enums.DataType.Interactable:   InitializeInteractableProperties(); break;
        }

        if (indexSwitch != null)
        {
            var enabled = Id > 0;

            indexSwitch.EnableElement(enabled);
            indexSwitch.InitializeSwitch(this, Index);
        }  
    }

    private void InitializeItemProperties()
    {
        GetComponent<ObjectProperties>().castShadow = false;
    }

    private void InitializeInteractableProperties()
    {
        GetComponent<ObjectProperties>().castShadow = true;
    }

    public void InitializeSegment()
    {
        var modelElementData = new ModelElementData()
        {
            Id = ModelId,
            DataElement = iconEditorElement.DataElement
        };

        modelElementData.SetOriginalValues();

        var modelData = new Data();

        modelData.dataController = modelDataController;
        modelData.dataList = new List<IElementData>() { modelElementData };
        modelData.searchProperties = modelDataController.SearchProperties;
        
        iconEditorElement.DataElement.Data = modelData;
        iconEditorElement.DataElement.Id = ModelId;
        
        SetModelData();

        iconEditorElement.DataElement.InitializeElement();
    }
    
    private void SetModelData()
    {
        modelDataController.Data = iconEditorElement.DataElement.Data;

        ModelElementData.Id = ModelId;
        ModelElementData.Path = ModelPath;
        ModelElementData.IconPath = ModelIconPath;
    }

    public void OpenSegment()
    {
        if (indexSwitch != null)
            indexSwitch.Activate();

        if (Id > 0)
            idText.text = Id.ToString();
        else
            idText.text = "New";

        inputField.text = Name;

        SelectionElementManager.Add(iconEditorElement);
        SelectionManager.SelectData(iconEditorElement.DataElement.Data.dataList);

        iconEditorElement.DataElement.SetElement();
        iconEditorElement.SetOverlay();

        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData)
    {
        switch (mergedElementData.DataType)
        {
            case Enums.DataType.Model:

                var modelElementData = (ModelElementData)mergedElementData;
                UpdateModel(modelElementData);

                break;

            default: Debug.Log("CASE MISSING: " + mergedElementData.DataType); break;
        }
    }

    public void UpdateModel(ModelElementData modelElementData)
    {
        ModelId = modelElementData.Id;
        ModelPath = modelElementData.Path;
        ModelIconPath = modelElementData.IconPath;

        Height = modelElementData.Height;
        Width = modelElementData.Width;
        Depth = modelElementData.Depth;

        SetModelData();

        DataEditor.UpdateEditor();
    }

    public void UpdateName()
    {
        Name = inputField.text;

        DataEditor.UpdateEditor();
    }

    public void UpdateSegment() { }

    public void CloseSegment()
    {
        if (indexSwitch != null)
            indexSwitch.Deactivate();

        SelectionElementManager.Remove(iconEditorElement);

        gameObject.SetActive(false);
    }
}
