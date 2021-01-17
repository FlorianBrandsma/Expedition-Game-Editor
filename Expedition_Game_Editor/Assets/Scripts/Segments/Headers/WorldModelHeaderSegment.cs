using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WorldModelHeaderSegment : MonoBehaviour, ISegment
{
    public ModelDataController modelDataController;

    public EditorElement iconEditorElement;
    public Text headerText;
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
                case Enums.DataType.WorldObject:
                    return ((WorldObjectEditor)DataEditor).Id;

                case Enums.DataType.SceneProp:
                    return ((ScenePropEditor)DataEditor).Id;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
    }

    private string Title
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.WorldObject:
                    return ((WorldObjectEditor)DataEditor).ModelName;

                case Enums.DataType.SceneProp:
                    return ((ScenePropEditor)DataEditor).ModelName;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return ""; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.WorldObject:

                    var worldObjectEditor = (WorldObjectEditor)DataEditor;
                    worldObjectEditor.ModelName = value;

                    break;

                case Enums.DataType.SceneProp:

                    var scenePropEditor = (ScenePropEditor)DataEditor;
                    scenePropEditor.ModelName = value;

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
                case Enums.DataType.WorldObject:
                    return ((WorldObjectEditor)DataEditor).ModelId;

                case Enums.DataType.SceneProp:
                    return ((ScenePropEditor)DataEditor).ModelId;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.WorldObject:

                    var worldObjectEditor = (WorldObjectEditor)DataEditor;
                    worldObjectEditor.ModelId = value;

                    break;

                case Enums.DataType.SceneProp:

                    var scenePropEditor = (ScenePropEditor)DataEditor;
                    scenePropEditor.ModelId = value;

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
            }
        }
    }

    private string ModelPath
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.WorldObject:
                    return ((WorldObjectEditor)DataEditor).ModelPath;

                case Enums.DataType.SceneProp:
                    return ((ScenePropEditor)DataEditor).ModelPath;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return ""; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.WorldObject:

                    var worldObjectEditor = (WorldObjectEditor)DataEditor;
                    worldObjectEditor.ModelPath = value;

                    break;

                case Enums.DataType.SceneProp:

                    var scenePropEditor = (ScenePropEditor)DataEditor;
                    scenePropEditor.ModelPath = value;

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
                case Enums.DataType.WorldObject:
                    return ((WorldObjectEditor)DataEditor).ModelIconPath;

                case Enums.DataType.SceneProp:
                    return ((ScenePropEditor)DataEditor).ModelIconPath;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return ""; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.WorldObject:

                    var worldObjectEditor = (WorldObjectEditor)DataEditor;
                    worldObjectEditor.ModelIconPath = value;

                    break;

                case Enums.DataType.SceneProp:

                    var scenePropEditor = (ScenePropEditor)DataEditor;
                    scenePropEditor.ModelIconPath = value;

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
                case Enums.DataType.WorldObject:
                    return ((WorldObjectEditor)DataEditor).Height;

                case Enums.DataType.SceneProp:
                    return ((ScenePropEditor)DataEditor).Height;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.WorldObject:

                    var worldObjectEditor = (WorldObjectEditor)DataEditor;
                    worldObjectEditor.Height = value;

                    break;

                case Enums.DataType.SceneProp:

                    var scenePropEditor = (ScenePropEditor)DataEditor;
                    scenePropEditor.Height = value;

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
                case Enums.DataType.WorldObject:
                    return ((WorldObjectEditor)DataEditor).Width;

                case Enums.DataType.SceneProp:
                    return ((ScenePropEditor)DataEditor).Width;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.WorldObject:

                    var worldObjectEditor = (WorldObjectEditor)DataEditor;
                    worldObjectEditor.Width = value;

                    break;

                case Enums.DataType.SceneProp:

                    var scenePropEditor = (ScenePropEditor)DataEditor;
                    scenePropEditor.Width = value;

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
                case Enums.DataType.WorldObject:
                    return ((WorldObjectEditor)DataEditor).Depth;

                case Enums.DataType.SceneProp:
                    return ((ScenePropEditor)DataEditor).Depth;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.WorldObject:

                    var worldObjectEditor = (WorldObjectEditor)DataEditor;
                    worldObjectEditor.Depth = value;

                    break;

                case Enums.DataType.SceneProp:

                    var scenePropEditor = (ScenePropEditor)DataEditor;
                    scenePropEditor.Depth = value;

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
            }
        }
    }
    #endregion

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData()
    {
        InitializeDependencies();
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
        headerText.text = Title;
        idText.text = Id > 0 ? Id.ToString() : "New";
        
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
        modelElementData.DataElement.Id = modelElementData.Id;

        Title = modelElementData.Name;

        ModelId = modelElementData.Id;
        ModelPath = modelElementData.Path;
        ModelIconPath = modelElementData.IconPath;

        Height = modelElementData.Height;
        Width = modelElementData.Width;
        Depth = modelElementData.Depth;

        SetModelData();

        DataEditor.UpdateEditor();
    }

    public void UpdateSegment() { }

    public void CloseSegment()
    {
        SelectionElementManager.Remove(iconEditorElement);

        gameObject.SetActive(false);
    }
}
