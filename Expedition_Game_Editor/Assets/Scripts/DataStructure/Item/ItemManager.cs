using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemManager
{
    private ItemController dataController;
    private List<ItemData> itemData_list;

    private List<DataManager.ObjectGraphicData> objectGraphicData_list;

    private DataManager dataManager = new DataManager();

    public List<ItemDataElement> GetItemDataElements(ItemController dataController)
    {
        this.dataController = dataController;

        GetItemData();
        GetObjectGraphicData();

        var list = (from itemData in itemData_list
                    join objectGraphicData in objectGraphicData_list on itemData.object_id equals objectGraphicData.id
                    select new ItemDataElement()
                    {
                        id = itemData.id,
                        table = itemData.table,
                        type = itemData.type,
                        index = itemData.index,

                        name = itemData.name,

                        object_name = objectGraphicData.name,
                        icon = objectGraphicData.icon

                    }).OrderBy(x => x.index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    internal void GetItemData()
    {
        itemData_list = new List<ItemData>();

        //Temporary
        for (int i = 0; i < dataController.temp_id_count; i++)
        {
            var itemData = new ItemData();

            itemData.id = (i + 1);
            itemData.table = "Item";
            itemData.type = (int)dataController.type;
            itemData.index = i;

            itemData.object_id = 2;
            itemData.name = "Item " + (i + 1);

            itemData_list.Add(itemData);
        }
    }

    internal void GetObjectGraphicData()
    {
        objectGraphicData_list = dataManager.GetObjectGraphicData(itemData_list.Select(x => x.object_id).Distinct().ToList(), true);
    }

    internal class ItemData : GeneralData
    {
        public int index;
        public string name;
        public int object_id;
    }
}
