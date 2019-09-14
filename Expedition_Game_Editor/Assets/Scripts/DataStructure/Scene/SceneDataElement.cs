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
        public int regionId;
        public string name;

        public List<TerrainTileDataElement> terrainTileDataList;
        public List<InteractionDataElement> interactionDataList;
        public List<SceneObjectDataElement> sceneObjectDataList;
        
        public void SetOriginalValues()
        {
            terrainTileDataList.ForEach(x => x.SetOriginalValues());
            interactionDataList.ForEach(x => x.SetOriginalValues());
            sceneObjectDataList.ForEach(x => x.SetOriginalValues());
        }
    }

    #region DataElement

    public SelectionElement SelectionElement { get; set; }

    public int Id { get; set; }

    public bool Changed { get; set; }

    public void Update() { }

    public void UpdateSearch() { }

    public void SetOriginalValues()
    {
        terrainDataList.ForEach(x => x.SetOriginalValues());
    }

    public void GetOriginalValues() { }

    public void ClearChanges() { }

    #endregion
}
