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

    private Vector2 startPosition;
    private Vector2 positionTracker = new Vector2();

    private IDataController dataController;
    
    private SceneProperties properties;

    private CustomScrollRect ScrollRect { get { return GetComponent<CustomScrollRect>(); } }
    private CameraManager CameraManager { get { return GetComponent<CameraManager>(); } }

    public void InitializeOrganizer()
    {
        dataController = CameraManager.cameraProperties.DataController;

        sceneData = (SceneDataElement)dataController.DataList.FirstOrDefault();

        tileBoundSize = new Vector3(EditorManager.UI.localScale.x * sceneData.tileSize, 
                                    0, 
                                    EditorManager.UI.localScale.z * sceneData.tileSize) * 3;

        SetRegionSize();
    }

    private void SetRegionSize()
    {
        var regionSize = new Vector2(sceneData.regionSize * sceneData.terrainSize * sceneData.tileSize,
                                     sceneData.regionSize * sceneData.terrainSize * sceneData.tileSize);

        ScrollRect.content.sizeDelta = regionSize * 2;
        CameraManager.content.sizeDelta = regionSize;
    }

    public void InitializeProperties()
    {
        properties = CameraManager.cameraProperties.GetComponent<SceneProperties>();
    }

    public void UpdateData()
    {
        if (ScrollRect.content.localPosition.x >= positionTracker.x + sceneData.tileSize ||
            ScrollRect.content.localPosition.x <= positionTracker.x - sceneData.tileSize ||
            ScrollRect.content.localPosition.y >= positionTracker.y + sceneData.tileSize ||
            ScrollRect.content.localPosition.y <= positionTracker.y - sceneData.tileSize)
        {
            //Debug.Log("Update");

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
        planes = GeometryUtility.CalculateFrustumPlanes(CameraManager.cam);

        foreach (SceneDataElement.TerrainData terrainData in sceneData.terrainDataList)
        {
            SetTerrain(terrainData);
        }
    }

    private void SetTerrain(SceneDataElement.TerrainData terrainData)
    {
        var startPosition = new Vector2(-((sceneData.regionSize * sceneData.terrainSize * sceneData.tileSize / 2) - sceneData.tileSize / 2) + ((terrainData.index % sceneData.regionSize) * (sceneData.terrainSize * sceneData.tileSize)),
                                         ((sceneData.regionSize * sceneData.terrainSize * sceneData.tileSize / 2) - sceneData.tileSize / 2) - (Mathf.Floor(terrainData.index / sceneData.regionSize) * (sceneData.terrainSize * sceneData.tileSize)));

        foreach (SceneDataElement.TerrainData.TerrainTileData terrainTileData in terrainData.terrainTileDataList)
        {
            var tilePosition = new Vector2( startPosition.x + (sceneData.tileSize * (terrainTileData.index % sceneData.terrainSize)),
                                            startPosition.y - (sceneData.tileSize * (Mathf.Floor(terrainTileData.index / sceneData.terrainSize))));

            if (GeometryUtility.TestPlanesAABB(planes, new Bounds(CameraManager.content.TransformPoint(tilePosition), tileBoundSize)))
            {
                Tile prefab = Resources.Load<Tile>("Objects/Tile/" + sceneData.tileSetName + "/" + terrainTileData.tileId);

                Tile tile = (Tile)ObjectManager.SpawnObject(terrainTileData.tileId, prefab.PoolType, prefab);

                tileList.Add(tile);

                tile.transform.SetParent(CameraManager.content.transform, false);

                tile.transform.localPosition = new Vector3(tilePosition.x, tilePosition.y, tile.transform.localPosition.z);

                tile.gameObject.SetActive(true);
            }
        }
    }


    public void ResetData(List<IDataElement> filter)
    {
        CloseOrganizer();
        SetData();
    }
    
    public void ClearOrganizer()
    {
        tileList.ForEach(x => x.gameObject.SetActive(false));

        tileList.Clear();
    }

    public void CloseOrganizer()
    {
        DestroyImmediate(this);
    }
}
