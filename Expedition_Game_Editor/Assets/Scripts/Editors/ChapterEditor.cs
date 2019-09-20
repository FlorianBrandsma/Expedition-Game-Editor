using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterEditor : MonoBehaviour, IEditor
{
    private ChapterDataElement chapterData;

    public List<PartyMemberDataElement> partyMemberDataList;
    public List<SceneInteractableDataElement> sceneInteractableDataList;
    public List<ChapterRegionDataElement> chapterRegionDataList;

    private DataManager dataManager = new DataManager();

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get { return PathController.loaded; } }
    public Route.Data Data { get; set; }

    public List<IDataElement> DataElements
    {
        get
        {
            var list = new List<IDataElement>();

            list.Add(chapterData);

            partyMemberDataList.ForEach(x => list.Add(x));
            sceneInteractableDataList.ForEach(x => list.Add(x));
            chapterRegionDataList.ForEach(x => list.Add(x));

            return list;
        }
    }
    
    public void InitializeEditor()
    {
        if (Loaded) return;

        Data = PathController.route.data;

        chapterData = (ChapterDataElement)Data.dataElement;
        partyMemberDataList.Clear();
        sceneInteractableDataList.Clear();
        chapterRegionDataList.Clear();

        DataElements.ForEach(x => x.ClearChanges());
    }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void UpdateIndex(int index)
    {
        var list = Data.dataController.DataList.Cast<ChapterDataElement>().ToList();

        list.RemoveAt(chapterData.Index);
        list.Insert(index, chapterData);

        Data.dataController.DataList = list.Cast<IDataElement>().ToList();

        for (int i = 0; i < list.Count; i++)
        {
            list[i].Index = i;
            list[i].UpdateIndex();
        }

        SelectionElementManager.UpdateElements(chapterData, true);
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

        SelectionElementManager.UpdateElements(chapterData);

        UpdateEditor();

        UpdatePhaseElements();
    }

    private void ChangedRegion(ChapterRegionDataElement chapterRegion)
    {
        //0. Find all (PHASE)REGIONS linked with the changed CHAPTERREGIONS
        var phaseRegions = Fixtures.regionList.Where(x => x.chapterRegionId == chapterRegion.id).Distinct().ToList();
        var terrains = Fixtures.terrainList.Where(x => phaseRegions.Select(y => y.id).Contains(x.regionId)).Distinct().ToList();
        var terrainTiles = Fixtures.terrainTileList.Where(x => terrains.Select(y => y.id).Contains(x.terrainId)).Distinct().ToList();
        
        //1. Remove all SCENEOBJECTS of the selected (PHASE)REGIONS
        var sceneObjects = Fixtures.sceneObjectList.Where(x => phaseRegions.Select(y => y.id).Contains(x.regionId)).Distinct().ToList();
        Fixtures.sceneObjectList.RemoveAll(x => sceneObjects.Contains(x));

        //2. Remove all INTERACTIONS of the selected (PHASE)REGIONS where OBJECTIVEID equals 0
        var baseInteractions = Fixtures.interactionList.Where(x => x.objectiveId == 0 && phaseRegions.Select(y => y.id).Contains(x.regionId)).Distinct().ToList();
        Fixtures.interactionList.RemoveAll(x => baseInteractions.Contains(x));

        //3. Remove all SCENEINTERACTABLES linked with INTERACTIONS from step 2
        var sceneInteractables = Fixtures.sceneInteractableList.Where(x => baseInteractions.Select(y => y.sceneInteractableId).Contains(x.id)).Distinct().ToList();
        Fixtures.sceneInteractableList.RemoveAll(x => sceneInteractables.Contains(x));

        //4. Set (PHASE)REGION of all INTERACTIONS linked with selected (PHASE)REGIONS to 0 where OBJECTIVE ID does not equal 0
        var objectiveInteractions = Fixtures.interactionList.Where(x => x.objectiveId != 0 && phaseRegions.Select(y => y.id).Contains(x.regionId)).Distinct().ToList();
        objectiveInteractions.ForEach(x => x.regionId = 0);

        //5. Remove all TERRAINS and TERRAINTILES of the selected (PHASE)REGIONS
        Fixtures.terrainTileList.RemoveAll(x => terrainTiles.Contains(x));
        Fixtures.terrainList.RemoveAll(x => terrains.Contains(x));

        //6. Create TERRAINS and TERRAINTILES for selected (PHASE)REGIONS based on those of the newly selected CHAPTERREGION's REGION
        //7. Also create SCENEOBJECTS, SCENEINTERACTABLES and INTERACTIONS for selected (PHASE)REGIONS based the same criteria as step 6
        var regionSource = Fixtures.regionList.Where(x => x.id == chapterRegion.RegionId).FirstOrDefault();

        foreach(Fixtures.Region phaseRegion in phaseRegions)
        {
            phaseRegion.tileSetId = regionSource.tileSetId;

            phaseRegion.name = regionSource.name;
            phaseRegion.regionSize = regionSource.regionSize;
            phaseRegion.terrainSize = regionSource.terrainSize;

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

            var sceneInteractableSourceList = Fixtures.sceneInteractableList.Where(x => Fixtures.interactionList.Where(y => y.regionId == regionSource.id).Select(y => y.sceneInteractableId).Contains(x.id)).Distinct().ToList();

            foreach (Fixtures.SceneInteractable sceneInteractableSource in sceneInteractableSourceList)
            {
                var sceneInteractable = new Fixtures.SceneInteractable();

                int sceneInteractableId = Fixtures.sceneInteractableList.Count > 0 ? (Fixtures.sceneInteractableList[Fixtures.sceneInteractableList.Count - 1].id + 1) : 1;

                sceneInteractable.id = sceneInteractableId;

                sceneInteractable.chapterId = sceneInteractableSource.chapterId;
                sceneInteractable.objectiveId = sceneInteractableSource.objectiveId;
                sceneInteractable.interactableId = sceneInteractableSource.interactableId;
                sceneInteractable.interactionIndex = sceneInteractableSource.interactionIndex;

                var interactionSourceList = Fixtures.interactionList.Where(x => x.sceneInteractableId == sceneInteractableSource.id).OrderBy(x => x.index).Distinct().ToList();

                foreach (Fixtures.Interaction interactionSource in interactionSourceList)
                {
                    var interaction = new Fixtures.Interaction();

                    int interactionId = Fixtures.interactionList.Count > 0 ? (Fixtures.interactionList[Fixtures.interactionList.Count - 1].id + 1) : 1;

                    interaction.id = interactionId;
                    interaction.sceneInteractableId = sceneInteractable.id;
                    interaction.objectiveId = interactionSource.objectiveId;
                    interaction.regionId = phaseRegion.id;

                    interaction.index = interactionSource.index;
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

            var sceneObjectSourceList = Fixtures.sceneObjectList.Where(x => x.regionId == regionSource.id).Distinct().ToList();

            foreach (Fixtures.SceneObject sceneObjectSource in sceneObjectSourceList)
            {
                var sceneObject = new Fixtures.SceneObject();

                int sceneObjectId = Fixtures.sceneObjectList.Count > 0 ? (Fixtures.sceneObjectList[Fixtures.sceneObjectList.Count - 1].id + 1) : 1;

                sceneObject.id = sceneObjectId;
                sceneObject.regionId = phaseRegion.id;

                sceneObject.index = sceneObjectSource.index;
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
