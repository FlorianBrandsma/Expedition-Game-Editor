using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemManager
{
    private ItemController dataController;
    private List<ItemData> itemDataList;

    private List<DataManager.ObjectGraphicData> objectGraphicData_list;

    private DataManager dataManager = new DataManager();

    public List<ItemDataElement> GetItemDataElements(ItemController dataController)
    {
        this.dataController = dataController;

        GetItemData();
        GetObjectGraphicData();

        var list = (from itemData in itemDataList
                    join objectGraphicData in objectGraphicData_list on itemData.objectId equals objectGraphicData.id
                    select new ItemDataElement()
                    {
                        id      = itemData.id,
                        table   = itemData.table,
                        type    = itemData.type,
                        Index   = itemData.index,

                        ObjectGraphicId = itemData.objectId,
                        Name    = itemData.name,

                        objectGraphicName   = objectGraphicData.name,
                        objectGraphicIcon   = objectGraphicData.icon

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    internal void GetItemData()
    {
        itemDataList = new List<ItemData>();

        //Temporary
        for (int i = 0; i < dataController.temp_id_count; i++)
        {
            var itemData = new ItemData();

            itemData.id = (i + 1);
            itemData.table = "Item";
            itemData.type = (int)dataController.itemType;
            itemData.index = i;

            itemData.objectId = 2;
            itemData.name = "Item " + (i + 1);

            itemDataList.Add(itemData);
        }
    }

    internal void GetObjectGraphicData()
    {
        objectGraphicData_list = dataManager.GetObjectGraphicData(itemDataList.Select(x => x.objectId).Distinct().ToList(), true);
    }

    internal class ItemData : GeneralData
    {
        public int index;
        public string name;
        public int objectId;
    }
}
