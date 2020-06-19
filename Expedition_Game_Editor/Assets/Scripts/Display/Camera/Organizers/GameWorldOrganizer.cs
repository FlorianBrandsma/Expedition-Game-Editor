using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameWorldOrganizer : MonoBehaviour, IOrganizer
{
	private List<Tile> tileList = new List<Tile>();
	private List<GameElement> elementList = new List<GameElement>();
	
	private GameWorldDataElement gameWorldData;
	private RegionDataElement regionData;

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

	public void SetData(List<IDataElement> list)
	{
		if (GameManager.instance.ActiveRegionId == 0) return;

		Debug.Log("Organize game data: " + list.Count);

		gameWorldData = (GameWorldDataElement)list.First();

		InitializeCamera();
		
		//var worldInteractables  = gameWorldData.regionDataList.SelectMany(x => x.terrainDataList.SelectMany(y => y.interactionDataList.Select(z => z.worldInteractableId))).Distinct().ToList();
		//var interactions        = gameWorldData.regionDataList.SelectMany(x => x.terrainDataList.SelectMany(y => y.interactionDataList.Where(z => worldInteractables.Contains(z.worldInteractableId)))).ToList();

		//Debug.Log(worldInteractables.Count + ":" + interactions.Count);

		regionData = gameWorldData.regionDataList.Where(x => x.Id == GameManager.instance.ActiveRegionId).First();
		
		foreach (TerrainDataElement terrainData in regionData.terrainDataList)
		{
			SetTerrain(terrainData);
		}

		GameManager.instance.localNavMeshBuilder.UpdateNavMesh();

		//Spawn "agents" by going through ALL interaction data and add one as a world interactable when the interaction's world interactable is not found
	}

	private void SetTerrain(TerrainDataElement terrainData)
	{
		if (!activeRect.Overlaps(terrainData.gridElement.rect, true)) return;

		foreach (TerrainTileDataElement terrainTileData in terrainData.terrainTileDataList)
		{
			if (terrainTileData.active || !activeRect.Overlaps(terrainTileData.gridElement.rect, true)) continue;

			SetTerrainTile(terrainTileData);

			var worldObjectList = terrainData.worldObjectDataList.Where(x => x.TerrainTileId == terrainTileData.Id).ToList();
			
			foreach(WorldObjectDataElement worldObjectData in worldObjectList)
			{
				SetWorldObject(worldObjectData);
			}
		}
	}

	private void SetTerrainTile(TerrainTileDataElement terrainTileData)
	{
		Tile prefab = Resources.Load<Tile>("Objects/Tile/" + regionData.tileSetName + "/" + terrainTileData.TileId);

		Tile tile = (Tile)PoolManager.SpawnObject(prefab, terrainTileData.TileId);
		tileList.Add(tile);

		tile.gameObject.SetActive(true);

		tile.transform.SetParent(CameraManager.content, false);
		tile.transform.localPosition = new Vector3(terrainTileData.gridElement.startPosition.x, 0, terrainTileData.gridElement.startPosition.y);

		tile.DataType = Enums.DataType.TerrainTile;
		tile.DataElement = terrainTileData;

		terrainTileData.active = true;
	}

	//Could be a general "SetWorldElement" like editor world organizer
	private void SetWorldObject(WorldObjectDataElement worldObjectData)
	{
		var gameWorldElement = (ExGameWorldElement)PoolManager.SpawnObject(gameWorldElementPrefab);
		elementList.Add(gameWorldElement.Element);

		gameWorldElement.Element.DataElement = worldObjectData;

		gameWorldElement.Element.transform.SetParent(CameraManager.content, false);

		//Debugging
		GeneralData generalData = worldObjectData;
		gameWorldElement.name = generalData.DebugName + generalData.Id;
		//

		SetElement(gameWorldElement.Element);
	}

	private void SetElement(GameElement element)
	{
		element.gameObject.SetActive(true);

		element.SetElement();
	}

	public void UpdateData()
	{
		Debug.Log("Update game data");

		ClearOrganizer();
		
		SetData();
	}

	private void InitializeCamera()
	{
		var cameraTransform = CameraManager.cam.transform;

		cameraTransform.localPosition = new Vector3(gameWorldData.tempPlayerPosition.x, 10, gameWorldData.tempPlayerPosition.y - 10);

		var activeRangePosition = new Vector2(cameraTransform.localPosition.x - (GameManager.instance.TempActiveRange / 2), cameraTransform.localPosition.z + (GameManager.instance.TempActiveRange / 2));
		var activeRangeSize = new Vector2(GameManager.instance.TempActiveRange, -GameManager.instance.TempActiveRange);

		activeRect = new Rect(activeRangePosition, activeRangeSize);
	}

	public void ResetData(List<IDataElement> filter)
	{
		CloseOrganizer();
		SetData();
	}

	public void ClearOrganizer()
	{
		tileList.ForEach(x => PoolManager.ClosePoolObject(x));
		tileList.Clear();

		elementList.ForEach(x => PoolManager.ClosePoolObject(x.Poolable));
		elementList.Clear();
		//SelectionElementManager.CloseElement(elementList);
	}

	public void CloseOrganizer()
	{
		ClearOrganizer();

		DestroyImmediate(this);
	}
}
