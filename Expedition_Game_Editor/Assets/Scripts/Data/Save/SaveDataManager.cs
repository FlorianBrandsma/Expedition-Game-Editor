using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SaveDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<SaveData> saveDataList;

    public SaveDataManager(SaveController saveController)
    {
        DataController = saveController;
    }

    public List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Save>().First();

        GetSaveData(searchParameters);

        if (saveDataList.Count == 0) return new List<IElementData>();

        var list = (from saveData in saveDataList
                    select new SaveElementData()
                    {
                        Id = saveData.Id,
                        GameId = saveData.gameId,

                        name = "Save " + (saveData.Index + 1)

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public void GetSaveData(Search.Save searchParameters)
    {
        saveDataList = new List<SaveData>();

        foreach (Fixtures.Save save in Fixtures.saveList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(save.Id))          continue;
            if (searchParameters.gameId.Count   > 0 && !searchParameters.gameId.Contains(save.gameId))  continue;

            var saveData = new SaveData();

            saveData.Id = save.Id;

            saveData.gameId = save.gameId;

            saveDataList.Add(saveData);
        }
    }

    internal class SaveData : GeneralData
    {
        public int gameId;
    }
}
