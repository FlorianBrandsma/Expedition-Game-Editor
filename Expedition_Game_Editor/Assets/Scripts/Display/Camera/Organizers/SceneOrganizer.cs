using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class SceneOrganizer : MonoBehaviour, IOrganizer
{
    static public List<Tile> tileList = new List<Tile>();
    private Vector3 tileBoundSize;
    
    private Plane[] planes;

    public SceneDataElement.TerrainData activeTerrainData;
    private InteractionDataElement interactionData;
    private SceneInteractableDataElement sceneInteractableData = new SceneInteractableDataElement();
    private SceneObjectDataElement sceneObjectData = new SceneObjectDataElement();
    private List<IDataElement> dataList;

    private SceneDataElement sceneData;

    private Vector2 sceneStartPosition;
    private Vector2 positionTracker = new Vector2();

    private IDisplayManager DisplayManager  { get { return GetComponent<IDisplayManager>(); } }
    private CameraManager CameraManager     { get { return (CameraManager)DisplayManager; } }
    
    private CameraProperties CameraProperties { get { return (CameraProperties)DisplayManager.Display; } }
    private SceneProperties SceneProperties { get { return (SceneProperties)DisplayManager.Display.Properties; } }

    private IDataController DataController  { get { return CameraManager.Display.DataController; } }
    private DataController sceneInteractableController = new DataController(Enums.DataType.SceneInteractable);
    private DataController interactionController = new DataController(Enums.DataType.Interaction);
    private DataController sceneObjectController = new DataController(Enums.DataType.SceneObject);

    public List<SelectionElement> elementList = new List<SelectionElement>();

    private CustomScrollRect ScrollRect { get { return GetComponent<CustomScrollRect>(); } }

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

        sceneData = (SceneDataElement)DataController.DataList.FirstOrDefault();
        
        GetSelectedElement(DataController.SegmentController.MainPath, Enums.DataType.Interaction);

        //Only get other data if no interaction was selected. Technically both could be selected,
        //but this is never necessary and so the scene object won't stay active
        if(interactionData == null)
        {
            GetSelectedElement(DataController.SegmentController.MainPath, Enums.DataType.SceneInteractable);
            GetSelectedElement(DataController.SegmentController.MainPath, Enums.DataType.SceneObject);
        }
        
        tileBoundSize = new Vector3(sceneData.tileSize, 0, sceneData.tileSize) * 3;

        CameraManager.cam.transform.localPosition = new Vector3(0, 10 - (sceneData.tileSize * 0.75f), -10);

        SetRegionSize();
    }

    private void GetSelectedElement(Path path, Enums.DataType dataType)
    {
        //Get selected elements from all paths
        var paths = EditorManager.editorManager.forms.Select(x => x.activePath).ToList();
        var route = paths.Select(x => x.FindLastRoute(dataType)).Where(x => x != null).FirstOrDefault();
        
        if (route == null) return;

        switch (dataType)
        {
            case Enums.DataType.SceneInteractable:

                sceneInteractableData = sceneData.terrainDataList.SelectMany(x => x.sceneInteractableDataList.Where(y => y.Id == route.GeneralData.Id)).FirstOrDefault();

                break;

            case Enums.DataType.Interaction:
                
                interactionData = sceneData.terrainDataList.SelectMany(x => x.interactionDataList.Where(y => y.Id == route.GeneralData.Id)).FirstOrDefault();

                break;

            case Enums.DataType.SceneObject:

                sceneObjectData = sceneData.terrainDataList.SelectMany(x => x.sceneObjectDataList.Where(y => y.Id == route.GeneralData.Id)).FirstOrDefault();

                break;
        }
    }

    private void InitializeControllers()
    {
        sceneInteractableController.SegmentController = DataController.SegmentController;
        interactionController.SegmentController = DataController.SegmentController;
        sceneObjectController.SegmentController = DataController.SegmentController;

        sceneInteractableController.DataCategory = Enums.DataCategory.Navigation;
        interactionController.DataCategory = Enums.DataCategory.Navigation;
    }

    public void SelectData()
    {
        dataList =  sceneData.terrainDataList.SelectMany(x => x.sceneInteractableDataList).Cast<IDataElement>().Concat(
                    sceneData.terrainDataList.SelectMany(x => x.interactionDataList).Cast<IDataElement>()).Concat(
                    sceneData.terrainDataList.SelectMany(x => x.sceneObjectDataList).Cast<IDataElement>()).ToList();

        SelectionManager.SelectData(dataList, DisplayManager);
    }

    private void SetRegionSize()
    {
        var regionSize = new Vector2(sceneData.regionSize * sceneData.terrainSize * sceneData.tileSize,
                                     sceneData.regionSize * sceneData.terrainSize * sceneData.tileSize);

        ScrollRect.content.sizeDelta = regionSize * 2;
        CameraManager.content.sizeDelta = regionSize;
    }

    public void UpdateData()
    {
        if (ScrollRect.content.localPosition.x >= positionTracker.x + sceneData.tileSize ||
            ScrollRect.content.localPosition.x <= positionTracker.x - sceneData.tileSize ||
            ScrollRect.content.localPosition.y >= positionTracker.y + sceneData.tileSize ||
            ScrollRect.content.localPosition.y <= positionTracker.y - sceneData.tileSize)
        {
            positionTracker = FixTrackerPosition(ScrollRect.content.localPosition);
            
            ClearOrganizer();

            sceneInteractableController.DataList.Clear();
            interactionController.DataList.Clear();
            sceneObjectController.DataList.Clear();

            SetData();

            dataSet = true;
        }
    }

    private Vector2 FixTrackerPosition(Vector2 cameraPosition)
    {
        return new Vector2( Mathf.Floor((cameraPosition.x + (sceneData.tileSize / 2)) / sceneData.tileSize) * sceneData.tileSize,
                            Mathf.Floor((cameraPosition.y + (sceneData.tileSize / 2)) / sceneData.tileSize) * sceneData.tileSize);
    }

    public void SetData()
    {
        FixLostInteractions();
        FixLostSceneObjects();

        SetData(DataController.DataList);
    }

    private void FixLostInteractions()
    {
        var lostInteractions = sceneData.terrainDataList.SelectMany(x => x.interactionDataList.Where(y => y.TerrainId != x.Id)).ToList();

        sceneData.terrainDataList.ForEach(x => x.interactionDataList.RemoveAll(y => lostInteractions.Select(z => z.TerrainId).Contains(y.TerrainId)));
        sceneData.terrainDataList.ForEach(x => x.interactionDataList.AddRange(lostInteractions.Where(y => y.TerrainId == x.Id)));
    }

    private void FixLostSceneObjects()
    {
        var lostSceneObjects = sceneData.terrainDataList.SelectMany(x => x.sceneObjectDataList.Where(y => y.TerrainId != 0 && y.TerrainId != x.Id)).ToList();

        sceneData.terrainDataList.ForEach(x => x.sceneObjectDataList.RemoveAll(y => lostSceneObjects.Select(z => z.TerrainId).Contains(y.TerrainId)));
        sceneData.terrainDataList.ForEach(x => x.sceneObjectDataList.AddRange(lostSceneObjects.Where(y => y.TerrainId == x.Id)));
    }
    
    private void SetData(List<IDataElement> list)
    {
        if (dataSet) return;
        
        planes = GeometryUtility.CalculateFrustumPlanes(CameraManager.cam);

        sceneStartPosition = sceneData.startPosition;
        
        foreach (SceneDataElement.TerrainData terrainData in sceneData.terrainDataList)
        {
            SetTerrain(terrainData);
            
            interactionController.DataList.AddRange(terrainData.interactionDataList.Where(x => x.TerrainTileId == 0).Cast<IDataElement>());
            sceneInteractableController.DataList.AddRange(terrainData.sceneInteractableDataList.Where(x => x.terrainTileId == 0 || x.Id == sceneInteractableData.Id).Cast<IDataElement>());
            sceneObjectController.DataList.AddRange(terrainData.sceneObjectDataList.Where(x => x.TerrainTileId == 0 || x.Id == sceneObjectData.Id).Cast<IDataElement>());
        }

        //Extract interactions that will be obtained by the region navigation dropdown.
        //Also maintains selection focus
        if (interactionData != null)
            ExtractInteractions();

        //Set scene interactables that are bound to this tile by their first interaction
        SetSceneElements(sceneInteractableController);
        
        //Set interactions that are bound to this terrain tile
        SetSceneElements(interactionController);

        //Set objects that are bound to this terrain tile
        SetSceneElements(sceneObjectController);

        GetActiveTerrain();
    }

    private void ExtractInteractions()
    {
        var interactionList = sceneData.terrainDataList.SelectMany(x => x.interactionDataList
                                                       .Where(y => y.objectiveId == interactionData.objectiveId && y.SceneInteractableId == interactionData.SceneInteractableId))
                                                       .OrderBy(x => x.Index).Cast<IDataElement>().ToList();

        interactionController.DataList.RemoveAll(x => interactionList.Contains(x));

        var dataElement = interactionList.Where(x => x.Id == interactionData.Id).FirstOrDefault();

        DataController.SegmentController.Path.ReplaceDataLists(0, Enums.DataType.Interaction, interactionList, dataElement);

        SetSceneElements(DataController.SegmentController.Path.FindLastRoute(Enums.DataType.Interaction).data.dataController);
    }

    private void SetTerrain(SceneDataElement.TerrainData terrainData)
    {
        var terrainStartPosition = new Vector2( (sceneStartPosition.x + sceneData.tileSize / 2) + ((terrainData.Index % sceneData.regionSize) * (sceneData.terrainSize * sceneData.tileSize)),
                                                (sceneStartPosition.y - sceneData.tileSize / 2) - (Mathf.Floor(terrainData.Index / sceneData.regionSize) * (sceneData.terrainSize * sceneData.tileSize)));

        foreach (TerrainTileDataElement terrainTileData in terrainData.terrainTileDataList)
        {
            var tilePosition = new Vector2( terrainStartPosition.x + (sceneData.tileSize * (terrainTileData.Index % sceneData.terrainSize)),
                                            terrainStartPosition.y - (sceneData.tileSize * (Mathf.Floor(terrainTileData.Index / sceneData.terrainSize))));
            
            if (GeometryUtility.TestPlanesAABB(planes, new Bounds(CameraManager.content.TransformPoint(tilePosition), tileBoundSize)))
            {
                Tile prefab = Resources.Load<Tile>("Objects/Tile/" + sceneData.tileSetName + "/" + terrainTileData.TileId);

                Tile tile = (Tile)PoolManager.SpawnObject(terrainTileData.TileId, prefab.PoolType, prefab);
                tileList.Add(tile);

                tile.transform.SetParent(CameraManager.content.transform, false);
                tile.transform.localPosition = new Vector3(tilePosition.x, tilePosition.y, tile.transform.localPosition.z);
                
                interactionController.DataList.AddRange(terrainData.interactionDataList.Where(x => x.TerrainTileId == terrainTileData.Id).Cast<IDataElement>());
                sceneInteractableController.DataList.AddRange(terrainData.sceneInteractableDataList.Where(x => x.terrainTileId == terrainTileData.Id && x.Id != sceneInteractableData.Id).Cast<IDataElement>());
                sceneObjectController.DataList.AddRange(terrainData.sceneObjectDataList.Where(x => x.TerrainTileId == terrainTileData.Id && x.Id != sceneObjectData.Id).Cast<IDataElement>());

                tile.gameObject.SetActive(true);
            }
        }
    }

    private void SetSceneElements(IDataController dataController)
    {
        if (dataController.DataList.Count == 0) return;
        
        SelectionElement elementPrefab = Resources.Load<SelectionElement>("Scene/EditorSceneElement");

        foreach (IDataElement dataElement in dataController.DataList)
        {
            SelectionElement element = SelectionElementManager.SpawnElement(elementPrefab, CameraManager.content,
                                                                            Enums.ElementType.SceneElement, DisplayManager, 
                                                                            DisplayManager.Display.SelectionType,
                                                                            DisplayManager.Display.SelectionProperty);

            elementList.Add(element);

            dataElement.SelectionElement = element;
            element.dataController = dataController;
            element.data = new SelectionElement.Data(dataController, dataElement);
            
            //Debugging
            GeneralData generalData = (GeneralData)dataElement;
            element.name = generalData.DebugName + generalData.Id;
            //

            SetStatus(element);
            SetElement(element);
        }
    }

    private void SetStatus(SelectionElement element)
    {
        switch (element.GeneralData.DataType)
        {
            case Enums.DataType.SceneInteractable: /*SetSceneInteractableStatus();*/ break;
            case Enums.DataType.Interaction: SetInteractionStatus(element); break;
            case Enums.DataType.SceneObject: SetSceneObjectStatus(element); break;

            default: Debug.Log("CASE MISSING: " + element.GeneralData.DataType); break;
        }
    }
    
    private void SetInteractionStatus(SelectionElement element)
    {
        if (interactionData == null) return;

        var dataElement = (InteractionDataElement)element.data.dataElement;

        /*
        Debug.Log(  interactionData.Id + "/" + dataElement.id + ":" + 
                    interactionData.SceneInteractableId + "/" + dataElement.SceneInteractableId + ":" +
                    interactionData.questId + "/" + dataElement.questId + ":" +
                    interactionData.objectiveId + "/" + dataElement.objectiveId);
        */
        
        //Hidden: Interactions belonging to the same interactable and selected objective
        if (dataElement.Id != interactionData.Id && 
            dataElement.SceneInteractableId == interactionData.SceneInteractableId && 
            dataElement.objectiveId == interactionData.objectiveId)
        {
            element.elementStatus = Enums.ElementStatus.Hidden;
            return;
        }

        //Locked: Interactions that aren't selected and don't belong to a quest, only when an interaction with a quest is selected
        if (dataElement.Id != interactionData.Id && 
            interactionData.questId != 0 && dataElement.questId == 0)
        {
            element.elementStatus = Enums.ElementStatus.Locked;
            return;
        }

        //Enabled: Interactions belonging to a different interactable, but same objective
        if (dataElement.SceneInteractableId != interactionData.SceneInteractableId && 
            dataElement.objectiveId == interactionData.objectiveId)
        {
            element.elementStatus = Enums.ElementStatus.Enabled;
            return;
        }

        //Related: Interactions belonging to a different interactable, different objective but same quest
        if (dataElement.SceneInteractableId != interactionData.SceneInteractableId && 
            dataElement.questId == interactionData.questId)
        {
            element.elementStatus = Enums.ElementStatus.Related;
            return;
        }

        //Unrelated: Interactions belonging to a different quest
        if (dataElement.questId > 0 && 
            dataElement.questId != interactionData.questId)
        {
            element.elementStatus = Enums.ElementStatus.Unrelated;
            return;
        }

        // ???: Same interactable belonging to a different objective (shouldn't be visible?)
    }

    private void SetSceneObjectStatus(SelectionElement element)
    {
        if (sceneData.regionType == Enums.RegionType.Interaction)
        {
            element.elementStatus = Enums.ElementStatus.Locked;
            return;
        }
    }

    private void InitializeStatusIconOverlay(SelectionElement element)
    {
        var statusIconManager = CameraManager.overlayManager.GetComponent<StatusIconManager>();

        if (element.data.dataElement.SelectionStatus != Enums.SelectionStatus.None)
            element.glow = statusIconManager.StatusIcon(element, StatusIconManager.StatusIconType.Selection);

        if (element.elementStatus == Enums.ElementStatus.Locked)
            element.lockIcon = statusIconManager.StatusIcon(element, StatusIconManager.StatusIconType.Lock);
    }

    private void SetElement(SelectionElement element)
    {
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

        tileList.ForEach(x => x.ClosePoolable());

        SelectionElementManager.CloseElement(elementList);
        
        tileList.Clear();
    }

    private void CancelSelection()
    {
        SelectionManager.CancelSelection(dataList);
    }

    private void GetActiveTerrain()
    {
        var localCameraPosition = new Vector2(ScrollRect.content.localPosition.x - sceneStartPosition.x, sceneStartPosition.y - ScrollRect.content.localPosition.y);

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

    private SceneDataElement.TerrainData GetTerrain(float posX, float posY)
    {
        var terrainSize = sceneData.terrainSize * sceneData.tileSize;

        var terrainCoordinates = new Vector2(Mathf.Floor(posX / terrainSize),
                                             Mathf.Floor(posY / terrainSize));

        var terrainIndex = (sceneData.regionSize * terrainCoordinates.y) + terrainCoordinates.x;

        return sceneData.terrainDataList.Where(x => x.Index == terrainIndex).FirstOrDefault();
    }

    private float RegionSize()
    {
        return sceneData.regionSize * sceneData.terrainSize * sceneData.tileSize;
    }

    public void CloseOrganizer()
    {
        ClearOrganizer();
        CancelSelection();

        DestroyImmediate(this);
    } 
}
