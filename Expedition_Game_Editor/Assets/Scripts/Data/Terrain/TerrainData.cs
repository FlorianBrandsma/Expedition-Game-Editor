using UnityEngine;
using System.Linq;

public class TerrainData : TerrainBaseData
{
    public GridElement GridElement  { get; set; }

    public int TileSetId            { get; set; }

    public string IconPath          { get; set; }
    public string BaseTilePath      { get; set; }
    
    public override void GetOriginalValues(TerrainData originalData)
    {
        GridElement = originalData.GridElement;

        TileSetId = originalData.TileSetId;

        IconPath = originalData.IconPath;
        BaseTilePath = originalData.BaseTilePath;
        
        base.GetOriginalValues(originalData);
    }

    public TerrainData Clone()
    {
        var data = new TerrainData();
        
        data.GridElement = GridElement;

        data.TileSetId = TileSetId;

        data.IconPath = IconPath;
        data.BaseTilePath = BaseTilePath;
        
        base.Clone(data);

        return data;
    }

    public virtual void Clone(TerrainElementData elementData)
    {
        elementData.GridElement = GridElement;

        elementData.TileSetId = TileSetId;

        elementData.IconPath = IconPath;
        elementData.BaseTilePath = BaseTilePath;
        
        base.Clone(elementData);
    }
}
