using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SceneOrganizer : MonoBehaviour, IOrganizer
{
    private List<Tile> tileList = new List<Tile>();
    Vector3 tileBoundSize;

    Plane[] planes;
    private List<Bounds> boundTest = new List<Bounds>();

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
        SetData(dataController.DataList);
    }

    public void SetData(List<IDataElement> list)
    {
        planes = GeometryUtility.CalculateFrustumPlanes(cameraManager.cam);

        sceneStartPosition = sceneData.startPosition;
        
        foreach (SceneDataElement.TerrainData terrainData in sceneData.terrainDataList)
        {
            SetTerrain(terrainData);

            var interactionList = terrainData.interactionDataList.Where(x => x.TerrainTileId == 0).Cast<IDataElement>().ToList();
            var sceneObjectList = terrainData.sceneObjectDataList.Where(x => x.TerrainTileId == 0).Cast<IDataElement>().ToList();

            //Set interactions that are not bound to this terrain tile
            SetSceneElements(interactionList);

            //Set objects that are not bound to this terrain tile
            SetSceneElements(sceneObjectList);
        }
    }

    private void SetTerrain(SceneDataElement.TerrainData terrainData)
    {
        var terrainStartPosition = new Vector2( (sceneStartPosition.x + sceneData.tileSize / 2) + ((terrainData.index % sceneData.regionSize) * (sceneData.terrainSize * sceneData.tileSize)),
                                                (sceneStartPosition.y - sceneData.tileSize / 2) - (Mathf.Floor(terrainData.index / sceneData.regionSize) * (sceneData.terrainSize * sceneData.tileSize)));

        foreach (TerrainTileDataElement terrainTileData in terrainData.terrainTileDataList)
        {
            var tilePosition = new Vector2( terrainStartPosition.x + (sceneData.tileSize * (terrainTileData.index % sceneData.terrainSize)),
                                            terrainStartPosition.y - (sceneData.tileSize * (Mathf.Floor(terrainTileData.index / sceneData.terrainSize))));

            if (GeometryUtility.TestPlanesAABB(planes, new Bounds(cameraManager.content.TransformPoint(tilePosition), tileBoundSize)))
            {
                Tile prefab = Resources.Load<Tile>("Objects/Tile/" + sceneData.tileSetName + "/" + terrainTileData.TileId);

                Tile tile = (Tile)PoolManager.SpawnObject(terrainTileData.TileId, prefab.PoolType, prefab);
                tileList.Add(tile);

                tile.transform.SetParent(cameraManager.content.transform, false);
                tile.transform.localPosition = new Vector3(tilePosition.x, tilePosition.y, tile.transform.localPosition.z);

                var interactionList = terrainData.interactionDataList.Where(x => x.TerrainTileId == terrainTileData.id).Cast<IDataElement>().ToList();
                var sceneObjectList = terrainData.sceneObjectDataList.Where(x => x.TerrainTileId == terrainTileData.id).Cast<IDataElement>().ToList();
                
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
            element.name = generalData.DebugName + generalData.id;
            //
            
            SetElement(element);
        }
    }

    void SetElement(SelectionElement element)
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
