using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class PhaseDataManager
{
    private static List<PhaseBaseData> phaseDataList;

    private static List<ChapterBaseData> chapterDataList;
    private static List<WorldInteractableBaseData> worldInteractableDataList;
    private static List<InteractableBaseData> interactableDataList;
    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    private static List<RegionBaseData> regionDataList;
    private static List<TerrainBaseData> terrainDataList;
    private static List<TerrainTileBaseData> terrainTileDataList;
    private static List<TileSetBaseData> tileSetDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Phase>().First();

        GetPhaseData(searchParameters);

        if (phaseDataList.Count == 0) return new List<IElementData>();
        
        GetChapterData();
        GetWorldInteractableData();
        GetInteractableData();
        GetModelData();
        GetIconData();

        GetRegionData();
        GetTileSetData();
        GetTerrainData();
        GetTerrainTileData();

        var list = (from phaseData      in phaseDataList
                    join chapterData    in chapterDataList  on phaseData.ChapterId          equals chapterData.Id

                    join regionData     in regionDataList   on phaseData.DefaultRegionId    equals regionData.Id
                    join tileSetData    in tileSetDataList  on regionData.TileSetId         equals tileSetData.Id

                    join leftJoin in (from worldInteractableData    in worldInteractableDataList
                                      join interactableData         in interactableDataList on worldInteractableData.InteractableId equals interactableData.Id
                                      join modelData                in modelDataList        on interactableData.ModelId             equals modelData.Id
                                      join iconData                 in iconDataList         on modelData.IconId                     equals iconData.Id
                                      select new { worldInteractableData, interactableData, modelData, iconData }) on chapterData.Id equals leftJoin.worldInteractableData.ChapterId into worldInteractableData

                    select new PhaseElementData()
                    {
                        Id = phaseData.Id,
                        Index = phaseData.Index,

                        ChapterId = phaseData.ChapterId,

                        Name = phaseData.Name,

                        DefaultRegionId = phaseData.DefaultRegionId,

                        DefaultPositionX = phaseData.DefaultPositionX,
                        DefaultPositionY = phaseData.DefaultPositionY,
                        DefaultPositionZ = phaseData.DefaultPositionZ,

                        DefaultRotationX = phaseData.DefaultRotationX,
                        DefaultRotationY = phaseData.DefaultRotationY,
                        DefaultRotationZ = phaseData.DefaultRotationZ,

                        Scale = worldInteractableData.First().interactableData.Scale,

                        DefaultTime = phaseData.DefaultTime,

                        PublicNotes = phaseData.PublicNotes,
                        PrivateNotes = phaseData.PrivateNotes,

                        TerrainTileId = RegionManager.GetTerrainTileId(regionData, terrainDataList, terrainTileDataList, tileSetData.TileSize, phaseData.DefaultPositionX, phaseData.DefaultPositionZ),

                        WorldInteractableId = worldInteractableData.First().worldInteractableData.Id,
                        
                        ModelId = worldInteractableData.First().modelData.Id,
                        ModelPath = worldInteractableData.First().modelData.Path,

                        ModelIconPath = worldInteractableData.First().iconData.Path,
                        
                        Height = worldInteractableData.First().modelData.Height,
                        Width = worldInteractableData.First().modelData.Width,
                        Depth = worldInteractableData.First().modelData.Depth,

                        InteractableName = worldInteractableData.First().interactableData.Name,
                        LocationName = RegionManager.LocationName(phaseData.DefaultPositionX, phaseData.DefaultPositionZ, tileSetData.TileSize, regionData, terrainDataList)
                        
                    }).OrderBy(x => x.Id > 0).ThenBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetPhaseData(Search.Phase searchParameters)
    {
        phaseDataList = new List<PhaseBaseData>();

        foreach(PhaseBaseData phase in Fixtures.phaseList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(phase.Id))                 continue;
            if (searchParameters.chapterId.Count    > 0 && !searchParameters.chapterId.Contains(phase.ChapterId))   continue;

            phaseDataList.Add(phase);
        }
    }

    private static void GetChapterData()
    {
        var searchParameters = new Search.Chapter();
        searchParameters.id = phaseDataList.Select(x => x.ChapterId).Distinct().ToList();

        chapterDataList = DataManager.GetChapterData(searchParameters);
    }

    private static void GetWorldInteractableData()
    {
        var searchParameters = new Search.WorldInteractable();
        searchParameters.chapterId = chapterDataList.Select(x => x.Id).Distinct().ToList();

        worldInteractableDataList = DataManager.GetWorldInteractableData(searchParameters);
    }

    private static void GetInteractableData()
    {
        var searchParameters = new Search.Interactable();
        searchParameters.id = worldInteractableDataList.Select(x => x.InteractableId).Distinct().ToList();

        interactableDataList = DataManager.GetInteractableData(searchParameters);
    }

    private static void GetModelData()
    {
        var searchParameters = new Search.Model();
        searchParameters.id = interactableDataList.Select(x => x.ModelId).Distinct().ToList();

        modelDataList = DataManager.GetModelData(searchParameters);
    }

    private static void GetIconData()
    {
        var searchParameters = new Search.Icon();
        searchParameters.id = modelDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(searchParameters);
    }

    private static void GetRegionData()
    {
        var searchParameters = new Search.Region();
        searchParameters.id = phaseDataList.Select(x => x.DefaultRegionId).Distinct().ToList();

        regionDataList = DataManager.GetRegionData(searchParameters);
    }

    private static void GetTileSetData()
    {
        var searchParameters = new Search.TileSet();
        searchParameters.id = regionDataList.Select(x => x.TileSetId).Distinct().ToList();

        tileSetDataList = DataManager.GetTileSetData(searchParameters);
    }

    private static void GetTerrainData()
    {
        var searchParameters = new Search.Terrain();
        searchParameters.regionId = regionDataList.Select(x => x.Id).Distinct().ToList();

        terrainDataList = DataManager.GetTerrainData(searchParameters);
    }

    private static void GetTerrainTileData()
    {
        var searchParameters = new Search.TerrainTile();
        searchParameters.terrainId = terrainDataList.Select(x => x.Id).Distinct().ToList();

        terrainTileDataList = DataManager.GetTerrainTileData(searchParameters);
    }

    public static void AddData(PhaseElementData elementData, DataRequest dataRequest)
    {

    }
    
    public static void UpdateData(PhaseElementData elementData, DataRequest dataRequest)
    {
        var data = Fixtures.phaseList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedName)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.Name = elementData.Name;
            else { }
        }

        if (elementData.ChangedDefaultRegionId)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.DefaultRegionId = elementData.DefaultRegionId;
            else { }
        }

        if (elementData.ChangedDefaultPositionX)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.DefaultPositionX = elementData.DefaultPositionX;
            else { }
        }

        if (elementData.ChangedDefaultPositionY)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.DefaultPositionY = elementData.DefaultPositionY;
            else { }
        }

        if (elementData.ChangedDefaultPositionZ)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.DefaultPositionZ = elementData.DefaultPositionZ;
            else { }
        }

        if (elementData.ChangedDefaultRotationX)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.DefaultRotationX = elementData.DefaultRotationX;
            else { }
        }

        if (elementData.ChangedDefaultRotationY)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.DefaultRotationY = elementData.DefaultRotationY;
            else { }
        }

        if (elementData.ChangedDefaultRotationZ)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.DefaultRotationZ = elementData.DefaultRotationZ;
            else { }
        }

        if (elementData.ChangedDefaultTime)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.DefaultTime = elementData.DefaultTime;
            else { }
        }

        if (elementData.ChangedPublicNotes)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.PublicNotes = elementData.PublicNotes;
            else { }
        }

        if (elementData.ChangedPrivateNotes)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.PrivateNotes = elementData.PrivateNotes;
            else { }
        }
    }

    static public void RemoveData(PhaseElementData elementData, DataRequest dataRequest)
    {

    }

    static public void UpdateIndex(PhaseElementData elementData)
    {
        var data = Fixtures.phaseList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        data.Index = elementData.Index;
    }
}
