using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ItemDataManager
{
    private ItemController itemController;
    private List<ItemData> itemDataList;

    private List<DataManager.ObjectGraphicData> objectGraphicData_list;

    private DataManager dataManager = new DataManager();

    public void InitializeManager(ItemController itemController)
    {
        this.itemController = itemController;
    }

    public List<ItemDataElement> GetItemDataElements(IEnumerable searchParameters)
    {
        var searchItem = searchParameters.Cast<Search.Item>().FirstOrDefault();

        GetItemData(searchItem);
        GetObjectGraphicData();

        var list = (from itemData in itemDataList
                    join objectGraphicData in objectGraphicData_list on itemData.objectId equals objectGraphicData.id
                    select new ItemDataElement()
                    {
                        id      = itemData.id,
                        table   = itemData.table,

                        Type    = itemData.type,
                        Index   = itemData.index,
                        ObjectGraphicId = itemData.objectId,
                        Name    = itemData.name,

                        objectGraphicName   = objectGraphicData.name,
                        objectGraphicIcon   = objectGraphicData.icon

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    internal void GetItemData(Search.Item searchParameters)
    {
        itemDataList = new List<ItemData>();

        int index = 0;

        for (int i = 0; i < searchParameters.temp_id_count; i++)
        {
            var itemData = new ItemData();

            int id = (i + 1);

            itemData.id = id;
            itemData.table = "Item";

            int type = (i / (searchParameters.temp_id_count / 3));

            if (searchParameters.type.Count > 0 && !searchParameters.type.Contains(type)) continue;

            itemData.type = type;
            itemData.index = index;

            itemData.objectId = 1;
            itemData.name = "Item " + id;

            itemDataList.Add(itemData);

            index++;
        }
    }

    internal void GetObjectGraphicData()
    {
        objectGraphicData_list = dataManager.GetObjectGraphicData(itemDataList.Select(x => x.objectId).Distinct().ToList(), true);
    }

    internal class ItemData : GeneralData
    {
        public int type;
        public int index;
        public string name;
        public int objectId;
    }
}
