using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TerrainDataElement : TerrainCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public TerrainDataElement() : base()
    {
        DataType = Enums.DataType.Terrain;
    }

    public GridElement gridElement;

    public int tileSetId;

    public string iconPath;
    public string baseTilePath;
    
    //Original
    public string originalIconPath;

    //List
    public List<AtmosphereDataElement> atmosphereDataList = new List<AtmosphereDataElement>();
    public List<TerrainTileDataElement> terrainTileDataList = new List<TerrainTileDataElement>();
    public List<WorldInteractableDataElement> worldInteractableDataList = new List<WorldInteractableDataElement>();
    public List<InteractionDataElement> interactionDataList = new List<InteractionDataElement>();
    public List<WorldObjectDataElement> worldObjectDataList = new List<WorldObjectDataElement>();

    public override void Update()
    {
        if (!Changed) return;

        base.Update();

        SetOriginalValues();
    }

    public override void SetOriginalValues()
    {
        base.SetOriginalValues();

        originalIconPath = iconPath;

        atmosphereDataList.ForEach(x => x.SetOriginalValues());
        terrainTileDataList.ForEach(x => x.SetOriginalValues());
        worldInteractableDataList.ForEach(x => x.SetOriginalValues());
        interactionDataList.ForEach(x => x.SetOriginalValues());
        worldObjectDataList.ForEach(x => x.SetOriginalValues());

        ClearChanges();
    }

    public new void GetOriginalValues()
    {
        iconPath = originalIconPath;
    }

    public override void ClearChanges()
    {
        if (!Changed) return;

        base.ClearChanges();

        GetOriginalValues();
    }

    public IDataElement Clone()
    {
        var dataElement = new TerrainDataElement();

        CloneGeneralData(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        var terrainDataSource = (TerrainDataElement)dataSource;

        tileSetId = terrainDataSource.tileSetId;

        iconPath = terrainDataSource.iconPath;
        baseTilePath = terrainDataSource.baseTilePath;

        for (int i = 0; i < atmosphereDataList.Count; i++)
        {
            var atmosphereDataSource = terrainDataSource.atmosphereDataList[i];
            atmosphereDataList[i].Copy(atmosphereDataSource);
        }

        for (int i = 0; i < terrainTileDataList.Count; i++)
        {
            var terrainTileDataSource = terrainDataSource.terrainTileDataList[i];
            terrainTileDataList[i].Copy(terrainTileDataSource);
        }

        for (int i = 0; i < worldInteractableDataList.Count; i++)
        {
            var worldInteractableDataSource = terrainDataSource.worldInteractableDataList[i];
            worldInteractableDataList[i].Copy(worldInteractableDataSource);
        }

        for (int i = 0; i < interactionDataList.Count; i++)
        {
            var interactionDataSource = terrainDataSource.interactionDataList[i];
            interactionDataList[i].Copy(interactionDataSource);
        }

        for (int i = 0; i < worldObjectDataList.Count; i++)
        {
            var worldObjectDataSource = terrainDataSource.worldObjectDataList[i];
            worldObjectDataList[i].Copy(worldObjectDataSource);
        }

        SetOriginalValues();
    }
}