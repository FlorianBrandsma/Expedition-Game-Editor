using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectiveDataManager
{
    private ObjectiveController objectiveController;
    private List<ObjectiveData> objectiveDataList;

    public void InitializeManager(ObjectiveController objectiveController)
    {
        this.objectiveController = objectiveController;
    }

    public List<ObjectiveDataElement> GetObjectiveDataElements(IEnumerable searchParameters)
    {
        var objectiveSearchData = searchParameters.Cast<Search.Objective>().FirstOrDefault();

        GetObjectiveData(objectiveSearchData);

        var list = (from objectiveData in objectiveDataList
                    select new ObjectiveDataElement()
                    {
                        id = objectiveData.id,
                        table = objectiveData.table,

                        Index = objectiveData.index,
                        Name = objectiveData.name

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetObjectiveData(Search.Objective searchParameters)
    {
        objectiveDataList = new List<ObjectiveData>();

        int index = 0;

        for (int i = 0; i < searchParameters.temp_id_count; i++)
        {
            var objectiveData = new ObjectiveData();

            int id = (i + 1);

            objectiveData.id = id;
            objectiveData.table = "Objective";
            objectiveData.index = index;

            objectiveData.name = "Objective " + id;

            objectiveDataList.Add(objectiveData);

            index++;
        }
    }

    internal class ObjectiveData : GeneralData
    {
        public int index;
        public string name;
    }
}
