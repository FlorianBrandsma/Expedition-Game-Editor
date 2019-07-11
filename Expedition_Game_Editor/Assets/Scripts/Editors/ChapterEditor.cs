using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterEditor : MonoBehaviour, IEditor
{
    private ChapterDataElement chapterData;

    public List<PartyMemberDataElement> partyElementDataList;
    public List<TerrainInteractableDataElement> terrainElementDataList;
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
            partyElementDataList.ForEach(x => list.Add(x));
            terrainElementDataList.ForEach(x => list.Add(x));
            chapterRegionDataList.ForEach(x => list.Add(x));

            return list;
        }
    }
    
    public void InitializeEditor()
    {
        if (Loaded) return;

        Data = PathController.route.data;

        chapterData = (ChapterDataElement)Data.DataElement;
        partyElementDataList.Clear();
        terrainElementDataList.Clear();
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

        //2. Remove all TASKS of the selected (PHASE)REGIONS where OBJECTIVEID equals 0
        var baseTasks = Fixtures.interactionList.Where(x => x.objectiveId == 0 && phaseRegions.Select(y => y.id).Contains(x.regionId)).Distinct().ToList();
        Fixtures.interactionList.RemoveAll(x => baseTasks.Contains(x));

        //3. Remove all TERRAINELEMENTS linked with TASKS from step 2
        var terrainElements = Fixtures.terrainInteractableList.Where(x => baseTasks.Select(y => y.terrainInteractableId).Contains(x.id)).Distinct().ToList();
        Fixtures.terrainInteractableList.RemoveAll(x => terrainElements.Contains(x));

        //4. Set (PHASE)REGION of all TASKS linked with selected (PHASE)REGIONS to 0 where OBJECTIVE ID does not equal 0
        var objectiveTasks = Fixtures.interactionList.Where(x => x.objectiveId != 0 && phaseRegions.Select(y => y.id).Contains(x.regionId)).Distinct().ToList();
        objectiveTasks.ForEach(x => x.regionId = 0);

        //5. Remove all TERRAINS and TERRAINTILES of the selected (PHASE)REGIONS
        Fixtures.terrainTileList.RemoveAll(x => terrainTiles.Contains(x));
        Fixtures.terrainList.RemoveAll(x => terrains.Contains(x));

        //6. Create TERRAINS and TERRAINTILES for selected (PHASE)REGIONS based on those of the newly selected CHAPTERREGION's REGION
        //7. Also create TERRAINOBJECTS, TERRAINELEMENTS and TASKS for selected (PHASE)REGIONS based the same criteria as step 6
        var regionSource = Fixtures.regionList.Where(x => x.id == chapterRegion.RegionId).FirstOrDefault();

        foreach(Fixtures.Region phaseRegion in phaseRegions)
        {
            phaseRegion.tileSetId = regionSource.tileSetId;

            phaseRegion.name = regionSource.name;
            phaseRegion.regionSize = regionSource.regionSize;
            phaseRegion.terrainSize = regionSource.terrainSize;

            var terrainElementSourceList = Fixtures.terrainInteractableList.Where(x => Fixtures.interactionList.Where(y => y.regionId == regionSource.id).Select(y => y.terrainInteractableId).Contains(x.id)).Distinct().ToList();

            foreach (Fixtures.TerrainInteractable terrainElementSource in terrainElementSourceList)
            {
                var terrainElement = new Fixtures.TerrainInteractable();

                int terrainElementId = Fixtures.terrainInteractableList.Count > 0 ? (Fixtures.terrainInteractableList[Fixtures.terrainInteractableList.Count - 1].id + 1) : 1;

                terrainElement.id = terrainElementId;

                terrainElement.chapterId = terrainElementSource.chapterId;
                terrainElement.objectiveId = terrainElementSource.objectiveId;
                terrainElement.interactableId = terrainElementSource.interactableId;
                terrainElement.interactionIndex = terrainElementSource.interactionIndex;

                var taskSourceList = Fixtures.interactionList.Where(x => x.terrainInteractableId == terrainElementSource.id).OrderBy(x => x.index).Distinct().ToList();

                foreach (Fixtures.Interaction taskSource in taskSourceList)
                {
                    var task = new Fixtures.Interaction();

                    int taskId = Fixtures.interactionList.Count > 0 ? (Fixtures.interactionList[Fixtures.interactionList.Count - 1].id + 1) : 1;

                    task.id = taskId;
                    task.terrainInteractableId = terrainElement.id;
                    task.objectiveId = taskSource.objectiveId;
                    task.regionId = phaseRegion.id;

                    task.index = taskSource.index;
                    task.description = taskSource.description;

                    task.xPos = taskSource.xPos;
                    task.yPos = taskSource.yPos;
                    task.zPos = taskSource.zPos;

                    task.xRot = taskSource.xRot;
                    task.yRot = taskSource.yRot;
                    task.zRot = taskSource.zRot;

                    Fixtures.interactionList.Add(task);
                }

                Fixtures.terrainInteractableList.Add(terrainElement);
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
