using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class EditorWorldOrganizer : MonoBehaviour, IOrganizer
{
    private List<Tile> tileList = new List<Tile>();
    private List<SelectionElement> elementList = new List<SelectionElement>();

    private Vector3 tileBoundSize;
    
    private Plane[] planes;

    public TerrainDataElement activeTerrainData;

    private List<WorldObjectDataElement> worldObjectData                = new List<WorldObjectDataElement>();
    private List<WorldInteractableDataElement> worldInteractableData    = new List<WorldInteractableDataElement>();
    private InteractionDataElement interactionData;
    
    private List<IDataElement> dataList;
    
    private EditorWorldDataElement worldData;

    private Vector2 worldStartPosition;
    private Vector2 positionTracker = new Vector2();

    private IDisplayManager DisplayManager      { get { return GetComponent<IDisplayManager>(); } }
    private CameraManager CameraManager         { get { return (CameraManager)DisplayManager; } }
    
    private CameraProperties CameraProperties   { get { return (CameraProperties)DisplayManager.Display; } }
    private EditorWorldProperties WorldProperties     { get { return (EditorWorldProperties)DisplayManager.Display.Properties; } }

    private IDataController DataController      { get { return CameraManager.Display.DataController; } }

    private DataController worldObjectController                = new DataController(Enums.DataType.WorldObject);
    private DataController worldInteractableCharacterController = new DataController(Enums.DataType.WorldInteractable);
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
        InitializeControllers();

        worldData = (EditorWorldDataElement)DataController.DataList.FirstOrDefault();

        SetRegionSize();

        GetSelectedElement(DataController.SegmentController.MainPath, Enums.DataType.Interaction);

        //Only get other data if no interaction was selected. Technically both could be selected,
        //but this is never the case and so the world object doesn't have to stay active
        if (interactionData == null)
        {
            GetSelectedElement(DataController.SegmentController.MainPath, Enums.DataType.WorldInteractable);
            GetSelectedElement(DataController.SegmentController.MainPath, Enums.DataType.WorldObject);
        }
        
        tileBoundSize = new Vector3(worldData.tileSize, worldData.tileSize, 0) * 3;
        
        CameraManager.cam.transform.localPosition = new Vector3(0, 10, -(worldData.tileSize * 0.5f));
    }

    private void GetSelectedElement(Path path, Enums.DataType dataType)
    {
        //Get selected elements from all paths
        var paths = RenderManager.layoutManager.forms.Select(x => x.activePath).ToList();
        var routes = paths.Select(x => x.FindLastRoute(dataType)).Where(x => x != null).ToList();
        
        if (routes == null) return;

        switch (dataType)
        {
            case Enums.DataType.WorldObject:

                worldObjectData = worldData.terrainDataList.SelectMany(x => x.worldObjectDataList.Where(y => routes.Select(z => z.GeneralData.Id).Contains(y.Id))).Distinct().ToList();

                break;

            case Enums.DataType.WorldInteractable:
                
                worldInteractableData = worldData.terrainDataList.SelectMany(x => x.worldInteractableDataList.Where(y => routes.Select(z => z.GeneralData.Id).Contains(y.Id))).Distinct().ToList();

                break;

            case Enums.DataType.Interaction:
                
                interactionData = worldData.terrainDataList.SelectMany(x => x.interactionDataList.Where(y => routes.Select(z => z.GeneralData.Id).Contains(y.Id))).FirstOrDefault();

                break; 
        }
    }

    private void InitializeControllers()
    {
        worldObjectController.SegmentController                 = DataController.SegmentController;
        worldInteractableCharacterController.SegmentController  = DataController.SegmentController;
        worldInteractableObjectController.SegmentController     = DataController.SegmentController;
        interactionController.SegmentController                 = DataController.SegmentController;
        
        worldInteractableCharacterController.DataCategory   = Enums.DataCategory.Navigation;
        worldInteractableObjectController.DataCategory      = Enums.DataCategory.Navigation;
        interactionController.DataCategory                  = Enums.DataCategory.Navigation;
    }

    public void SelectData()
    {
        dataList =  worldData.terrainDataList.SelectMany(x => x.worldObjectDataList).Cast<IDataElement>().Concat(
                    worldData.terrainDataList.SelectMany(x => x.worldInteractableDataList).Cast<IDataElement>()).Concat(
                    worldData.terrainDataList.SelectMany(x => x.interactionDataList).Cast<IDataElement>()).ToList();

        SelectionManager.SelectData(dataList, DisplayManager);
    }

    private void SetRegionSize()
    {
        var regionSize = new Vector2(worldData.regionSize * worldData.terrainSize * worldData.tileSize,
                                     worldData.regionSize * worldData.terrainSize * worldData.tileSize);

        ScrollRect.content.sizeDelta = regionSize * 2;
        CameraManager.content.sizeDelta = regionSize;
    }
    
    public void UpdateData()
    {
        SetCameraPosition();

        if (ScrollRect.content.localPosition.x >= positionTracker.x + worldData.tileSize ||
            ScrollRect.content.localPosition.x <= positionTracker.x - worldData.tileSize ||
            ScrollRect.content.localPosition.y >= positionTracker.y + worldData.tileSize ||
            ScrollRect.content.localPosition.y <= positionTracker.y - worldData.tileSize)
        {
            ClearOrganizer();

            worldObjectController.DataList.Clear();
            worldInteractableCharacterController.DataList.Clear();
            worldInteractableObjectController.DataList.Clear();
            interactionController.DataList.Clear();
            
            SetData();

            dataSet = true;
        }
    }

    public void SetCameraPosition()
    {
        CameraManager.cameraParent.transform.localPosition = new Vector3(CameraManager.ScrollRectContent.localPosition.x,
                                                                         CameraManager.cameraParent.transform.localPosition.y,
                                                                         CameraManager.ScrollRectContent.localPosition.y);
    }

    private Vector2 FixTrackerPosition(Vector2 cameraPosition)
    {
        return new Vector2( Mathf.Floor((cameraPosition.x + (worldData.tileSize / 2)) / worldData.tileSize) * worldData.tileSize,
                            Mathf.Floor((cameraPosition.y + (worldData.tileSize / 2)) / worldData.tileSize) * worldData.tileSize);
    }

    public void SetData()
    {
        positionTracker = FixTrackerPosition(ScrollRect.content.localPosition);

        FixLostInteractions();
        FixLostWorldObjects();

        SetData(DataController.DataList);
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
    
    private void SetData(List<IDataElement> list)
    {
        if (dataSet) return;

        planes = GeometryUtility.CalculateFrustumPlanes(CameraManager.cam);

        worldStartPosition = worldData.startPosition;

        //Confirm which atmosphere's timeframes contain the active time
        ValidateAtmosphereTime();

        foreach (TerrainDataElement terrainData in worldData.terrainDataList)
        {
            SetTerrain(terrainData);

            worldObjectController.DataList.AddRange(terrainData.worldObjectDataList.Where(x => x.TerrainTileId == 0 || worldObjectData.Select(y => y.Id).Contains(x.Id)).Cast<IDataElement>());
            
            worldInteractableCharacterController.DataList.AddRange(terrainData.worldInteractableDataList.Where(x => x.Type == (int)Enums.InteractableType.Agent)
                                                                                                        .Where(x => x.terrainTileId == 0 || worldInteractableData.Select(y => y.Id).Contains(x.Id)).Cast<IDataElement>());

            worldInteractableCharacterController.DataList.AddRange(terrainData.worldInteractableDataList.Where(x => x.Type == (int)Enums.InteractableType.Object)
                                                                                                        .Where(x => x.terrainTileId == 0 || worldInteractableData.Select(y => y.Id).Contains(x.Id)).Cast<IDataElement>());

            interactionController.DataList.AddRange(terrainData.interactionDataList.Where(x => x.TerrainTileId == 0).Cast<IDataElement>());
        }
        
        if (interactionData != null)
        {
            //Confirm which interactions's timeframes contain the active time
            ValidateInteractionsTime();

            //Extract interactions that will be obtained by the region navigation dropdown and also maintains selection focus
            ExtractInteractions();

        } else {
            
            //There are no world interactables to validate when there is an interaction
            ValidateWorldInteractablesTime();

            //Only show interactables where the active time is within their first interaction's timeframe
            FilterWorldInteractables();
        }
        
        //Set objects that are bound to this terrain tile
        SetWorldElements(worldObjectController);

        //Set world interactables that are bound to this tile by their first interaction
        SetWorldElements(worldInteractableCharacterController);

        SetWorldElements(worldInteractableObjectController);

        //Set interactions that are bound to this terrain tile
        SetWorldElements(interactionController);
        
        GetActiveTerrain();
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

    private void FilterWorldInteractables()
    {
        var characterList = worldInteractableCharacterController.DataList.Where(x => ((WorldInteractableDataElement)x).containsActiveTime).GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();
        worldInteractableCharacterController.DataList = characterList;

        var objectList = worldInteractableObjectController.DataList.Where(x => ((WorldInteractableDataElement)x).containsActiveTime).GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();
        worldInteractableObjectController.DataList = objectList;
    }

    private void ExtractInteractions()
    {
        var interactionList = worldData.terrainDataList.SelectMany(x => x.interactionDataList
                                                       .Where(y => y.objectiveId == interactionData.objectiveId && 
                                                                   y.worldInteractableId == interactionData.worldInteractableId &&
                                                                   y.TaskId == interactionData.TaskId))
                                                       .OrderBy(x => x.Index).Cast<IDataElement>().ToList();

        interactionController.DataList.RemoveAll(x => interactionList.Contains(x));

        var dataElement = interactionList.Where(x => x.Id == interactionData.Id).FirstOrDefault();

        DataController.SegmentController.Path.ReplaceDataLists(0, Enums.DataType.Interaction, interactionList, dataElement);

        SetWorldElements(DataController.SegmentController.Path.FindLastRoute(Enums.DataType.Interaction).data.dataController);
    }

    private void SetTerrain(TerrainDataElement terrainData)
    {
        var terrainStartPosition = new Vector2( (worldStartPosition.x + worldData.tileSize / 2) + ((terrainData.Index % worldData.regionSize) * (worldData.terrainSize * worldData.tileSize)),
                                                (worldStartPosition.y - worldData.tileSize / 2) - (Mathf.Floor(terrainData.Index / worldData.regionSize) * (worldData.terrainSize * worldData.tileSize)));
        
        foreach (TerrainTileDataElement terrainTileData in terrainData.terrainTileDataList)
        {
            var tilePosition = new Vector2( terrainStartPosition.x + (worldData.tileSize * (terrainTileData.Index % worldData.terrainSize)),
                                            terrainStartPosition.y - (worldData.tileSize * (Mathf.Floor(terrainTileData.Index / worldData.terrainSize))));

            //if (GeometryUtility.TestPlanesAABB(planes, new Bounds(CameraManager.content.TransformPoint(tilePosition), tileBoundSize)))
            //{
                Tile prefab = Resources.Load<Tile>("Objects/Tile/" + worldData.tileSetName + "/" + terrainTileData.TileId);

                Tile tile = (Tile)PoolManager.SpawnObject(terrainTileData.TileId, prefab);
                tileList.Add(tile);

                tile.gameObject.SetActive(true);

                tile.transform.SetParent(CameraManager.content, false);
                tile.transform.localPosition = new Vector3(tilePosition.x, 0, tilePosition.y);

                tile.DataType = Enums.DataType.TerrainTile;
                tile.DataElement = terrainTileData;

                terrainTileData.active = true;
                
                interactionController.DataList.AddRange(terrainData.interactionDataList.Where(x => x.TerrainTileId == terrainTileData.Id).Cast<IDataElement>());

                worldInteractableCharacterController.DataList.AddRange(terrainData.worldInteractableDataList.Where(x => x.Type == (int)Enums.InteractableType.Agent && x.terrainTileId == terrainTileData.Id && !worldInteractableData
                                                                                                            .Select(y => y.Id).Contains(x.Id)).Cast<IDataElement>());

                worldInteractableObjectController.DataList.AddRange(terrainData.worldInteractableDataList.Where(x => x.Type == (int)Enums.InteractableType.Object && x.terrainTileId == terrainTileData.Id && !worldInteractableData
                                                                                                         .Select(y => y.Id).Contains(x.Id)).Cast<IDataElement>());

                worldObjectController.DataList.AddRange(terrainData.worldObjectDataList.Where(x => x.TerrainTileId == terrainTileData.Id && !worldObjectData.Select(y => y.Id).Contains(x.Id)).Cast<IDataElement>());
            //}
        }
    }
    
    private void SetWorldElements(IDataController dataController)
    {
        if (dataController.DataList.Count == 0) return;
        
        var prefab = Resources.Load<ExEditorWorldElement>("Elements/World/EditorWorldElement");
        
        foreach (IDataElement dataElement in dataController.DataList)
        {
            var worldElement = (ExEditorWorldElement)PoolManager.SpawnObject(0, prefab);

            SelectionElementManager.InitializeElement(  worldElement.Element, CameraManager.content,
                                                        DisplayManager,
                                                        DisplayManager.Display.SelectionType,
                                                        DisplayManager.Display.SelectionProperty);
            elementList.Add(worldElement.Element);

            dataElement.SelectionElement = worldElement.Element;
            worldElement.Element.data.dataController = dataController;
            worldElement.Element.data = new SelectionElement.Data(dataController, dataElement);

            //Debugging
            GeneralData generalData = (GeneralData)dataElement;
            worldElement.name = generalData.DebugName + generalData.Id;
            //

            SetStatus(worldElement.Element);
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
        if (interactionData == null) return;

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
        if (interactionDataElement.Id != interactionData.Id &&
            interactionDataElement.worldInteractableId == interactionData.worldInteractableId &&
            interactionDataElement.objectiveId == interactionData.objectiveId)
        {
            element.elementStatus = Enums.ElementStatus.Hidden;
            return;
        }

        //Locked: Interactions that aren't selected and don't belong to a quest, only when an interaction with a quest is selected
        if (interactionDataElement.Id != interactionData.Id &&
            interactionData.objectiveId != 0 && interactionDataElement.objectiveId == 0)
        {
            element.elementStatus = Enums.ElementStatus.Locked;
            return;
        }

        //Enabled: Interactions belonging to a different interactable, but same objective
        if (interactionDataElement.worldInteractableId != interactionData.worldInteractableId &&
            interactionDataElement.objectiveId == interactionData.objectiveId)
        {
            element.elementStatus = Enums.ElementStatus.Enabled;
            return;
        }
        
        //Related: Interactions belonging to a different interactable, different objective but same quest
        if (interactionDataElement.worldInteractableId != interactionData.worldInteractableId &&
            interactionDataElement.questId == interactionData.questId)
        {
            element.elementStatus = Enums.ElementStatus.Related;
            return;
        }
        
        //Unrelated: Interactions belonging to a different quest
        if (interactionDataElement.questId > 0 &&
            interactionDataElement.questId != interactionData.questId)
        {
            element.elementStatus = Enums.ElementStatus.Unrelated;
            return;
        }
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

    private void SetElement(SelectionElement element)
    {
        if (element.elementStatus == Enums.ElementStatus.Hidden)
        {
            element.CloseElement();
            return;
        }

        element.gameObject.SetActive(true);
        
        element.SetElement();

        InitializeStatusIconOverlay(element);

        element.SetOverlay();
    }
    
    public void ResetData(List<IDataElement> filter)
    {
        CloseOrganizer();
        SetData();
    }
    
    public void ClearOrganizer()
    {
        dataSet = false;

        tileList.ForEach(x => PoolManager.ClosePoolObject(x));
        tileList.Clear();

        elementList.ForEach(x => PoolManager.ClosePoolObject(x.Poolable));
        SelectionElementManager.CloseElement(elementList);
    }

    private void CancelSelection()
    {
        SelectionManager.CancelSelection(dataList);
    }

    private void GetActiveTerrain()
    {
        var localCameraPosition = new Vector2(ScrollRect.content.localPosition.x - worldStartPosition.x, worldStartPosition.y - ScrollRect.content.localPosition.y);

        if (localCameraPosition.x < 0)
            localCameraPosition = new Vector2(1, localCameraPosition.y);
        if (localCameraPosition.x > RegionSize())
            localCameraPosition = new Vector2(RegionSize() - 1, localCameraPosition.y);
        if (localCameraPosition.y < 0)
            localCameraPosition = new Vector2(localCameraPosition.x, 1);
        if (localCameraPosition.y > RegionSize())
            localCameraPosition = new Vector2(localCameraPosition.x, RegionSize() - 1);
        
        activeTerrainData = GetTerrain(localCameraPosition.x, localCameraPosition.y);
    }

    private TerrainDataElement GetTerrain(float posX, float posY)
    {
        var terrainSize = worldData.terrainSize * worldData.tileSize;

        var terrainCoordinates = new Vector2(Mathf.Floor(posX / terrainSize),
                                             Mathf.Floor(posY / terrainSize));

        var terrainIndex = (worldData.regionSize * terrainCoordinates.y) + terrainCoordinates.x;

        return worldData.terrainDataList.Where(x => x.Index == terrainIndex).FirstOrDefault();
    }

    private float RegionSize()
    {
        return worldData.regionSize * worldData.terrainSize * worldData.tileSize;
    }

    public void CloseOrganizer()
    {
        ClearOrganizer();
        CancelSelection();

        DestroyImmediate(this);
    } 
}
