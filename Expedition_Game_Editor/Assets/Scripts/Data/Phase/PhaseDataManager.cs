using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class PhaseDataManager
{
    private static List<PhaseBaseData> phaseDataList;

    private static List<ChapterBaseData> chapterDataList;
    private static List<PartyMemberBaseData> partyMemberDataList;
    private static List<InteractableBaseData> interactableDataList;
    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    private static List<RegionBaseData> regionDataList;
    private static List<TerrainBaseData> terrainDataList;
    private static List<TileSetBaseData> tileSetDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Phase>().First();

        GetPhaseData(searchParameters);

        if (phaseDataList.Count == 0) return new List<IElementData>();

        GetChapterData();
        GetPartyMemberData();
        GetInteractableData();
        GetModelData();
        GetIconData();

        GetRegionData();
        GetTileSetData();
        GetTerrainData();

        var list = (from phaseData      in phaseDataList
                    join chapterData    in chapterDataList  on phaseData.ChapterId          equals chapterData.Id

                    join regionData     in regionDataList   on phaseData.DefaultRegionId    equals regionData.Id
                    join tileSetData    in tileSetDataList  on regionData.TileSetId         equals tileSetData.Id

                    join leftJoin in (from partyMemberData  in partyMemberDataList
                                      join interactableData in interactableDataList on partyMemberData.InteractableId   equals interactableData.Id
                                      join modelData        in modelDataList        on interactableData.ModelId         equals modelData.Id
                                      join iconData         in iconDataList         on modelData.IconId                 equals iconData.Id
                                      select new { partyMemberData, interactableData, modelData, iconData }) on chapterData.Id equals leftJoin.partyMemberData.ChapterId into partyMemberData

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

                        Scale = partyMemberData.First().interactableData.Scale,

                        DefaultTime = phaseData.DefaultTime,

                        PublicNotes = phaseData.PublicNotes,
                        PrivateNotes = phaseData.PrivateNotes,

                        TerrainTileId = TerrainTileId(phaseData.DefaultRegionId, phaseData.DefaultPositionX, phaseData.DefaultPositionZ),

                        PartyMemberId = partyMemberData.First().partyMemberData.Id,
                        
                        ModelId = partyMemberData.First().modelData.Id,
                        ModelPath = partyMemberData.First().modelData.Path,

                        ModelIconPath = partyMemberData.First().iconData.Path,
                        
                        Height = partyMemberData.First().modelData.Height,
                        Width = partyMemberData.First().modelData.Width,
                        Depth = partyMemberData.First().modelData.Depth,

                        InteractableName = partyMemberData.First().interactableData.Name,
                        LocationName = RegionManager.LocationName(phaseData.DefaultPositionX, phaseData.DefaultPositionZ, tileSetData.TileSize, regionData, terrainDataList)
                        
                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetPhaseData(Search.Phase searchParameters)
    {
        phaseDataList = new List<PhaseBaseData>();

        foreach(PhaseBaseData phase in Fixtures.phaseList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(phase.Id)) continue;
            if (searchParameters.chapterId.Count    > 0 && !searchParameters.chapterId.Contains(phase.ChapterId)) continue;

            var phaseData = new PhaseBaseData();

            phaseData.Id = phase.Id;
            phaseData.Index = phase.Index;

            phaseData.ChapterId = phase.ChapterId;

            phaseData.Name = phase.Name;

            phaseData.DefaultRegionId = phase.DefaultRegionId;

            phaseData.DefaultPositionX = phase.DefaultPositionX;
            phaseData.DefaultPositionY = phase.DefaultPositionY;
            phaseData.DefaultPositionZ = phase.DefaultPositionZ;

            phaseData.DefaultRotationX = phase.DefaultRotationX;
            phaseData.DefaultRotationY = phase.DefaultRotationY;
            phaseData.DefaultRotationZ = phase.DefaultRotationZ;

            phaseData.DefaultTime = phase.DefaultTime;

            phaseData.PublicNotes = phase.PublicNotes;
            phaseData.PrivateNotes = phase.PrivateNotes;

            phaseDataList.Add(phaseData);
        }
    }

    private static void GetChapterData()
    {
        var chapterSearchParameters = new Search.Chapter();

        chapterSearchParameters.id = phaseDataList.Select(x => x.ChapterId).Distinct().ToList();

        chapterDataList = DataManager.GetChapterData(chapterSearchParameters);
    }

    private static void GetPartyMemberData()
    {
        var partyMemberSearchParameters = new Search.PartyMember();

        partyMemberSearchParameters.chapterId = chapterDataList.Select(x => x.Id).Distinct().ToList();

        partyMemberDataList = DataManager.GetPartyMemberData(partyMemberSearchParameters);
    }

    private static void GetInteractableData()
    {
        var interactableSearchParameters = new Search.Interactable();

        interactableSearchParameters.id = partyMemberDataList.Select(x => x.InteractableId).Distinct().ToList();

        interactableDataList = DataManager.GetInteractableData(interactableSearchParameters);
    }

    private static void GetModelData()
    {
        var modelSearchParameters = new Search.Model();

        modelSearchParameters.id = interactableDataList.Select(x => x.ModelId).Distinct().ToList();

        modelDataList = DataManager.GetModelData(modelSearchParameters);
    }

    private static void GetIconData()
    {
        var iconSearchParameters = new Search.Icon();
        iconSearchParameters.id = modelDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(iconSearchParameters);
    }

    private static void GetRegionData()
    {
        var regionSearchParameters = new Search.Region();
        regionSearchParameters.id = phaseDataList.Select(x => x.DefaultRegionId).Distinct().ToList();

        regionDataList = DataManager.GetRegionData(regionSearchParameters);
    }

    private static void GetTileSetData()
    {
        var tileSetSearchParameters = new Search.TileSet();
        tileSetSearchParameters.id = regionDataList.Select(x => x.TileSetId).Distinct().ToList();

        tileSetDataList = DataManager.GetTileSetData(tileSetSearchParameters);
    }

    private static void GetTerrainData()
    {
        var terrainSearchParameters = new Search.Terrain();
        terrainSearchParameters.regionId = regionDataList.Select(x => x.Id).Distinct().ToList();

        terrainDataList = DataManager.GetTerrainData(terrainSearchParameters);
    }

    private static int TerrainTileId(int regionId, float positionX, float positionZ)
    {
        var terrainId = Fixtures.GetTerrain(regionId, positionX, positionZ);

        var terrainTileId = Fixtures.GetTerrainTile(terrainId, positionX, positionZ);

        return terrainTileId;
    }

    public static void UpdateData(PhaseElementData elementData)
    {
        var data = Fixtures.phaseList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedName)
            data.Name = elementData.Name;

        if (elementData.ChangedDefaultRegionId)
            data.DefaultRegionId = elementData.DefaultRegionId;

        if (elementData.ChangedDefaultPositionX)
            data.DefaultPositionX = elementData.DefaultPositionX;

        if (elementData.ChangedDefaultPositionY)
            data.DefaultPositionY = elementData.DefaultPositionY;

        if (elementData.ChangedDefaultPositionZ)
            data.DefaultPositionZ = elementData.DefaultPositionZ;

        if (elementData.ChangedDefaultRotationX)
            data.DefaultRotationX = elementData.DefaultRotationX;

        if (elementData.ChangedDefaultRotationY)
            data.DefaultRotationY = elementData.DefaultRotationY;

        if (elementData.ChangedDefaultRotationZ)
            data.DefaultRotationZ = elementData.DefaultRotationZ;

        if (elementData.ChangedDefaultTime)
            data.DefaultTime = elementData.DefaultTime;

        if (elementData.ChangedPublicNotes)
            data.PublicNotes = elementData.PublicNotes;

        if (elementData.ChangedPrivateNotes)
            data.PrivateNotes = elementData.PrivateNotes;
    }

    static public void UpdateIndex(TaskElementData elementData)
    {
        var data = Fixtures.phaseList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        data.Index = elementData.Index;
    }
}
