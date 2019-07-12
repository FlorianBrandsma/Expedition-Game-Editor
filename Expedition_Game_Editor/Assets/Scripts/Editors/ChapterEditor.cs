using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterEditor : MonoBehaviour, IEditor
{
    private ChapterDataElement chapterData;

    public List<PartyMemberDataElement> partyMemberDataList;
    public List<TerrainInteractableDataElement> terrainInteractableDataList;
    public List<ChapterRegionDataElement> chapterRegionDataList;

    private DataManager dataManager = new DataManager();

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get { return PathController.loaded; } }
    public Data Data { get; set; }

    public List<IDataElement> DataElements
    {
        get
        {
            var list = new List<IDataElement>();

            list.Add(chapterData);
            partyMemberDataList.ForEach(x => list.Add(x));
            terrainInteractableDataList.ForEach(x => list.Add(x));
            chapterRegionDataList.ForEach(x => list.Add(x));

            return list;
        }
    }
    
    public void InitializeEditor()
    {
        if (Loaded) return;

        Data = PathController.route.data;

        chapterData = (ChapterDataElement)Data.DataElement;
        partyMemberDataList.Clear();
        terrainInteractableDataList.Clear();
        chapterRegionDataList.Clear();

        DataElements.ForEach(x => x.ClearChanges());
    }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void UpdateIndex(int index)
    {
        var list = Data.DataController.DataList.Cast<ChapterDataElement>().ToList();

        list.RemoveAt(chapterData.Index);
        list.Insert(index, chapterData);

        Data.DataController.DataList = list.Cast<IDataElement>().ToList();

        for (int i = 0; i < list.Count; i++)
        {
            list[i].Index = i;
            list[i].UpdateIndex();
        }

        UpdateList();
    }

    private void UpdateList()
    {
        if (PathController.Origin == null) return;

        PathController.Origin.ListManager.UpdateData();
    }

    public void OpenEditor()
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

        DataElements.ForEach(x => x.Update());

        UpdateList();

        UpdateEditor();

        UpdatePhaseElements();
    }

    private void ChangedRegion(ChapterRegionDataElement chapterRegion)
    {
        //0. Find all (PHASE)REGIONS linked with the changed CHAPTERREGIONS
        var phaseRegions = Fixtures.regionList.Where(x => x.chapterRegionId == chapterRegion.id).Distinct().ToList();
        var terrains = Fixtures.terrainList.Where(x => phaseRegions.Select(y => y.id).Contains(x.regionId)).Distinct().ToList();
        var terrainTiles = Fixtures.terrainTileList.Where(x => terrains.Select(y => y.id).Contains(x.terrainId)).Distinct().ToList();
        
        //1. Remove all TERRAINOBJECTS of the selected (PHASE)REGIONS
        var terrainObjects = Fixtures.terrainObjectList.Where(x => phaseRegions.Select(y => y.id).Contains(x.regionId)).Distinct().ToList();
        Fixtures.terrainObjectList.RemoveAll(x => terrainObjects.Contains(x));

        //2. Remove all INTERACTIONS of the selected (PHASE)REGIONS where OBJECTIVEID equals 0
        var baseInteractions = Fixtures.interactionList.Where(x => x.objectiveId == 0 && phaseRegions.Select(y => y.id).Contains(x.regionId)).Distinct().ToList();
        Fixtures.interactionList.RemoveAll(x => baseInteractions.Contains(x));

        //3. Remove all TERRAININTERACTABLES linked with INTERACTIONS from step 2
        var terrainInteractables = Fixtures.terrainInteractableList.Where(x => baseInteractions.Select(y => y.terrainInteractableId).Contains(x.id)).Distinct().ToList();
        Fixtures.terrainInteractableList.RemoveAll(x => terrainInteractables.Contains(x));

        //4. Set (PHASE)REGION of all INTERACTIONS linked with selected (PHASE)REGIONS to 0 where OBJECTIVE ID does not equal 0
        var objectiveInteractions = Fixtures.interactionList.Where(x => x.objectiveId != 0 && phaseRegions.Select(y => y.id).Contains(x.regionId)).Distinct().ToList();
        objectiveInteractions.ForEach(x => x.regionId = 0);

        //5. Remove all TERRAINS and TERRAINTILES of the selected (PHASE)REGIONS
        Fixtures.terrainTileList.RemoveAll(x => terrainTiles.Contains(x));
        Fixtures.terrainList.RemoveAll(x => terrains.Contains(x));

        //6. Create TERRAINS and TERRAINTILES for selected (PHASE)REGIONS based on those of the newly selected CHAPTERREGION's REGION
        //7. Also create TERRAINOBJECTS, TERRAININTERACTABLES and INTERACTIONS for selected (PHASE)REGIONS based the same criteria as step 6
        var regionSource = Fixtures.regionList.Where(x => x.id == chapterRegion.RegionId).FirstOrDefault();

        foreach(Fixtures.Region phaseRegion in phaseRegions)
        {
            phaseRegion.tileSetId = regionSource.tileSetId;

            phaseRegion.name = regionSource.name;
            phaseRegion.regionSize = regionSource.regionSize;
            phaseRegion.terrainSize = regionSource.terrainSize;

            var terrainInteractableSourceList = Fixtures.terrainInteractableList.Where(x => Fixtures.interactionList.Where(y => y.regionId == regionSource.id).Select(y => y.terrainInteractableId).Contains(x.id)).Distinct().ToList();

            foreach (Fixtures.TerrainInteractable terrainInteractableSource in terrainInteractableSourceList)
            {
                var terrainInteractable = new Fixtures.TerrainInteractable();

                int terrainInteractableId = Fixtures.terrainInteractableList.Count > 0 ? (Fixtures.terrainInteractableList[Fixtures.terrainInteractableList.Count - 1].id + 1) : 1;

                terrainInteractable.id = terrainInteractableId;

                terrainInteractable.chapterId = terrainInteractableSource.chapterId;
                terrainInteractable.objectiveId = terrainInteractableSource.objectiveId;
                terrainInteractable.interactableId = terrainInteractableSource.interactableId;
                terrainInteractable.interactionIndex = terrainInteractableSource.interactionIndex;

                var interactionSourceList = Fixtures.interactionList.Where(x => x.terrainInteractableId == terrainInteractableSource.id).OrderBy(x => x.index).Distinct().ToList();

                foreach (Fixtures.Interaction interactionSource in interactionSourceList)
                {
                    var interaction = new Fixtures.Interaction();

                    int interactionId = Fixtures.interactionList.Count > 0 ? (Fixtures.interactionList[Fixtures.interactionList.Count - 1].id + 1) : 1;

                    interaction.id = interactionId;
                    interaction.terrainInteractableId = terrainInteractable.id;
                    interaction.objectiveId = interactionSource.objectiveId;
                    interaction.regionId = phaseRegion.id;

                    interaction.index = interactionSource.index;
                    interaction.description = interactionSource.description;

                    interaction.xPos = interactionSource.xPos;
                    interaction.yPos = interactionSource.yPos;
                    interaction.zPos = interactionSource.zPos;

                    interaction.xRot = interactionSource.xRot;
                    interaction.yRot = interactionSource.yRot;
                    interaction.zRot = interactionSource.zRot;

                    Fixtures.interactionList.Add(interaction);
                }

                Fixtures.terrainInteractableList.Add(terrainInteractable);
            }

            var terrainObjectSourceList = Fixtures.terrainObjectList.Where(x => x.regionId == regionSource.id).Distinct().ToList();

            foreach (Fixtures.TerrainObject terrainObjectSource in terrainObjectSourceList)
            {
                var terrainObject = new Fixtures.TerrainObject();

                int terrainObjectId = Fixtures.terrainObjectList.Count > 0 ? (Fixtures.terrainObjectList[Fixtures.terrainObjectList.Count - 1].id + 1) : 1;

                terrainObject.id = terrainObjectId;
                terrainObject.regionId = phaseRegion.id;

                terrainObject.index = terrainObjectSource.index;
                terrainObject.objectGraphicId = terrainObjectSource.objectGraphicId;
                terrainObject.boundToTile = terrainObjectSource.boundToTile;

                Fixtures.terrainObjectList.Add(terrainObject);
            }

            var terrainSourceList = Fixtures.terrainList.Where(x => x.regionId == regionSource.id).OrderBy(x => x.index).Distinct().ToList();

            foreach (Fixtures.Terrain terrainSource in terrainSourceList)
            {
                var terrain = new Fixtures.Terrain();

                int terrainId = Fixtures.terrainList.Count > 0 ? (Fixtures.terrainList[Fixtures.terrainList.Count - 1].id + 1) : 1;

                terrain.id = terrainId;
                terrain.regionId = phaseRegion.id;

                terrain.index = terrainSource.index;

                terrain.iconId = terrainSource.iconId;
                terrain.name = terrainSource.name;

                var terrainTileSourceList = Fixtures.terrainTileList.Where(x => x.terrainId == terrainSource.id).OrderBy(x => x.index).Distinct().ToList();

                foreach (Fixtures.TerrainTile terrainTileSource in terrainTileSourceList)
                {
                    var terrainTile = new Fixtures.TerrainTile();

                    int terrainTileId = Fixtures.terrainTileList.Count > 0 ? (Fixtures.terrainTileList[Fixtures.terrainTileList.Count - 1].id + 1) : 1;

                    terrainTile.id = terrainTileId;
                    terrainTile.terrainId = terrain.id;

                    terrainTile.index = terrainTileSource.index;

                    terrainTile.tileId = terrainTileSource.tileId;

                    Fixtures.terrainTileList.Add(terrainTile);
                }

                Fixtures.terrainList.Add(terrain);
            }
        }
    }

    private void UpdatePhaseElements()
    {
        var phaseList = dataManager.GetPhaseData(chapterData.id, true);

        var phaseElementList = dataManager.GetPhaseInteractableData(phaseList.Select(x => x.id).Distinct().ToList());
    }

    public void CancelEdit()
    {
        
    }

    public void CloseEditor()
    {
        
    }    
}
