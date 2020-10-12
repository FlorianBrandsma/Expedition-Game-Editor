using UnityEngine;
using System.Collections.Generic;

public class GameTerrainData
{
    public int Id                   { get; set; }

    public int Index                { get; set; }

    public string Name              { get; set; }

    public GridElement GridElement  { get; set; }

    public List<GameAtmosphereElementData> AtmosphereDataList               { get; set; } = new List<GameAtmosphereElementData>();
    public List<GameTerrainTileElementData> TerrainTileDataList             { get; set; } = new List<GameTerrainTileElementData>();
    public List<GameWorldInteractableElementData> WorldInteractableDataList { get; set; } = new List<GameWorldInteractableElementData>();
    public List<GameWorldObjectElementData> WorldObjectDataList             { get; set; } = new List<GameWorldObjectElementData>();

    public virtual void GetOriginalValues(GameTerrainData originalData)
    {
        Id          = originalData.Id;

        Index       = originalData.Index;

        Name        = originalData.Name;

        GridElement = originalData.GridElement;
    }

    public GameTerrainData Clone()
    {
        var data = new GameTerrainData();
        
        data.Id             = Id;

        data.Index          = Index;

        data.Name           = Name;

        data.GridElement    = GridElement;

        return data;
    }

    public virtual void Clone(GameTerrainElementData elementData)
    {
        elementData.Id          = Id;

        elementData.Index       = Index;

        elementData.Name        = Name;

        elementData.GridElement = GridElement;
    }
}
