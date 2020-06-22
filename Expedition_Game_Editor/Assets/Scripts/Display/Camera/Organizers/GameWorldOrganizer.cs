using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameWorldOrganizer : MonoBehaviour, IOrganizer
{
	private List<Tile> tileList = new List<Tile>();
	
	private GameWorldElementData gameWorldData;
	private RegionElementData regionData;

	private ExGameWorldElement gameWorldElementPrefab;
	private ExGameWorldAgent gameWorldAgentPrefab;

	private Rect activeRect;

	private IDisplayManager DisplayManager      { get { return GetComponent<IDisplayManager>(); } }
	private CameraManager CameraManager         { get { return (CameraManager)DisplayManager; } }

	private CameraProperties CameraProperties   { get { return (CameraProperties)DisplayManager.Display; } }
	private ObjectProperties ObjectProperties   { get { return (ObjectProperties)DisplayManager.Display.Properties; } }

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
		if (GameManager.instance.ActiveRegionId == 0) return;

		Debug.Log("Organize game data: " + list.Count);

		gameWorldData = (GameWorldElementData)list.First();

		SetCameraPosition();
		SetActiveRect();
		
		//var worldInteractables  = gameWorldData.regionDataList.SelectMany(x => x.terrainDataList.SelectMany(y => y.interactionDataList.Select(z => z.worldInteractableId))).Distinct().ToList();
		//var interactions        = gameWorldData.regionDataList.SelectMany(x => x.terrainDataList.SelectMany(y => y.interactionDataList.Where(z => worldInteractables.Contains(z.worldInteractableId)))).ToList();

		//Debug.Log(worldInteractables.Count + ":" + interactions.Count);

		regionData = gameWorldData.regionDataList.Where(x => x.Id == GameManager.instance.ActiveRegionId).First();
		
		foreach (TerrainElementData terrainData in regionData.terrainDataList)
		{
			SetTerrain(terrainData);
		}

		//Set elements that are not bound to a tile
		SetWorldObjects();

		//GetActiveTerrain();

		//GameManager.instance.localNavMeshBuilder.UpdateNavMesh();

		//Spawn "agents" by going through ALL interaction data and add one as a world interactable when the interaction's world interactable is not found
	}

	private void SetTerrain(TerrainElementData terrainData)
	{
		if (!activeRect.Overlaps(terrainData.gridElement.rect, true)) return;

		foreach (TerrainTileElementData terrainTileData in terrainData.terrainTileDataList)
		{
			if (terrainTileData.active || !activeRect.Overlaps(terrainTileData.gridElement.rect, true)) continue;

			SetTerrainTile(terrainTileData);

			//Set objects that are bound to this terrain tile
			SetWorldObjects(terrainTileData.Id);

			//var worldObjectList = terrainData.worldObjectDataList.Where(x => x.TerrainTileId == terrainTileData.Id).ToList();

			//foreach(WorldObjectDataElement worldObjectData in worldObjectList)
			//{
			//	SetWorldObject(worldObjectData);
			//}
		}
	}

	private void SetTerrainTile(TerrainTileElementData terrainTileData)
	{
		Tile prefab = Resources.Load<Tile>("Objects/Tile/" + regionData.tileSetName + "/" + terrainTileData.TileId);

		Tile tile = (Tile)PoolManager.SpawnObject(prefab, terrainTileData.TileId);
		tileList.Add(tile);

		tile.gameObject.SetActive(true);

		tile.transform.SetParent(CameraManager.content, false);
		tile.transform.localPosition = new Vector3(terrainTileData.gridElement.startPosition.x, 0, terrainTileData.gridElement.startPosition.y);

		tile.DataType = Enums.DataType.TerrainTile;
		tile.ElementData = terrainTileData;

		terrainTileData.active = true;
	}

	private void SetWorldObjects(int terrainTileId = 0)
	{
		var worldObjectDataList = regionData.terrainDataList.SelectMany(x => x.worldObjectDataList.Where(y => y.TerrainTileId == terrainTileId)).Cast<IElementData>().ToList();

		SetWorldElements(worldObjectDataList);
	}

	private void SetWorldElements(List<IElementData> dataList)
	{
		if (dataList.Count == 0) return;
		
		foreach(IElementData elementData in dataList)
		{
			var gameWorldElement = (ExGameWorldElement)PoolManager.SpawnObject(gameWorldElementPrefab);

			gameWorldElement.GameElement.transform.SetParent(CameraManager.content, false);

			gameWorldElement.GameElement.DataElement.data.elementData = elementData;
			
			//Debugging
			GeneralData generalData = (GeneralData)elementData;
			gameWorldElement.name = generalData.DebugName + generalData.Id;
			//

			elementData.DataElement = gameWorldElement.GameElement.DataElement;

			SetElement(gameWorldElement.GameElement);
		}
	}

	//private void SetWorldElements(IDataController dataController, List<IDataElement> dataList)
	//{
	//    if (dataList.Count == 0) return;

	//    var prefab = Resources.Load<ExEditorWorldElement>("Elements/World/EditorWorldElement");

	//    foreach (IDataElement dataElement in dataList)
	//    {
	//        var worldElement = (ExEditorWorldElement)PoolManager.SpawnObject(prefab);

	//        SelectionElementManager.InitializeElement(worldElement.Element, CameraManager.content,
	//                                                    DisplayManager,
	//                                                    DisplayManager.Display.SelectionType,
	//                                                    DisplayManager.Display.SelectionProperty);

	//        worldElement.Element.data.dataController = dataController;
	//        worldElement.Element.data = new SelectionElement.Data(dataController, dataElement);

	//        //Debugging
	//        GeneralData generalData = (GeneralData)dataElement;
	//        worldElement.name = generalData.DebugName + generalData.Id;
	//        //

	//        SetStatus(worldElement.Element);

	//        if (worldElement.Element.elementStatus == Enums.ElementStatus.Hidden)
	//        {
	//            PoolManager.ClosePoolObject(worldElement.Element.Poolable);
	//            SelectionElementManager.CloseElement(worldElement.Element);

	//            continue;
	//        }

	//        dataElement.SelectionElement = worldElement.Element;

	//        SetElement(worldElement.Element);
	//    }
	//}

	private void SetElement(GameElement element)
	{
		element.gameObject.SetActive(true);

		element.SetElement();
	}

	public void UpdateData()
	{
		Debug.Log("Update game data");
		
		//if (ScrollRect.content.localPosition.x >= positionTracker.x + worldData.tileSize ||
		//    ScrollRect.content.localPosition.x <= positionTracker.x - worldData.tileSize ||
		//    ScrollRect.content.localPosition.y >= positionTracker.y + worldData.tileSize ||
		//    ScrollRect.content.localPosition.y <= positionTracker.y - worldData.tileSize)
		//{
		CloseInactiveElements();

		SetData(DataController.DataList);
		//}
	}
	
	public void SetCameraPosition()
	{
		var cameraTransform = CameraManager.cam.transform;

		cameraTransform.localPosition = new Vector3(gameWorldData.tempPlayerPosition.x, 10, gameWorldData.tempPlayerPosition.y - 10);

		SetActiveRect();
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
		var inactiveTiles = tileList.Where(x => !activeRect.Overlaps(((TerrainTileElementData)x.ElementData).gridElement.rect, true)).ToList();
		ClearTiles(inactiveTiles);
	}

	public void ResetData(List<IElementData> filter)
	{
		CloseOrganizer();
		SetData();
	}

	public void ClearOrganizer()
	{
		ClearTiles(tileList);

		//SelectionElementManager.CloseElement(elementList);
	}

	private void ClearTiles(List<Tile> inactiveTileList)
	{
		inactiveTileList.ForEach(x =>
		{
			ClearTileElements((TerrainTileElementData)x.ElementData);
			PoolManager.ClosePoolObject(x);
		});

		tileList.RemoveAll(x => inactiveTileList.Contains(x));
	}

	private void ClearTileElements(TerrainTileElementData terrainTileData)
	{
		ClearWorldObjects(terrainTileData);
		//ClearWorldInteractableAgents(terrainTileData);
		//ClearWorldInteractableObjects(terrainTileData);
	}

	private void ClearWorldObjects(TerrainTileElementData terrainTileData)
	{
		var inactiveWorldObjectList = regionData.terrainDataList.SelectMany(x => x.worldObjectDataList.Where(y => y.TerrainTileId == terrainTileData.Id)).ToList();

		inactiveWorldObjectList.ForEach(x =>
		{
			PoolManager.ClosePoolObject(x.DataElement.Poolable);
			x.DataElement.Element.CloseElement();
		});
	}

	//private void ClearWorldInteractableAgents(TerrainTileDataElement terrainTileData)
	//{
	//    var inactiveWorldInteractableAgentList = worldInteractableAgentElementList.Where(x => x.terrainTileId == terrainTileData.Id).ToList();

	//    inactiveWorldInteractableAgentList.ForEach(x =>
	//    {
	//        PoolManager.ClosePoolObject(x.SelectionElement.Poolable);
	//        SelectionElementManager.CloseElement(x.SelectionElement);
	//    });
	//}

	//private void ClearWorldInteractableObjects(TerrainTileDataElement terrainTileData)
	//{
	//    var inactiveWorldInteractableObjectList = worldInteractableObjectElementList.Where(x => x.terrainTileId == terrainTileData.Id).ToList();

	//    inactiveWorldInteractableObjectList.ForEach(x =>
	//    {
	//        PoolManager.ClosePoolObject(x.SelectionElement.Poolable);
	//        SelectionElementManager.CloseElement(x.SelectionElement);
	//    });
	//}

	public void CloseOrganizer()
	{
		ClearOrganizer();

		DestroyImmediate(this);
	}
}
