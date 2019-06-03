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

    public void InitializeManager(ItemController itemController)
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
                    join objectGraphicData in objectGraphicDataList on itemData.objectId equals objectGraphicData.id
                    join iconData in iconDataList on objectGraphicData.iconId equals iconData.id
                    select new ItemDataElement()
                    {
                        id      = itemData.id,
                        table   = itemData.table,

                        Type    = itemData.type,
                        Index   = itemData.index,
                        ObjectGraphicId = itemData.objectId,
                        Name    = itemData.name,

                        objectGraphicPath   = objectGraphicData.path,
                        objectGraphicIconPath   = iconData.path

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    internal void GetItemData(Search.Item searchParameters)
    {
        itemDataList = new List<ItemData>();

        foreach(Fixtures.Item item in Fixtures.itemList)
        {
            var itemData = new ItemData();

            itemData.id = item.id;
            itemData.table = "Item";

            if (searchParameters.type.Count > 0 && !searchParameters.type.Contains(item.type)) continue;

            itemData.type = item.type;
            itemData.index = item.index;

            itemData.objectId = item.objectGraphicId;
            itemData.name = item.name;

            itemDataList.Add(itemData);
        }
    }

    internal void GetObjectGraphicData()
    {
        objectGraphicDataList = dataManager.GetObjectGraphicData(itemDataList.Select(x => x.objectId).Distinct().ToList(), true);
    }

    internal void GetIconData()
    {
        iconDataList = dataManager.GetIconData(objectGraphicDataList.Select(x => x.iconId).Distinct().ToList(), true);
    }

    internal class ItemData : GeneralData
    {
        public int type;
        public string name;
        public int objectId;
    }
}
