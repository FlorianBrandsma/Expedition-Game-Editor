using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour, IPoolable
{
    public Enums.DataType DataType  { get; set; }
    public IElementData ElementData { get; set; }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return Enums.ElementType.Tile; } }
    public int Id                           { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

    public IPoolable Instantiate()
    {
        return Instantiate(this);
    }

    public void ClosePoolable()
    {
        switch(DataType)
        {
            case Enums.DataType.TerrainTile: CloseTerrainTile(); break;

            default: Debug.Log("CASE MISSING: " + DataType); break;
        }

        ElementData = null;

        gameObject.SetActive(false);
    }

    private void CloseTerrainTile()
    {
        var terrainTileData = (TerrainTileElementData)ElementData;

        terrainTileData.active = false;
    }
}
