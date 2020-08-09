using System.Collections.Generic;

public class TerrainElementData : TerrainCore, IElementData
{
    public DataElement DataElement { get; set; }

    public TerrainElementData() : base()
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
    public List<AtmosphereElementData> atmosphereDataList = new List<AtmosphereElementData>();
    public List<TerrainTileElementData> terrainTileDataList = new List<TerrainTileElementData>();
    public List<WorldInteractableElementData> worldInteractableDataList = new List<WorldInteractableElementData>();
    public List<InteractionDestinationElementData> interactionDestinationDataList = new List<InteractionDestinationElementData>();
    public List<WorldObjectElementData> worldObjectDataList = new List<WorldObjectElementData>();

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
        interactionDestinationDataList.ForEach(x => x.SetOriginalValues());
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

    public IElementData Clone()
    {
        var elementData = new TerrainElementData();

        CloneGeneralData(elementData);

        return elementData;
    }

    public override void Copy(IElementData dataSource)
    {
        base.Copy(dataSource);

        var terrainDataSource = (TerrainElementData)dataSource;

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

        for (int i = 0; i < interactionDestinationDataList.Count; i++)
        {
            var interactionDataSource = terrainDataSource.interactionDestinationDataList[i];
            interactionDestinationDataList[i].Copy(interactionDataSource);
        }

        for (int i = 0; i < worldObjectDataList.Count; i++)
        {
            var worldObjectDataSource = terrainDataSource.worldObjectDataList[i];
            worldObjectDataList[i].Copy(worldObjectDataSource);
        }

        SetOriginalValues();
    }
}