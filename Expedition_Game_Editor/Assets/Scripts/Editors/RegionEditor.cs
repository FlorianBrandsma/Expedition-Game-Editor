using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RegionEditor : MonoBehaviour, IEditor
{
    public RegionDataElement RegionData { get { return (RegionDataElement)Data.dataElement; } }

    private List<SegmentController> editorSegments = new List<SegmentController>();

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get; set; }

    public Route.Data Data { get { return PathController.route.data; } }

    public List<IDataElement> DataList
    {
        get { return SelectionElementManager.FindDataElements(RegionData).Concat(new[] { RegionData }).ToList(); }
    }

    public List<IDataElement> DataElements
    {
        get
        {
            var list = new List<IDataElement>();

            DataList.ForEach(x => list.Add(x));

            return list;
        }
    }

    public List<SegmentController> EditorSegments
    {
        get { return editorSegments; }
    }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void SetEditor()
    {
        PathController.editorSection.SetActionButtons();
    }

    public bool Changed()
    {
        return DataElements.Any(x => x.Changed);
    }

    public void ApplyChanges()
    {
        if (RegionData.changedName)
            ChangedName();

        if (RegionData.changedTileSetId)
            ChangedTileSet();

        RegionData.Update();

        DataElements.ForEach(x =>
        {
            if (((GeneralData)x).Equals(RegionData))
                x.SetOriginalValues();
            else
                x.Update();

            if (x.SelectionElement != null)
                x.SelectionElement.UpdateElement();
        });

        UpdateEditor();
    }

    private void ChangedName()
    {
        var chapterRegions = Fixtures.chapterRegionList.Where(x => x.regionId == RegionData.Id).Distinct().ToList();
        var regions = Fixtures.regionList.Where(x => chapterRegions.Select(y => y.Id).Contains(x.chapterRegionId)).Distinct().ToList();

        regions.ForEach(x => x.name = RegionData.Name);
    }

    private void ChangedTileSet()
    {
        var chapterRegions = Fixtures.chapterRegionList.Where(x => x.regionId == RegionData.Id).Distinct().ToList();
        var regions = Fixtures.regionList.Where(x => chapterRegions.Select(y => y.Id).Contains(x.chapterRegionId)).Distinct().ToList();

        regions.ForEach(x => x.tileSetId = RegionData.TileSetId);

        regions.Add(Fixtures.regionList.Where(x => x.Id == RegionData.Id).FirstOrDefault());

        var terrains = Fixtures.terrainList.Where(x => regions.Select(y => y.Id).Distinct().ToList().Contains(x.regionId)).Distinct().ToList();
        var terrainTiles = Fixtures.terrainTileList.Where(x => terrains.Select(y => y.Id).Distinct().ToList().Contains(x.terrainId)).Distinct().ToList();
        var firstTile = Fixtures.tileList.Where(x => x.tileSetId == RegionData.TileSetId).FirstOrDefault();
        
        terrainTiles.ForEach(x => x.tileId = firstTile.Id);
        
        var interactions = Fixtures.interactionList.Where(x => regions.Select(y => y.Id).Contains(x.regionId)).Distinct().ToList();

        Fixtures.sceneInteractableList.RemoveAll(x => interactions.Where(y => y.objectiveId == 0).Select(y => y.sceneInteractableId).Contains(x.Id));
        Fixtures.sceneObjectList.RemoveAll(x => regions.Select(y => y.Id).Contains(x.regionId));

        Fixtures.interactionList.RemoveAll(x => interactions.Where(y => y.objectiveId == 0).Select(y => y.Id).Contains(x.Id));
    }

    public void CancelEdit()
    {
        DataElements.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}
