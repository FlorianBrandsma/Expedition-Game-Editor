using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class EditorWorldOrganizer : MonoBehaviour, IOrganizer
{
    private List<Tile> tileList = new List<Tile>();
    private List<ExEditorWorldElement> worldElementList = new List<ExEditorWorldElement>();
    
    public TerrainDataElement activeTerrainData;

    private List<WorldObjectDataElement> selectedWorldObjects                   = new List<WorldObjectDataElement>();
    private List<WorldInteractableDataElement> selectedWorldInteractableAgents  = new List<WorldInteractableDataElement>();
    private List<WorldInteractableDataElement> selectedWorldInteractableObjects = new List<WorldInteractableDataElement>();
    private List<InteractionDataElement> selectedInteractions                   = new List<InteractionDataElement>();
    
    private List<IDataElement> selectableDataList;
    
    private EditorWorldDataElement worldData;

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
    private DataController interactionController                = new DataController(Enums.DataType.Interaction);
    
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
            
            if (allowSelection && Physics.Raycast(ray, out hit) && hit.collider.GetComponent<ObjectGraphic>() != null)
            {
                if (GameObject.Find("Dropdown List") != null) return;
                
                var selectionElement = hit.collider.GetComponent<ObjectGraphic>().selectionElement;

                selectionElement.InvokeSelection();
            }

            allowSelection = true;
        }
    }

    public void InitializeOrganizer()
    {
        worldData = (EditorWorldDataElement)DataController.DataList.FirstOrDefault();

        //Get selected interaction
        GetSelectedElement(DataController.SegmentController.MainPath, Enums.DataType.Interaction);

        //Only get other data if no interaction was selected. Technically both could be selected,
        //but this is never the case and so the world object doesn't have to stay active
        if (selectedInteractions.FirstOrDefault() == null)
        {
            //Get selected interactable agents
            GetSelectedElement(DataController.SegmentController.MainPath, Enums.DataType.WorldInteractable, Enums.InteractableType.Agent);

            //Get selected interactable objects
            GetSelectedElement(DataController.SegmentController.MainPath, Enums.DataType.WorldInteractable, Enums.InteractableType.Object);

            //Get selected world objects
            GetSelectedElement(DataController.SegmentController.MainPath, Enums.DataType.WorldObject);
        }

        InitializeControllers();
        
        SetWorldSize();

        CameraManager.cam.transform.localPosition = new Vector3(0, 10, -(worldData.tileSize * 0.5f));
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
            case Enums.DataType.WorldObject:        GetSelectedWorldObjects(routes);                            break;
            case Enums.DataType.WorldInteractable:  GetSelectedWorldInteractables(routes, interactableType);    break;
            case Enums.DataType.Interaction:        GetSelectedInteractions(routes);                            break; 
        }
    }

    private void GetSelectedWorldObjects(List<Route> routes)
    {
        selectedWorldObjects = worldData.terrainDataList.SelectMany(x => x.worldObjectDataList.Where(y => routes.Select(z => z.GeneralData.Id).Contains(y.Id))).Distinct().ToList();
    }

    private void GetSelectedWorldInteractables(List<Route> routes, Enums.InteractableType interactableType)
    {
        if (interactableType == Enums.InteractableType.Agent)
        {
            var selectedWorldInteractables = worldData.terrainDataList.SelectMany(x => x.worldInteractableDataList.Where(y => y.Type == (int)Enums.InteractableType.Agent).Where(y => routes.Select(z => z.GeneralData.Id).Contains(y.Id))).Cast<IDataElement>().ToList();
            selectedWorldInteractableAgents = FilterWorldInteractables(selectedWorldInteractables).Cast<WorldInteractableDataElement>().ToList();
        }
        
        if (interactableType == Enums.InteractableType.Object)
        {
            var selectedWorldInteractables = worldData.terrainDataList.SelectMany(x => x.worldInteractableDataList.Where(y => y.Type == (int)Enums.InteractableType.Object).Where(y => routes.Select(z => z.GeneralData.Id).Contains(y.Id))).Cast<IDataElement>().ToList();
            selectedWorldInteractableObjects = FilterWorldInteractables(selectedWorldInteractables).Cast<WorldInteractableDataElement>().ToList();
        }
    }

    private void GetSelectedInteractions(List<Route> routes)
    {
        selectedInteractions = worldData.terrainDataList.SelectMany(x => x.interactionDataList.Where(y => routes.Select(z => z.GeneralData.Id).Contains(y.Id))).Distinct().ToList();
    }

    private void InitializeControllers()
    {
        worldObjectController.SegmentController             = DataController.SegmentController;
        worldInteractableAgentController.SegmentController  = DataController.SegmentController;
        worldInteractableObjectController.SegmentController = DataController.SegmentController;
        interactionController.SegmentController             = DataController.SegmentController;
        
        worldInteractableAgentController.DataCategory   = Enums.DataCategory.Navigation;
        worldInteractableObjectController.DataCategory  = Enums.DataCategory.Navigation;
        interactionController.DataCategory              = Enums.DataCategory.Navigation;

        GetData();
    }

    private void GetData()
    {
        FixLostInteractions();
        FixLostWorldObjects();
        
        //Confirm which atmosphere's timeframes contain the active time
        ValidateAtmosphereTime();

        //Confirm which interactions's timeframes contain the active time
        ValidateInteractionsTime();

        //Confirm which world interactable's timeframes contain the active time
        ValidateWorldInteractablesTime();

        worldObjectController.DataList = worldData.terrainDataList.SelectMany(x => x.worldObjectDataList.Where(y => !selectedWorldObjects.Select(z => z.Id).Contains(y.Id))).Cast<IDataElement>().ToList();

        worldInteractableAgentController.DataList = worldData.terrainDataList.SelectMany(x => x.worldInteractableDataList.Where(y => y.Type == (int)Enums.InteractableType.Agent &&
                                                                                              !selectedWorldInteractableAgents.Select(z => z.Id).Contains(y.Id))).Cast<IDataElement>().ToList();

        worldInteractableObjectController.DataList = worldData.terrainDataList.SelectMany(x => x.worldInteractableDataList.Where(y => y.Type == (int)Enums.InteractableType.Object &&
                                                                                               !selectedWorldInteractableObjects.Select(z => z.Id).Contains(y.Id))).Cast<IDataElement>().ToList();

        interactionController.DataList = worldData.terrainDataList.SelectMany(x => x.interactionDataList.Where(y => !selectedInteractions.Select(z => z.Id).Contains(y.Id))).Cast<IDataElement>().ToList();
    }

    private void FixLostInteractions()
    {
        var lostInteractions = worldData.terrainDataList.SelectMany(x => x.interactionDataList.Where(y => y.TerrainId != x.Id)).ToList();

        worldData.terrainDataList.ForEach(x => x.interactionDataList.RemoveAll(y => lostInteractions.Select(z => z.TerrainId).Contains(y.TerrainId)));
        worldData.terrainDataList.ForEach(x => x.interactionDataList.AddRange(lostInteractions.Where(y => y.TerrainId == x.Id)));
    }

    private void FixLostWorldObjects()
    {
        var lostWorldObjects = worldData.terrainDataList.SelectMany(x => x.worldObjectDataList.Where(y => y.TerrainId != 0 && y.TerrainId != x.Id)).ToList();

        worldData.terrainDataList.ForEach(x => x.worldObjectDataList.RemoveAll(y => lostWorldObjects.Select(z => z.TerrainId).Contains(y.TerrainId)));
        worldData.terrainDataList.ForEach(x => x.worldObjectDataList.AddRange(lostWorldObjects.Where(y => y.TerrainId == x.Id)));
    }

    private void ValidateAtmosphereTime()
    {
        worldData.terrainDataList.ForEach(x => x.atmosphereDataList.ForEach(y => y.containsActiveTime = false));

        worldData.terrainDataList.SelectMany(x => x.atmosphereDataList.GroupBy(y => y.TerrainId)
                                                                      .Select(y => y.Where(z => TimeManager.TimeInFrame(TimeManager.activeTime, z.StartTime, z.EndTime) || z.Default)
                                                                      .OrderBy(z => z.Default).First())).ToList()
                                                                      .ForEach(x => x.containsActiveTime = true);
    }

    private void ValidateInteractionsTime()
    {
        worldData.terrainDataList.ForEach(x => x.interactionDataList.ForEach(y => y.containsActiveTime = false));

        worldData.terrainDataList.SelectMany(x => x.interactionDataList.GroupBy(y => y.TaskId)
                                                                       .Select(y => y.Where(z => TimeManager.TimeInFrame(TimeManager.activeTime, z.StartTime, z.EndTime) || z.Default)
                                                                       .OrderBy(z => z.Default).First())).ToList()
                                                                       .ForEach(x => x.containsActiveTime = true);
    }

    private void ValidateWorldInteractablesTime()
    {
        worldData.terrainDataList.ForEach(x => x.worldInteractableDataList.ForEach(y => y.containsActiveTime = false));

        worldData.terrainDataList.SelectMany(x => x.worldInteractableDataList.GroupBy(y => y.taskGroup)
                                                                             .Select(y => y.Where(z => TimeManager.TimeInFrame(TimeManager.activeTime, z.startTime, z.endTime) || z.isDefault)
                                                                             .OrderBy(z => z.isDefault).First())).ToList()
                                                                             .ForEach(x => x.containsActiveTime = true);
    }
    
    public void SelectData()
    {
        selectableDataList =    worldData.terrainDataList.SelectMany(x => x.worldObjectDataList).Cast<IDataElement>().Concat(
                                worldData.terrainDataList.SelectMany(x => x.worldInteractableDataList).Cast<IDataElement>()).Concat(
                                worldData.terrainDataList.SelectMany(x => x.interactionDataList).Cast<IDataElement>()).ToList();

        SelectionManager.SelectData(selectableDataList, DisplayManager);
    }

    private void SetWorldSize()
    {
        worldSize = (worldData.regionSize * worldData.terrainSize * worldData.tileSize);

        ScrollRect.content.sizeDelta = new Vector2(worldSize, worldSize) * 2;
        CameraManager.content.sizeDelta = new Vector2(worldSize, worldSize);
    }
    
    public void UpdateData()
    {
        SetCameraPosition();

        if (ScrollRect.content.localPosition.x >= positionTracker.x + worldData.tileSize ||
            ScrollRect.content.localPosition.x <= positionTracker.x - worldData.tileSize ||
            ScrollRect.content.localPosition.y >= positionTracker.y + worldData.tileSize ||
            ScrollRect.content.localPosition.y <= positionTracker.y - worldData.tileSize)
        {
            CloseInactiveElements();
            
            SetData(DataController.DataList);

            dataSet = true;
        }
    }

    public void SetCameraPosition()
    {
        var worldSize = worldData.regionSize * worldData.terrainSize * worldData.tileSize;

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
        var inactiveTiles = tileList.Where(x => !activeRect.Overlaps(((TerrainTileDataElement)x.DataElement).gridElement.rect, true)).ToList();
        ClearTiles(inactiveTiles);

        dataSet = false;
    }
    
    public void SetData()
    {
        positionTracker = FixTrackerPosition(ScrollRect.content.localPosition);

        //Selected elements only spawn once on start up
        if (selectedInteractions.FirstOrDefault() != null)
        {
            SetWorldElements(interactionController, selectedInteractions.Cast<IDataElement>().ToList());

        } else {

            //Only show interactables where the active time is within their first interaction's timeframe
            worldInteractableAgentController.DataList = FilterWorldInteractables(worldInteractableAgentController.DataList);
            worldInteractableObjectController.DataList = FilterWorldInteractables(worldInteractableObjectController.DataList);

            SetWorldElements(worldInteractableAgentController, selectedWorldInteractableAgents.Cast<IDataElement>().ToList());
            SetWorldElements(worldInteractableObjectController, selectedWorldInteractableObjects.Cast<IDataElement>().ToList());
            SetWorldElements(worldObjectController, selectedWorldObjects.Cast<IDataElement>().ToList());
        }

        SetData(DataController.DataList);
    }

    private Vector2 FixTrackerPosition(Vector2 cameraPosition)
    {
        return new Vector2(Mathf.Floor((cameraPosition.x + (worldData.tileSize / 2)) / worldData.tileSize) * worldData.tileSize,
                           Mathf.Floor((cameraPosition.y + (worldData.tileSize / 2)) / worldData.tileSize) * worldData.tileSize);
    }

    private List<IDataElement> FilterWorldInteractables(List<IDataElement> dataList)
    {
        var worldInteractableDataList = dataList.Where(x => ((WorldInteractableDataElement)x).containsActiveTime).GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();

        return worldInteractableDataList;
    }
    
    private void SetData(List<IDataElement> list)
    {
        if (dataSet) return;
        
        foreach (TerrainDataElement terrainData in worldData.terrainDataList)
        {
            SetTerrain(terrainData);
        }
        
        //Set elements that are not bound to a tile
        SetWorldObjects();
        
        GetActiveTerrain();
    }
    
    private void SetTerrain(TerrainDataElement terrainData)
    {
        if (!activeRect.Overlaps(terrainData.gridElement.rect, true)) return;

        foreach (TerrainTileDataElement terrainTileData in terrainData.terrainTileDataList)
        {
            if (terrainTileData.active || !activeRect.Overlaps(terrainTileData.gridElement.rect, true)) continue;

            SetTerrainTile(terrainTileData);

            //Set objects that are bound to this terrain tile
            SetWorldObjects(terrainTileData.Id);

            //Set world interactable agents that are bound to this tile by their first interaction
            SetWorldInteractableAgents(terrainTileData.Id);

            //Set world interactable objects that are bound to this tile by their first interaction
            SetWorldInteractableObjects(terrainTileData.Id);

            //Set interactions that are bound to this terrain tile
            SetInteractions(terrainTileData.Id);
        }
    }
    
    private void SetTerrainTile(TerrainTileDataElement terrainTileData)
    {
        Tile prefab = Resources.Load<Tile>("Objects/Tile/" + worldData.tileSetName + "/" + terrainTileData.TileId);

        Tile tile = (Tile)PoolManager.SpawnObject(prefab, terrainTileData.TileId);
        tileList.Add(tile);

        tile.gameObject.SetActive(true);

        tile.transform.SetParent(CameraManager.content, false);
        tile.transform.localPosition = new Vector3(terrainTileData.gridElement.startPosition.x, 0, terrainTileData.gridElement.startPosition.y);

        tile.DataType = Enums.DataType.TerrainTile;
        tile.DataElement = terrainTileData;

        terrainTileData.active = true;
    }

    private void SetWorldObjects(int terrainTileId = 0)
    {
        var worldObjectDataList = worldObjectController.DataList.Where(x => ((WorldObjectDataElement)x).TerrainTileId == terrainTileId).ToList();

        SetWorldElements(worldObjectController, worldObjectDataList);
    }

    private void SetWorldInteractableAgents(int terrainTileId)
    {
        var worldInteractableAgentDataList = worldInteractableAgentController.DataList.Where(x => ((WorldInteractableDataElement)x).terrainTileId == terrainTileId).ToList();
        
        SetWorldElements(worldInteractableAgentController, worldInteractableAgentDataList);
    }

    private void SetWorldInteractableObjects(int terrainTileId)
    {
        var worldInteractableObjectDataList = worldInteractableObjectController.DataList.Where(x => ((WorldInteractableDataElement)x).terrainTileId == terrainTileId).ToList();

        SetWorldElements(worldInteractableObjectController, worldInteractableObjectDataList);
    }

    private void SetInteractions(int terrainTileId)
    {
        var interactionDataList = interactionController.DataList.Where(x => ((InteractionDataElement)x).TerrainTileId == terrainTileId).ToList();

        SetWorldElements(interactionController, interactionDataList);
    }

    private void SetWorldElements(IDataController dataController, List<IDataElement> dataList)
    {
        if (dataList.Count == 0) return;
        
        var prefab = Resources.Load<ExEditorWorldElement>("Elements/World/EditorWorldElement");
        
        foreach (IDataElement dataElement in dataList)
        {
            var worldElement = (ExEditorWorldElement)PoolManager.SpawnObject(prefab);

            SelectionElementManager.InitializeElement(  worldElement.Element, CameraManager.content,
                                                        DisplayManager,
                                                        DisplayManager.Display.SelectionType,
                                                        DisplayManager.Display.SelectionProperty);
            worldElementList.Add(worldElement);
            
            worldElement.Element.data.dataController = dataController;
            worldElement.Element.data = new SelectionElement.Data(dataController, dataElement);

            //Debugging
            GeneralData generalData = (GeneralData)dataElement;
            worldElement.name = generalData.DebugName + generalData.Id;
            //

            SetStatus(worldElement.Element);

            if(worldElement.Element.elementStatus == Enums.ElementStatus.Hidden)
            {
                PoolManager.ClosePoolObject(worldElement.Element.Poolable);
                SelectionElementManager.CloseElement(worldElement.Element);

                continue;
            }

            dataElement.SelectionElement = worldElement.Element;

            SetElement(worldElement.Element);
        }
    }

    private void SetStatus(SelectionElement element)
    {
        switch (element.GeneralData.DataType)
        {
            case Enums.DataType.WorldObject:        SetWorldObjectStatus(element);          break;
            case Enums.DataType.WorldInteractable:  SetWorldInteractableStatus(element);    break;
            case Enums.DataType.Interaction:        SetInteractionStatus(element);          break;
            
            default: Debug.Log("CASE MISSING: " + element.GeneralData.DataType); break;
        }
    }

    private void SetWorldObjectStatus(SelectionElement element)
    {
        if (worldData.regionType == Enums.RegionType.Interaction || worldData.regionType == Enums.RegionType.Game)
        {
            element.elementStatus = Enums.ElementStatus.Locked;
            return;
        }
    }

    private void SetWorldInteractableStatus(SelectionElement element)
    {
        var worldInteractableDataElement = (WorldInteractableDataElement)element.data.dataElement;

        //Hidden: Interactables where the active time is not within its timeframe
        if (!worldInteractableDataElement.containsActiveTime)
        {
            element.elementStatus = Enums.ElementStatus.Hidden;
            return;
        }
    }

    private void SetInteractionStatus(SelectionElement element)
    {
        var selectedInteraction = selectedInteractions.FirstOrDefault();

        if (selectedInteraction == null) return;

        var interactionDataElement = (InteractionDataElement)element.data.dataElement;

        //Debug.Log(  interactionData.Id + "/" + interactionDataElement.Id + "   " +
        //            interactionData.TaskId + "/" + interactionDataElement.TaskId + "   " +
        //            interactionData.worldInteractableId + "/" + interactionDataElement.worldInteractableId + "   " +
        //            interactionData.questId + "/" + interactionDataElement.questId + "   " +
        //            interactionData.objectiveId + "/" + interactionDataElement.objectiveId);

        //Hidden: Interactions where the active time is not within its timeframe
        if (!interactionDataElement.containsActiveTime)
        {
            element.elementStatus = Enums.ElementStatus.Hidden;
            return;
        }

        //Hidden: Interactions belonging to the same world interactable and objective
        if (interactionDataElement.Id != selectedInteraction.Id &&
            interactionDataElement.worldInteractableId == selectedInteraction.worldInteractableId &&
            interactionDataElement.objectiveId == selectedInteraction.objectiveId)
        {
            element.elementStatus = Enums.ElementStatus.Hidden;
            return;
        }

        //Locked: Interactions that aren't selected and don't belong to a quest, only when an interaction with a quest is selected
        if (interactionDataElement.Id != selectedInteraction.Id &&
            selectedInteraction.objectiveId != 0 && interactionDataElement.objectiveId == 0)
        {
            element.elementStatus = Enums.ElementStatus.Locked;
            return;
        }

        //Enabled: Interactions belonging to a different interactable, but same objective
        if (interactionDataElement.worldInteractableId != selectedInteraction.worldInteractableId &&
            interactionDataElement.objectiveId == selectedInteraction.objectiveId)
        {
            element.elementStatus = Enums.ElementStatus.Enabled;
            return;
        }
        
        //Related: Interactions belonging to a different interactable, different objective but same quest
        if (interactionDataElement.worldInteractableId != selectedInteraction.worldInteractableId &&
            interactionDataElement.questId == selectedInteraction.questId)
        {
            element.elementStatus = Enums.ElementStatus.Related;
            return;
        }
        
        //Unrelated: Interactions belonging to a different quest
        if (interactionDataElement.questId > 0 &&
            interactionDataElement.questId != selectedInteraction.questId)
        {
            element.elementStatus = Enums.ElementStatus.Unrelated;
            return;
        }
    }
    
    private bool SetElement(SelectionElement element)
    {
        element.gameObject.SetActive(true);
        
        element.SetElement();

        InitializeStatusIconOverlay(element);

        element.SetOverlay();

        return true;
    }

    private void InitializeStatusIconOverlay(SelectionElement element)
    {
        if (element.glow != null && element.glow.activeInHierarchy) return;

        var statusIconManager = CameraManager.overlayManager.GetComponent<StatusIconOverlay>();

        if (element.elementStatus == Enums.ElementStatus.Locked)
            element.lockIcon = statusIconManager.StatusIcon(element, StatusIconOverlay.StatusIconType.Lock);

        if (element.data.dataElement.SelectionStatus != Enums.SelectionStatus.None)
            element.glow = statusIconManager.StatusIcon(element, StatusIconOverlay.StatusIconType.Selection);
    }

    public void ResetData(List<IDataElement> filter) { }
    
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
                PoolManager.ClosePoolObject(x.SelectionElement.Poolable);
                SelectionElementManager.CloseElement(x.SelectionElement);
            });

            worldElementList.RemoveAll(x => selectedWorldObjects.Select(y => y.Id).Contains(x.Id));
        }
        
        if(selectedWorldInteractableAgents.Count > 0)
        {
            selectedWorldInteractableAgents.ForEach(x => 
            {
                PoolManager.ClosePoolObject(x.SelectionElement.Poolable);
                SelectionElementManager.CloseElement(x.SelectionElement);
            });

            worldElementList.RemoveAll(x => selectedWorldInteractableAgents.Select(y => y.Id).Contains(x.Id));
        }

        if (selectedWorldInteractableObjects.Count > 0)
        {
            selectedWorldInteractableObjects.ForEach(x => 
            {
                PoolManager.ClosePoolObject(x.SelectionElement.Poolable);
                SelectionElementManager.CloseElement(x.SelectionElement);
            });

            worldElementList.RemoveAll(x => selectedWorldInteractableObjects.Select(y => y.Id).Contains(x.Id));
        }

        if (selectedInteractions.Count > 0)
        {
            selectedInteractions.ForEach(x =>
            {
                PoolManager.ClosePoolObject(x.SelectionElement.Poolable);
                SelectionElementManager.CloseElement(x.SelectionElement);
            });
            
            worldElementList.RemoveAll(x => selectedInteractions.Select(y => y.Id).Contains(x.Id));
        }
    }

    private void ClearTiles(List<Tile> inactiveTileList)
    {
        inactiveTileList.ForEach(x => 
        {
            ClearTileElements((TerrainTileDataElement)x.DataElement);
            PoolManager.ClosePoolObject(x);
        });

        tileList.RemoveAll(x => inactiveTileList.Contains(x));
    }

    private void ClearTileElements(TerrainTileDataElement terrainTileData)
    {
        ClearWorldObjects(terrainTileData);
        ClearWorldInteractableAgents(terrainTileData);
        ClearWorldInteractableObjects(terrainTileData);
        ClearInteractions(terrainTileData);
    }

    private void ClearWorldObjects(TerrainTileDataElement terrainTileData)
    {
        var worldObjectElementList = worldObjectController.DataList.Cast<WorldObjectDataElement>().Where(x => x.SelectionElement != null).ToList();
        
        var inactiveWorldObjectList = worldObjectElementList.Where(x => !selectedWorldObjects.Select(y => y.Id).Contains(x.Id) && 
                                                                   x.TerrainTileId == terrainTileData.Id).ToList();

        inactiveWorldObjectList.ForEach(x => 
        {
            PoolManager.ClosePoolObject(x.SelectionElement.Poolable);
            SelectionElementManager.CloseElement(x.SelectionElement);
        });
        
        worldElementList.RemoveAll(x => inactiveWorldObjectList.Contains(x.Element.data.dataElement));
    }

    private void ClearWorldInteractableAgents(TerrainTileDataElement terrainTileData)
    {
        var worldInteractableAgentElementList = worldInteractableAgentController.DataList.Cast<WorldInteractableDataElement>().Where(x => x.SelectionElement != null).ToList();

        var inactiveWorldInteractableAgentList = worldInteractableAgentElementList.Where(x => !selectedWorldInteractableAgents.Select(y => y.Id).Contains(x.Id) && 
                                                                                              x.terrainTileId == terrainTileData.Id).ToList();

        inactiveWorldInteractableAgentList.ForEach(x => 
        {
            PoolManager.ClosePoolObject(x.SelectionElement.Poolable);
            SelectionElementManager.CloseElement(x.SelectionElement);
        });

        worldElementList.RemoveAll(x => inactiveWorldInteractableAgentList.Contains(x.Element.data.dataElement));
    }

    private void ClearWorldInteractableObjects(TerrainTileDataElement terrainTileData)
    {
        var worldInteractableObjectElementList = worldInteractableObjectController.DataList.Cast<WorldInteractableDataElement>().Where(x => x.SelectionElement != null).ToList();

        var inactiveWorldInteractableObjectList = worldInteractableObjectElementList.Where(x => !selectedWorldInteractableObjects.Select(y => y.Id).Contains(x.Id) && 
                                                                                                x.terrainTileId == terrainTileData.Id).ToList();

        inactiveWorldInteractableObjectList.ForEach(x => 
        {
            PoolManager.ClosePoolObject(x.SelectionElement.Poolable);
            SelectionElementManager.CloseElement(x.SelectionElement);
        });
        
        worldElementList.RemoveAll(x => inactiveWorldInteractableObjectList.Contains(x.Element.data.dataElement));
    }

    private void ClearInteractions(TerrainTileDataElement terrainTileData)
    {
        var interactionElementList = interactionController.DataList.Cast<InteractionDataElement>().Where(x => x.SelectionElement != null).ToList();

        var inactiveInteractionList = interactionElementList.Where(x => !selectedInteractions.Select(y => y.Id).Contains(x.Id) && 
                                                                        x.TerrainTileId == terrainTileData.Id).ToList();

        inactiveInteractionList.ForEach(x => 
        {
            PoolManager.ClosePoolObject(x.SelectionElement.Poolable);
            SelectionElementManager.CloseElement(x.SelectionElement);
        });
        
        worldElementList.RemoveAll(x => inactiveInteractionList.Contains(x.Element.data.dataElement));
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

    private TerrainDataElement GetTerrain(Vector3 cameraPosition)
    {
        var terrainSize = worldData.terrainSize * worldData.tileSize;

        var terrainCoordinates = new Vector3(Mathf.Floor(cameraPosition.x   / terrainSize),
                                             Mathf.Floor(cameraPosition.y   / terrainSize),
                                             Mathf.Floor(-cameraPosition.z  / terrainSize));

        var terrainIndex = (worldData.regionSize * terrainCoordinates.z) + terrainCoordinates.x;
        
        return worldData.terrainDataList.Where(x => x.Index == terrainIndex).FirstOrDefault();
    }

    public void CloseOrganizer()
    {
        CancelSelection();

        DestroyImmediate(this);
    } 
}
