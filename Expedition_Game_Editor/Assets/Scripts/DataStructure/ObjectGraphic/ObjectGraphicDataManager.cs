using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectGraphicDataManager
{
    private ObjectGraphicController objectGraphicController;
    private List<ObjectGraphicData> objectGraphicDataList;

    public void InitializeManager(ObjectGraphicController objectGraphicController)
    {
        this.objectGraphicController = objectGraphicController;
    }

    public List<IDataElement> GetObjectGraphicDataElements(IEnumerable searchParameters)
    {
        var objectGraphicSearchData = searchParameters.Cast<Search.ObjectGraphic>().FirstOrDefault();

        GetObjectGraphicData(objectGraphicSearchData);

        var list = (from objectGraphicData in objectGraphicDataList
                    select new ObjectGraphicDataElement()
                    {
                        id = objectGraphicData.id,
                        table = objectGraphicData.table,

                        Name = objectGraphicData.name,
                        Path = objectGraphicData.path,
                        Icon = objectGraphicData.icon

                    }).OrderByDescending(x => x.id == 1).ThenBy(x => x.Name).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetObjectGraphicData(Search.ObjectGraphic searchParameters)
    {
        objectGraphicDataList = new List<ObjectGraphicData>();

        foreach(Fixtures.ObjectGraphic objectGraphic in Fixtures.objectGraphicList)
        {
            var objectGraphicData = new ObjectGraphicData();

            objectGraphicData.id = objectGraphic.id;
            objectGraphicData.table = "ObjectGraphic";

            objectGraphicData.name = objectGraphic.name;
            objectGraphicData.path = objectGraphic.path;
            objectGraphicData.icon = objectGraphic.icon;

            objectGraphicDataList.Add(objectGraphicData);
        }
    }

    internal class ObjectGraphicData : GeneralData
    {
        public string name;
        public string path;
        public string icon;
    }
}
