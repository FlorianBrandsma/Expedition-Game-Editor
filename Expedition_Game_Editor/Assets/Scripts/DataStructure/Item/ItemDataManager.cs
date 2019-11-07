using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ItemDataManager
{
    private ItemController itemController;
    private List<ItemData> itemDataList;

    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    private DataManager dataManager = new DataManager();

    public ItemDataManager(ItemController itemController)
    {
        this.itemController = itemController;
    }

    public List<IDataElement> GetItemDataElements(IEnumerable searchParameters)
    {
        var searchItem = searchParameters.Cast<Search.Item>().FirstOrDefault();

        GetItemData(searchItem);
        GetObjectGraphicData();
        GetIconData();

        var list = (from itemData in itemDataList
                    join objectGraphicData in objectGraphicDataList on itemData.objectGraphicId equals objectGraphicData.Id
                    join iconData in iconDataList on objectGraphicData.iconId equals iconData.Id
                    select new ItemDataElement()
                    {
                        DataType = Enums.DataType.Item,

                        Id = itemData.Id,
                        Index = itemData.Index,

                        Type = itemData.type,
                        ObjectGraphicId = itemData.objectGraphicId,
                        Name = itemData.name,

                        objectGraphicPath = objectGraphicData.path,
                        objectGraphicIconPath = iconData.path,
                        
                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    internal void GetItemData(Search.Item searchParameters)
    {
        itemDataList = new List<ItemData>();

        foreach(Fixtures.Item item in Fixtures.itemList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(item.Id)) continue;
            if (searchParameters.type.Count > 0 && !searchParameters.type.Contains(item.type)) continue;

            var itemData = new ItemData();

            itemData.Id = item.Id;
            itemData.Index = item.Index;

            itemData.type = item.type;
            itemData.objectGraphicId = item.objectGraphicId;
            itemData.name = item.name;

            itemDataList.Add(itemData);
        }
    }

    internal void GetObjectGraphicData()
    {
        var objectGraphicSearchParameters = new Search.ObjectGraphic();

        objectGraphicSearchParameters.id = itemDataList.Select(x => x.objectGraphicId).Distinct().ToList();

        objectGraphicDataList = dataManager.GetObjectGraphicData(objectGraphicSearchParameters);
    }

    internal void GetIconData()
    {
        iconDataList = dataManager.GetIconData(objectGraphicDataList.Select(x => x.iconId).Distinct().ToList(), true);
    }

    internal class ItemData : GeneralData
    {
        public int type;
        public string name;
        public int objectGraphicId;
    }
}
