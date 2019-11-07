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

    private SceneDataElement sceneData;

    private Vector2 sceneStartPosition;
    private Vector2 positionTracker = new Vector2();
    
    private CameraManager cameraManager;
    private IDisplayManager DisplayManager { get { return GetComponent<IDisplayManager>(); } }
    private SceneProperties sceneProperties;

    private IDataController dataController;

    public List<SelectionElement> ElementList { get; set; }

    private CustomScrollRect ScrollRect { get { return GetComponent<CustomScrollRect>(); } }

    public void InitializeOrganizer()
    {
        cameraManager = (CameraManager)DisplayManager;

        dataController = cameraManager.Display.DataController;

        ElementList = new List<SelectionElement>();
    }

    private void SetRegionSize()
    {
        var regionSize = new Vector2(sceneData.regionSize * sceneData.terrainSize * sceneData.tileSize,
                                     sceneData.regionSize * sceneData.terrainSize * sceneData.tileSize);

        ScrollRect.content.sizeDelta = regionSize * 2;
        cameraManager.content.sizeDelta = regionSize;
    }

    public void InitializeProperties()
    {
        sceneProperties = (SceneProperties)DisplayManager.Display.Properties;
        
        sceneData = (SceneDataElement)dataController.DataList.FirstOrDefault();

        tileBoundSize = new Vector3(EditorManager.UI.localScale.x * sceneData.tileSize,
                                    0,
                                    EditorManager.UI.localScale.z * sceneData.tileSize) * 3;

        SetRegionSize();
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
        GetSelectedInteraction(dataController.SegmentController.Path);

        SetData(dataController.DataList);
    }

    private void GetSelectedInteraction(Path path)
    {
        var interactionRoute = path.FindLastRoute(Enums.DataType.Interaction);

        if (interactionRoute == null) return;

        interactionData = sceneData.terrainDataList.SelectMany(x => x.interactionDataList.Where(y => y.Id == interactionRoute.GeneralData.Id)).FirstOrDefault();
    }

    private void SetData(List<IDataElement> list)
    {
        planes = GeometryUtility.CalculateFrustumPlanes(cameraManager.cam);

        sceneStartPosition = sceneData.startPosition;
        
        foreach (SceneDataElement.TerrainData terrainData in sceneData.terrainDataList)
        {
            SetTerrain(terrainData);

            var sceneInteractableList = terrainData.sceneInteractableDataList.Where(x => x.terrainTileId == 0).Cast<IDataElement>().ToList();
            var interactionList = terrainData.interactionDataList.Where(x => x.TerrainTileId == 0).Cast<IDataElement>().ToList();
            var sceneObjectList = terrainData.sceneObjectDataList.Where(x => x.TerrainTileId == 0).Cast<IDataElement>().ToList();

            //Set scene interactables that are not bound to this tile by their first interaction
            SetSceneElements(sceneInteractableList);

            //Set interactions that are not bound to this terrain tile
            SetSceneElements(interactionList);

            //Set objects that are not bound to this terrain tile
            SetSceneElements(sceneObjectList);
        }
    }

    private void SetTerrain(SceneDataElement.TerrainData terrainData)
    {
        var terrainStartPosition = new Vector2( (sceneStartPosition.x + sceneData.tileSize / 2) + ((terrainData.Index % sceneData.regionSize) * (sceneData.terrainSize * sceneData.tileSize)),
                                                (sceneStartPosition.y - sceneData.tileSize / 2) - (Mathf.Floor(terrainData.Index / sceneData.regionSize) * (sceneData.terrainSize * sceneData.tileSize)));

        foreach (TerrainTileDataElement terrainTileData in terrainData.terrainTileDataList)
        {
            var tilePosition = new Vector2( terrainStartPosition.x + (sceneData.tileSize * (terrainTileData.Index % sceneData.terrainSize)),
                                            terrainStartPosition.y - (sceneData.tileSize * (Mathf.Floor(terrainTileData.Index / sceneData.terrainSize))));

            if (GeometryUtility.TestPlanesAABB(planes, new Bounds(cameraManager.content.TransformPoint(tilePosition), tileBoundSize)))
            {
                Tile prefab = Resources.Load<Tile>("Objects/Tile/" + sceneData.tileSetName + "/" + terrainTileData.TileId);

                Tile tile = (Tile)PoolManager.SpawnObject(terrainTileData.TileId, prefab.PoolType, prefab);
                tileList.Add(tile);

                tile.transform.SetParent(cameraManager.content.transform, false);
                tile.transform.localPosition = new Vector3(tilePosition.x, tilePosition.y, tile.transform.localPosition.z);

                var sceneInteractableList = terrainData.sceneInteractableDataList.Where(x => x.terrainTileId == terrainTileData.Id).Cast<IDataElement>().ToList();
                var interactionList = terrainData.interactionDataList.Where(x => x.TerrainTileId == terrainTileData.Id).Cast<IDataElement>().ToList();
                var sceneObjectList = terrainData.sceneObjectDataList.Where(x => x.TerrainTileId == terrainTileData.Id).Cast<IDataElement>().ToList();

                //Set scene interactables that are bound to this tile by their first interaction
                SetSceneElements(sceneInteractableList);

                //Set interactions that are bound to this terrain tile
                SetSceneElements(interactionList);

                //Set objects that are bound to this terrain tile
                SetSceneElements(sceneObjectList);
                
                tile.gameObject.SetActive(true);
            }
        }
    }

    private void SetSceneElements(List<IDataElement> dataList)
    {
        if (dataList.Count == 0) return;

        SelectionElement elementPrefab = Resources.Load<SelectionElement>("Scene/EditorSceneElement");

        foreach(IDataElement data in dataList)
        {
            SelectionElement element = SelectionElementManager.SpawnElement(elementPrefab, cameraManager.content,
                                                                            Enums.ElementType.SceneElement, DisplayManager, 
                                                                            DisplayManager.Display.SelectionType,
                                                                            DisplayManager.Display.SelectionProperty);

            ElementList.Add(element);

            data.SelectionElement = element;
            element.data = new SelectionElement.Data(dataController, data);

            //Debugging
            GeneralData generalData = (GeneralData)data;
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
            case Enums.DataType.SceneObject: /*SetSceneObjectStatus();*/ break;

            default: Debug.Log("CASE MISSING: " + element.GeneralData.DataType); break;
        }
    }

    private void SetInteractionStatus(SelectionElement element)
    {
        if (interactionData == null) return;

        var data = element.data;
        var dataElement = (InteractionDataElement)data.dataElement;

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
        SelectionElementManager.CloseElement(ElementList);

        tileList.Clear();
    }

    public void CloseOrganizer()
    {
        ClearOrganizer();

        DestroyImmediate(this);
    }
}
