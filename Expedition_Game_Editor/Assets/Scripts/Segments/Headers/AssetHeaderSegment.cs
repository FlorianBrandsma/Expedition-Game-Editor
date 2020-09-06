using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class AssetHeaderSegment : MonoBehaviour, ISegment
{
    public ModelDataController modelController;
    private ModelElementData ModelElementData { get { return (ModelElementData)iconEditorElement.DataElement.ElementData; } }

    public ExIndexSwitch indexSwitch;
    public EditorElement iconEditorElement;
    public InputField inputField;
    public Text idText;

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    public int Id
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:
                    return ((ItemEditor)DataEditor).Id;

                default: return -1;
            }
        }
    }

    public int Index
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:
                    return ((ItemEditor)DataEditor).Index;

                default: return -1;
            }
        }
    }

    public string Name
    {
        get
        {
            switch(DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:
                    return ((ItemEditor)DataEditor).Name;

                default: return "Error";
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

                    var interactableData = (InteractableElementData)DataEditor.ElementData;

                    interactableData.Name = value;

                    break;
            }
        }
    }

    public int ModelId
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:
                    return ((ItemEditor)DataEditor).ModelId;

                default: return -1;
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
            }
        }
    }

    public string ModelPath
    {
        get
        {
            switch(DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:
                    return ((ItemEditor)DataEditor).ModelPath;

                default: return "Error";
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
            }
        }
    }

    public string ModelIconPath
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:
                    return ((ItemEditor)DataEditor).ModelIconPath;

                default: return "Error";
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
            }
        }
    }

    public float ModelHeight
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:
                    return ((ItemEditor)DataEditor).ModelHeight;

                default: return -1;
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:

                    var itemEditor = (ItemEditor)DataEditor;

                    itemEditor.ModelHeight = value;

                    break;
            }
        }
    }

    public float ModelWidth
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:
                    return ((ItemEditor)DataEditor).ModelWidth;

                default: return -1;
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:

                    var itemEditor = (ItemEditor)DataEditor;

                    itemEditor.ModelWidth = value;

                    break;
            }
        }
    }

    public float ModelDepth
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:
                    return ((ItemEditor)DataEditor).ModelDepth;

                default: return 404;
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:

                    var itemEditor = (ItemEditor)DataEditor;

                    itemEditor.ModelDepth = value;

                    break;
            }
        }
    }

    public void UpdateName()
    {
        Name = inputField.text;

        DataEditor.UpdateEditor();
    }

    public void UpdateModel(ModelElementData modelElementData)
    {
        ModelId = modelElementData.Id;
        ModelPath = modelElementData.Path;
        ModelIconPath = modelElementData.IconPath;

        ModelHeight = modelElementData.Height;
        ModelWidth = modelElementData.Width;
        ModelDepth = modelElementData.Depth;

        SetModelData();
        
        DataEditor.UpdateEditor();
    }

    public void InitializeDependencies()
    {
        modelController.InitializeController();

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
            case Enums.DataType.Item:           InitializeItemData();           break;
            case Enums.DataType.Interactable:   InitializeInteractableData();   break;
        }

        if (indexSwitch != null)
            indexSwitch.InitializeSwitch(this, Index);
    }

    private void InitializeItemData()
    {
        GetComponent<ObjectProperties>().castShadow = false;
    }

    private void InitializeInteractableData()
    {
        GetComponent<ObjectProperties>().castShadow = true;
    }

    public void InitializeSegment()
    {
        var modelData = new Data();

        modelData.dataController = modelController;
        modelData.dataList = new List<IElementData>() { new ModelElementData() { Id = ModelId, DataElement = iconEditorElement.DataElement } };
        modelData.searchProperties = modelController.SearchProperties;

        iconEditorElement.DataElement.Data = modelData;
        iconEditorElement.DataElement.Id = ModelId;
        
        SetModelData();

        iconEditorElement.DataElement.InitializeElement();
    }
    
    private void SetModelData()
    {
        modelController.Data = iconEditorElement.DataElement.Data;

        ModelElementData.Id = ModelId;
        ModelElementData.Path = ModelPath;
        ModelElementData.IconPath = ModelIconPath;
    }

    public void OpenSegment()
    {
        if (indexSwitch != null)
            indexSwitch.Activate();

        idText.text = Id.ToString();

        inputField.text = Name;

        SelectionElementManager.Add(iconEditorElement);

        SelectionManager.SelectData(iconEditorElement.DataElement.Data.dataList);

        iconEditorElement.DataElement.SetElement();
        iconEditorElement.SetOverlay();

        gameObject.SetActive(true);
    }

    public void CloseSegment()
    {
        if (indexSwitch != null)
            indexSwitch.Deactivate();

        SelectionElementManager.elementPool.Remove(iconEditorElement);

        gameObject.SetActive(false);
    }

    public void SetSearchResult(DataElement dataElement)
    {
        switch(dataElement.Data.dataController.DataType)
        {
            case Enums.DataType.Model:

                var modelElementData = (ModelElementData)dataElement.ElementData;

                UpdateModel(modelElementData);
                
                break;

            default: Debug.Log("CASE MISSING"); break;
        }
    }
}
