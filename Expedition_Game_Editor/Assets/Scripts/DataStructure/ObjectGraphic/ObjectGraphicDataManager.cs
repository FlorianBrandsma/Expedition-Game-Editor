using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectGraphicDataManager : IDataManager
{
    public IDataController DataController { get; set; }
    private List<ObjectGraphicData> objectGraphicDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.IconData> iconDataList;

    public ObjectGraphicDataManager(ObjectGraphicController objectGraphicController)
    {
        this.DataController = objectGraphicController;
    }

    public List<IDataElement> GetDataElements(IEnumerable searchParameters)
    {
        var objectGraphicSearchData = searchParameters.Cast<Search.ObjectGraphic>().FirstOrDefault();

        GetObjectGraphicData(objectGraphicSearchData);
        GetIconData();

        var list = (from objectGraphicData in objectGraphicDataList
                    join iconData in iconDataList on objectGraphicData.iconId equals iconData.Id
                    select new ObjectGraphicDataElement()
                    {
                        DataType = Enums.DataType.ObjectGraphic,

                        Id = objectGraphicData.Id,

                        IconId = objectGraphicData.iconId,
                        
                        Name = objectGraphicData.name,
                        Path = objectGraphicData.path,

                        Height = objectGraphicData.height,
                        Width = objectGraphicData.width,
                        Depth = objectGraphicData.depth,

                        iconPath = iconData.path,
                        category = iconData.category

                    }).OrderByDescending(x => x.Id == 1).ThenBy(x => x.category).ThenBy(x => x.Name).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetObjectGraphicData(Search.ObjectGraphic searchParameters)
    {
        objectGraphicDataList = new List<ObjectGraphicData>();

        foreach(Fixtures.ObjectGraphic objectGraphic in Fixtures.objectGraphicList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(objectGraphic.Id)) continue;

            var objectGraphicData = new ObjectGraphicData();

            objectGraphicData.Id = objectGraphic.Id;

            objectGraphicData.name = objectGraphic.name;
            objectGraphicData.path = objectGraphic.path;
            objectGraphicData.iconId = objectGraphic.iconId;

            objectGraphicData.height = objectGraphic.height;
            objectGraphicData.width = objectGraphic.width;
            objectGraphicData.depth = objectGraphic.depth;

            objectGraphicDataList.Add(objectGraphicData);
        }
    }

    internal void GetIconData()
    {
        var iconSearchParameters = new Search.Icon();
        iconSearchParameters.id = objectGraphicDataList.Select(x => x.iconId).Distinct().ToList();

        iconDataList = dataManager.GetIconData(iconSearchParameters);
    }

    internal class ObjectGraphicData : GeneralData
    {
        public int iconId;
        public string name;
        public string path;
        public float height;
        public float width;
        public float depth;
    }
}
