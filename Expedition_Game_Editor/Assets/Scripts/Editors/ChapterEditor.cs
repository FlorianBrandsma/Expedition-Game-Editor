using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterEditor : MonoBehaviour, IEditor
{
    public ChapterDataElement ChapterData { get { return (ChapterDataElement)Data.dataElement; } }

    private List<IDataElement> dataList = new List<IDataElement>();

    private List<SegmentController> editorSegments = new List<SegmentController>();

    public List<PartyMemberDataElement> partyMemberDataList;
    public List<SceneInteractableDataElement> sceneInteractableDataList;
    public List<ChapterRegionDataElement> chapterRegionDataList;
    
    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get; set; }

    public Route.Data Data { get { return PathController.route.data; } }

    public List<IDataElement> DataList
    {
        get { return SelectionElementManager.FindDataElements(ChapterData); }
    }

    public List<IDataElement> DataElements
    {
        get
        {
            var list = new List<IDataElement>();

            DataList.ForEach(x => list.Add(x));

            partyMemberDataList.ForEach(x => list.Add(x));
            sceneInteractableDataList.ForEach(x => list.Add(x));
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
        
        DataElements.Where(x => x.SelectionElement != null).ToList().ForEach(x =>
        {
            x.Update();
            x.SelectionElement.UpdateElement();
        });
        
        UpdateEditor();

        UpdatePhaseElements();
    }

    private void ChangedRegion(ChapterRegionDataElement chapterRegion)
    {
        //0. Find all (PHASE)REGIONS linked with the changed CHAPTERREGIONS
        var phaseRegions = Fixtures.regionList.Where(x => x.chapterRegionId == chapterRegion.Id).Distinct().ToList();
        var terrains = Fixtures.terrainList.Where(x => phaseRegions.Select(y => y.Id).Contains(x.regionId)).Distinct().ToList();
        var terrainTiles = Fixtures.terrainTileList.Where(x => terrains.Select(y => y.Id).Contains(x.terrainId)).Distinct().ToList();
        
        //1. Remove all SCENEOBJECTS of the selected (PHASE)REGIONS
        var sceneObjects = Fixtures.sceneObjectList.Where(x => phaseRegions.Select(y => y.Id).Contains(x.regionId)).Distinct().ToList();
        Fixtures.sceneObjectList.RemoveAll(x => sceneObjects.Contains(x));

        //2. Remove all INTERACTIONS of the selected (PHASE)REGIONS where OBJECTIVEID equals 0
        var baseInteractions = Fixtures.interactionList.Where(x => x.objectiveId == 0 && phaseRegions.Select(y => y.Id).Contains(x.regionId)).Distinct().ToList();
        Fixtures.interactionList.RemoveAll(x => baseInteractions.Contains(x));

        //3. Remove all SCENEINTERACTABLES linked with INTERACTIONS from step 2
        var sceneInteractables = Fixtures.sceneInteractableList.Where(x => baseInteractions.Select(y => y.sceneInteractableId).Contains(x.Id)).Distinct().ToList();
        Fixtures.sceneInteractableList.RemoveAll(x => sceneInteractables.Contains(x));

        //4. Set (PHASE)REGION of all INTERACTIONS linked with selected (PHASE)REGIONS to 0 where OBJECTIVE ID does not equal 0
        var objectiveInteractions = Fixtures.interactionList.Where(x => x.objectiveId != 0 && phaseRegions.Select(y => y.Id).Contains(x.regionId)).Distinct().ToList();
        objectiveInteractions.ForEach(x => x.regionId = 0);

        //5. Remove all TERRAINS and TERRAINTILES of the selected (PHASE)REGIONS
        Fixtures.terrainTileList.RemoveAll(x => terrainTiles.Contains(x));
        Fixtures.terrainList.RemoveAll(x => terrains.Contains(x));

        //6. Create TERRAINS and TERRAINTILES for selected (PHASE)REGIONS based on those of the newly selected CHAPTERREGION's REGION
        //7. Also create SCENEOBJECTS, SCENEINTERACTABLES and INTERACTIONS for selected (PHASE)REGIONS based the same criteria as step 6
        var regionSource = Fixtures.regionList.Where(x => x.Id == chapterRegion.RegionId).FirstOrDefault();

        foreach(Fixtures.Region phaseRegion in phaseRegions)
        {
            phaseRegion.tileSetId = regionSource.tileSetId;

            phaseRegion.name = regionSource.name;
            phaseRegion.regionSize = regionSource.regionSize;
            phaseRegion.terrainSize = regionSource.terrainSize;

            var terrainSourceList = Fixtures.terrainList.Where(x => x.regionId == regionSource.Id).OrderBy(x => x.Index).Distinct().ToList();

            foreach (Fixtures.Terrain terrainSource in terrainSourceList)
            {
                var terrain = new Fixtures.Terrain();

                int terrainId = Fixtures.terrainList.Count > 0 ? (Fixtures.terrainList[Fixtures.terrainList.Count - 1].Id + 1) : 1;

                terrain.Id = terrainId;
                terrain.regionId = phaseRegion.Id;

                terrain.Index = terrainSource.Index;

                terrain.iconId = terrainSource.iconId;
                terrain.name = terrainSource.name;

                var terrainTileSourceList = Fixtures.terrainTileList.Where(x => x.terrainId == terrainSource.Id).OrderBy(x => x.Index).Distinct().ToList();

                foreach (Fixtures.TerrainTile terrainTileSource in terrainTileSourceList)
                {
                    var terrainTile = new Fixtures.TerrainTile();

                    int terrainTileId = Fixtures.terrainTileList.Count > 0 ? (Fixtures.terrainTileList[Fixtures.terrainTileList.Count - 1].Id + 1) : 1;

                    terrainTile.Id = terrainTileId;
                    terrainTile.terrainId = terrain.Id;

                    terrainTile.Index = terrainTileSource.Index;

                    terrainTile.tileId = terrainTileSource.tileId;

                    Fixtures.terrainTileList.Add(terrainTile);
                }

                Fixtures.terrainList.Add(terrain);
            }

            var sceneInteractableSourceList = Fixtures.sceneInteractableList.Where(x => Fixtures.interactionList.Where(y => y.regionId == regionSource.Id).Select(y => y.sceneInteractableId).Contains(x.Id)).Distinct().ToList();

            foreach (Fixtures.SceneInteractable sceneInteractableSource in sceneInteractableSourceList)
            {
                var sceneInteractable = new Fixtures.SceneInteractable();

                int sceneInteractableId = Fixtures.sceneInteractableList.Count > 0 ? (Fixtures.sceneInteractableList[Fixtures.sceneInteractableList.Count - 1].Id + 1) : 1;

                sceneInteractable.Id = sceneInteractableId;

                sceneInteractable.chapterId = sceneInteractableSource.chapterId;
                sceneInteractable.objectiveId = sceneInteractableSource.objectiveId;
                sceneInteractable.interactableId = sceneInteractableSource.interactableId;
                sceneInteractable.interactionIndex = sceneInteractableSource.interactionIndex;

                var interactionSourceList = Fixtures.interactionList.Where(x => x.sceneInteractableId == sceneInteractableSource.Id).OrderBy(x => x.Index).Distinct().ToList();

                foreach (Fixtures.Interaction interactionSource in interactionSourceList)
                {
                    var interaction = new Fixtures.Interaction();

                    int interactionId = Fixtures.interactionList.Count > 0 ? (Fixtures.interactionList[Fixtures.interactionList.Count - 1].Id + 1) : 1;

                    interaction.Id = interactionId;
                    interaction.sceneInteractableId = sceneInteractable.Id;
                    interaction.objectiveId = interactionSource.objectiveId;
                    interaction.regionId = phaseRegion.Id;

                    interaction.Index = interactionSource.Index;
                    interaction.description = interactionSource.description;

                    interaction.positionX = interactionSource.positionX;
                    interaction.positionY = interactionSource.positionY;
                    interaction.positionZ = interactionSource.positionZ;

                    interaction.terrainId = Fixtures.GetTerrain(interaction.regionId, interaction.positionX, interaction.positionY);
                    interaction.terrainTileId = Fixtures.GetTerrainTile(interaction.terrainId, interaction.positionX, interaction.positionY);

                    interaction.rotationX = interactionSource.rotationX;
                    interaction.rotationY = interactionSource.rotationY;
                    interaction.rotationZ = interactionSource.rotationZ;

                    interaction.scaleMultiplier = interactionSource.scaleMultiplier;

                    interaction.animation = interactionSource.animation;

                    Fixtures.interactionList.Add(interaction);
                }

                Fixtures.sceneInteractableList.Add(sceneInteractable);
            }

            var sceneObjectSourceList = Fixtures.sceneObjectList.Where(x => x.regionId == regionSource.Id).Distinct().ToList();

            foreach (Fixtures.SceneObject sceneObjectSource in sceneObjectSourceList)
            {
                var sceneObject = new Fixtures.SceneObject();

                int sceneObjectId = Fixtures.sceneObjectList.Count > 0 ? (Fixtures.sceneObjectList[Fixtures.sceneObjectList.Count - 1].Id + 1) : 1;

                sceneObject.Id = sceneObjectId;
                sceneObject.regionId = phaseRegion.Id;

                sceneObject.Index = sceneObjectSource.Index;
                sceneObject.objectGraphicId = sceneObjectSource.objectGraphicId;
                
                sceneObject.positionX = sceneObjectSource.positionX;
                sceneObject.positionY = sceneObjectSource.positionY;
                sceneObject.positionZ = sceneObjectSource.positionZ;

                sceneObject.terrainId = Fixtures.GetTerrain(sceneObject.regionId, sceneObject.positionX, sceneObject.positionY);
                sceneObject.terrainTileId = Fixtures.GetTerrainTile(sceneObject.terrainId, sceneObject.positionX, sceneObject.positionY);

                sceneObject.rotationX = sceneObjectSource.rotationX;
                sceneObject.rotationY = sceneObjectSource.rotationY;
                sceneObject.rotationZ = sceneObjectSource.rotationZ;

                sceneObject.scaleMultiplier = sceneObjectSource.scaleMultiplier;

                sceneObject.animation = sceneObjectSource.animation;

                Fixtures.sceneObjectList.Add(sceneObject);
            }
        }
    }

    private void UpdatePhaseElements() { }

    public void CancelEdit()
    {
        partyMemberDataList.Clear();
        sceneInteractableDataList.Clear();
        chapterRegionDataList.Clear();

        DataElements.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }    
}
