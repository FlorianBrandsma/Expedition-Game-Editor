using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour, IPoolable
{
    public Enums.DataType DataType  { get; set; }
    public IElementData ElementData { get; set; }

    public GameElement GameElement  { get { return GetComponent<GameElement>(); } }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return Enums.ElementType.Tile; } }
    public int PoolId                           { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

    public IPoolable Instantiate()
    {
        return Instantiate(this);
    }

    public void ClosePoolable()
    {
        switch(DataType)
        {
            case Enums.DataType.TerrainTile:        CloseTerrainTile();     break;
            case Enums.DataType.GameTerrainTile:    CloseGameTerrainTile(); break;

            default: Debug.Log("CASE MISSING: " + DataType); break;
        }

        ElementData = null;

        gameObject.SetActive(false);
    }

    private void CloseTerrainTile()
    {
        var terrainTileData = (TerrainTileElementData)ElementData;

        terrainTileData.Active = false;
    }

    private void CloseGameTerrainTile()
    {
        var terrainTileData = (GameTerrainTileElementData)ElementData;

        terrainTileData.Active = false;
    }
}
