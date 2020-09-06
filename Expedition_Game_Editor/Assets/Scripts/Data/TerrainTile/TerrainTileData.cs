using UnityEngine;

public class TerrainTileData : TerrainTileBaseData
{
    public bool Active              { get; set; }

    public GridElement GridElement  { get; set; }

    public string IconPath          { get; set; }

    public override void GetOriginalValues(TerrainTileData originalData)
    {
        Active = originalData.Active;

        GridElement = originalData.GridElement;

        IconPath = originalData.IconPath;

        base.GetOriginalValues(originalData);
    }

    public TerrainTileData Clone()
    {
        var data = new TerrainTileData();
        
        data.Active = Active;

        data.GridElement = GridElement;

        data.IconPath = IconPath;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(TerrainTileElementData elementData)
    {
        elementData.Active = Active;

        elementData.GridElement = GridElement;

        elementData.IconPath = IconPath;

        base.Clone(elementData);
    }
}
