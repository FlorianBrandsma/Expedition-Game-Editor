using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class EditorWorldOrganizer : MonoBehaviour, IOrganizer
{
    public static EditorWorldOrganizer instance;

    private float cameraDistanceFromCenter = 15;

    private List<Tile> tileList = new List<Tile>();

    public TerrainElementData activeTerrainData;

    private List<WorldObjectElementData> selectedWorldObjects                       = new List<WorldObjectElementData>();
    private List<WorldInteractableElementData> selectedWorldInteractableAgents      = new List<WorldInteractableElementData>();
    private List<WorldInteractableElementData> selectedWorldInteractableObjects     = new List<WorldInteractableElementData>();
    private List<InteractionDestinationElementData> selectedInteractionDestinations = new List<InteractionDestinationElementData>();
    private List<PhaseElementData> selectedPhase                                    = new List<PhaseElementData>();
    private List<SceneActorElementData> selectedSceneActors                         = new List<SceneActorElementData>();
    private List<ScenePropElementData> selectedSceneProps                           = new List<ScenePropElementData>();
    
    private List<IElementData> selectableDataList;
    
    private EditorWorldElementData worldData;

    private float worldSize;

    private Rect activeRect;
    private Vector2 positionTracker = new Vector2();

    private IDisplayManager DisplayManager          { get { return GetComponent<IDisplayManager>(); } }
    private CameraManager CameraManager             { get { return (CameraManager)DisplayManager; } }
    
    private CameraProperties CameraProperties       { get { return (CameraProperties)DisplayManager.Display; } }
    private EditorWorldProperties WorldProperties   { get { return (EditorWorldProperties)DisplayManager.Display.Properties; } }

    private IDataController DataController          { get { return CameraManager.Display.DataController; } }

    private WorldObjectDataController WorldObjectDataController                         { get { return ((EditorWorldDataController)DataController).WorldObjectDataController; } }
    private WorldInteractableDataController WorldInteractableAgentDataController        { get { return ((EditorWorldDataController)DataController).WorldInteractableAgentDataController; } }
    private WorldInteractableDataController WorldInteractableObjectDataController       { get { return ((EditorWorldDataController)DataController).WorldInteractableObjectDataController; } }
    private InteractionDestinationDataController InteractionDestinationDataController   { get { return ((EditorWorldDataController)DataController).InteractionDestinationDataController; } }
    private PhaseDataController PhaseDataController                                     { get { return ((EditorWorldDataController)DataController).PhaseDataController; } }
    private SceneActorDataController SceneActorDataController                           { get { return ((EditorWorldDataController)DataController).SceneActorDataController; } }
    private ScenePropDataController ScenePropDataController                             { get { return ((EditorWorldDataController)DataController).ScenePropDataController; } }

    private ExScrollRect ScrollRect { get { return GetComponent<ExScrollRect>(); } }

    private bool allowSelection = true;
    private bool dataSet;

    private ExEditorWorldElement editorWorldElementPrefab;
    private ExSelectionIcon selectionIconPrefab;

    private void Awake()
    {
        instance = this;

        editorWorldElementPrefab    = Resources.Load<ExEditorWorldElement>("Elements/World/EditorWorldElement");
        selectionIconPrefab         = Resources.Load<ExSelectionIcon>("Elements/UI/SelectionIcon");
    }

    void Update()
    {
        var ray = CameraManager.cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (ScrollRect.m_Dragging)
            allowSelection = false;
        
        if (Input.GetMouseButtonUp(0))
        {
            //Selecting through UI elements is blocked, unless it's the element that's required to scroll the camera
            if (IsPointerOverUIObject() && EventSystem.current.currentSelectedGameObject != CameraManager.gameObject)
                return;
            
            if (allowSelection && Physics.Raycast(ray, out hit) && hit.collider.GetComponent<Model>() != null)
            {
                if (GameObject.Find("Dropdown List") != null) return;
                
                var editorElement = hit.collider.GetComponent<Model>().EditorElement;

                SelectElement(editorElement);
            }

            allowSelection = true;
        }
    }

    private bool IsPointerOverUIObject()
    {
        var eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }

    private void SelectElement(EditorElement editorElement)
    {
        var route = DataController.SegmentController.MainPath.FindLastRoute(editorElement.DataElement.ElementData.DataType);

        //Don't try to select an element if it is already selected
        if (route != null && editorElement.DataElement.Id == route.id) return;

        editorElement.InvokeSelection();
    }

    public void InitializeOrganizer()
    {        
        worldData = (EditorWorldElementData)DataController.Data.dataList.FirstOrDefault();

        //Get selected interaction destinations
        GetSelectedElement(Enums.DataType.InteractionDestination);
        
        if(worldData.RegionType != Enums.RegionType.Scene)
        {
            //Get selected interactable agents
            GetSelectedElement(Enums.DataType.WorldInteractable, Enums.InteractableType.Agent);

            //Get selected interactable objects
            GetSelectedElement(Enums.DataType.WorldInteractable, Enums.InteractableType.Object);
        }
        
        //Get selected world objects
        GetSelectedElement(Enums.DataType.WorldObject);
        
        if(worldData.RegionType == Enums.RegionType.Controllable)
        {
            //Get selected phases
            GetSelectedElement(Enums.DataType.Phase);
        }
        
        //Get selected scene actors
        GetSelectedElement(Enums.DataType.SceneActor);

        //Get selected scene props
        GetSelectedElement(Enums.DataType.SceneProp);

        InitializeControllers();
        
        SetWorldSize();

        SetCamera();
    }
    
    private void GetSelectedElement(Enums.DataType dataType, Enums.InteractableType interactableType = 0)
    {
        //Get selected elements from all paths
        var paths = RenderManager.layoutManager.forms.Select(x => x.activePath).ToList();
        var routes = paths.Select(x => x.FindLastRoute(dataType)).Where(x => x != null).ToList();
        
        if (routes == null) return;

        switch (dataType)
        {
            case Enums.DataType.WorldObject:            GetSelectedWorldObjects(routes);                            break;
            case Enums.DataType.WorldInteractable:      GetSelectedWorldInteractables(routes, interactableType);    break;
            case Enums.DataType.InteractionDestination: GetSelectedInteractionDestinations(routes);                 break;
            case Enums.DataType.Phase:                  GetSelectedPhases(routes);                                  break;
            case Enums.DataType.SceneActor:             GetSelectedSceneActors(routes);                             break;
            case Enums.DataType.SceneProp:              GetSelectedSceneProps(routes);                              break;
        }
    }

    private void GetSelectedWorldObjects(List<Route> routes)
    {
        selectedWorldObjects = worldData.TerrainDataList.SelectMany(x => x.WorldObjectDataList.Where(y => routes.Select(z => z.ElementData.Id).Contains(y.Id))).Distinct().ToList();
    }

    private void GetSelectedWorldInteractables(List<Route> routes, Enums.InteractableType interactableType)
    {
        if (interactableType == Enums.InteractableType.Agent)
        {
            var selectedWorldInteractables = worldData.TerrainDataList.SelectMany(x => x.WorldInteractableDataList.Where(y => y.Type == (int)Enums.InteractableType.Agent).Where(y => routes.Select(z => z.ElementData.Id).Contains(y.Id))).Cast<IElementData>().ToList();
            selectedWorldInteractableAgents = FilterWorldInteractables(selectedWorldInteractables).Cast<WorldInteractableElementData>().ToList();
        }
        
        if (interactableType == Enums.InteractableType.Object)
        {
            var selectedWorldInteractables = worldData.TerrainDataList.SelectMany(x => x.WorldInteractableDataList.Where(y => y.Type == (int)Enums.InteractableType.Object).Where(y => routes.Select(z => z.ElementData.Id).Contains(y.Id))).Cast<IElementData>().ToList();
            selectedWorldInteractableObjects = FilterWorldInteractables(selectedWorldInteractables).Cast<WorldInteractableElementData>().ToList();
        }
    }

    private void GetSelectedInteractionDestinations(List<Route> routes)
    {
        selectedInteractionDestinations = worldData.TerrainDataList.SelectMany(x => x.InteractionDestinationDataList.Where(y => routes.Select(z => z.ElementData.Id).Contains(y.Id))).Distinct().ToList();
    }

    private void GetSelectedPhases(List<Route> routes)
    {
        selectedPhase = worldData.PhaseDataList.Where(x => routes.Select(y => y.ElementData.Id).Contains(x.Id)).Distinct().ToList();
    }

    private void GetSelectedSceneActors(List<Route> routes)
    {
        selectedSceneActors = worldData.TerrainDataList.SelectMany(x => x.SceneActorDataList.Where(y => routes.Select(z => z.id).Contains(y.Id))).Distinct().ToList();
    }

    private void GetSelectedSceneProps(List<Route> routes)
    {
        selectedSceneProps = worldData.TerrainDataList.SelectMany(x => x.ScenePropDataList.Where(y => routes.Select(z => z.id).Contains(y.Id))).Distinct().ToList();
    }

    private void InitializeControllers()
    {
        WorldObjectDataController.Data = new Data()
        {
            dataController = WorldObjectDataController,
            searchProperties = WorldObjectDataController.SearchProperties
        };

        WorldInteractableAgentDataController.Data = new Data()
        {
            dataController = WorldInteractableAgentDataController,
            searchProperties = WorldInteractableAgentDataController.SearchProperties
        };

        WorldInteractableObjectDataController.Data = new Data()
        {
            dataController = WorldInteractableObjectDataController,
            searchProperties = WorldInteractableObjectDataController.SearchProperties
        };

        InteractionDestinationDataController.Data = new Data()
        {
            dataController = InteractionDestinationDataController,
            searchProperties = InteractionDestinationDataController.SearchProperties
        };

        PhaseDataController.Data = new Data()
        {
            dataController = PhaseDataController,
            searchProperties = PhaseDataController.SearchProperties
        };

        SceneActorDataController.Data = new Data()
        {
            dataController = SceneActorDataController,
            searchProperties = SceneActorDataController.SearchProperties
        };

        ScenePropDataController.Data = new Data()
        {
            dataController = ScenePropDataController,
            searchProperties = ScenePropDataController.SearchProperties
        };
        
        GetData();
    }

    private void GetData()
    {
        //Is this necessary?
        //FixLostInteractions();
        //FixLostWorldObjects();
        //-----------------

        //Confirm which atmosphere's timeframes contain the active time
        ValidateAtmosphereTime();

        //Confirm which interactions's timeframes contain the active time
        ValidateInteractionDestinationTime();

        //Confirm which world interactable's timeframes contain the active time
        ValidateWorldInteractableTime();

        //Confirm which interactions's timeframes contain the active time
        ValidateSceneActorTime();

        //Confirm which interactions's timeframes contain the active time
        ValidateScenePropTime();

        WorldObjectDataController.Data.dataList = worldData.TerrainDataList.SelectMany(x => x.WorldObjectDataList).Cast<IElementData>().ToList();
        
        //Only show interactables where the active time is within their first interaction's timeframe
        WorldInteractableAgentDataController.Data.dataList = FilterWorldInteractables(worldData.TerrainDataList.SelectMany(x => x.WorldInteractableDataList.Where(y => y.Type == (int)Enums.InteractableType.Agent)).Cast<IElementData>().ToList());

        //Only show interactables where the active time is within their first interaction's timeframe
        WorldInteractableObjectDataController.Data.dataList = FilterWorldInteractables(worldData.TerrainDataList.SelectMany(x => x.WorldInteractableDataList.Where(y => y.Type == (int)Enums.InteractableType.Object)).Cast<IElementData>().ToList());
        
        InteractionDestinationDataController.Data.dataList = worldData.TerrainDataList.SelectMany(x => x.InteractionDestinationDataList).Cast<IElementData>().ToList();
        
        PhaseDataController.Data = new Data()
        {
            dataController = PhaseDataController,
            dataList = worldData.PhaseDataList.Cast<IElementData>().ToList()
        };

        SceneActorDataController.Data.dataList = worldData.TerrainDataList.SelectMany(x => x.SceneActorDataList).Cast<IElementData>().ToList();

        ScenePropDataController.Data.dataList = worldData.TerrainDataList.SelectMany(x => x.ScenePropDataList).Cast<IElementData>().ToList();
    }

    private void FixLostInteractions()
    {
        var lostInteractions = worldData.TerrainDataList.SelectMany(x => x.InteractionDestinationDataList.Where(y => y.TerrainId != x.Id)).ToList();

        worldData.TerrainDataList.ForEach(x => x.InteractionDestinationDataList.RemoveAll(y => lostInteractions.Select(z => z.TerrainId).Contains(y.TerrainId)));
        worldData.TerrainDataList.ForEach(x => x.InteractionDestinationDataList.AddRange(lostInteractions.Where(y => y.TerrainId == x.Id)));
    }

    private void FixLostWorldObjects()
    {
        var lostWorldObjects = worldData.TerrainDataList.SelectMany(x => x.WorldObjectDataList.Where(y => y.TerrainId != 0 && y.TerrainId != x.Id)).ToList();

        worldData.TerrainDataList.ForEach(x => x.WorldObjectDataList.RemoveAll(y => lostWorldObjects.Select(z => z.TerrainId).Contains(y.TerrainId)));
        worldData.TerrainDataList.ForEach(x => x.WorldObjectDataList.AddRange(lostWorldObjects.Where(y => y.TerrainId == x.Id)));
    }

    private void ValidateAtmosphereTime()
    {
        worldData.TerrainDataList.ForEach(x => x.AtmosphereDataList.ForEach(y => y.ContainsActiveTime = false));

        worldData.TerrainDataList.SelectMany(x => x.AtmosphereDataList).GroupBy(y => y.TerrainId)
                                                                       .Select(y => y.Where(z => TimeManager.TimeInFrame(TimeManager.instance.ActiveTime, z.StartTime, z.EndTime) || z.Default)
                                                                       .OrderBy(z => z.Default).First()).ToList()
                                                                       .ForEach(x => x.ContainsActiveTime = true);
    }

    private void ValidateInteractionDestinationTime()
    {
        worldData.TerrainDataList.ForEach(x => x.InteractionDestinationDataList.ForEach(y => y.ContainsActiveTime = false));

        //Create groups of interaction destinations as interactions, using destination values that were taken from their parent interaction
        var interactionGroup = worldData.TerrainDataList.SelectMany(x => x.InteractionDestinationDataList).GroupBy(y => y.InteractionId)
                                                                                                          .Select(grp => new
                                                                                                          {
                                                                                                              grp.First().TaskId,
                                                                                                              grp.First().StartTime,
                                                                                                              grp.First().EndTime,
                                                                                                              grp.First().DefaultInteraction,
                                                                                                              InteractionDestinationList = grp.ToList()
                                                                                                          }).ToList();

        interactionGroup.GroupBy(x => x.TaskId).Select(x => x.Where(y => TimeManager.TimeInFrame(TimeManager.instance.ActiveTime, y.StartTime, y.EndTime) || y.DefaultInteraction)
                                                                         .OrderBy(y => y.DefaultInteraction).First()).ToList()
                                                                         .ForEach(y => y.InteractionDestinationList.ForEach(z => z.ContainsActiveTime = true));
    }

    private void ValidateWorldInteractableTime()
    {
        worldData.TerrainDataList.ForEach(x => x.WorldInteractableDataList.ForEach(y => y.ContainsActiveTime = false));

        worldData.TerrainDataList.SelectMany(x => x.WorldInteractableDataList).GroupBy(y => y.TaskGroup)
                                                                              .Select(y => y.Where(z => TimeManager.TimeInFrame(TimeManager.instance.ActiveTime, z.StartTime, z.EndTime) || z.Default)
                                                                              .OrderBy(z => z.Default).First()).ToList()
                                                                              .ForEach(x => x.ContainsActiveTime = true);
    }

    private void ValidateSceneActorTime()
    {
        worldData.TerrainDataList.ForEach(x => x.SceneActorDataList.ForEach(y => y.ContainsActiveTime = false));

        //Create groups of scene props as interactions, using destination values that were taken from their parent interaction
        var interactionGroup = worldData.TerrainDataList.SelectMany(x => x.SceneActorDataList).GroupBy(y => y.InteractionId)
                                                                                                             .Select(grp => new
                                                                                                             {
                                                                                                                 grp.First().TaskId,
                                                                                                                 grp.First().StartTime,
                                                                                                                 grp.First().EndTime,
                                                                                                                 grp.First().Default,
                                                                                                                 SceneActorList = grp.ToList()
                                                                                                             }).ToList();

        interactionGroup.GroupBy(x => x.TaskId).Select(x => x.Where(y => TimeManager.TimeInFrame(TimeManager.instance.ActiveTime, y.StartTime, y.EndTime) || y.Default)
                                                                         .OrderBy(y => y.Default).First()).ToList()
                                                                         .ForEach(y => y.SceneActorList.ForEach(z => z.ContainsActiveTime = true));
    }

    private void ValidateScenePropTime()
    {
        worldData.TerrainDataList.ForEach(x => x.ScenePropDataList.ForEach(y => y.ContainsActiveTime = false));

        //Create groups of scene props as interactions, using destination values that were taken from their parent interaction
        var interactionGroup = worldData.TerrainDataList.SelectMany(x => x.ScenePropDataList).GroupBy(y => y.InteractionId)
                                                                                                            .Select(grp => new
                                                                                                            {
                                                                                                                grp.First().TaskId,
                                                                                                                grp.First().StartTime,
                                                                                                                grp.First().EndTime,
                                                                                                                grp.First().Default,
                                                                                                                ScenePropList = grp.ToList()
                                                                                                            }).ToList();

        interactionGroup.GroupBy(x => x.TaskId).Select(x => x.Where(y => TimeManager.TimeInFrame(TimeManager.instance.ActiveTime, y.StartTime, y.EndTime) || y.Default)
                                                                         .OrderBy(y => y.Default).First()).ToList()
                                                                         .ForEach(y => y.ScenePropList.ForEach(z => z.ContainsActiveTime = true));
    }

    public void SelectData()
    {
        selectableDataList =    worldData.TerrainDataList.SelectMany(x => x.WorldObjectDataList).Cast<IElementData>().Concat(
                                worldData.TerrainDataList.SelectMany(x => x.WorldInteractableDataList).Cast<IElementData>()).Concat(
                                worldData.TerrainDataList.SelectMany(x => x.InteractionDestinationDataList).Cast<IElementData>()).Concat(
                                worldData.PhaseDataList.Cast<IElementData>()).Concat(
                                worldData.TerrainDataList.SelectMany(x => x.SceneActorDataList).Cast<IElementData>()).Concat(
                                worldData.TerrainDataList.SelectMany(x => x.ScenePropDataList).Cast<IElementData>()).ToList();
        
        SelectionManager.SelectData(selectableDataList, DisplayManager);
    }

    private void SetWorldSize()
    {
        worldSize = (worldData.RegionSize * worldData.TerrainSize * worldData.TileSize);
        
        ScrollRect.content.sizeDelta = new Vector2(worldSize, worldSize) * 2;
        CameraManager.content.sizeDelta = new Vector2(worldSize, worldSize);
    }
    
    public void UpdateData()
    {
        SetCamera();

        var worldSize = worldData.RegionSize * worldData.TerrainSize * worldData.TileSize;

        var worldCameraPosition = new Vector2(CameraManager.cameraParent.transform.localPosition.x - worldSize / 2,
                                              CameraManager.cameraParent.transform.localPosition.z + worldSize / 2);
        
        if (worldCameraPosition.x >= positionTracker.x + worldData.TileSize ||
            worldCameraPosition.x <= positionTracker.x - worldData.TileSize ||
            worldCameraPosition.y >= positionTracker.y + worldData.TileSize ||
            worldCameraPosition.y <= positionTracker.y - worldData.TileSize)
        {
            UpdatePositionTracker(worldCameraPosition);

            CloseInactiveElements();
            
            SetData(DataController.Data.dataList);

            dataSet = true;
        }
    }

    private void SetAddElementDefaultPosition()
    {
        var defaultPosition = AddElementDefaultPosition();

        //Update the position of unselected world objects
        var addWorldObjectElementData = WorldObjectDataController.Data.dataList.Where(x => x.Id == 0 && !selectedWorldObjects.Select(y => y.Id).Contains(x.Id)).FirstOrDefault();

        if (addWorldObjectElementData != null)
        {
            //Find all related selection elements and apply the default position
            var addWorldObjectElementDatalist = SelectionElementManager.FindElementData(addWorldObjectElementData).Concat(new[] { addWorldObjectElementData }).Distinct().Cast<WorldObjectElementData>().ToList();

            addWorldObjectElementDatalist.ForEach(elementData =>
            {
                elementData.PositionX = defaultPosition.x;
                elementData.PositionY = defaultPosition.y;
                elementData.PositionZ = defaultPosition.z;
            });
        }
    }

    public Vector3 AddElementDefaultPosition()
    {
        var cameraPosition = CameraManager.cameraParent.transform.localPosition;

        var defaultPosition = new Vector3(cameraPosition.x, 0, -cameraPosition.z - cameraDistanceFromCenter);
        
        return defaultPosition;
    }

    private void UpdatePositionTracker(Vector2 cameraPosition)
    {
        positionTracker = new Vector2(Mathf.Floor((cameraPosition.x + (worldData.TileSize / 2)) / worldData.TileSize) * worldData.TileSize,
                                      Mathf.Floor((cameraPosition.y + (worldData.TileSize / 2)) / worldData.TileSize) * worldData.TileSize);
    }

    public void ResetSelectedElement(IElementData elementData)
    {
        switch(elementData.DataType)
        {
            case Enums.DataType.SceneActor: ResetSelectedSceneActor(); break;

            default: Debug.Log("CASE MISSING: " + elementData.DataType); break;
        }
    }

    private void ResetSelectedSceneActor()
    {
        selectedSceneActors.Where(x => x.DataElement != null && x.DataElement.gameObject.activeInHierarchy).ToList().ForEach(x =>
        {
            PoolManager.ClosePoolObject(x.DataElement.Poolable);
            SelectionElementManager.CloseElement(x.DataElement);
        });

        SetWorldElements(SceneActorDataController, selectedSceneActors.Cast<IElementData>().ToList());
    }

    public void SetCamera()
    {
        var defaultCameraPosition = new Vector3(0, 10, -cameraDistanceFromCenter);
        var defaultCameraRotation = new Vector3(40, 0, 0);

        var sceneShotRoute = DataController.SegmentController.MainPath.FindLastRoute(Enums.DataType.SceneShot);

        if (worldData.RegionType != Enums.RegionType.Scene || SceneShotManager.activeShotType == Enums.SceneShotType.Base)
        {
            var worldSize = worldData.RegionSize * worldData.TerrainSize * worldData.TileSize;

            CameraManager.cameraParent.transform.localPosition = new Vector3(defaultCameraPosition.x + CameraManager.ScrollRectContent.localPosition.x + (worldSize / 2),
                                                                             defaultCameraPosition.y,
                                                                             defaultCameraPosition.z + CameraManager.ScrollRectContent.localPosition.y - (worldSize / 2));

            CameraManager.cameraParent.transform.localEulerAngles = defaultCameraRotation;

            CameraManager.EnableScrolling(true);

        } else {
            
            var sceneShotElementData = SceneShotManager.GetActiveElementData(sceneShotRoute);

            var sceneCameraPosition = new Vector3();
            var positionTarget = (SceneActorElementData)SceneActorDataController.Data.dataList.Where(x => x.Id == sceneShotElementData.PositionTargetSceneActorId).FirstOrDefault();

            if(positionTarget != null)
            {
                var targetPosition = new Vector3(positionTarget.PositionX, positionTarget.PositionY, -positionTarget.PositionZ);

                sceneCameraPosition = new Vector3(defaultCameraPosition.x + positionTarget.PositionX,
                                                  defaultCameraPosition.y + positionTarget.PositionY,
                                                  defaultCameraPosition.z - positionTarget.PositionZ);
            } else {

                sceneCameraPosition = new Vector3( sceneShotElementData.PositionX,
                                                   sceneShotElementData.PositionY,
                                                  -sceneShotElementData.PositionZ);
            }
            
            CameraManager.cameraParent.transform.localPosition = sceneCameraPosition;

            var sceneCameraRotation = new Vector3();
            var rotationTarget = (SceneActorElementData)SceneActorDataController.Data.dataList.Where(x => x.Id == sceneShotElementData.RotationTargetSceneActorId).FirstOrDefault();
            
            if(rotationTarget != null)
            {
                var targetPosition = new Vector3(rotationTarget.PositionX, rotationTarget.PositionY, -rotationTarget.PositionZ);

                sceneCameraRotation = Quaternion.LookRotation((targetPosition - sceneCameraPosition).normalized).eulerAngles;
                
            } else {

                sceneCameraRotation = new Vector3(sceneShotElementData.RotationX,
                                                  sceneShotElementData.RotationY,
                                                  sceneShotElementData.RotationZ);
            }

            CameraManager.cameraParent.transform.localEulerAngles = sceneCameraRotation;

            CameraManager.EnableScrolling(false);
        }

        SetAddElementDefaultPosition();

        SetActiveRect();
    }
    
    private void SetActiveRect()
    {
        var cameraTransform = CameraManager.cameraParent.transform;

        var tempActiveRange = 200;

        var activeRangePosition = new Vector2(cameraTransform.localPosition.x - (tempActiveRange / 2), cameraTransform.localPosition.z + (tempActiveRange / 2));
        var activeRangeSize = new Vector2(tempActiveRange, -tempActiveRange);

        activeRect = new Rect(activeRangePosition, activeRangeSize);
    }

    private void CloseInactiveElements()
    {
        var inactiveTiles = tileList.Where(x => !activeRect.Overlaps(((TerrainTileElementData)x.ElementData).GridElement.rect, true)).ToList();
        ClearTiles(inactiveTiles);

        dataSet = false;
    }
    
    public void SetData()
    {
        //Selected elements only spawn once on start up
        if (selectedInteractionDestinations.Count > 0)
            SetWorldElements(InteractionDestinationDataController, selectedInteractionDestinations.Cast<IElementData>().ToList());
        
        if (selectedWorldInteractableAgents.Count > 0)
            SetWorldElements(WorldInteractableAgentDataController, selectedWorldInteractableAgents.Cast<IElementData>().ToList());
        
        if (selectedWorldInteractableObjects.Count > 0)
            SetWorldElements(WorldInteractableObjectDataController, selectedWorldInteractableObjects.Cast<IElementData>().ToList());

        if (selectedWorldObjects.Count > 0)
            SetWorldElements(WorldObjectDataController, selectedWorldObjects.Cast<IElementData>().ToList());

        if (selectedPhase.Count > 0)
            SetWorldElements(PhaseDataController, selectedPhase.Cast<IElementData>().ToList());

        if (selectedSceneActors.Count > 0)
            SetWorldElements(SceneActorDataController, selectedSceneActors.Cast<IElementData>().ToList());

        if (selectedSceneProps.Count > 0)
            SetWorldElements(ScenePropDataController, selectedSceneProps.Cast<IElementData>().ToList());

        //Set elements that are not bound to a tile
        SetWorldObjects();
        
        SetData(DataController.Data.dataList);
    }
    
    private List<IElementData> FilterWorldInteractables(List<IElementData> dataList)
    {
        var worldInteractableDataList = dataList.Where(x => ((WorldInteractableElementData)x).ContainsActiveTime).GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();

        return worldInteractableDataList;
    }
    
    private void SetData(List<IElementData> list)
    {
        if (dataSet) return;

        foreach (TerrainElementData terrainData in worldData.TerrainDataList)
        {
            SetTerrain(terrainData);
        }

        GetActiveTerrain();
    }
    
    private void SetTerrain(TerrainElementData terrainData)
    {
        if (!activeRect.Overlaps(terrainData.GridElement.rect, true)) return;

        foreach (TerrainTileElementData terrainTileData in terrainData.TerrainTileDataList)
        {
            if (terrainTileData.Active || !activeRect.Overlaps(terrainTileData.GridElement.rect, true)) continue;

            SetTerrainTile(terrainTileData);

            //Set objects that are bound to this terrain tile
            SetWorldObjects(terrainTileData.Id);

            //Set world interactable agents that are bound to this tile by their first interaction
            SetWorldInteractableAgents(terrainTileData.Id);

            //Set world interactable objects that are bound to this tile by their first interaction
            SetWorldInteractableObjects(terrainTileData.Id);

            //Set interactions that are bound to this terrain tile
            SetInteractionDestinations(terrainTileData.Id);

            //Set default controllable representation of the active phase if it is bound to this tile 
            SetPhase(terrainTileData.Id);

            //Set scene actors that are bound to this terrain tile
            SetSceneActors(terrainTileData.Id);

            //Set scene props that are bound to this terrain tile
            SetSceneProps(terrainTileData.Id);
        }
    }
    
    private void SetTerrainTile(TerrainTileElementData terrainTileData)
    {
        var prefab = Resources.Load<Tile>("Objects/Tile/" + worldData.TileSetName + "/" + terrainTileData.TileId);

        var tile = (Tile)PoolManager.SpawnObject(prefab, terrainTileData.TileId);
        tileList.Add(tile);

        tile.gameObject.SetActive(true);

        tile.transform.SetParent(CameraManager.content, false);
        tile.transform.localPosition = new Vector3(terrainTileData.GridElement.startPosition.x, tile.transform.localPosition.y, terrainTileData.GridElement.startPosition.y);

        tile.DataType = Enums.DataType.TerrainTile;
        tile.ElementData = terrainTileData;

        tile.name = terrainTileData.DebugName + terrainTileData.Id;

        terrainTileData.Active = true;
    }

    private void SetWorldObjects(int terrainTileId = 0)
    {
        var worldObjectDataList = WorldObjectDataController.Data.dataList.Where(x => !selectedWorldObjects.Select(y => y.Id).Contains(x.Id) &&
                                                                                     ((WorldObjectElementData)x).TerrainTileId == terrainTileId).ToList(); /*&&
                                                                                     ((WorldObjectElementData)x).RegionId == worldData.Id).ToList();*/

        SetWorldElements(WorldObjectDataController, worldObjectDataList);
    }
    
    private void SetWorldInteractableAgents(int terrainTileId)
    {
        var worldInteractableAgentDataList = WorldInteractableAgentDataController.Data.dataList.Where(x => !selectedWorldInteractableAgents.Select(y => y.Id).Contains(x.Id) && 
                                                                                                           ((WorldInteractableElementData)x).TerrainTileId == terrainTileId).ToList();

        SetWorldElements(WorldInteractableAgentDataController, worldInteractableAgentDataList);
    }
    
    private void SetWorldInteractableObjects(int terrainTileId)
    {
        var worldInteractableObjectDataList = WorldInteractableObjectDataController.Data.dataList.Where(x => !selectedWorldInteractableObjects.Select(y => y.Id).Contains(x.Id) && 
                                                                                                             ((WorldInteractableElementData)x).TerrainTileId == terrainTileId).ToList();

        SetWorldElements(WorldInteractableObjectDataController, worldInteractableObjectDataList);
    }
    
    private void SetInteractionDestinations(int terrainTileId)
    {
        var interactionDestinationDataList = InteractionDestinationDataController.Data.dataList.Where(x => !selectedInteractionDestinations.Select(y => y.Id).Contains(x.Id) && 
                                                                                                           ((InteractionDestinationElementData)x).TerrainTileId == terrainTileId).ToList();

        SetWorldElements(InteractionDestinationDataController, interactionDestinationDataList);
    }

    private void SetPhase(int terrainTileId)
    {
        var phaseDataList = PhaseDataController.Data.dataList.Where(x => !selectedPhase.Select(y => y.Id).Contains(x.Id) && 
                                                                         ((PhaseElementData)x).TerrainTileId == terrainTileId).ToList();

        SetWorldElements(PhaseDataController, phaseDataList);
    }

    private void SetSceneActors(int terrainTileId)
    {
        var sceneActorDataList = SceneActorDataController.Data.dataList.Where(x => !selectedSceneActors.Select(y => y.Id).Contains(x.Id) &&
                                                                                   ((SceneActorElementData)x).TerrainTileId == terrainTileId).ToList();

        SetWorldElements(SceneActorDataController, sceneActorDataList);
    }

    private void SetSceneProps(int terrainTileId)
    {
        var scenePropDataList = ScenePropDataController.Data.dataList.Where(x => !selectedSceneProps.Select(y => y.Id).Contains(x.Id) &&
                                                                                 ((ScenePropElementData)x).TerrainTileId == terrainTileId).ToList();

        SetWorldElements(ScenePropDataController, scenePropDataList);
    }

    private void SetWorldElements(IDataController dataController, List<IElementData> dataList)
    {
        if (dataList.Count == 0) return;

        foreach (IElementData elementData in dataList)
        {
            var worldElement = (ExEditorWorldElement)PoolManager.SpawnObject(editorWorldElementPrefab);

            elementData.DataElement = worldElement.EditorElement.DataElement;
            
            worldElement.EditorElement.DataElement.Data = dataController.Data;
            worldElement.EditorElement.DataElement.Id   = elementData.Id;
            worldElement.EditorElement.DataElement.Path = DisplayManager.Display.DataController.SegmentController.Path;

            SelectionElementManager.InitializeElement(  worldElement.EditorElement.DataElement, CameraManager.content,
                                                        DisplayManager,
                                                        DisplayManager.Display.SelectionType,
                                                        DisplayManager.Display.SelectionProperty,
                                                        DisplayManager.Display.AddProperty,
                                                        DisplayManager.Display.UniqueSelection);
            
            //Debugging
            worldElement.name = elementData.DebugName + elementData.Id;

            SetStatus(worldElement.EditorElement);

            SetElement(worldElement.EditorElement);
        }
    }

    private void SetStatus(EditorElement element)
    {
        switch (element.DataElement.ElementData.DataType)
        {
            case Enums.DataType.WorldObject:            SetWorldObjectStatus(element);              break;
            case Enums.DataType.WorldInteractable:      SetWorldInteractableStatus(element);        break;
            case Enums.DataType.InteractionDestination: SetInteractionDestinationStatus(element);   break;
            case Enums.DataType.Phase:                  SetPhaseStatus(element);                    break;
            case Enums.DataType.SceneActor:             SetSceneActorStatus(element);               break;
            case Enums.DataType.SceneProp:              SetScenePropStatus(element);                break;
            
            default: Debug.Log("CASE MISSING: " + element.DataElement.ElementData.DataType); break;
        }
    }

    private void SetWorldObjectStatus(EditorElement element)
    {
        var worldObjectElementData = (WorldObjectElementData)element.DataElement.ElementData;

        if (worldData.RegionType == Enums.RegionType.InteractionDestination || 
            worldData.RegionType == Enums.RegionType.Controllable           || 
            worldData.RegionType == Enums.RegionType.Scene                  || 
            worldData.RegionType == Enums.RegionType.Game)
        {
            element.elementStatus = Enums.ElementStatus.Locked;
            return;
        }

        if(worldObjectElementData.ModelId == 1)
        {
            element.elementStatus = Enums.ElementStatus.Hidden;
            return;
        }
    }

    private void SetWorldInteractableStatus(EditorElement element)
    {
        var worldInteractableElementData = (WorldInteractableElementData)element.DataElement.ElementData;

        //Locked when editing default controllable transform
        if (worldData.RegionType == Enums.RegionType.Controllable || worldData.RegionType == Enums.RegionType.Scene)
        {
            element.elementStatus = Enums.ElementStatus.Locked;
            return;
        }

        //Hidden: Interactables where the active time is not within its timeframe
        if (!worldInteractableElementData.ContainsActiveTime)
        {
            element.elementStatus = Enums.ElementStatus.Hidden;
            return;
        }
    }

    private void SetInteractionDestinationStatus(EditorElement element)
    {
        var selectedInteractionDestination = selectedInteractionDestinations.FirstOrDefault();

        if (selectedInteractionDestination == null) return;

        var interactionDestinationElementData = (InteractionDestinationElementData)element.DataElement.ElementData;

        //Locked when editing default controllable transform
        if (worldData.RegionType == Enums.RegionType.Controllable || worldData.RegionType == Enums.RegionType.Scene)
        {
            element.elementStatus = Enums.ElementStatus.Locked;
            return;
        }

        //Debug.Log(  interactionData.Id + "/" + interactionDataElement.Id + "   " +
        //            interactionData.TaskId + "/" + interactionDataElement.TaskId + "   " +
        //            interactionData.worldInteractableId + "/" + interactionDataElement.worldInteractableId + "   " +
        //            interactionData.questId + "/" + interactionDataElement.questId + "   " +
        //            interactionData.objectiveId + "/" + interactionDataElement.objectiveId);

        //Hidden: Interactions where the active time is not within its timeframe
        if (!interactionDestinationElementData.ContainsActiveTime)
        {
            element.elementStatus = Enums.ElementStatus.Hidden;
            return;
        }

        //Hidden: Interactions belonging to the same world interactable and objective
        if (interactionDestinationElementData.Id != selectedInteractionDestination.Id &&
            interactionDestinationElementData.WorldInteractableId == selectedInteractionDestination.WorldInteractableId &&
            interactionDestinationElementData.ObjectiveId == selectedInteractionDestination.ObjectiveId)
        {
            element.elementStatus = Enums.ElementStatus.Hidden;
            return;
        }

        //Locked: Interactions that aren't selected and don't belong to a quest, only when an interaction with a quest is selected
        if (interactionDestinationElementData.Id != selectedInteractionDestination.Id &&
            selectedInteractionDestination.ObjectiveId != 0 && interactionDestinationElementData.ObjectiveId == 0)
        {
            element.elementStatus = Enums.ElementStatus.Locked;
            return;
        }

        //Enabled: Interactions belonging to a different interactable, but same objective
        if (interactionDestinationElementData.WorldInteractableId != selectedInteractionDestination.WorldInteractableId &&
            interactionDestinationElementData.ObjectiveId == selectedInteractionDestination.ObjectiveId)
        {
            element.elementStatus = Enums.ElementStatus.Enabled;
            return;
        }
        
        //Related: Interactions belonging to a different interactable, different objective but same quest
        if (interactionDestinationElementData.WorldInteractableId != selectedInteractionDestination.WorldInteractableId &&
            interactionDestinationElementData.QuestId == selectedInteractionDestination.QuestId)
        {
            element.elementStatus = Enums.ElementStatus.Related;
            return;
        }
        
        //Unrelated: Interactions belonging to a different quest
        if (interactionDestinationElementData.QuestId > 0 &&
            interactionDestinationElementData.QuestId != selectedInteractionDestination.QuestId)
        {
            element.elementStatus = Enums.ElementStatus.Unrelated;
            return;
        }
    }
    
    private void SetPhaseStatus(EditorElement element)
    {
        var phaseElementData = (PhaseElementData)element.DataElement.ElementData;

        //Locked when not editing default controllable transform
        if (worldData.RegionType != Enums.RegionType.Controllable)
        {
            element.elementStatus = Enums.ElementStatus.Locked;
            return;
        }
    }

    private void SetSceneActorStatus(EditorElement element)
    {
        var sceneRoute = DataController.SegmentController.MainPath.FindLastRoute(Enums.DataType.Scene);

        if (sceneRoute == null) return;

        var sceneActorElementData = (SceneActorElementData)element.DataElement.ElementData;
        var sceneElementData = (SceneElementData)sceneRoute.ElementData;

        //Hidden: actors where the active time is not within its timeframe
        if (!sceneActorElementData.ContainsActiveTime)
        {
            element.elementStatus = Enums.ElementStatus.Hidden;
            return;
        }

        //Hidden: actors whose position does not change or whose position changes but also freezes
        if (!sceneActorElementData.ChangePosition)
        {
            element.elementStatus = Enums.ElementStatus.Hidden;
            return;
        }

        //Hidden: actors belonging to a different scene but to the same interaction
        if (sceneActorElementData.SceneId != sceneElementData.Id &&
            sceneActorElementData.InteractionId == sceneElementData.Id)
        {
            element.elementStatus = Enums.ElementStatus.Hidden;
            return;
        }
        
        //Unrelated: actors belonging to a different scene and to a different interaction
        if (sceneActorElementData.SceneId != sceneElementData.Id &&
            sceneActorElementData.InteractionId != sceneElementData.Id)
        {
            element.elementStatus = Enums.ElementStatus.Locked;
            //element.elementStatus = Enums.ElementStatus.Unrelated;
            return;
        }

        //Enabled: actors belonging to the same scene and to the same interaction
        if (sceneActorElementData.SceneId == sceneElementData.Id &&
            sceneActorElementData.InteractionId == sceneElementData.Id)
        {
            element.elementStatus = Enums.ElementStatus.Enabled;
            return;
        }
    }

    private void SetScenePropStatus(EditorElement element)
    {
        var sceneRoute = DataController.SegmentController.MainPath.FindLastRoute(Enums.DataType.Scene);

        if (sceneRoute == null) return;

        var scenePropElementData = (ScenePropElementData)element.DataElement.ElementData;
        var sceneElementData = (SceneElementData)sceneRoute.ElementData;

        //Enabled: Props belonging to the same scene and to the same interaction
        if (scenePropElementData.SceneId == sceneElementData.Id &&
            scenePropElementData.InteractionId == sceneElementData.InteractionId)
        {
            element.elementStatus = Enums.ElementStatus.Enabled;
            return;
        }

        //Hidden: Interactions where the active time is not within its timeframe
        if (!scenePropElementData.ContainsActiveTime)
        {
            element.elementStatus = Enums.ElementStatus.Hidden;
            return;
        }

        //Hidden: Props belonging to a different scene but to the same interaction
        if (scenePropElementData.SceneId != sceneElementData.Id &&
            scenePropElementData.InteractionId == sceneElementData.InteractionId)
        {
            element.elementStatus = Enums.ElementStatus.Hidden;
            return;
        }

        //Unrelated: Props belonging to a different scene and to a different interaction
        if (scenePropElementData.SceneId != sceneElementData.Id &&
            scenePropElementData.InteractionId != sceneElementData.InteractionId)
        {
            element.elementStatus = Enums.ElementStatus.Locked;
            //element.elementStatus = Enums.ElementStatus.Unrelated;
            return;
        }
    }

    private bool SetElement(EditorElement element)
    {
        element.gameObject.SetActive(true);
        
        element.DataElement.SetElement();

        InitializeTrackingElementOverlay(element);

        element.SetOverlay();

        return true;
    }

    private void InitializeTrackingElementOverlay(EditorElement element)
    {
        if (element.glow != null && element.glow.activeInHierarchy) return;

        ExSelectionIcon selectionIcon = null;

        var trackingElementOverlay = CameraManager.overlayManager.TrackingElementOverlay;
        
        if (element.elementStatus == Enums.ElementStatus.Locked)
        {
            selectionIcon = trackingElementOverlay.SpawnSelectionIcon(selectionIconPrefab, Enums.SelectionIconType.Lock, Enums.TrackingElementType.Free);
            element.lockIcon = selectionIcon.gameObject;
        }

        if (element.DataElement.ElementData.SelectionStatus != Enums.SelectionStatus.None)
        {
            selectionIcon = trackingElementOverlay.SpawnSelectionIcon(selectionIconPrefab, Enums.SelectionIconType.Select, Enums.TrackingElementType.Limited);
            element.glow = selectionIcon.gameObject;
        }

        if(selectionIcon != null)
            selectionIcon.TrackingElement.SetTrackingTarget(element.DataElement);   
    }

    public void ResetData(List<IElementData> filter)
    {
        ClearOrganizer();
        SetData();
    }
    
    public void ClearOrganizer()
    {
        dataSet = false;

        ClearWorldObjects();
        ClearSelectedElements();
        ClearTiles(tileList);        
    }

    private void ClearSelectedElements()
    {
        if(selectedWorldObjects.Count > 0)
        {
            selectedWorldObjects.Where(x => x.DataElement != null).ToList().ForEach(x => 
            {
                PoolManager.ClosePoolObject(x.DataElement.Poolable);
                SelectionElementManager.CloseElement(x.DataElement);
            });
        }
        
        if(selectedWorldInteractableAgents.Count > 0)
        {
            selectedWorldInteractableAgents.Where(x => x.DataElement != null).ToList().ForEach(x => 
            {
                PoolManager.ClosePoolObject(x.DataElement.Poolable);
                SelectionElementManager.CloseElement(x.DataElement);
            });
        }

        if (selectedWorldInteractableObjects.Count > 0)
        {
            selectedWorldInteractableObjects.Where(x => x.DataElement != null).ToList().ForEach(x => 
            {
                PoolManager.ClosePoolObject(x.DataElement.Poolable);
                SelectionElementManager.CloseElement(x.DataElement);
            });
        }

        if (selectedInteractionDestinations.Count > 0)
        {
            selectedInteractionDestinations.Where(x => x.DataElement != null).ToList().ForEach(x =>
            {
                PoolManager.ClosePoolObject(x.DataElement.Poolable);
                SelectionElementManager.CloseElement(x.DataElement);
            });
        }

        if(selectedPhase.Count > 0)
        {
            selectedPhase.Where(x => x.DataElement != null).ToList().ForEach(x =>
            {
                PoolManager.ClosePoolObject(x.DataElement.Poolable);
                SelectionElementManager.CloseElement(x.DataElement);
            });
        }

        if (selectedSceneActors.Count > 0)
        {
            selectedSceneActors.Where(x => x.DataElement != null).ToList().ForEach(x =>
            {
                PoolManager.ClosePoolObject(x.DataElement.Poolable);
                SelectionElementManager.CloseElement(x.DataElement);
            });
        }

        if (selectedSceneProps.Count > 0)
        {
            selectedSceneProps.Where(x => x.DataElement != null).ToList().ForEach(x =>
            {
                PoolManager.ClosePoolObject(x.DataElement.Poolable);
                SelectionElementManager.CloseElement(x.DataElement);
            });
        }
    }

    private void ClearTiles(List<Tile> inactiveTileList)
    {
        inactiveTileList.ForEach(x => 
        {
            ClearTileElements(x.ElementData.Id);
            PoolManager.ClosePoolObject(x);
        });

        tileList.RemoveAll(x => inactiveTileList.Contains(x));
    }

    private void ClearTileElements(int terrainTileId)
    {
        ClearWorldObjects(terrainTileId);
        ClearWorldInteractableAgents(terrainTileId);
        ClearWorldInteractableObjects(terrainTileId);
        ClearInteractionDestinations(terrainTileId);
        ClearPhases(terrainTileId);
        ClearSceneActors(terrainTileId);
        ClearSceneProps(terrainTileId);
    }

    private void ClearWorldObjects(int terrainTileId = 0)
    {
        var worldObjectElementList = WorldObjectDataController.Data.dataList.Cast<WorldObjectElementData>().Where(x => x.DataElement != null).ToList();
        
        var inactiveWorldObjectList = worldObjectElementList.Where(x => !selectedWorldObjects.Select(y => y.Id).Contains(x.Id) && 
                                                                        x.TerrainTileId == terrainTileId).ToList();

        inactiveWorldObjectList.ForEach(x => 
        {
            PoolManager.ClosePoolObject(x.DataElement.Poolable);
            SelectionElementManager.CloseElement(x.DataElement);
        });
    }

    private void ClearWorldInteractableAgents(int terrainTileId)
    {
        var worldInteractableAgentElementList = WorldInteractableAgentDataController.Data.dataList.Cast<WorldInteractableElementData>().Where(x => x.DataElement != null).ToList();

        var inactiveWorldInteractableAgentList = worldInteractableAgentElementList.Where(x => !selectedWorldInteractableAgents.Select(y => y.Id).Contains(x.Id) && 
                                                                                              x.TerrainTileId == terrainTileId).ToList();

        inactiveWorldInteractableAgentList.ForEach(x => 
        {
            PoolManager.ClosePoolObject(x.DataElement.Poolable);
            SelectionElementManager.CloseElement(x.DataElement);
        });
    }

    private void ClearWorldInteractableObjects(int terrainTileId)
    {
        var worldInteractableObjectElementList = WorldInteractableObjectDataController.Data.dataList.Cast<WorldInteractableElementData>().Where(x => x.DataElement != null).ToList();

        var inactiveWorldInteractableObjectList = worldInteractableObjectElementList.Where(x => !selectedWorldInteractableObjects.Select(y => y.Id).Contains(x.Id) && 
                                                                                                x.TerrainTileId == terrainTileId).ToList();

        inactiveWorldInteractableObjectList.ForEach(x => 
        {
            PoolManager.ClosePoolObject(x.DataElement.Poolable);
            SelectionElementManager.CloseElement(x.DataElement);
        });
    }

    private void ClearInteractionDestinations(int terrainTileId)
    {
        var interactionDestinationElementList = InteractionDestinationDataController.Data.dataList.Cast<InteractionDestinationElementData>().Where(x => x.DataElement != null).ToList();

        var inactiveInteractionDestinationList = interactionDestinationElementList.Where(x => !selectedInteractionDestinations.Select(y => y.Id).Contains(x.Id) && 
                                                                                              x.TerrainTileId == terrainTileId).ToList();

        inactiveInteractionDestinationList.ForEach(x => 
        {
            PoolManager.ClosePoolObject(x.DataElement.Poolable);
            SelectionElementManager.CloseElement(x.DataElement);
        });
    }

    private void ClearPhases(int terrainTileId)
    {
        var phaseElementList = PhaseDataController.Data.dataList.Cast<PhaseElementData>().Where(x => x.DataElement != null).ToList();

        var inactivePhaseList = phaseElementList.Where(x => !selectedPhase.Select(y => y.Id).Contains(x.Id) &&
                                                            x.TerrainTileId == terrainTileId).ToList();

        inactivePhaseList.ForEach(x =>
        {
            PoolManager.ClosePoolObject(x.DataElement.Poolable);
            SelectionElementManager.CloseElement(x.DataElement);
        });
    }

    private void ClearSceneActors(int terrainTileId)
    {
        var sceneActorElementList = SceneActorDataController.Data.dataList.Cast<SceneActorElementData>().Where(x => x.DataElement != null).ToList();

        var inactiveSceneActorList = sceneActorElementList.Where(x => !selectedSceneActors.Select(y => y.Id).Contains(x.Id) &&
                                                                      x.TerrainTileId == terrainTileId).ToList();

        inactiveSceneActorList.ForEach(x =>
        {
            PoolManager.ClosePoolObject(x.DataElement.Poolable);
            SelectionElementManager.CloseElement(x.DataElement);
        });
    }

    private void ClearSceneProps(int terrainTileId)
    {
        var scenePropElementList = ScenePropDataController.Data.dataList.Cast<ScenePropElementData>().Where(x => x.DataElement != null).ToList();

        var inactiveScenePropList = scenePropElementList.Where(x => !selectedSceneProps.Select(y => y.Id).Contains(x.Id) &&
                                                                    x.TerrainTileId == terrainTileId).ToList();

        inactiveScenePropList.ForEach(x =>
        {
            PoolManager.ClosePoolObject(x.DataElement.Poolable);
            SelectionElementManager.CloseElement(x.DataElement);
        });
    }

    private void CancelSelection()
    {
        SelectionManager.CancelSelection(selectableDataList);
    }

    private void GetActiveTerrain()
    {
        var localCameraPosition = CameraManager.cameraParent.localPosition;

        if (localCameraPosition.x < 0)
            localCameraPosition = new Vector3(1, localCameraPosition.y, localCameraPosition.z);
        if (localCameraPosition.x > worldSize)
            localCameraPosition = new Vector3(worldSize - 1, localCameraPosition.y, localCameraPosition.z);
        if (localCameraPosition.z + cameraDistanceFromCenter > 0)
            localCameraPosition = new Vector3(localCameraPosition.x, localCameraPosition.y, -1);
        if (localCameraPosition.z - cameraDistanceFromCenter < -worldSize)
            localCameraPosition = new Vector3(localCameraPosition.x, localCameraPosition.y, -worldSize + 1);
        
        activeTerrainData = GetTerrain(localCameraPosition);
    }

    private TerrainElementData GetTerrain(Vector3 cameraPosition)
    {
        var terrainSize = worldData.TerrainSize * worldData.TileSize;

        var terrainCoordinates = new Vector3(Mathf.Floor(cameraPosition.x   / terrainSize),
                                             Mathf.Floor(cameraPosition.y   / terrainSize),
                                             Mathf.Floor(-cameraPosition.z  / terrainSize));

        var terrainIndex = (worldData.RegionSize * terrainCoordinates.z) + terrainCoordinates.x;

        return worldData.TerrainDataList.Where(x => x.Index == terrainIndex).FirstOrDefault();
    }
    
    public void CloseOrganizer()
    {
        CancelSelection();

        instance = null;

        DestroyImmediate(this);
    } 
}
