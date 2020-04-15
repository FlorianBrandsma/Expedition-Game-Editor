using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterEditor : MonoBehaviour, IEditor
{
    public ChapterDataElement ChapterData { get { return (ChapterDataElement)Data.dataElement; } }

    private List<SegmentController> editorSegments = new List<SegmentController>();

    public List<PartyMemberDataElement> partyMemberDataList;
    public List<ChapterInteractableDataElement> chapterInteractableDataList;
    public List<ChapterRegionDataElement> chapterRegionDataList;
    
    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get; set; }

    public Route.Data Data { get { return PathController.route.data; } }

    public List<IDataElement> DataList
    {
        get { return SelectionElementManager.FindDataElements(ChapterData).Concat(new[] { ChapterData }).Distinct().ToList(); }
    }

    public List<IDataElement> DataElements
    {
        get
        {
            var list = new List<IDataElement>();

            DataList.ForEach(x => list.Add(x));

            partyMemberDataList.ForEach(x => list.Add(x));
            chapterInteractableDataList.ForEach(x => list.Add(x));
            chapterRegionDataList.ForEach(x => list.Add(x));

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
        chapterRegionDataList.ForEach(x => { if (x.Changed) ChangedRegion(x); });

        ChapterData.Update();

        DataElements.ForEach(x =>
        {
            if (((GeneralData)x).Equals(ChapterData))
                x.Copy(ChapterData);
            else
                x.Update();

            if(x.SelectionElement != null)
                x.SelectionElement.UpdateElement();
        });
        
        UpdateEditor();

        UpdatePhaseElements();
    }

    private void ChangedRegion(ChapterRegionDataElement chapterRegion)
    {
        ////0. Find all (PHASE)REGIONS linked with the changed CHAPTERREGIONS
        //var phaseRegions = Fixtures.regionList.Where(x => x.chapterRegionId == chapterRegion.Id).Distinct().ToList();
        //var terrains = Fixtures.terrainList.Where(x => phaseRegions.Select(y => y.Id).Contains(x.regionId)).Distinct().ToList();
        //var terrainTiles = Fixtures.terrainTileList.Where(x => terrains.Select(y => y.Id).Contains(x.terrainId)).Distinct().ToList();
        
        ////1. Remove all WORLDOBJECTS of the selected (PHASE)REGIONS
        //var worldObjects = Fixtures.worldObjectList.Where(x => phaseRegions.Select(y => y.Id).Contains(x.regionId)).Distinct().ToList();
        //Fixtures.worldObjectList.RemoveAll(x => worldObjects.Contains(x));

        ////2. Remove all INTERACTIONS of the selected (PHASE)REGIONS where OBJECTIVEID equals 0
        //var baseInteractions = Fixtures.interactionList.Where(x => x.objectiveId == 0 && phaseRegions.Select(y => y.Id).Contains(x.regionId)).Distinct().ToList();
        //Fixtures.interactionList.RemoveAll(x => baseInteractions.Contains(x));

        ////3. Remove all WORLDINTERACTABLES linked with INTERACTIONS from step 2
        //var worldInteractables = Fixtures.worldInteractableList.Where(x => baseInteractions.Select(y => y.worldInteractableId).Contains(x.Id)).Distinct().ToList();
        //Fixtures.worldInteractableList.RemoveAll(x => worldInteractables.Contains(x));

        ////4. Set (PHASE)REGION of all INTERACTIONS linked with selected (PHASE)REGIONS to 0 where OBJECTIVE ID does not equal 0
        //var objectiveInteractions = Fixtures.interactionList.Where(x => x.objectiveId != 0 && phaseRegions.Select(y => y.Id).Contains(x.regionId)).Distinct().ToList();
        //objectiveInteractions.ForEach(x => x.regionId = 0);

        ////5. Remove all TERRAINS and TERRAINTILES of the selected (PHASE)REGIONS
        //Fixtures.terrainTileList.RemoveAll(x => terrainTiles.Contains(x));
        //Fixtures.terrainList.RemoveAll(x => terrains.Contains(x));

        ////6. Create TERRAINS and TERRAINTILES for selected (PHASE)REGIONS based on those of the newly selected CHAPTERREGION's REGION
        ////7. Also create WORLDOBJECTS, WORLDINTERACTABLES and INTERACTIONS for selected (PHASE)REGIONS based the same criteria as step 6
        //var regionSource = Fixtures.regionList.Where(x => x.Id == chapterRegion.RegionId).FirstOrDefault();

        //foreach(Fixtures.Region phaseRegion in phaseRegions)
        //{
        //    phaseRegion.tileSetId = regionSource.tileSetId;

        //    phaseRegion.name = regionSource.name;
        //    phaseRegion.regionSize = regionSource.regionSize;
        //    phaseRegion.terrainSize = regionSource.terrainSize;

        //    var terrainSourceList = Fixtures.terrainList.Where(x => x.regionId == regionSource.Id).OrderBy(x => x.Index).Distinct().ToList();

        //    foreach (Fixtures.Terrain terrainSource in terrainSourceList)
        //    {
        //        var terrain = new Fixtures.Terrain();

        //        int terrainId = Fixtures.terrainList.Count > 0 ? (Fixtures.terrainList[Fixtures.terrainList.Count - 1].Id + 1) : 1;

        //        terrain.Id = terrainId;
        //        terrain.regionId = phaseRegion.Id;

        //        terrain.Index = terrainSource.Index;

        //        terrain.iconId = terrainSource.iconId;
        //        terrain.name = terrainSource.name;

        //        var terrainTileSourceList = Fixtures.terrainTileList.Where(x => x.terrainId == terrainSource.Id).OrderBy(x => x.Index).Distinct().ToList();

        //        foreach (Fixtures.TerrainTile terrainTileSource in terrainTileSourceList)
        //        {
        //            var terrainTile = new Fixtures.TerrainTile();

        //            int terrainTileId = Fixtures.terrainTileList.Count > 0 ? (Fixtures.terrainTileList[Fixtures.terrainTileList.Count - 1].Id + 1) : 1;

        //            terrainTile.Id = terrainTileId;
        //            terrainTile.terrainId = terrain.Id;

        //            terrainTile.Index = terrainTileSource.Index;

        //            terrainTile.tileId = terrainTileSource.tileId;

        //            Fixtures.terrainTileList.Add(terrainTile);
        //        }

        //        Fixtures.terrainList.Add(terrain);
        //    }

        //    var worldInteractableSourceList = Fixtures.worldInteractableList.Where(x => Fixtures.interactionList.Where(y => y.regionId == regionSource.Id).Select(y => y.worldInteractableId).Contains(x.Id)).Distinct().ToList();

        //    foreach (Fixtures.WorldInteractable worldInteractableSource in worldInteractableSourceList)
        //    {
        //        var worldInteractable = new Fixtures.WorldInteractable();

        //        int worldInteractableId = Fixtures.worldInteractableList.Count > 0 ? (Fixtures.worldInteractableList[Fixtures.worldInteractableList.Count - 1].Id + 1) : 1;

        //        worldInteractable.Id = worldInteractableId;

        //        worldInteractable.chapterId = worldInteractableSource.chapterId;
        //        worldInteractable.objectiveId = worldInteractableSource.objectiveId;
        //        worldInteractable.interactableId = worldInteractableSource.interactableId;
        //        worldInteractable.interactionIndex = worldInteractableSource.interactionIndex;

        //        var interactionSourceList = Fixtures.interactionList.Where(x => x.worldInteractableId == worldInteractableSource.Id).OrderBy(x => x.Index).Distinct().ToList();

        //        foreach (Fixtures.Interaction interactionSource in interactionSourceList)
        //        {
        //            var interaction = new Fixtures.Interaction();

        //            int interactionId = Fixtures.interactionList.Count > 0 ? (Fixtures.interactionList[Fixtures.interactionList.Count - 1].Id + 1) : 1;

        //            interaction.Id = interactionId;
        //            interaction.worldInteractableId = worldInteractable.Id;
        //            interaction.objectiveId = interactionSource.objectiveId;
        //            interaction.regionId = phaseRegion.Id;

        //            interaction.Index = interactionSource.Index;
        //            interaction.description = interactionSource.description;

        //            interaction.positionX = interactionSource.positionX;
        //            interaction.positionY = interactionSource.positionY;
        //            interaction.positionZ = interactionSource.positionZ;

        //            interaction.terrainId = Fixtures.GetTerrain(interaction.regionId, interaction.positionX, interaction.positionY);
        //            interaction.terrainTileId = Fixtures.GetTerrainTile(interaction.terrainId, interaction.positionX, interaction.positionY);

        //            interaction.rotationX = interactionSource.rotationX;
        //            interaction.rotationY = interactionSource.rotationY;
        //            interaction.rotationZ = interactionSource.rotationZ;

        //            interaction.scaleMultiplier = interactionSource.scaleMultiplier;

        //            interaction.animation = interactionSource.animation;

        //            Fixtures.interactionList.Add(interaction);
        //        }

        //        Fixtures.worldInteractableList.Add(worldInteractable);
        //    }

        //    var worldObjectSourceList = Fixtures.worldObjectList.Where(x => x.regionId == regionSource.Id).Distinct().ToList();

        //    foreach (Fixtures.WorldObject worldObjectSource in worldObjectSourceList)
        //    {
        //        var worldObject = new Fixtures.WorldObject();

        //        int worldObjectId = Fixtures.worldObjectList.Count > 0 ? (Fixtures.worldObjectList[Fixtures.worldObjectList.Count - 1].Id + 1) : 1;

        //        worldObject.Id = worldObjectId;
        //        worldObject.regionId = phaseRegion.Id;

        //        worldObject.Index = worldObjectSource.Index;
        //        worldObject.objectGraphicId = worldObjectSource.objectGraphicId;
                
        //        worldObject.positionX = worldObjectSource.positionX;
        //        worldObject.positionY = worldObjectSource.positionY;
        //        worldObject.positionZ = worldObjectSource.positionZ;

        //        worldObject.terrainId = Fixtures.GetTerrain(worldObject.regionId, worldObject.positionX, worldObject.positionY);
        //        worldObject.terrainTileId = Fixtures.GetTerrainTile(worldObject.terrainId, worldObject.positionX, worldObject.positionY);

        //        worldObject.rotationX = worldObjectSource.rotationX;
        //        worldObject.rotationY = worldObjectSource.rotationY;
        //        worldObject.rotationZ = worldObjectSource.rotationZ;

        //        worldObject.scaleMultiplier = worldObjectSource.scaleMultiplier;

        //        worldObject.animation = worldObjectSource.animation;

        //        Fixtures.worldObjectList.Add(worldObject);
        //    }
        //}
    }

    private void UpdatePhaseElements() { }

    public void CancelEdit()
    {
        partyMemberDataList.Clear();
        chapterInteractableDataList.Clear();
        chapterRegionDataList.Clear();

        DataElements.ForEach(x => x.ClearChanges());
        
        Loaded = false;
    }

    public void CloseEditor() { }    
}
