using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class EditorWorldOrganizer : MonoBehaviour, IOrganizer
{
    private List<Tile> tileList = new List<Tile>();

    public TerrainElementData activeTerrainData;

    private List<WorldObjectElementData> selectedWorldObjects                       = new List<WorldObjectElementData>();
    private List<WorldInteractableElementData> selectedWorldInteractableAgents      = new List<WorldInteractableElementData>();
    private List<WorldInteractableElementData> selectedWorldInteractableObjects     = new List<WorldInteractableElementData>();
    private List<InteractionDestinationElementData> selectedInteractionDestinations = new List<InteractionDestinationElementData>();
    private List<PhaseElementData> selectedParty                                    = new List<PhaseElementData>();
    
    private List<IElementData> selectableDataList;
    
    private EditorWorldElementData worldData;

    private float worldSize;

    private Rect activeRect;
    private Vector2 positionTracker = new Vector2();

    private IDisplayManager DisplayManager      { get { return GetComponent<IDisplayManager>(); } }
    private CameraManager CameraManager         { get { return (CameraManager)DisplayManager; } }
    
    private CameraProperties CameraProperties   { get { return (CameraProperties)DisplayManager.Display; } }
    private EditorWorldProperties WorldProperties     { get { return (EditorWorldProperties)DisplayManager.Display.Properties; } }

    private IDataController DataController      { get { return CameraManager.Display.DataController; } }

    private DataController worldObjectController                = new DataController(Enums.DataType.WorldObject);
    private DataController worldInteractableAgentController     = new DataController(Enums.DataType.WorldInteractable);
    private DataController worldInteractableObjectController    = new DataController(Enums.DataType.WorldInteractable);
    private DataController interactionDestinationController     = new DataController(Enums.DataType.InteractionDestination);
    private DataController partyController                      = new DataController(Enums.DataType.Phase);
    
    private ExScrollRect ScrollRect { get { return GetComponent<ExScrollRect>(); } }

    private bool allowSelection = true;
    private bool dataSet;

    void Update()
    {
        var ray = CameraManager.cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (ScrollRect.m_Dragging)
            allowSelection = false;
        
        if (Input.GetMouseButtonUp(0))
        {
            var fingerId = -1;

            if (Input.touchCount > 0)
                fingerId = Input.GetTouch(0).fingerId;

            //Selecting through UI elements is blocked, unless it's the element that's required to scroll the camera
            if (EventSystem.current.IsPointerOverGameObject(fingerId) && EventSystem.current.currentSelectedGameObject != CameraManager.gameObject)
                return;
            
            if (allowSelection && Physics.Raycast(ray, out hit) && hit.collider.GetComponent<Model>() != null)
            {
                if (GameObject.Find("Dropdown List") != null) return;
                
                var editorElement = hit.collider.GetComponent<Model>().EditorElement;

                editorElement.InvokeSelection();
            }

            allowSelection = true;
        }
    }

    public void InitializeOrganizer()
    {
        worldData = (EditorWorldElementData)DataController.Data.dataList.FirstOrDefault();

        //Get selected interaction destinations
        GetSelectedElement(DataController.SegmentController.MainPath, Enums.DataType.InteractionDestination);
        
        //Get selected interactable agents
        GetSelectedElement(DataController.SegmentController.MainPath, Enums.DataType.WorldInteractable, Enums.InteractableType.Agent);

        //Get selected interactable objects
        GetSelectedElement(DataController.SegmentController.MainPath, Enums.DataType.WorldInteractable, Enums.InteractableType.Object);

        //Get selected world objects
        GetSelectedElement(DataController.SegmentController.MainPath, Enums.DataType.WorldObject);

        //Get selected phase if the region type is "party"
        if(worldData.RegionType == Enums.RegionType.Party)
            GetSelectedElement(DataController.SegmentController.MainPath, Enums.DataType.Phase);

        InitializeControllers();
        
        SetWorldSize();

        CameraManager.cam.transform.localPosition = new Vector3(0, 10, -(worldData.TileSize * 0.5f));
        SetCameraPosition();
    }
    
    private void GetSelectedElement(Path path, Enums.DataType dataType, Enums.InteractableType interactableType = 0)
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
        selectedParty = worldData.PhaseDataList.Where(x => routes.Select(y => y.ElementData.Id).Contains(x.Id)).Distinct().ToList();
    }

    private void InitializeControllers()
    {
        worldObjectController.SegmentController             = DataController.SegmentController;
        worldInteractableAgentController.SegmentController  = DataController.SegmentController;
        worldInteractableObjectController.SegmentController = DataController.SegmentController;
        interactionDestinationController.SegmentController  = DataController.SegmentController;
        partyController.SegmentController                   = DataController.SegmentController;
        
        worldInteractableAgentController.DataCategory   = Enums.DataCategory.Navigation;
        worldInteractableObjectController.DataCategory  = Enums.DataCategory.Navigation;
        interactionDestinationController.DataCategory   = Enums.DataCategory.Navigation;
        partyController.DataCategory                    = Enums.DataCategory.Navigation;

        GetData();
    }

    private void GetData()
    {
        //Is this necessary?
        FixLostInteractions();
        FixLostWorldObjects();
        //-----------------

        //Confirm which atmosphere's timeframes contain the active time
        ValidateAtmosphereTime();

        //Confirm which interactions's timeframes contain the active time
        ValidateInteractionDestinationTime();

        //Confirm which world interactable's timeframes contain the active time
        ValidateWorldInteractableTime();
        
        worldObjectController.Data.dataList = worldData.TerrainDataList.SelectMany(x => x.WorldObjectDataList).Cast<IElementData>().ToList();
        
        //Only show interactables where the active time is within their first interaction's timeframe
        worldInteractableAgentController.Data.dataList = FilterWorldInteractables(worldData.TerrainDataList.SelectMany(x => x.WorldInteractableDataList.Where(y => y.Type == (int)Enums.InteractableType.Agent)).Cast<IElementData>().ToList());
        
        //Only show interactables where the active time is within their first interaction's timeframe
        worldInteractableObjectController.Data.dataList = FilterWorldInteractables(worldData.TerrainDataList.SelectMany(x => x.WorldInteractableDataList.Where(y => y.Type == (int)Enums.InteractableType.Object)).Cast<IElementData>().ToList());
        
        interactionDestinationController.Data.dataList = worldData.TerrainDataList.SelectMany(x => x.InteractionDestinationDataList).Cast<IElementData>().ToList();
        
        partyController.Data = new Data()
        {
            dataController = partyController,
            dataList = worldData.PhaseDataList.Cast<IElementData>().ToList()
        };
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

        worldData.TerrainDataList.SelectMany(x => x.AtmosphereDataList.GroupBy(y => y.TerrainId)
                                                                      .Select(y => y.Where(z => TimeManager.TimeInFrame(TimeManager.instance.ActiveTime, z.StartTime, z.EndTime) || z.Default)
                                                                      .OrderBy(z => z.Default).First())).ToList()
                                                                      .ForEach(x => x.ContainsActiveTime = true);
    }

    private void ValidateInteractionDestinationTime()
    {
        worldData.TerrainDataList.ForEach(x => x.InteractionDestinationDataList.ForEach(y => y.ContainsActiveTime = false));

        //Create groups of interaction destinations as interactions, using destination values that were taken from their parent interaction
        var interactionGroup = worldData.TerrainDataList.SelectMany(x => x.InteractionDestinationDataList.GroupBy(y => y.InteractionId)
                                                                                                         .Select(grp => new
                                                                                                         {
                                                                                                             grp.First().TaskId,
                                                                                                             grp.First().StartTime,
                                                                                                             grp.First().EndTime,
                                                                                                             grp.First().Default,
                                                                                                             InteractionDestinationList = grp.ToList()
                                                                                                         })).ToList();

        interactionGroup.GroupBy(x => x.TaskId).Select(x => x.Where(y => TimeManager.TimeInFrame(TimeManager.instance.ActiveTime, y.StartTime, y.EndTime) || y.Default)
                                                                         .OrderBy(y => y.Default).First()).ToList()
                                                                         .ForEach(y => y.InteractionDestinationList.ForEach(z => z.ContainsActiveTime = true));
    }

    private void ValidateWorldInteractableTime()
    {
        worldData.TerrainDataList.ForEach(x => x.WorldInteractableDataList.ForEach(y => y.ContainsActiveTime = false));

        worldData.TerrainDataList.SelectMany(x => x.WorldInteractableDataList.GroupBy(y => y.TaskGroup)
                                                                             .Select(y => y.Where(z => TimeManager.TimeInFrame(TimeManager.instance.ActiveTime, z.StartTime, z.EndTime) || z.Default)
                                                                             .OrderBy(z => z.Default).First())).ToList()
                                                                             .ForEach(x => x.ContainsActiveTime = true);
    }
    
    public void SelectData()
    {
        selectableDataList =    worldData.TerrainDataList.SelectMany(x => x.WorldObjectDataList).Cast<IElementData>().Concat(
                                worldData.TerrainDataList.SelectMany(x => x.WorldInteractableDataList).Cast<IElementData>()).Concat(
                                worldData.TerrainDataList.SelectMany(x => x.InteractionDestinationDataList).Cast<IElementData>()).Concat(
                                worldData.PhaseDataList.Cast<IElementData>()).ToList();
        
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
        SetCameraPosition();

        if (ScrollRect.content.localPosition.x >= positionTracker.x + worldData.TileSize ||
            ScrollRect.content.localPosition.x <= positionTracker.x - worldData.TileSize ||
            ScrollRect.content.localPosition.y >= positionTracker.y + worldData.TileSize ||
            ScrollRect.content.localPosition.y <= positionTracker.y - worldData.TileSize)
        {
            CloseInactiveElements();
            
            SetData(DataController.Data.dataList);

            dataSet = true;
        }
    }

    public void SetCameraPosition()
    {
        var worldSize = worldData.RegionSize * worldData.TerrainSize * worldData.TileSize;

        CameraManager.cameraParent.transform.localPosition = new Vector3(CameraManager.ScrollRectContent.localPosition.x + (worldSize / 2),
                                                                         CameraManager.cameraParent.transform.localPosition.y,
                                                                         CameraManager.ScrollRectContent.localPosition.y - (worldSize / 2));

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
        positionTracker = FixTrackerPosition(ScrollRect.content.localPosition);

        //Selected elements only spawn once on start up
        if (selectedInteractionDestinations.Count > 0)
            SetWorldElements(interactionDestinationController, selectedInteractionDestinations.Cast<IElementData>().ToList());
        
        if (selectedWorldInteractableAgents.Count > 0)
            SetWorldElements(worldInteractableAgentController, selectedWorldInteractableAgents.Cast<IElementData>().ToList());
        
        if (selectedWorldInteractableObjects.Count > 0)
            SetWorldElements(worldInteractableObjectController, selectedWorldInteractableObjects.Cast<IElementData>().ToList());

        if (selectedWorldObjects.Count > 0)
            SetWorldElements(worldObjectController, selectedWorldObjects.Cast<IElementData>().ToList());

        if (selectedParty.Count > 0)
            SetWorldElements(partyController, selectedParty.Cast<IElementData>().ToList());

        SetData(DataController.Data.dataList);
    }

    private Vector2 FixTrackerPosition(Vector2 cameraPosition)
    {
        return new Vector2(Mathf.Floor((cameraPosition.x + (worldData.TileSize / 2)) / worldData.TileSize) * worldData.TileSize,
                           Mathf.Floor((cameraPosition.y + (worldData.TileSize / 2)) / worldData.TileSize) * worldData.TileSize);
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
        
        //Set elements that are not bound to a tile
        SetWorldObjects();
        
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

            //Set default party representation of the active phase if it is bound to this tile 
            SetParty(terrainTileData.Id);
        }
    }
    
    private void SetTerrainTile(TerrainTileElementData terrainTileData)
    {
        Tile prefab = Resources.Load<Tile>("Objects/Tile/" + worldData.TileSetName + "/" + terrainTileData.TileId);

        Tile tile = (Tile)PoolManager.SpawnObject(prefab, terrainTileData.TileId);
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
        var worldObjectDataList = worldObjectController.Data.dataList.Where(x => !selectedWorldObjects.Select(y => y.Id).Contains(x.Id) && 
                                                                                 ((WorldObjectElementData)x).TerrainTileId == terrainTileId).ToList();

        SetWorldElements(worldObjectController, worldObjectDataList);
    }
    
    private void SetWorldInteractableAgents(int terrainTileId)
    {
        var worldInteractableAgentDataList = worldInteractableAgentController.Data.dataList.Where(x => !selectedWorldInteractableAgents.Select(y => y.Id).Contains(x.Id) && 
                                                                                                       ((WorldInteractableElementData)x).TerrainTileId == terrainTileId).ToList();
        
        SetWorldElements(worldInteractableAgentController, worldInteractableAgentDataList);
    }
    
    private void SetWorldInteractableObjects(int terrainTileId)
    {
        var worldInteractableObjectDataList = worldInteractableObjectController.Data.dataList.Where(x => !selectedWorldInteractableObjects.Select(y => y.Id).Contains(x.Id) && 
                                                                                                         ((WorldInteractableElementData)x).TerrainTileId == terrainTileId).ToList();

        SetWorldElements(worldInteractableObjectController, worldInteractableObjectDataList);
    }
    
    private void SetInteractionDestinations(int terrainTileId)
    {
        var interactionDestinationDataList = interactionDestinationController.Data.dataList.Where(x => !selectedInteractionDestinations.Select(y => y.Id).Contains(x.Id) && 
                                                                                                       ((InteractionDestinationElementData)x).TerrainTileId == terrainTileId).ToList();

        SetWorldElements(interactionDestinationController, interactionDestinationDataList);
    }

    private void SetParty(int terrainTileId)
    {
        var partyDataList = partyController.Data.dataList.Where(x => !selectedParty.Select(y => y.Id).Contains(x.Id) && 
                                                                     ((PhaseElementData)x).TerrainTileId == terrainTileId).ToList();

        SetWorldElements(partyController, partyDataList);
    }

    private void SetWorldElements(IDataController dataController, List<IElementData> dataList)
    {
        if (dataList.Count == 0) return;
        
        var prefab = Resources.Load<ExEditorWorldElement>("Elements/World/EditorWorldElement");
        
        foreach (IElementData elementData in dataList)
        {
            var worldElement = (ExEditorWorldElement)PoolManager.SpawnObject(prefab);

            SelectionElementManager.InitializeElement(  worldElement.EditorElement.DataElement, CameraManager.content,
                                                        DisplayManager,
                                                        DisplayManager.Display.SelectionType,
                                                        DisplayManager.Display.SelectionProperty);

            worldElement.EditorElement.DataElement.Data = dataController.Data;
            worldElement.EditorElement.DataElement.Id = elementData.Id;

            worldElement.EditorElement.DataElement.Path = DisplayManager.Display.DataController.SegmentController.Path;

            //Debugging
            worldElement.name = elementData.DebugName + elementData.Id;

            SetStatus(worldElement.EditorElement);

            if(worldElement.EditorElement.elementStatus == Enums.ElementStatus.Hidden)
            {
                PoolManager.ClosePoolObject(worldElement.EditorElement.DataElement.Poolable);
                SelectionElementManager.CloseElement(worldElement.EditorElement);

                continue;
            }

            //Must be assigned after the status check
            elementData.DataElement = worldElement.EditorElement.DataElement;

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
            case Enums.DataType.Phase:                  SetPartyStatus(element);                    break;
            
            default: Debug.Log("CASE MISSING: " + element.DataElement.ElementData.DataType); break;
        }
    }

    private void SetWorldObjectStatus(EditorElement element)
    {
        if (worldData.RegionType == Enums.RegionType.InteractionDestination || worldData.RegionType == Enums.RegionType.Party || worldData.RegionType == Enums.RegionType.Game)
        {
            element.elementStatus = Enums.ElementStatus.Locked;
            return;
        }
    }

    private void SetWorldInteractableStatus(EditorElement element)
    {
        var worldInteractableDataElement = (WorldInteractableElementData)element.DataElement.ElementData;

        //Locked when editing default party transform
        if (worldData.RegionType == Enums.RegionType.Party)
        {
            element.elementStatus = Enums.ElementStatus.Locked;
            return;
        }

        //Hidden: Interactables where the active time is not within its timeframe
        if (!worldInteractableDataElement.ContainsActiveTime)
        {
            element.elementStatus = Enums.ElementStatus.Hidden;
            return;
        }
    }

    private void SetInteractionDestinationStatus(EditorElement element)
    {
        var selectedInteractionDestination = selectedInteractionDestinations.FirstOrDefault();

        if (selectedInteractionDestination == null) return;

        var interactionDestinationDataElement = (InteractionDestinationElementData)element.DataElement.ElementData;

        //Locked when editing default party transform
        if (worldData.RegionType == Enums.RegionType.Party)
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
        if (!interactionDestinationDataElement.ContainsActiveTime)
        {
            element.elementStatus = Enums.ElementStatus.Hidden;
            return;
        }

        //Hidden: Interactions belonging to the same world interactable and objective
        if (interactionDestinationDataElement.Id != selectedInteractionDestination.Id &&
            interactionDestinationDataElement.WorldInteractableId == selectedInteractionDestination.WorldInteractableId &&
            interactionDestinationDataElement.ObjectiveId == selectedInteractionDestination.ObjectiveId)
        {
            element.elementStatus = Enums.ElementStatus.Hidden;
            return;
        }

        //Locked: Interactions that aren't selected and don't belong to a quest, only when an interaction with a quest is selected
        if (interactionDestinationDataElement.Id != selectedInteractionDestination.Id &&
            selectedInteractionDestination.ObjectiveId != 0 && interactionDestinationDataElement.ObjectiveId == 0)
        {
            element.elementStatus = Enums.ElementStatus.Locked;
            return;
        }

        //Enabled: Interactions belonging to a different interactable, but same objective
        if (interactionDestinationDataElement.WorldInteractableId != selectedInteractionDestination.WorldInteractableId &&
            interactionDestinationDataElement.ObjectiveId == selectedInteractionDestination.ObjectiveId)
        {
            element.elementStatus = Enums.ElementStatus.Enabled;
            return;
        }
        
        //Related: Interactions belonging to a different interactable, different objective but same quest
        if (interactionDestinationDataElement.WorldInteractableId != selectedInteractionDestination.WorldInteractableId &&
            interactionDestinationDataElement.QuestId == selectedInteractionDestination.QuestId)
        {
            element.elementStatus = Enums.ElementStatus.Related;
            return;
        }
        
        //Unrelated: Interactions belonging to a different quest
        if (interactionDestinationDataElement.QuestId > 0 &&
            interactionDestinationDataElement.QuestId != selectedInteractionDestination.QuestId)
        {
            element.elementStatus = Enums.ElementStatus.Unrelated;
            return;
        }
    }
    
    private void SetPartyStatus(EditorElement element)
    {
        var partyDataElement = (PhaseElementData)element.DataElement.ElementData;

        //Locked when not editing default party transform
        if (worldData.RegionType != Enums.RegionType.Party)
        {
            element.elementStatus = Enums.ElementStatus.Locked;
            return;
        }
    }

    private bool SetElement(EditorElement element)
    {
        element.gameObject.SetActive(true);
        
        element.DataElement.SetElement();

        InitializeStatusIconOverlay(element);

        element.SetOverlay();

        return true;
    }

    private void InitializeStatusIconOverlay(EditorElement element)
    {
        if (element.glow != null && element.glow.activeInHierarchy) return;

        var statusIconOverlay = CameraManager.overlayManager.StatusIconOverlay;

        if (element.elementStatus == Enums.ElementStatus.Locked)
            element.lockIcon = statusIconOverlay.StatusIcon(element, StatusIconOverlay.StatusIconType.Lock);

        if (element.DataElement.ElementData.SelectionStatus != Enums.SelectionStatus.None)
            element.glow = statusIconOverlay.StatusIcon(element, StatusIconOverlay.StatusIconType.Selection);
    }

    public void ResetData(List<IElementData> filter) { }
    
    public void ClearOrganizer()
    {
        dataSet = false;

        ClearSelectedElements();
        ClearTiles(tileList);        
    }

    private void ClearSelectedElements()
    {
        if(selectedWorldObjects.Count > 0)
        {
            selectedWorldObjects.ForEach(x => 
            {
                PoolManager.ClosePoolObject(x.DataElement.Poolable);
                SelectionElementManager.CloseElement(x.DataElement);
            });
        }
        
        if(selectedWorldInteractableAgents.Count > 0)
        {
            selectedWorldInteractableAgents.ForEach(x => 
            {
                PoolManager.ClosePoolObject(x.DataElement.Poolable);
                SelectionElementManager.CloseElement(x.DataElement);
            });
        }

        if (selectedWorldInteractableObjects.Count > 0)
        {
            selectedWorldInteractableObjects.ForEach(x => 
            {
                PoolManager.ClosePoolObject(x.DataElement.Poolable);
                SelectionElementManager.CloseElement(x.DataElement);
            });
        }

        if (selectedInteractionDestinations.Count > 0)
        {
            selectedInteractionDestinations.ForEach(x =>
            {
                PoolManager.ClosePoolObject(x.DataElement.Poolable);
                SelectionElementManager.CloseElement(x.DataElement);
            });
        }

        if(selectedParty.Count > 0)
        {
            selectedParty.ForEach(x =>
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
            ClearTileElements((TerrainTileElementData)x.ElementData);
            PoolManager.ClosePoolObject(x);
        });

        tileList.RemoveAll(x => inactiveTileList.Contains(x));
    }

    private void ClearTileElements(TerrainTileElementData terrainTileData)
    {
        ClearWorldObjects(terrainTileData);
        ClearWorldInteractableAgents(terrainTileData);
        ClearWorldInteractableObjects(terrainTileData);
        ClearInteractionDestinations(terrainTileData);
        ClearParty(terrainTileData);
    }

    private void ClearWorldObjects(TerrainTileElementData terrainTileData)
    {
        var worldObjectElementList = worldObjectController.Data.dataList.Cast<WorldObjectElementData>().Where(x => x.DataElement != null).ToList();
        
        var inactiveWorldObjectList = worldObjectElementList.Where(x => !selectedWorldObjects.Select(y => y.Id).Contains(x.Id) && 
                                                                   x.TerrainTileId == terrainTileData.Id).ToList();

        inactiveWorldObjectList.ForEach(x => 
        {
            PoolManager.ClosePoolObject(x.DataElement.Poolable);
            SelectionElementManager.CloseElement(x.DataElement);
        });
    }

    private void ClearWorldInteractableAgents(TerrainTileElementData terrainTileData)
    {
        var worldInteractableAgentElementList = worldInteractableAgentController.Data.dataList.Cast<WorldInteractableElementData>().Where(x => x.DataElement != null).ToList();

        var inactiveWorldInteractableAgentList = worldInteractableAgentElementList.Where(x => !selectedWorldInteractableAgents.Select(y => y.Id).Contains(x.Id) && 
                                                                                              x.TerrainTileId == terrainTileData.Id).ToList();

        inactiveWorldInteractableAgentList.ForEach(x => 
        {
            PoolManager.ClosePoolObject(x.DataElement.Poolable);
            SelectionElementManager.CloseElement(x.DataElement);
        });
    }

    private void ClearWorldInteractableObjects(TerrainTileElementData terrainTileData)
    {
        var worldInteractableObjectElementList = worldInteractableObjectController.Data.dataList.Cast<WorldInteractableElementData>().Where(x => x.DataElement != null).ToList();

        var inactiveWorldInteractableObjectList = worldInteractableObjectElementList.Where(x => !selectedWorldInteractableObjects.Select(y => y.Id).Contains(x.Id) && 
                                                                                                x.TerrainTileId == terrainTileData.Id).ToList();

        inactiveWorldInteractableObjectList.ForEach(x => 
        {
            PoolManager.ClosePoolObject(x.DataElement.Poolable);
            SelectionElementManager.CloseElement(x.DataElement);
        });
    }

    private void ClearInteractionDestinations(TerrainTileElementData terrainTileData)
    {
        var interactionDestinationElementList = interactionDestinationController.Data.dataList.Cast<InteractionDestinationElementData>().Where(x => x.DataElement != null).ToList();

        var inactiveInteractionDestinationList = interactionDestinationElementList.Where(x => !selectedInteractionDestinations.Select(y => y.Id).Contains(x.Id) && 
                                                                                         x.TerrainTileId == terrainTileData.Id).ToList();

        inactiveInteractionDestinationList.ForEach(x => 
        {
            PoolManager.ClosePoolObject(x.DataElement.Poolable);
            SelectionElementManager.CloseElement(x.DataElement);
        });
    }

    private void ClearParty(TerrainTileElementData terrainTileData)
    {
        var partyElementList = partyController.Data.dataList.Cast<PhaseElementData>().Where(x => x.DataElement != null).ToList();

        var inactivePartyList = partyElementList.Where(x => !selectedParty.Select(y => y.Id).Contains(x.Id) &&
                                                            x.TerrainTileId == terrainTileData.Id).ToList();

        inactivePartyList.ForEach(x =>
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
        if (localCameraPosition.z > 0)
            localCameraPosition = new Vector3(localCameraPosition.x, localCameraPosition.y, -1);
        if (localCameraPosition.z < -worldSize)
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

        DestroyImmediate(this);
    } 
}
