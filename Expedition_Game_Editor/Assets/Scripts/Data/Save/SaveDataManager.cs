using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class SaveDataManager
{
    private static List<SaveBaseData> saveDataList;

    private static List<PlayerSaveBaseData> playerSaveDataList;

    private static List<PartyMemberBaseData> partyMemberDataList;
    private static List<InteractableBaseData> interactableDataList;
    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    private static List<RegionBaseData> regionDataList;
    private static List<TerrainBaseData> terrainDataList;
    private static List<TileSetBaseData> tileSetDataList;

    private static List<PhaseBaseData> phaseDataList;
    private static List<ChapterBaseData> chapterDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Save>().First();

        GetSaveData(searchParameters);

        if (saveDataList.Count == 0) return new List<IElementData>();

        GetPlayerSaveData();

        GetPartyMemberData();
        GetInteractableData();
        GetModelData();
        GetIconData();

        GetRegionData();
        GetTerrainData();
        GetTileSetData();
        
        GetPhaseData();
        GetChapterData();

        var list = (from saveData in saveDataList
                    join playerSaveData     in playerSaveDataList       on saveData.Id                      equals playerSaveData.SaveId

                    join partyMemberData    in partyMemberDataList      on playerSaveData.PartyMemberId     equals partyMemberData.Id
                    join interactableData   in interactableDataList     on partyMemberData.InteractableId   equals interactableData.Id
                    join modelData          in modelDataList            on interactableData.ModelId         equals modelData.Id
                    join iconData           in iconDataList             on modelData.IconId                 equals iconData.Id

                    join regionData         in regionDataList           on playerSaveData.RegionId          equals regionData.Id
                    join tileSetData        in tileSetDataList          on regionData.TileSetId             equals tileSetData.Id

                    join phaseData          in phaseDataList            on regionData.PhaseId               equals phaseData.Id
                    join chapterData        in chapterDataList          on phaseData.ChapterId              equals chapterData.Id
                    select new SaveElementData()
                    {
                        Id = saveData.Id,
                        Index = saveData.Index,

                        GameId = saveData.GameId,

                        ModelIconPath = iconData.Path,

                        Name = "Ch. " + (chapterData.Index + 1) + ": " + chapterData.Name,
                        LocationName = RegionManager.LocationName(playerSaveData.PositionX, playerSaveData.PositionZ, tileSetData.TileSize, regionData, terrainDataList),

                        Time = TimeManager.TimeFromSeconds(playerSaveData.PlayedTime)

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetSaveData(Search.Save searchParameters)
    {
        saveDataList = new List<SaveBaseData>();

        foreach (SaveBaseData save in Fixtures.saveList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(save.Id))          continue;
            if (searchParameters.gameId.Count   > 0 && !searchParameters.gameId.Contains(save.GameId))  continue;

            var saveData = new SaveBaseData();

            saveData.Id = save.Id;
            saveData.Index = save.Index;

            saveData.GameId = save.GameId;

            saveDataList.Add(saveData);
        }
    }

    private static void GetPlayerSaveData()
    {
        var searchParameters = new Search.PlayerSave();
        searchParameters.saveId = saveDataList.Select(x => x.Id).Distinct().ToList();

        playerSaveDataList = DataManager.GetPlayerSaveData(searchParameters);
    }

    private static void GetPartyMemberData()
    {
        var searchParameters = new Search.PartyMember();
        searchParameters.id = playerSaveDataList.Select(x => x.PartyMemberId).Distinct().ToList();

        partyMemberDataList = DataManager.GetPartyMemberData(searchParameters);
    }

    private static void GetInteractableData()
    {
        var searchParameters = new Search.Interactable();
        searchParameters.id = partyMemberDataList.Select(x => x.InteractableId).Distinct().ToList();

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
        searchParameters.id = playerSaveDataList.Select(x => x.RegionId).Distinct().ToList();

        regionDataList = DataManager.GetRegionData(searchParameters);
    }

    private static void GetTerrainData()
    {
        var searchParameters = new Search.Terrain();
        searchParameters.regionId = regionDataList.Select(x => x.Id).Distinct().ToList();

        terrainDataList = DataManager.GetTerrainData(searchParameters);
    }

    private static void GetTileSetData()
    {
        var tileSetSearchParameters = new Search.TileSet();
        tileSetSearchParameters.id = regionDataList.Select(x => x.TileSetId).Distinct().ToList();

        tileSetDataList = DataManager.GetTileSetData(tileSetSearchParameters);
    }

    private static void GetPhaseData()
    {
        var searchParameters = new Search.Phase();
        searchParameters.id = regionDataList.Select(x => x.PhaseId).Distinct().ToList();

        phaseDataList = DataManager.GetPhaseData(searchParameters);
    }

    private static void GetChapterData()
    {
        var searchParameters = new Search.Chapter();
        searchParameters.id = phaseDataList.Select(x => x.ChapterId).Distinct().ToList();

        chapterDataList = DataManager.GetChapterData(searchParameters);
    }

    public static void UpdateData(SaveElementData elementData)
    {
        var data = Fixtures.saveList.Where(x => x.Id == elementData.Id).FirstOrDefault();
    }
}
