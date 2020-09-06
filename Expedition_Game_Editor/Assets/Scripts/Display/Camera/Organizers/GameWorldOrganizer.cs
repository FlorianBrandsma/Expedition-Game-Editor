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
        SetData(DataController.Data.dataList);
    }

    public void SetData(List<IElementData> list)
    {
        if (list == null) return;

        SetActiveRect();

        CloseInactiveElements();

        foreach (GameTerrainElementData terrainData in RegionData.TerrainDataList)
            SetTerrain(terrainData);
        
        //Set elements that are not bound to a tile
        SetWorldObjects();
        
        SetPartyMembers();
    }
    
    private void SetTerrain(GameTerrainElementData terrainData)
    {
        if (!activeRect.Overlaps(terrainData.GridElement.rect, true)) return;

        foreach (GameTerrainTileElementData terrainTileData in terrainData.TerrainTileDataList)
        {
            if (activeRect.Overlaps(terrainTileData.GridElement.rect, true))
            {
                if(!terrainTileData.Active)
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
        Tile prefab = Resources.Load<Tile>("Objects/Tile/" + RegionData.TileSetName + "/" + terrainTileData.TileId);

        Tile tile = (Tile)PoolManager.SpawnObject(prefab, terrainTileData.TileId);
        tileList.Add(tile);

        tile.gameObject.SetActive(true);

        tile.transform.SetParent(CameraManager.content, false);
        tile.transform.localPosition = new Vector3(terrainTileData.GridElement.startPosition.x, tile.transform.localPosition.y, terrainTileData.GridElement.startPosition.y);

        tile.DataType = Enums.DataType.GameTerrainTile;
        tile.ElementData = terrainTileData;

        terrainTileData.Active = true;
    }

    private void SetWorldObjects(int terrainTileId = 0)
    {
        var worldObjectDataList = RegionData.TerrainDataList.SelectMany(x => x.WorldObjectDataList.Where(y => y.TerrainTileId == terrainTileId)).Cast<IElementData>().ToList();

        SetWorldElements(worldObjectDataList);
    }
    
    private void SetWorldInteractableAgents(int terrainTileId)
    {
        var worldInteractableAgentDataList = GameWorldData.WorldInteractableDataList.Where(x => x.DataElement == null && 
                                                                                                x.Type == (int)Enums.InteractableType.Agent && 
                                                                                                x.TerrainTileId == terrainTileId).Cast<IElementData>().ToList();
        SetWorldAgents(worldInteractableAgentDataList);
    }

    private void SetWorldInteractableObjects(int terrainTileId)
    {
        var worldInteractableObjectDataList = GameWorldData.WorldInteractableDataList.Where(x => x.DataElement == null && 
                                                                                                 x.Type == (int)Enums.InteractableType.Object && 
                                                                                                 x.TerrainTileId == terrainTileId).Cast<IElementData>().ToList();

        SetWorldElements(worldInteractableObjectDataList);
    }

    private void SetPartyMembers()
    {
        var partyMemberDataList = GameWorldData.PartyMemberList.Where(x => x.DataElement == null).Cast<IElementData>().ToList();

        SetWorldAgents(partyMemberDataList);
    }

    private void SetWorldElements(List<IElementData> dataList)
    {
        if (dataList.Count == 0) return;
        
        foreach(IElementData elementData in dataList)
        {
            var gameWorldElement = (ExGameWorldElement)PoolManager.SpawnObject(gameWorldElementPrefab);

            elementData.DataElement = gameWorldElement.GameElement.DataElement;

            gameWorldElement.GameElement.transform.SetParent(CameraManager.content, false);
            
            gameWorldElement.GameElement.DataElement.Data = DataController.Data;
            gameWorldElement.GameElement.DataElement.Id = elementData.Id;

            //Debugging
            gameWorldElement.name = elementData.DebugName + elementData.Id;

            SetElement(gameWorldElement.GameElement);
        }
    }

    private void SetWorldAgents(List<IElementData> dataList)
    {
        foreach (IElementData elementData in dataList)
        {
            //Spawns a unique agent prefab to actually get a fresh agent (circumvents reset issues when agents are not on a navigation mesh)
            var gameWorldAgent = (ExGameWorldAgent)PoolManager.SpawnObject(gameWorldAgentPrefab, elementData.Id);

            gameWorldAgent.GameElement.transform.SetParent(CameraManager.content, false);

            gameWorldAgent.GameElement.DataElement.Data = DataController.Data;
            gameWorldAgent.GameElement.DataElement.Id = elementData.Id;

            elementData.DataElement = gameWorldAgent.GameElement.DataElement;
            
            //Debugging
            gameWorldAgent.name = elementData.DebugName + "Agent" + elementData.Id;

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
        var inactiveTiles = tileList.Where(x => !activeRect.Overlaps(((GameTerrainTileElementData)x.ElementData).GridElement.rect, true)).ToList();
        ClearTiles(inactiveTiles);
    }

    public void UpdateData()
    {
        var cameraTransform = CameraManager.cam.transform;

        if (cameraTransform.localPosition.x >= positionTracker.x + RegionData.TileSize ||
            cameraTransform.localPosition.x <= positionTracker.x - RegionData.TileSize ||
            cameraTransform.localPosition.z >= positionTracker.y + RegionData.TileSize ||
            cameraTransform.localPosition.z <= positionTracker.y - RegionData.TileSize)
        {
            SetData(DataController.Data.dataList);

            positionTracker = FixTrackerPosition(CameraManager.cam.transform.localPosition);

            GameManager.instance.localNavMeshBuilder.UpdateNavMesh(true);
        }
    }

    private Vector2 FixTrackerPosition(Vector3 cameraPosition)
    {
        return new Vector2(Mathf.Floor((cameraPosition.x + (RegionData.TileSize / 2)) / RegionData.TileSize) * RegionData.TileSize,
                           Mathf.Floor((cameraPosition.z + (RegionData.TileSize / 2)) / RegionData.TileSize) * RegionData.TileSize);
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
        var inactiveWorldObjectList = RegionData.TerrainDataList.SelectMany(x => x.WorldObjectDataList.Where(y => y.TerrainTileId == terrainTileData.Id)).ToList();

        inactiveWorldObjectList.ForEach(x =>
        {
            PoolManager.ClosePoolObject(x.DataElement.Poolable);
            x.DataElement.Element.CloseElement();
        });
    }

    private void ClearWorldInteractables(GameTerrainTileElementData terrainTileData)
    {
        var inactiveWorldInteractableList = GameWorldData.WorldInteractableDataList.Where(x => x.TerrainTileId == terrainTileData.Id).ToList();

        inactiveWorldInteractableList.ForEach(x =>
        {
            PoolManager.ClosePoolObject(x.DataElement.Poolable);
            x.DataElement.Element.CloseElement();
        });
    }

    private void ClearPartyMembers()
    {
        GameWorldData.PartyMemberList.ForEach(x =>
        {
            PoolManager.ClosePoolObject(x.DataElement.Poolable);
            x.DataElement.Element.CloseElement();
        });
    }

    public void CloseOrganizer()
    {
        ClearOrganizer();
        ClearPartyMembers();

        positionTracker = new Vector2();

        GameManager.instance.CloseGame();
        
        Debug.Log("Close for real");
        DestroyImmediate(this);
    }
}
