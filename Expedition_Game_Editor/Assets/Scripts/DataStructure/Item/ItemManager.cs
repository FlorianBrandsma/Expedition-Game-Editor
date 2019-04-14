using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemManager
{
    private ItemController dataController;
    private List<ItemData> itemData_list;

    public List<ItemDataElement> GetItemDataElements(ItemController dataController)
    {
        this.dataController = dataController;

        GetItemData();
        //GetIconData()?

        var list = (from oCore in itemData_list
                    select new ItemDataElement()
                    {
                        id = oCore.id,
                        table = oCore.table,
                        type = oCore.type,
                        index = oCore.index,
                        name = oCore.name,

                    }).OrderBy(x => x.index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetItemData()
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

            itemData.name = "Item " + (i + 1);

            itemData_list.Add(itemData);
        }
    }

    internal class ItemData : GeneralData
    {
        public int index;
        public string name;
    }
}
