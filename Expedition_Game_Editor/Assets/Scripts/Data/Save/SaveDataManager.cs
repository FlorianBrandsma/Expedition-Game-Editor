using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SaveDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<SaveData> saveDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.PlayerSaveData> playerSaveDataList;

    private List<DataManager.PartyMemberData> partyMemberDataList;
    private List<DataManager.InteractableData> interactableDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    private List<DataManager.RegionData> regionDataList;
    private List<DataManager.TerrainData> terrainDataList;
    private List<DataManager.TileSetData> tileSetDataList;

    private List<DataManager.PhaseData> phaseDataList;
    private List<DataManager.ChapterData> chapterDataList;

    public SaveDataManager(SaveController saveController)
    {
        DataController = saveController;
    }

    public List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Save>().First();

        GetSaveData(searchParameters);

        if (saveDataList.Count == 0) return new List<IElementData>();

        GetPlayerSaveData();

        GetPartyMemberData();
        GetInteractableData();
        GetObjectGraphicData();
        GetIconData();

        GetRegionData();
        GetTerrainData();
        GetTileSetData();
        
        GetPhaseData();
        GetChapterData();

        var list = (from saveData in saveDataList
                    join playerSaveData     in playerSaveDataList       on saveData.id                      equals playerSaveData.saveId

                    join partyMemberData    in partyMemberDataList      on playerSaveData.partyMemberId     equals partyMemberData.id
                    join interactableData   in interactableDataList     on partyMemberData.interactableId   equals interactableData.id
                    join objectGraphicData  in objectGraphicDataList    on interactableData.objectGraphicId equals objectGraphicData.id
                    join iconData           in iconDataList             on objectGraphicData.iconId         equals iconData.id

                    join regionData         in regionDataList           on playerSaveData.regionId          equals regionData.id
                    join tileSetData        in tileSetDataList          on regionData.tileSetId             equals tileSetData.id

                    join phaseData          in phaseDataList            on regionData.phaseId               equals phaseData.id
                    join chapterData        in chapterDataList          on phaseData.chapterId              equals chapterData.id
                    select new SaveElementData()
                    {
                        Id = saveData.id,
                        GameId = saveData.gameId,

                        objectGraphicIconPath = iconData.path,

                        name = "Ch. " + (chapterData.index + 1) + ": " + chapterData.name,
                        locationName = RegionManager.LocationName(playerSaveData.positionX, playerSaveData.positionZ, tileSetData.tileSize, regionData, terrainDataList),

                        time = TimeManager.TimeFromSeconds(playerSaveData.playedSeconds)

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public void GetSaveData(Search.Save searchParameters)
    {
        saveDataList = new List<SaveData>();

        foreach (Fixtures.Save save in Fixtures.saveList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(save.id))          continue;
            if (searchParameters.gameId.Count   > 0 && !searchParameters.gameId.Contains(save.gameId))  continue;

            var saveData = new SaveData();

            saveData.id = save.id;

            saveData.gameId = save.gameId;

            saveDataList.Add(saveData);
        }
    }

    internal void GetPlayerSaveData()
    {
        var searchParameters = new Search.PlayerSave();
        searchParameters.saveId = saveDataList.Select(x => x.id).Distinct().ToList();

        playerSaveDataList = dataManager.GetPlayerSaveData(searchParameters);
    }

    internal void GetPartyMemberData()
    {
        var searchParameters = new Search.PartyMember();
        searchParameters.id = playerSaveDataList.Select(x => x.partyMemberId).Distinct().ToList();

        partyMemberDataList = dataManager.GetPartyMemberData(searchParameters);
    }

    internal void GetInteractableData()
    {
        var searchParameters = new Search.Interactable();
        searchParameters.id = partyMemberDataList.Select(x => x.interactableId).Distinct().ToList();

        interactableDataList = dataManager.GetInteractableData(searchParameters);
    }

    internal void GetObjectGraphicData()
    {
        var searchParameters = new Search.ObjectGraphic();
        searchParameters.id = interactableDataList.Select(x => x.objectGraphicId).Distinct().ToList();

        objectGraphicDataList = dataManager.GetObjectGraphicData(searchParameters);
    }

    internal void GetIconData()
    {
        var searchParameters = new Search.Icon();
        searchParameters.id = objectGraphicDataList.Select(x => x.iconId).Distinct().ToList();

        iconDataList = dataManager.GetIconData(searchParameters);
    }

    internal void GetRegionData()
    {
        var searchParameters = new Search.Region();
        searchParameters.id = playerSaveDataList.Select(x => x.regionId).Distinct().ToList();

        regionDataList = dataManager.GetRegionData(searchParameters);
    }

    internal void GetTerrainData()
    {
        var searchParameters = new Search.Terrain();
        searchParameters.regionId = regionDataList.Select(x => x.id).Distinct().ToList();

        terrainDataList = dataManager.GetTerrainData(searchParameters);
    }

    private void GetTileSetData()
    {
        var tileSetSearchParameters = new Search.TileSet();
        tileSetSearchParameters.id = regionDataList.Select(x => x.tileSetId).Distinct().ToList();

        tileSetDataList = dataManager.GetTileSetData(tileSetSearchParameters);
    }

    internal void GetPhaseData()
    {
        var searchParameters = new Search.Phase();
        searchParameters.id = regionDataList.Select(x => x.phaseId).Distinct().ToList();

        phaseDataList = dataManager.GetPhaseData(searchParameters);
    }

    internal void GetChapterData()
    {
        var searchParameters = new Search.Chapter();
        searchParameters.id = phaseDataList.Select(x => x.chapterId).Distinct().ToList();

        chapterDataList = dataManager.GetChapterData(searchParameters);
    }

    internal class SaveData
    {
        public int id;
        public int index;

        public int gameId;
    }
}
