﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectGraphicDataManager
{
    private ObjectGraphicController objectGraphicController;
    private List<ObjectGraphicData> objectGraphicDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.IconData> iconDataList;

    public void InitializeManager(ObjectGraphicController objectGraphicController)
    {
        this.objectGraphicController = objectGraphicController;
    }

    public List<IDataElement> GetObjectGraphicDataElements(IEnumerable searchParameters)
    {
        var objectGraphicSearchData = searchParameters.Cast<Search.ObjectGraphic>().FirstOrDefault();

        GetObjectGraphicData(objectGraphicSearchData);
        GetIconData();

        var list = (from objectGraphicData in objectGraphicDataList
                    join iconData in iconDataList on objectGraphicData.iconId equals iconData.id
                    select new ObjectGraphicDataElement()
                    {
                        dataType = Enums.DataType.ObjectGraphic,

                        id = objectGraphicData.id,
                        
                        Name = objectGraphicData.name,
                        Path = objectGraphicData.path,
                        IconId = objectGraphicData.iconId,
                        iconPath = iconData.path

                    }).OrderByDescending(x => x.id == 1).ThenBy(x => x.Name).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetObjectGraphicData(Search.ObjectGraphic searchParameters)
    {
        objectGraphicDataList = new List<ObjectGraphicData>();

        foreach(Fixtures.ObjectGraphic objectGraphic in Fixtures.objectGraphicList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(objectGraphic.id)) continue;

            var objectGraphicData = new ObjectGraphicData();

            objectGraphicData.id = objectGraphic.id;

            objectGraphicData.name = objectGraphic.name;
            objectGraphicData.path = objectGraphic.path;
            objectGraphicData.iconId = objectGraphic.iconId;

            objectGraphicDataList.Add(objectGraphicData);
        }
    }

    internal void GetIconData()
    {
        iconDataList = dataManager.GetIconData(objectGraphicDataList.Select(x => x.iconId).Distinct().ToList(), true);
    }

    internal class ObjectGraphicData : GeneralData
    {
        public int iconId;
        public string name;
        public string path;
    }
}