using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SceneOrganizer : MonoBehaviour, IOrganizer
{
    private List<Tile> tileList = new List<Tile>();
    private Vector3 tileBoundSize;
    
    private Plane[] planes;

    private InteractionDataElement interactionData;
    private List<IDataElement> dataList;

    private SceneDataElement sceneData;

    private Vector2 sceneStartPosition;
    private Vector2 positionTracker = new Vector2();

    private IDisplayManager DisplayManager  { get { return GetComponent<IDisplayManager>(); } }
    private CameraManager CameraManager     { get { return (CameraManager)DisplayManager; } }
    
    private CameraProperties CameraProperties { get { return (CameraProperties)DisplayManager.Display; } }
    private SceneProperties SceneProperties { get { return (SceneProperties)DisplayManager.Display.Properties; } }

    private IDataController DataController  { get { return CameraManager.Display.DataController; } }
    private GenericDataController sceneInteractableController = new GenericDataController(Enums.DataType.SceneInteractable);
    private GenericDataController interactionController = new GenericDataController(Enums.DataType.Interaction);
    private GenericDataController sceneObjectController = new GenericDataController(Enums.DataType.SceneObject);

    public List<SelectionElement> elementList = new List<SelectionElement>();

    private CustomScrollRect ScrollRect { get { return GetComponent<CustomScrollRect>(); } }

    private bool allowSelection = true;

    void Update()
    {
        var ray = CameraManager.cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (ScrollRect.m_Dragging)
            allowSelection = false;
        
        if (Input.GetMouseButtonUp(0))
        {
            if (allowSelection && Physics.Raycast(ray, out hit) && hit.collider.GetComponent<ObjectGraphic>() != null)
            {
                var selectionElement = hit.collider.GetComponent<ObjectGraphic>().selectionElement;

                selectionElement.InvokeSelection();
            }

            allowSelection = true;
        }
    }

    public void InitializeOrganizer()
    {
        sceneData = (SceneDataElement)DataController.DataList.FirstOrDefault();

        GetSelectedInteraction(DataController.SegmentController.Path);

        tileBoundSize = new Vector3(sceneData.tileSize, 0, sceneData.tileSize) * 3;

        CameraManager.cam.transform.localPosition = new Vector3(0, 10 - (sceneData.tileSize * 0.75f), -10);

        SetRegionSize();
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
        }
    }

    private Vector2 FixTrackerPosition(Vector2 cameraPosition)
    {
        return new Vector2( Mathf.Floor((cameraPosition.x + (sceneData.tileSize / 2)) / sceneData.tileSize) * sceneData.tileSize,
                            Mathf.Floor((cameraPosition.y + (sceneData.tileSize / 2)) / sceneData.tileSize) * sceneData.tileSize);
    }

    public void SetData()
    {
        SetData(DataController.DataList);
    }

    private void GetSelectedInteraction(Path path)
    {
        var interactionRoute = path.FindLastRoute(Enums.DataType.Interaction);

        if (interactionRoute == null) return;

        interactionData = sceneData.terrainDataList.SelectMany(x => x.interactionDataList.Where(y => y.Id == interactionRoute.GeneralData.Id)).FirstOrDefault();
    }
    
    private void SetData(List<IDataElement> list)
    {
        planes = GeometryUtility.CalculateFrustumPlanes(CameraManager.cam);

        sceneStartPosition = sceneData.startPosition;
        
        foreach (SceneDataElement.TerrainData terrainData in sceneData.terrainDataList)
        {
            SetTerrain(terrainData);

            sceneInteractableController.DataList.AddRange(terrainData.sceneInteractableDataList.Where(x => x.terrainTileId == 0).Cast<IDataElement>());
            interactionController.DataList.AddRange(terrainData.interactionDataList.Where(x => x.TerrainTileId == 0).Cast<IDataElement>());
            sceneObjectController.DataList.AddRange(terrainData.sceneObjectDataList.Where(x => x.TerrainTileId == 0).Cast<IDataElement>());
        }

        //Set scene interactables that are bound to this tile by their first interaction
        SetSceneElements(sceneInteractableController);

        //Set interactions that are bound to this terrain tile
        SetSceneElements(interactionController);

        //Set objects that are bound to this terrain tile
        SetSceneElements(sceneObjectController);
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

                sceneInteractableController.DataList.AddRange(terrainData.sceneInteractableDataList.Where(x => x.terrainTileId == terrainTileData.Id).Cast<IDataElement>());
                interactionController.DataList.AddRange(terrainData.interactionDataList.Where(x => x.TerrainTileId == terrainTileData.Id).Cast<IDataElement>());
                sceneObjectController.DataList.AddRange(terrainData.sceneObjectDataList.Where(x => x.TerrainTileId == terrainTileData.Id).Cast<IDataElement>());
                
                tile.gameObject.SetActive(true);
            }
        }
    }

    private void SetSceneElements(IDataController dataController)
    {
        if (dataController.DataList.Count == 0) return;
        
        SelectionElement elementPrefab = Resources.Load<SelectionElement>("Scene/EditorSceneElement");
        
        foreach(IDataElement dataElement in dataController.DataList)
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
    }

    private void SetSceneObjectStatus(SelectionElement element)
    {
        if (sceneData.regionType == Enums.RegionType.Interaction)
        {
            element.elementStatus = Enums.ElementStatus.Locked;
            return;
        }
    }

    private void SetElement(SelectionElement element)
    {
        element.gameObject.SetActive(true);
        
        element.SetElement();
    }
    
    public void ResetData(List<IDataElement> filter)
    {
        CloseOrganizer();
        SetData();
    }
    
    public void ClearOrganizer()
    {
        tileList.ForEach(x => x.ClosePoolable());
        SelectionElementManager.CloseElement(elementList);

        tileList.Clear();
    }

    private void CancelSelection()
    {
        SelectionManager.CancelSelection(dataList);
    }

    public void CloseOrganizer()
    {
        ClearOrganizer();

        CancelSelection();

        DestroyImmediate(this);
    }
}
