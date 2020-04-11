using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WorldDataElement : GeneralData, IDataElement
{
    public class TerrainData : GeneralData
    {
        public int regionId;

        public string name;

        public List<TerrainTileDataElement> terrainTileDataList;
        public List<WorldInteractableDataElement> worldInteractableDataList;
        public List<InteractionDataElement> interactionDataList;
        public List<WorldObjectDataElement> worldObjectDataList;

        public void SetOriginalValues()
        {
            terrainTileDataList.ForEach(x => x.SetOriginalValues());
            worldInteractableDataList.ForEach(x => x.SetOriginalValues());
            interactionDataList.ForEach(x => x.SetOriginalValues());
            worldObjectDataList.ForEach(x => x.SetOriginalValues());
        }

        public void Copy(TerrainData terrainDataSource)
        {
            regionId = terrainDataSource.regionId;

            name = terrainDataSource.name;

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
        }
    }

    public Enums.RegionType regionType;

    public int regionSize;
    public int terrainSize;
    public float tileSize;

    public string tileSetName;

    public Vector2 startPosition;

    public List<TerrainData> terrainDataList;
    
    #region DataElement
    public SelectionElement SelectionElement { get; set; }

    public bool Changed { get; set; }

    public void Update() { }

    public void UpdateSearch() { }

    public void UpdateIndex() { }

    public void SetOriginalValues()
    {
        terrainDataList.ForEach(x => x.SetOriginalValues());
    }

    public void GetOriginalValues() { }

    public void ClearChanges() { }

    public IDataElement Clone()
    {
        var dataElement = new WorldDataElement();

        CloneGeneralData(dataElement);

        return dataElement;
    }

    new public virtual void Copy(IDataElement dataSource)
    {
        var worldDataSource = (WorldDataElement)dataSource;

        regionType = worldDataSource.regionType;

        regionSize = worldDataSource.regionSize;
        terrainSize = worldDataSource.terrainSize;
        tileSize = worldDataSource.tileSize;

        tileSetName = worldDataSource.tileSetName;

        startPosition = worldDataSource.startPosition;

        for(int i = 0; i < terrainDataList.Count; i++)
        {
            var terrainDataSource = worldDataSource.terrainDataList[i];
            terrainDataList[i].Copy(terrainDataSource);
        }        
    }
    #endregion
}
