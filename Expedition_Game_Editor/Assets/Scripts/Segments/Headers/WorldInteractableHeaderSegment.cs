using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class WorldInteractableHeaderSegment : MonoBehaviour, ISegment
{
    public WorldInteractableDataController worldInteractableDataController;

    public EditorElement iconEditorElement;
    public Text headerText;
    public Text idText;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private WorldInteractableElementData WorldInteractableElementData { get { return (WorldInteractableElementData)iconEditorElement.DataElement.ElementData; } }

    #region Data properties
    private int Id
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.SceneActor:
                    return ((SceneActorEditor)DataEditor).Id;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
    }

    private int WorldInteractableId
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.SceneActor:
                    return ((SceneActorEditor)DataEditor).WorldInteractableId;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.SceneActor:

                    var sceneActorEditor = (SceneActorEditor)DataEditor;
                    sceneActorEditor.WorldInteractableId = value;

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
                case Enums.DataType.SceneActor:
                    return ((SceneActorEditor)DataEditor).ModelId;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.SceneActor:

                    var sceneActorEditor = (SceneActorEditor)DataEditor;
                    sceneActorEditor.ModelId = value;

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
                case Enums.DataType.SceneActor:
                    return ((SceneActorEditor)DataEditor).ModelPath;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return ""; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.SceneActor:

                    var sceneActorEditor = (SceneActorEditor)DataEditor;
                    sceneActorEditor.ModelPath = value;

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
                case Enums.DataType.SceneActor:
                    return ((SceneActorEditor)DataEditor).ModelIconPath;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return ""; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.SceneActor:

                    var sceneActorEditor = (SceneActorEditor)DataEditor;
                    sceneActorEditor.ModelIconPath = value;

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
            }
        }
    }

    private string Title
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.SceneActor:
                    return ((SceneActorEditor)DataEditor).InteractableName;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return ""; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.SceneActor:

                    var sceneActorEditor = (SceneActorEditor)DataEditor;
                    sceneActorEditor.InteractableName = value;

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
                case Enums.DataType.SceneActor:
                    return ((SceneActorEditor)DataEditor).Height;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.SceneActor:

                    var scenePropEditor = (SceneActorEditor)DataEditor;
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
                case Enums.DataType.SceneActor:
                    return ((SceneActorEditor)DataEditor).Width;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.SceneActor:

                    var scenePropEditor = (SceneActorEditor)DataEditor;
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
                case Enums.DataType.SceneActor:
                    return ((SceneActorEditor)DataEditor).Depth;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.SceneActor:

                    var scenePropEditor = (SceneActorEditor)DataEditor;
                    scenePropEditor.Depth = value;

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
            }
        }
    }

    private float Scale
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.SceneActor:
                    return ((SceneActorEditor)DataEditor).Scale;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.SceneActor:

                    var scenePropEditor = (SceneActorEditor)DataEditor;
                    scenePropEditor.Scale = value;

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
        var worldInteractableElementData = new WorldInteractableElementData()
        {
            Id = WorldInteractableId,
            DataElement = iconEditorElement.DataElement,

            UniqueSelection = iconEditorElement.uniqueSelection
        };

        worldInteractableElementData.SetOriginalValues();

        var worldInteractableData = new Data();

        worldInteractableData.dataController = worldInteractableDataController;
        worldInteractableData.dataList = new List<IElementData>() { worldInteractableElementData };
        worldInteractableData.searchProperties = worldInteractableDataController.SearchProperties;

        iconEditorElement.DataElement.Data = worldInteractableData;
        iconEditorElement.DataElement.Id = WorldInteractableId;

        SetWorldInteractableData();

        iconEditorElement.DataElement.InitializeElement();
    }

    private void SetWorldInteractableData()
    {
        worldInteractableDataController.Data = iconEditorElement.DataElement.Data;

        WorldInteractableElementData.Id = WorldInteractableId;
        WorldInteractableElementData.ModelId = ModelId;
        WorldInteractableElementData.ModelPath = ModelPath;
        WorldInteractableElementData.ModelIconPath = ModelIconPath;

        SetSearchParameters();
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

    private void SetSearchParameters()
    {
        var path = RenderManager.layoutManager.forms.First().activePath;

        var searchParameters = worldInteractableDataController.SearchProperties.searchParameters.Cast<Search.WorldInteractable>().First();
        searchParameters.requestType = Search.WorldInteractable.RequestType.GetSceneActorWorldInteractables;

        //Region world interactables (all phase regions)
        var regionRoute = path.FindLastRoute(Enums.DataType.Region);

        if (regionRoute != null)
            searchParameters.regionId = new List<int>() { regionRoute.id };

        //Controllable world interactables
        var chapterRoute = path.FindLastRoute(Enums.DataType.Chapter);

        if (chapterRoute != null)
            searchParameters.chapterId = new List<int>() { chapterRoute.id };

        //Phase interactables belonging to the active quest
        var questRoute = path.FindLastRoute(Enums.DataType.Quest);

        if (questRoute != null)
            searchParameters.questId = new List<int>() { questRoute.id };

        //Temporary objective interactables
        var objectiveRoute = path.FindLastRoute(Enums.DataType.Objective);

        if (objectiveRoute != null)
            searchParameters.objectiveId = new List<int>() { objectiveRoute.id };

        searchParameters.excludeId = DataEditor.Data.dataController.Data.dataList.Cast<SceneActorElementData>().Select(x => x.WorldInteractableId).ToList();
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData)
    {
        switch (mergedElementData.DataType)
        {
            case Enums.DataType.WorldInteractable:

                var worldInteractableElementData = (WorldInteractableElementData)mergedElementData;
                UpdateWorldInteractable(worldInteractableElementData);

                break;
                
            default: Debug.Log("CASE MISSING: " + mergedElementData.DataType); break;
        }
    }

    public void UpdateWorldInteractable(WorldInteractableElementData worldInteractableElementData)
    {
        worldInteractableElementData.DataElement.Id = worldInteractableElementData.Id;

        Title = worldInteractableElementData.InteractableName;

        WorldInteractableId = worldInteractableElementData.Id;
        ModelId = worldInteractableElementData.ModelId;
        ModelPath = worldInteractableElementData.ModelPath;
        ModelIconPath = worldInteractableElementData.ModelIconPath;

        Height = worldInteractableElementData.Height;
        Width = worldInteractableElementData.Width;
        Depth = worldInteractableElementData.Depth;

        Scale = worldInteractableElementData.Scale;

        SetWorldInteractableData();

        DataEditor.UpdateEditor();
    }

    public void UpdateSegment() { }

    public void CloseSegment()
    {
        SelectionElementManager.Remove(iconEditorElement);

        gameObject.SetActive(false);
    }
}