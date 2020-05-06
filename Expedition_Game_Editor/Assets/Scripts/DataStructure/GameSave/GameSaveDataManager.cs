using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameSaveDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<GameSaveData> gameSaveDataList;

    public GameSaveDataManager(GameSaveController chapterController)
    {
        DataController = chapterController;
    }

    public List<IDataElement> GetDataElements(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.GameSave>().First();

        GetGameSaveData(searchParameters);

        if (gameSaveDataList.Count == 0) return new List<IDataElement>();

        var list = (from gameSaveData in gameSaveDataList
                    select new GameSaveDataElement()
                    {
                        Id = gameSaveData.Id,
                        GameId = gameSaveData.gameId,

                        name = "Save " + (gameSaveData.Index + 1)

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetGameSaveData(Search.GameSave searchParameters)
    {
        gameSaveDataList = new List<GameSaveData>();

        foreach (Fixtures.GameSave gameSave in Fixtures.gameSaveList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(gameSave.Id))          return;
            if (searchParameters.gameId.Count   > 0 && !searchParameters.gameId.Contains(gameSave.gameId))  return;

            var gameSaveData = new GameSaveData();

            gameSaveData.Id = gameSave.Id;

            gameSaveData.gameId = gameSave.gameId;

            gameSaveDataList.Add(gameSaveData);
        }
    }

    internal class GameSaveData : GeneralData
    {
        public int gameId;
    }
}
