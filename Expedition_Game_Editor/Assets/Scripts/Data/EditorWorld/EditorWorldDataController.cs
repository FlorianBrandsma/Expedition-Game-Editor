using UnityEngine;

public class EditorWorldDataController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public WorldObjectDataController WorldObjectDataController                          { get { return GetComponent<WorldObjectDataController>(); } }
    public WorldInteractableDataController WorldInteractableAgentDataController         { get { return GetComponents<WorldInteractableDataController>()[0]; } }
    public WorldInteractableDataController WorldInteractableObjectDataController        { get { return GetComponents<WorldInteractableDataController>()[1]; } }
    public InteractionDestinationDataController InteractionDestinationDataController    { get { return GetComponent<InteractionDestinationDataController>(); } }
    public PhaseDataController PhaseDataController                                      { get { return GetComponent<PhaseDataController>(); } }
    public SceneActorDataController SceneActorDataController                            { get { return GetComponent<SceneActorDataController>(); } }
    public ScenePropDataController ScenePropDataController                              { get { return GetComponent<ScenePropDataController>(); } }

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Data Data                            { get; set; }
    public Enums.DataType DataType              { get { return Enums.DataType.EditorWorld; } }
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
            dataList = EditorWorldDataManager.GetData(searchProperties),
            searchProperties = this.searchProperties
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(IElementData searchElementData, IElementData resultElementData) { }

    public void ToggleElement(EditorElement editorElement) { }
}
