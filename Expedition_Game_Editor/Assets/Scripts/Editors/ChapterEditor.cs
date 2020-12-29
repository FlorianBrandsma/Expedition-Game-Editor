using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterEditor : MonoBehaviour, IEditor
{
    private ChapterData chapterData;

    public List<WorldInteractableElementData> worldInteractableElementDataList      = new List<WorldInteractableElementData>();
    public List<ChapterInteractableElementData> chapterInteractableElementDataList  = new List<ChapterInteractableElementData>();
    public List<ChapterRegionElementData> chapterRegionElementDataList              = new List<ChapterRegionElementData>();

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == chapterData.Id).FirstOrDefault(); } }

    private PathController PathController           { get { return GetComponent<PathController>(); } }
    public List<SegmentController> EditorSegments   { get; } = new List<SegmentController>();

    public bool Loaded { get; set; }
    
    public List<IElementData> DataList
    {
        get { return new List<IElementData>() { EditData }; }
    }

    public List<IElementData> ElementDataList
    {
        get
        {
            var list = new List<IElementData>();

            DataList.ForEach(x => { if (x != null) list.Add(x); });

            worldInteractableElementDataList.ForEach(x => list.Add(x));
            chapterInteractableElementDataList.ForEach(x => list.Add(x));
            chapterRegionElementDataList.ForEach(x => list.Add(x));

            return list;
        }
    }

    #region Data properties
    public int Id
    {
        get { return chapterData.Id; }
    }

    public int Index
    {
        get { return chapterData.Index; }
    }

    public string Name
    {
        get { return chapterData.Name; }
        set
        {
            chapterData.Name = value;

            DataList.ForEach(x => ((ChapterElementData)x).Name = value);
        }
    }

    public float TimeSpeed
    {
        get { return chapterData.TimeSpeed; }
        set
        {
            chapterData.TimeSpeed = value;

            DataList.ForEach(x => ((ChapterElementData)x).TimeSpeed = value);
        }
    }

    public string PublicNotes
    {
        get { return chapterData.PublicNotes; }
        set
        {
            chapterData.PublicNotes = value;

            DataList.ForEach(x => ((ChapterElementData)x).PublicNotes = value);
        }
    }

    public string PrivateNotes
    {
        get { return chapterData.PrivateNotes; }
        set
        {
            chapterData.PrivateNotes = value;

            DataList.ForEach(x => ((ChapterElementData)x).PrivateNotes = value);
        }
    }
    #endregion

    public void InitializeEditor()
    {
        chapterData = (ChapterData)ElementData.Clone();
    }

    public void OpenEditor() { }

    public void UpdateEditor()
    {
        PathController.layoutSection.SetActionButtons();
    }

    public bool Changed()
    {
        return ElementDataList.Any(x => x.Changed);
    }
    
    public void ApplyChanges(DataRequest dataRequest)
    {
        ApplyChapterChanges(dataRequest);
        ApplyWorldInteractableChanges(dataRequest);
        ApplyChapterInteractableChanges(dataRequest);
        ApplyRegionChanges(dataRequest);
    }

    private void ApplyChapterChanges(DataRequest dataRequest)
    {
        switch(EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
                AddChapter(dataRequest);
                break;

            case Enums.ExecuteType.Update:
                UpdateChapter(dataRequest);
                break;

            case Enums.ExecuteType.Remove:
                RemoveChapter(dataRequest);
                break;
        }
    }

    private void AddChapter(DataRequest dataRequest)
    {
        //Create temporary data while the other data's id is being changed
        var tempData = EditData;

        EditData.Add(dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            chapterData.Id = tempData.Id;

            //Apply new chapter id to other elements
            worldInteractableElementDataList.ForEach(x => x.ChapterId = chapterData.Id);
            chapterInteractableElementDataList.ForEach(x => x.ChapterId = chapterData.Id);
            chapterRegionElementDataList.ForEach(x => x.ChapterId = chapterData.Id);
        }
    }

    private void UpdateChapter(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);
    }

    private void RemoveChapter(DataRequest dataRequest)
    {
        EditData.Remove(dataRequest);
    }

    private void ApplyWorldInteractableChanges(DataRequest dataRequest)
    {
        foreach (WorldInteractableElementData worldInteractableElementData in worldInteractableElementDataList)
        {
            switch(worldInteractableElementData.ExecuteType)
            {
                case Enums.ExecuteType.Add:
                    worldInteractableElementData.Add(dataRequest);
                    break;

                case Enums.ExecuteType.Update:
                    worldInteractableElementData.Update(dataRequest);
                    break;

                case Enums.ExecuteType.Remove:
                    worldInteractableElementData.Remove(dataRequest);
                    break;
            }
        }

        if(dataRequest.requestType == Enums.RequestType.Execute)
            worldInteractableElementDataList.RemoveAll(x => x.ExecuteType == Enums.ExecuteType.Remove); 
    }

    private void ApplyChapterInteractableChanges(DataRequest dataRequest)
    {
        foreach (ChapterInteractableElementData chapterInteractableElementData in chapterInteractableElementDataList)
        {
            switch (chapterInteractableElementData.ExecuteType)
            {
                case Enums.ExecuteType.Add:
                    chapterInteractableElementData.Add(dataRequest);
                    break;

                case Enums.ExecuteType.Update:
                    chapterInteractableElementData.Update(dataRequest);
                    break;

                case Enums.ExecuteType.Remove:
                    chapterInteractableElementData.Remove(dataRequest);
                    break;
            }
        }

        if (dataRequest.requestType == Enums.RequestType.Execute)
            chapterInteractableElementDataList.RemoveAll(x => x.ExecuteType == Enums.ExecuteType.Remove);
    }

    private void ApplyRegionChanges(DataRequest dataRequest)
    {
        foreach (ChapterRegionElementData chapterRegionElementData in chapterRegionElementDataList)
        {
            switch (chapterRegionElementData.ExecuteType)
            {
                case Enums.ExecuteType.Add:
                    chapterRegionElementData.Add(dataRequest);
                    break;

                case Enums.ExecuteType.Update:
                    chapterRegionElementData.Update(dataRequest);
                    break;

                case Enums.ExecuteType.Remove:
                    chapterRegionElementData.Remove(dataRequest);
                    break;
            }
        }

        if (dataRequest.requestType == Enums.RequestType.Execute)
            chapterRegionElementDataList.RemoveAll(x => x.ExecuteType == Enums.ExecuteType.Remove);

        //When adding a new region, check if there are any phases which belong
        //If so, add a copy of the added region to every phase in the same way as when you would add a phase to that chapter

        /*
        //0. Find all (PHASE)REGIONS linked with the changed CHAPTERREGIONS
        var phaseRegions = Fixtures.regionList.Where(x => x.chapterRegionId == chapterRegion.Id).Distinct().ToList();
        var terrains = Fixtures.terrainList.Where(x => phaseRegions.Select(y => y.Id).Contains(x.regionId)).Distinct().ToList();
        var terrainTiles = Fixtures.terrainTileList.Where(x => terrains.Select(y => y.Id).Contains(x.terrainId)).Distinct().ToList();

        //1. Remove all WORLDOBJECTS of the selected (PHASE)REGIONS
        var worldObjects = Fixtures.worldObjectList.Where(x => phaseRegions.Select(y => y.Id).Contains(x.regionId)).Distinct().ToList();
        Fixtures.worldObjectList.RemoveAll(x => worldObjects.Contains(x));

        //2. Remove all INTERACTIONS of the selected (PHASE)REGIONS where OBJECTIVEID equals 0
        var baseInteractions = Fixtures.interactionList.Where(x => x.objectiveId == 0 && phaseRegions.Select(y => y.Id).Contains(x.regionId)).Distinct().ToList();
        Fixtures.interactionList.RemoveAll(x => baseInteractions.Contains(x));

        //3. Remove all WORLDINTERACTABLES linked with INTERACTIONS from step 2
        var worldInteractables = Fixtures.worldInteractableList.Where(x => baseInteractions.Select(y => y.worldInteractableId).Contains(x.Id)).Distinct().ToList();
        Fixtures.worldInteractableList.RemoveAll(x => worldInteractables.Contains(x));

        //4. Set (PHASE)REGION of all INTERACTIONS linked with selected (PHASE)REGIONS to 0 where OBJECTIVE ID does not equal 0
        var objectiveInteractions = Fixtures.interactionList.Where(x => x.objectiveId != 0 && phaseRegions.Select(y => y.Id).Contains(x.regionId)).Distinct().ToList();
        objectiveInteractions.ForEach(x => x.regionId = 0);

        //5. Remove all TERRAINS and TERRAINTILES of the selected (PHASE)REGIONS
        Fixtures.terrainTileList.RemoveAll(x => terrainTiles.Contains(x));
        Fixtures.terrainList.RemoveAll(x => terrains.Contains(x));

        //6. Create TERRAINS and TERRAINTILES for selected (PHASE)REGIONS based on those of the newly selected CHAPTERREGION's REGION
        //7. Also create WORLDOBJECTS, WORLDINTERACTABLES and INTERACTIONS for selected (PHASE)REGIONS based the same criteria as step 6
        var regionSource = Fixtures.regionList.Where(x => x.Id == chapterRegion.RegionId).FirstOrDefault();

        foreach (Fixtures.Region phaseRegion in phaseRegions)
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

            var worldInteractableSourceList = Fixtures.worldInteractableList.Where(x => Fixtures.interactionList.Where(y => y.regionId == regionSource.Id).Select(y => y.worldInteractableId).Contains(x.Id)).Distinct().ToList();

            foreach (Fixtures.WorldInteractable worldInteractableSource in worldInteractableSourceList)
            {
                var worldInteractable = new Fixtures.WorldInteractable();

                int worldInteractableId = Fixtures.worldInteractableList.Count > 0 ? (Fixtures.worldInteractableList[Fixtures.worldInteractableList.Count - 1].Id + 1) : 1;

                worldInteractable.Id = worldInteractableId;

                worldInteractable.chapterId = worldInteractableSource.chapterId;
                worldInteractable.objectiveId = worldInteractableSource.objectiveId;
                worldInteractable.interactableId = worldInteractableSource.interactableId;
                worldInteractable.interactionIndex = worldInteractableSource.interactionIndex;

                var interactionSourceList = Fixtures.interactionList.Where(x => x.worldInteractableId == worldInteractableSource.Id).OrderBy(x => x.Index).Distinct().ToList();

                foreach (Fixtures.Interaction interactionSource in interactionSourceList)
                {
                    var interaction = new Fixtures.Interaction();

                    int interactionId = Fixtures.interactionList.Count > 0 ? (Fixtures.interactionList[Fixtures.interactionList.Count - 1].Id + 1) : 1;

                    interaction.Id = interactionId;
                    interaction.worldInteractableId = worldInteractable.Id;
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

                    interaction.scale = interactionSource.scale;

                    interaction.animation = interactionSource.animation;

                    Fixtures.interactionList.Add(interaction);
                }

                Fixtures.worldInteractableList.Add(worldInteractable);
            }

            var worldObjectSourceList = Fixtures.worldObjectList.Where(x => x.regionId == regionSource.Id).Distinct().ToList();

            foreach (Fixtures.WorldObject worldObjectSource in worldObjectSourceList)
            {
                var worldObject = new Fixtures.WorldObject();

                int worldObjectId = Fixtures.worldObjectList.Count > 0 ? (Fixtures.worldObjectList[Fixtures.worldObjectList.Count - 1].Id + 1) : 1;

                worldObject.Id = worldObjectId;
                worldObject.regionId = phaseRegion.Id;

                worldObject.Index = worldObjectSource.Index;
                worldObject.modelId = worldObjectSource.modelId;

                worldObject.positionX = worldObjectSource.positionX;
                worldObject.positionY = worldObjectSource.positionY;
                worldObject.positionZ = worldObjectSource.positionZ;

                worldObject.terrainId = Fixtures.GetTerrain(worldObject.regionId, worldObject.positionX, worldObject.positionY);
                worldObject.terrainTileId = Fixtures.GetTerrainTile(worldObject.terrainId, worldObject.positionX, worldObject.positionY);

                worldObject.rotationX = worldObjectSource.rotationX;
                worldObject.rotationY = worldObjectSource.rotationY;
                worldObject.rotationZ = worldObjectSource.rotationZ;

                worldObject.scale = worldObjectSource.scale;

                worldObject.animation = worldObjectSource.animation;

                Fixtures.worldObjectList.Add(worldObject);
            }
        }
        */
    }

    public void FinalizeChanges()
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
            case Enums.ExecuteType.Remove:
                RenderManager.PreviousPath();
                break;
            case Enums.ExecuteType.Update:
                ResetExecuteType();
                UpdateEditor();
                break;
        }
    }

    private void ResetExecuteType()
    {
        ElementDataList.ForEach(x => x.ExecuteType = Enums.ExecuteType.Update);
    }

    public void CancelEdit()
    {
        worldInteractableElementDataList.Clear();
        chapterInteractableElementDataList.Clear();
        chapterRegionElementDataList.Clear();

        ResetExecuteType();

        ElementDataList.ForEach(x =>
        {
            x.ClearChanges();
        });

        Loaded = false;
    }

    public void CloseEditor() { }    
}
