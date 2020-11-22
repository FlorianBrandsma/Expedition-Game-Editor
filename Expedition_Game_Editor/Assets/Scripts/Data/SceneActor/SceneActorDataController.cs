using UnityEngine;

public class SceneActorDataController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Data Data                            { get; set; }
    public Enums.DataType DataType              { get { return Enums.DataType.SceneActor; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }

    public SearchProperties SearchProperties
    {
        get { return searchProperties; }
        set { searchProperties = value; }
    }

    public void InitializeController()
    {
        SearchProperties.Initialize();
    }

    public void GetData()
    {
        GetData(searchProperties);
    }

    public void GetData(SearchProperties searchProperties)
    {
        Data = new Data()
        {
            dataController = this,
            dataList = SceneActorDataManager.GetData(searchProperties),
            searchProperties = this.searchProperties
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(IElementData searchElementData, IElementData resultElementData)
    {
        var searchSceneActorElementData = (SceneActorElementData)searchElementData;

        switch (resultElementData.DataType)
        {
            case Enums.DataType.WorldInteractable:

                var resultWorldInteractableElementData = (WorldInteractableElementData)resultElementData;

                searchSceneActorElementData.WorldInteractableId = resultWorldInteractableElementData.Id;
                searchSceneActorElementData.ModelId             = resultWorldInteractableElementData.ModelId;

                searchSceneActorElementData.ModelPath           = resultWorldInteractableElementData.ModelPath;
                searchSceneActorElementData.ModelIconPath       = resultWorldInteractableElementData.ModelIconPath;

                searchSceneActorElementData.InteractableName    = resultWorldInteractableElementData.InteractableName;

                searchSceneActorElementData.Height              = resultWorldInteractableElementData.Height;
                searchSceneActorElementData.Width               = resultWorldInteractableElementData.Width;
                searchSceneActorElementData.Depth               = resultWorldInteractableElementData.Depth;

                searchSceneActorElementData.Scale               = resultWorldInteractableElementData.Scale;

                break;

            case Enums.DataType.SceneActor:

                searchSceneActorElementData.DataElement.Id = resultElementData.Id;

                var resultSceneActorElementData = (SceneActorElementData)resultElementData;

                searchSceneActorElementData.Id = resultSceneActorElementData.Id;

                searchSceneActorElementData.ModelIconPath = resultSceneActorElementData.ModelIconPath;
                searchSceneActorElementData.InteractableName = resultSceneActorElementData.InteractableName;

                break;

            default: Debug.Log("CASE MISSING: " + resultElementData.DataType); break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}