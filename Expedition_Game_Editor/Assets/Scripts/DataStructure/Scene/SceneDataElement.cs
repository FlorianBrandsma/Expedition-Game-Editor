using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SceneDataElement : GeneralData, IDataElement
{
    public Enums.RegionType regionType;

    public int regionSize;
    public int terrainSize;
    public float tileSize;

    public string tileSetName;

    public Vector2 startPosition;

    public List<TerrainData> terrainDataList;

    public class TerrainData : GeneralData
    {
        public int regionId;
        public string name;

        public List<TerrainTileDataElement> terrainTileDataList;
        public List<SceneInteractableDataElement> sceneInteractableDataList;
        public List<InteractionDataElement> interactionDataList;
        public List<SceneObjectDataElement> sceneObjectDataList;
        
        public void SetOriginalValues()
        {
            terrainTileDataList.ForEach(x => x.SetOriginalValues());
            sceneInteractableDataList.ForEach(x => x.SetOriginalValues());
            interactionDataList.ForEach(x => x.SetOriginalValues());
            sceneObjectDataList.ForEach(x => x.SetOriginalValues());
        }
    }

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

    #endregion
}
