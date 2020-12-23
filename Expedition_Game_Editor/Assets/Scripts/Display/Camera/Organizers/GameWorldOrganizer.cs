using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameWorldOrganizer : MonoBehaviour, IOrganizer
{
    private List<GameTerrainTileElementData> tileList = new List<GameTerrainTileElementData>();

    private GameWorldElementData GameWorldData      { get { return GameManager.instance.gameWorldData; } }
    private GameRegionElementData RegionData        { get { return GameManager.instance.regionData; } }
    
    private ExGameWorldElement gameWorldObjectPrefab;
    private ExGameWorldElement gameWorldInteractableObjectPrefab;
    private ExGameWorldAgent gameWorldAgentPrefab;
    private ExGameWorldAgent gameWorldControllablePrefab;

    private Rect activeRect;
    private Vector2 positionTracker = new Vector2();

    private IDisplayManager DisplayManager          { get { return GetComponent<IDisplayManager>(); } }
    public CameraManager CameraManager              { get { return (CameraManager)DisplayManager; } }
    public OverlayManager OverlayManger             { get { return CameraManager.overlayManager; } }

    private CameraProperties CameraProperties       { get { return (CameraProperties)DisplayManager.Display; } }
    private GameWorldProperties GameWorldProperties { get { return (GameWorldProperties)DisplayManager.Display.Properties; } }

    private IDataController DataController          { get { return DisplayManager.Display.DataController; } }

    private DataController WorldObjectDataController                    { get { return GameManager.instance.WorldObjectDataController; } }
    private DataController WorldInteractableAgentDataController         { get { return GameManager.instance.WorldInteractableAgentDataController; } }
    private DataController WorldInteractableObjectDataController        { get { return GameManager.instance.WorldInteractableObjectDataController; } }
    private DataController WorldInteractableControllableDataController  { get { return GameManager.instance.WorldInteractableControllableDataController; } }
    private DataController ScenePropDataController                      { get { return GameManager.instance.ScenePropDataController; } }

    public void InitializeOrganizer()
    {
        gameWorldObjectPrefab               = Resources.Load<ExGameWorldElement>("Elements/World/GameWorldObject");
        gameWorldInteractableObjectPrefab   = Resources.Load<ExGameWorldElement>("Elements/World/GameWorldInteractableObject");
        gameWorldAgentPrefab                = Resources.Load<ExGameWorldAgent>("Elements/World/GameWorldAgent");
        gameWorldControllablePrefab         = Resources.Load<ExGameWorldAgent>("Elements/World/GameWorldControllable");
    }

    public void SelectData() { }

    public void SetData()
    {
        SetData(DataController.Data);
    }

    public void SetData(Data data)
    {
        if (data == null) return;

        SetActiveRect();
        
        //Close all inactive tiles and elements on the tiles
        CloseInactiveElements();

        RegionData.GameTerrainDataList.ForEach(terrain => SetTerrain(terrain));
        
        //Set elements that are not bound to a tile
        SetWorldObjects();
        
        SetWorldInteractableControllables();
    }
    
    private void SetTerrain(GameTerrainElementData gameTerrainData)
    {
        if (!activeRect.Overlaps(gameTerrainData.GridElement.rect, true)) return;

        foreach (GameTerrainTileElementData terrainTileData in gameTerrainData.GameTerrainTileDataList)
        {
            if (activeRect.Overlaps(terrainTileData.GridElement.rect, true))
            {
                if(!terrainTileData.Active)
                {
                    SetTerrainTile(terrainTileData);
                    
                    //Set objects that are bound to this terrain tile
                    SetWorldObjects(terrainTileData.Id);

                    //Set scene props that are bound to this terrain tile
                    SetSceneProps(terrainTileData.Id);
                }

                //Set world interactable agents that are bound to this tile by their first interaction
                SetWorldInteractableAgents(terrainTileData.Id);

                //Set world interactable objects that are bound to this tile by their first interaction
                SetWorldInteractableObjects(terrainTileData.Id);
            }
        }
    }

    private void SetTerrainTile(GameTerrainTileElementData gameTerrainTileElementData)
    {
        Tile prefab = Resources.Load<Tile>("Objects/Tile/" + RegionData.TileSetName + "/" + gameTerrainTileElementData.TileId);

        Tile tile = (Tile)PoolManager.SpawnObject(prefab, gameTerrainTileElementData.TileId);
        tileList.Add(gameTerrainTileElementData);

        tile.gameObject.SetActive(true);

        tile.transform.SetParent(CameraManager.content, false);
        tile.transform.localPosition = new Vector3(gameTerrainTileElementData.GridElement.startPosition.x, tile.transform.localPosition.y, gameTerrainTileElementData.GridElement.startPosition.y);

        tile.DataType = Enums.DataType.GameTerrainTile;
        tile.ElementData = gameTerrainTileElementData;
        gameTerrainTileElementData.DataElement = tile.GameElement.DataElement;

        gameTerrainTileElementData.Active = true;
    }
    
    private void SetWorldObjects(int terrainTileId = 0)
    {
        var worldObjectDataList = WorldObjectDataController.Data.dataList.Where(x => ((GameWorldObjectElementData)x).TerrainTileId == terrainTileId).ToList();
        worldObjectDataList.ForEach(elementData => SetElement(WorldObjectDataController, elementData, gameWorldObjectPrefab));
    }
    
    private void SetWorldInteractableAgents(int terrainTileId)
    {
        var worldInteractableAgentDataList = WorldInteractableAgentDataController.Data.dataList.Cast<GameWorldInteractableElementData>().ToList().Where(x => x.DataElement == null && 
                                                                                                                                                             x.TerrainTileId == terrainTileId).ToList();

        worldInteractableAgentDataList.ForEach(elementData => 
        {
            SetElement(WorldInteractableAgentDataController, elementData, gameWorldAgentPrefab, (int)elementData.Type);
        });
    }

    private void SetWorldInteractableObjects(int terrainTileId)
    {
        var worldInteractableObjectDataList = WorldInteractableObjectDataController.Data.dataList.Cast<GameWorldInteractableElementData>().ToList().Where(x => x.DataElement == null && 
                                                                                                                                                               x.TerrainTileId == terrainTileId).ToList();

        worldInteractableObjectDataList.ForEach(elementData => 
        {
            SetElement(WorldInteractableObjectDataController, elementData, gameWorldInteractableObjectPrefab, (int)elementData.Type);
        });
    }
    
    private void SetWorldInteractableControllables()
    {
        var worldInteractableControllableDataList = WorldInteractableControllableDataController.Data.dataList.Cast<GameWorldInteractableElementData>().ToList().ToList();

        worldInteractableControllableDataList.ForEach(elementData => 
        {
            if (elementData.DataElement == null)
            {
                SetElement(WorldInteractableControllableDataController, elementData, gameWorldControllablePrefab, (int)elementData.Type);

            } else {

                PlayerControlManager.instance.SetWorldInteractableControllableTerrainTileId(elementData);
            }
        });
    }
    
    private void SetSceneProps(int terrainTileId)
    {
        var scenePropDataList = ScenePropDataController.Data.dataList.Where(x => ((GameScenePropElementData)x).TerrainTileId == terrainTileId).ToList();
        scenePropDataList.ForEach(elementData => SetElement(ScenePropDataController, elementData, gameWorldObjectPrefab));
    }

    private void SetWorldInteractable(GameWorldInteractableElementData gameWorldInteractableElementData)
    {
        switch (gameWorldInteractableElementData.Type)
        {
            case Enums.InteractableType.Agent:  SetElement(WorldInteractableAgentDataController, gameWorldInteractableElementData, gameWorldAgentPrefab, (int)gameWorldInteractableElementData.Type);                   break;
            case Enums.InteractableType.Object: SetElement(WorldInteractableObjectDataController, gameWorldInteractableElementData, gameWorldInteractableObjectPrefab, (int)gameWorldInteractableElementData.Type);     break;

            default: Debug.Log("CASE MISSING: " + gameWorldInteractableElementData.Type); break;
        }
    }

    private void SetSceneProp(GameScenePropElementData gameScenePropElementData)
    {
        SetElement(ScenePropDataController, gameScenePropElementData, gameWorldObjectPrefab);
    }

    private void SetElement(IDataController dataController, IElementData elementData, IPoolable prefab, int poolableId = 0)
    {        
        var element = (IGameElement)PoolManager.SpawnObject(prefab, poolableId);
        element.GameElement.transform.SetParent(CameraManager.content, false);

        elementData.DataElement = element.GameElement.DataElement;

        element.GameElement.DataElement.Data = dataController.Data;
        element.GameElement.DataElement.Id = elementData.Id;
            
        //Debugging
        element.GameElement.name = elementData.DebugName + elementData.Id;

        element.GameElement.InitializeElement();

        element.GameElement.gameObject.SetActive(true);

        element.GameElement.SetElement();
    }
    
    private void ResetWorldInteractable(GameWorldInteractableElementData gameWorldInteractableElementData)
    {
        CloseWorldInteractable(gameWorldInteractableElementData);
        SetWorldInteractable(gameWorldInteractableElementData);
    }

    public void UpdateWorldInteractable(GameWorldInteractableElementData gameWorldInteractableElementData)
    {        
        //Set interactable's destination position based on its interaction's destination
        if(gameWorldInteractableElementData.Type != Enums.InteractableType.Controllable && 
           gameWorldInteractableElementData.DestinationType == DestinationType.Interaction)
        {
            //If the active world interactable contains no active time, deactivate it
            if (gameWorldInteractableElementData.ActiveInteraction == null)
            {
                gameWorldInteractableElementData.TerrainTileId = 0;

            } else {

                MovementManager.UpdateInteractionDestination(gameWorldInteractableElementData);
            }
        }
        
        var onActiveTile = tileList.Select(x => x.Id).Contains(gameWorldInteractableElementData.TerrainTileId);

        if (gameWorldInteractableElementData.DataElement == null)
        {
            //Spawn interactables without an element that are deemed active
            if (onActiveTile)
            {
                SetWorldInteractable(gameWorldInteractableElementData);

            } else {
                
                //Move the agent off-screen
                if (gameWorldInteractableElementData.Type == Enums.InteractableType.Agent)
                {
                    MovementManager.StartTravel(gameWorldInteractableElementData);
                }
            }

        } else {

            if (onActiveTile)
            {
                //Update the destination of active agents
                if (gameWorldInteractableElementData.Type == Enums.InteractableType.Agent)
                {
                    gameWorldInteractableElementData.DataElement.SelectionElement.Element.UpdateElement();
                }

                //Reset the interactable instead of just changing the position so a "disappearing" effect can be applied
                if (gameWorldInteractableElementData.Type == Enums.InteractableType.Object)
                {
                    ResetWorldInteractable(gameWorldInteractableElementData);
                }

            } else {

                //Despawn interactables with an element that are deemed inactive
                CloseWorldInteractable(gameWorldInteractableElementData);
            }
        }
    }

    public void UpdateSceneProp(GameScenePropElementData gameScenePropElementData)
    {
        var onActiveTile = tileList.Select(x => x.Id).Contains(gameScenePropElementData.TerrainTileId);

        if(onActiveTile)
            SetSceneProp(gameScenePropElementData);   
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
        var inactiveTiles = tileList.Where(x => !activeRect.Overlaps(x.GridElement.rect, true)).ToList();
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
            SetData(DataController.Data);

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
    
    private void ClearTiles(List<GameTerrainTileElementData> inactiveTileList)
    {
        inactiveTileList.ForEach(x =>
        {
            ClearTileElements(x);
            PoolManager.ClosePoolObject(x.DataElement.Poolable);
        });

        tileList.RemoveAll(x => inactiveTileList.Contains(x));
    }

    private void ClearTileElements(GameTerrainTileElementData gameTerrainTileData)
    {
        ClearWorldObjects(gameTerrainTileData);
        ClearWorldInteractables(gameTerrainTileData);
        ClearSceneProps(gameTerrainTileData);
    }

    private void ClearWorldObjects(GameTerrainTileElementData gameTerrainTileData)
    {
        var inactiveWorldObjectList = RegionData.GameTerrainDataList.SelectMany(x => x.GameWorldObjectDataList.Where(y => y.TerrainTileId == gameTerrainTileData.Id)).ToList();

        inactiveWorldObjectList.ForEach(x =>
        {
            PoolManager.ClosePoolObject(x.DataElement.Poolable);
            x.DataElement.SelectionElement.Element.CloseElement();
        });
    }

    private void ClearWorldInteractables(GameTerrainTileElementData gameTerrainTileData)
    {
        var inactiveWorldInteractableList = GameWorldData.WorldInteractableDataList.Where(x => x.TerrainTileId == gameTerrainTileData.Id).ToList();
        
        inactiveWorldInteractableList.ForEach(x =>
        {
            CloseWorldInteractable(x);
        });
    }
    
    private void CloseWorldInteractable(GameWorldInteractableElementData gameWorldInteractableElementData)
    {
        PoolManager.ClosePoolObject(gameWorldInteractableElementData.DataElement.Poolable);
        gameWorldInteractableElementData.DataElement.SelectionElement.Element.CloseElement();

        if(gameWorldInteractableElementData.Type == Enums.InteractableType.Agent)
            MovementManager.StartTravel(gameWorldInteractableElementData);
    }

    private void ClearSceneProps(GameTerrainTileElementData gameTerrainTileData)
    {
        var inactiveSceneProps = ScenePropDataController.Data.dataList.Cast<GameScenePropElementData>().ToList().Where(x => x.TerrainTileId == gameTerrainTileData.Id).ToList();

        inactiveSceneProps.ForEach(x =>
        {
            CloseSceneProp(x);
        });
    }

    public void CloseSceneProp(GameScenePropElementData gameScenePropElementData)
    {
        if (gameScenePropElementData.DataElement == null) return;

        PoolManager.ClosePoolObject(gameScenePropElementData.DataElement.Poolable);
        gameScenePropElementData.DataElement.SelectionElement.Element.CloseElement();
    }

    public void CloseOrganizer()
    {
        GameManager.instance.CloseGame();

        ClearOrganizer();

        positionTracker = new Vector2();
        
        Debug.Log("Close for real");
        DestroyImmediate(this);
    }
}
