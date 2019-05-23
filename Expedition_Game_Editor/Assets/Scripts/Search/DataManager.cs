using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataManager
{
    #region Functions

    public List<ObjectGraphicData> GetObjectGraphicData(List<int> idList, bool searchById = false)
    {
        List<ObjectGraphicData> dataList = new List<ObjectGraphicData>();

        foreach(Fixtures.ObjectGraphic objectGraphic in Fixtures.objectGraphicList)
        {
            var data = new ObjectGraphicData();

            data.id = objectGraphic.id;
            data.table = "ObjectGraphic";

            data.name = objectGraphic.name;
            data.path = objectGraphic.path;
            data.icon = objectGraphic.icon;

            if (searchById)
            {
                if (idList.Contains(objectGraphic.id))
                    dataList.Add(data);
            } else {
                dataList.Add(data);
            }
        }

        return dataList;
    }

    public List<ElementData> GetElementData()
    {
        return GetElementData(new List<int>());
    }

    public List<ElementData> GetElementData(List<int> idList, bool searchById = false)
    {
        List<ElementData> dataList = new List<ElementData>();

        foreach(Fixtures.Element element in Fixtures.elementList)
        {
            var data = new ElementData();
            
            data.id = element.id;
            data.table = "Element";

            data.objectGraphicId = element.objectGraphicId;

            if (searchById)
            {
                if (idList.Contains(element.id))
                    dataList.Add(data);
            }
            else
            {
                dataList.Add(data);
            }
        }

        return dataList;
    }

    public List<ChapterElementData> GetChapterElementData(int chapterId, bool searchById = false)
    {
        List<ChapterElementData> dataList = new List<ChapterElementData>();

        //for(int i = 0; i < 4; i++)
        //{
        //    var data = new ChapterElementData();

        //    int id = (i + 1);

        //    int chapterId = (i + 1);

        //    if(searchById && )
        //}

        return dataList;
    }

    public List<RegionData> GetRegionData(List<int> idList, bool searchById = false)
    {
        List<RegionData> dataList = new List<RegionData>();

        foreach (Fixtures.Region element in Fixtures.regionList)
        {
            var data = new RegionData();

            data.id = element.id;
            data.table = "Region";

            data.name = element.name;

            if (searchById)
            {
                if (idList.Contains(element.id))
                    dataList.Add(data);
            }
            else
            {
                dataList.Add(data);
            }
        }

        return dataList;
    }

    #endregion

    #region Classes

    public class ObjectGraphicData : GeneralData
    {
        public string name;
        public string path;
        public string icon;
    }

    public class ElementData : GeneralData
    {
        public int objectGraphicId;
    }

    public class ChapterElementData : GeneralData
    {

    }

    public class RegionData : GeneralData
    {
        public string name;
    }

    #endregion
}