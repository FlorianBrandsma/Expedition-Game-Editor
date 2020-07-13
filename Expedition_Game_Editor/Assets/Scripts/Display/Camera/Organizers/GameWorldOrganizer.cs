using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameWorldOrganizer : MonoBehaviour, IOrganizer
{
    private List<Tile> tileList = new List<Tile>();
    
    private GameWorldElementData GameWorldData { get { return GameManager.instance.gameWorldData; } }
    private GameRegionElementData RegionData { get { return GameManager.instance.regionData; } }

    private ExGameWorldElement gameWorldElementPrefab;
    private ExGameWorldAgent gameWorldAgentPrefab;

    private Rect activeRect;
    private Vector2 positionTracker = new Vector2();

    private IDisplayManager DisplayManager      { get { return GetComponent<IDisplayManager>(); } }
    private CameraManager CameraManager         { get { return (CameraManager)DisplayManager; } }

    private CameraProperties CameraProperties   { get { return (CameraProperties)DisplayManager.Display; } }
    private GameWorldProperties GameWorldProperties   { get { return (GameWorldProperties)DisplayManager.Display.Properties; } }

    private IDataController DataController      { get { return DisplayManager.Display.DataController; } }

    public void InitializeOrganizer()
    {
        gameWorldElementPrefab = Resources.Load<ExGameWorldElement>("Elements/World/GameWorldElement");
        gameWorldAgentPrefab = Resources.Load<ExGameWorldAgent>("Elements/World/GameWorldAgent");
    }
    
    public void SelectData() { }
    
    public void SetData()
    {
        SetData(DataController.DataList);
    }

    public void SetData(List<IElementData> list)
    {
        if (list == null) return;

        SetActiveRect();

        CloseInactiveElements();

        foreach (GameTerrainElementData terrainData in RegionData.terrainDataList)
            SetTerrain(terrainData);

        //Set elements that are not bound to a tile
        SetWorldObjects();
        
        SetPartyMembers();
    }
    
    private void SetTerrain(GameTerrainElementData terrainData)
    {
        if (!activeRect.Overlaps(terrainData.gridElement.rect, true)) return;

        foreach (GameTerrainTileElementData terrainTileData in terrainData.terrainTileDataList)
        {
            if (activeRect.Overlaps(terrainTileData.gridElement.rect, true))
            {
                if(!terrainTileData.active)
                {
                    SetTerrainTile(terrainTileData);
                    
                    //Set objects that are bound to this terrain tile
                    SetWorldObjects(terrainTileData.Id);
                }

                //Set world interactable agents that are bound to this tile by their first interaction
                SetWorldInteractableAgents(terrainTileData.Id);

                //Set world interactable objects that are bound to this tile by their first interaction
                SetWorldInteractableObjects(terrainTileData.Id);
            }
        }
    }

    private void SetTerrainTile(GameTerrainTileElementData terrainTileData)
    {
        Tile prefab = Resources.Load<Tile>("Objects/Tile/" + RegionData.tileSetName + "/" + terrainTileData.tileId);

        Tile tile = (Tile)PoolManager.SpawnObject(prefab, terrainTileData.tileId);
        tileList.Add(tile);

        tile.gameObject.SetActive(true);

        tile.transform.SetParent(CameraManager.content, false);
        tile.transform.localPosition = new Vector3(terrainTileData.gridElement.startPosition.x, 0, terrainTileData.gridElement.startPosition.y);

        tile.DataType = Enums.DataType.GameTerrainTile;
        tile.ElementData = terrainTileData;

        terrainTileData.active = true;
    }

    private void SetWorldObjects(int terrainTileId = 0)
    {
        var worldObjectDataList = RegionData.terrainDataList.SelectMany(x => x.worldObjectDataList.Where(y => y.terrainTileId == terrainTileId)).Cast<IElementData>().ToList();

        SetWorldElements(worldObjectDataList);
    }
    
    private void SetWorldInteractableAgents(int terrainTileId)
    {
        var worldInteractableAgentDataList = GameWorldData.worldInteractableDataList.Where(x => x.DataElement == null && 
                                                                                                x.type == (int)Enums.InteractableType.Agent && 
                                                                                                x.terrainTileId == terrainTileId).Cast<IElementData>().ToList();
        SetWorldAgents(worldInteractableAgentDataList);
    }

    private void SetWorldInteractableObjects(int terrainTileId)
    {
        var worldInteractableObjectDataList = GameWorldData.worldInteractableDataList.Where(x => x.DataElement == null && 
                                                                                                 x.type == (int)Enums.InteractableType.Object && 
                                                                                                 x.terrainTileId == terrainTileId).Cast<IElementData>().ToList();

        SetWorldElements(worldInteractableObjectDataList);
    }

    private void SetPartyMembers()
    {
        var partyMemberDataList = GameWorldData.partyMemberList.Where(x => x.DataElement == null).Cast<IElementData>().ToList();

        SetWorldAgents(partyMemberDataList);
    }

    private void SetWorldElements(List<IElementData> dataList)
    {
        if (dataList.Count == 0) return;
        
        foreach(IElementData elementData in dataList)
        {
            var gameWorldElement = (ExGameWorldElement)PoolManager.SpawnObject(gameWorldElementPrefab);

            gameWorldElement.GameElement.transform.SetParent(CameraManager.content, false);

            gameWorldElement.GameElement.DataElement.data.elementData = elementData;
            elementData.DataElement = gameWorldElement.GameElement.DataElement;

            //Debugging
            GeneralData generalData = (GeneralData)elementData;
            gameWorldElement.name = generalData.DebugName + generalData.Id;
            //
            
            SetElement(gameWorldElement.GameElement);
        }
    }

    private void SetWorldAgents(List<IElementData> dataList)
    {
        foreach (IElementData elementData in dataList)
        {
            var gameWorldAgent = (ExGameWorldAgent)PoolManager.SpawnObject(gameWorldAgentPrefab);

            gameWorldAgent.GameElement.transform.SetParent(CameraManager.content, false);

            gameWorldAgent.GameElement.DataElement.data.elementData = elementData;
            elementData.DataElement = gameWorldAgent.GameElement.DataElement;

            //Debugging
            GeneralData generalData = (GeneralData)elementData;
            gameWorldAgent.name = generalData.DebugName + "Agent" + generalData.Id;
            //

            SetElement(gameWorldAgent.GameElement);
        }
    }

    private void SetElement(GameElement element)
    {
        element.gameObject.SetActive(true);

        element.SetElement();
    }
    
    private void SetActiveRect()
    {
        var cameraTransform = CameraManager.cam.transform;

        var activeRangePosition = new Vector2(cameraTransform.localPosition.x - (GameManager.instance.TempActiveRange / 2), cameraTransform.localPosition.z + (GameManager.instance.TempActiveRange / 2));
        var activeRangeSize = new Vector2(GameManager.instance.TempActiveRange, -GameManager.instance.TempActiveRange);

        activeRect = new Rect(activeRangePosition, activeRangeSize);
    }

    private void CloseInactiveElements()
    {
        var inactiveTiles = tileList.Where(x => !activeRect.Overlaps(((GameTerrainTileElementData)x.ElementData).gridElement.rect, true)).ToList();
        ClearTiles(inactiveTiles);
    }

    public void UpdateData()
    {
        var cameraTransform = CameraManager.cam.transform;

        if (cameraTransform.localPosition.x >= positionTracker.x + RegionData.tileSize ||
            cameraTransform.localPosition.x <= positionTracker.x - RegionData.tileSize ||
            cameraTransform.localPosition.z >= positionTracker.y + RegionData.tileSize ||
            cameraTransform.localPosition.z <= positionTracker.y - RegionData.tileSize)
        {
            Debug.Log("Update game data");

            SetData(DataController.DataList);

            positionTracker = FixTrackerPosition(CameraManager.cam.transform.localPosition);

            GameManager.instance.localNavMeshBuilder.UpdateNavMesh(true);
        }
    }

    private Vector2 FixTrackerPosition(Vector3 cameraPosition)
    {
        return new Vector2(Mathf.Floor((cameraPosition.x + (RegionData.tileSize / 2)) / RegionData.tileSize) * RegionData.tileSize,
                           Mathf.Floor((cameraPosition.z + (RegionData.tileSize / 2)) / RegionData.tileSize) * RegionData.tileSize);
    }

    public void ResetData(List<IElementData> filter)
    {
        CloseOrganizer();
        SetData();
    }

    public void ClearOrganizer()
    {
        ClearTiles(tileList);
    }
    
    private void ClearTiles(List<Tile> inactiveTileList)
    {
        inactiveTileList.ForEach(x =>
        {
            ClearTileElements((GameTerrainTileElementData)x.ElementData);
            PoolManager.ClosePoolObject(x);
        });

        tileList.RemoveAll(x => inactiveTileList.Contains(x));
    }

    private void ClearTileElements(GameTerrainTileElementData terrainTileData)
    {
        ClearWorldObjects(terrainTileData);
        ClearWorldInteractables(terrainTileData);
    }

    private void ClearWorldObjects(GameTerrainTileElementData terrainTileData)
    {
        var inactiveWorldObjectList = RegionData.terrainDataList.SelectMany(x => x.worldObjectDataList.Where(y => y.terrainTileId == terrainTileData.Id)).ToList();

        inactiveWorldObjectList.ForEach(x =>
        {
            PoolManager.ClosePoolObject(x.DataElement.Poolable);
            x.DataElement.Element.CloseElement();
        });
    }

    private void ClearWorldInteractables(GameTerrainTileElementData terrainTileData)
    {
        var inactiveWorldInteractableList = GameWorldData.worldInteractableDataList.Where(x => x.terrainTileId == terrainTileData.Id).ToList();

        inactiveWorldInteractableList.ForEach(x =>
        {
            PoolManager.ClosePoolObject(x.DataElement.Poolable);
            x.DataElement.Element.CloseElement();
        });
    }

    private void ClearPartyMembers()
    {
        GameWorldData.partyMemberList.ForEach(x =>
        {
            PoolManager.ClosePoolObject(x.DataElement.Poolable);
            x.DataElement.Element.CloseElement();
        });
    }

    public void CloseOrganizer()
    {
        ClearOrganizer();
        ClearPartyMembers();

        GameManager.instance.CloseGame();

        Debug.Log("Close for real");
        DestroyImmediate(this);
    }
}
