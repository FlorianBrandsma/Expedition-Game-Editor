using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectiveElementDataManager
{
    private ObjectiveElementController objectiveElementController;
    private List<ObjectiveElementData> objectiveElementDataList;

    public void InitializeManager(ObjectiveElementController objectiveElementController)
    {
        this.objectiveElementController = objectiveElementController;
    }

    public List<ObjectiveElementDataElement> GetObjectiveElementDataElements(IEnumerable searchParameters)
    {
        var objectiveElementSearchData = searchParameters.Cast<Search.ObjectiveElement>().FirstOrDefault();

        GetObjectiveElementData(objectiveElementSearchData);

        var list = (from objectiveElementData in objectiveElementDataList
                    select new ObjectiveElementDataElement()
                    {
                        id = objectiveElementData.id,
                        table = objectiveElementData.table,

                        Index = objectiveElementData.index,
                        Name = objectiveElementData.name

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetObjectiveElementData(Search.ObjectiveElement searchParameters)
    {
        objectiveElementDataList = new List<ObjectiveElementData>();

        for (int i = 0; i < searchParameters.temp_id_count; i++)
        {
            var objectiveElementData = new ObjectiveElementData();

            objectiveElementData.id = (i + 1);
            objectiveElementData.table = "ObjectiveElement";
            objectiveElementData.index = i;

            objectiveElementData.name = "ObjectiveElement " + (i + 1);

            objectiveElementDataList.Add(objectiveElementData);
        }
    }

    internal class ObjectiveElementData : GeneralData
    {
        public int index;
        public string name;
    }
}
