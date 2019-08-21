using UnityEngine;
using System.Collections.Generic;

public class SceneDataElement : GeneralData, IDataElement
{
    public List<TerrainData> terrainDataList;

    public int regionSize;
    public int terrainSize;

    public string tileSetName;
    public float tileSize;

    public class TerrainData : GeneralData
    {
        public List<TerrainTileData> terrainTileDataList;

        public List<InteractionData> interactionDataList;

        public List<SceneObjectData> sceneObjectDataList;

        public class TerrainTileData : GeneralData
        {
            public int tileId;
        }

        public class InteractionData : GeneralData
        {

        }

        public class SceneObjectData : GeneralData
        {

        }
    }

    #region DataElement

    public SelectionElement SelectionElement { get; set; }

    public int Id { get; set; }

    public bool Changed { get; set; }

    public void Update() { }

    public void UpdateSearch() { }

    public void SetOriginalValues() { }

    public void GetOriginalValues() { }

    public void ClearChanges() { }

    #endregion
}
