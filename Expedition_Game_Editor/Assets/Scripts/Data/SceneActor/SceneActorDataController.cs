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

                searchSceneActorElementData.InteractableName = resultWorldInteractableElementData.InteractableName;
                searchSceneActorElementData.ModelIconPath = resultWorldInteractableElementData.ModelIconPath;

                break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}