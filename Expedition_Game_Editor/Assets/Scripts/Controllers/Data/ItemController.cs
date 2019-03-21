using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemController : MonoBehaviour
{
    public ItemData itemData { get; set; }
    public ObjectGraphicData objectGraphicData { get; set; }

    public void InitializeController(Route route)
    {
        GetData(route.GeneralData()); 
    }

    private void GetData(GeneralData data)
    {
        var itemData_list = new List<ItemData>();

        //Placeholder magic
        var new_itemData        = new ItemData();
        new_itemData.id         = data.id;
        new_itemData.table      = data.table;
        new_itemData.type       = data.type;
        new_itemData.index      = 0;
        new_itemData.name       = data.table;
        new_itemData.objectId   = 0;

        itemData_list.Add(new_itemData);
        //
        
        itemData = (from item in itemData_list
                    select new ItemData()
                    {
                        id      = item.id,
                        table   = item.table,
                        type    = item.type,
                        index   = item.index,
                        //name    = (item.table + " " + item.index),
                        //objectId = item.objectId
                        objectId = item.id
                    }).ToList().FirstOrDefault();

        var objectGraphicData_list = new List<ObjectGraphicData>();

        //Placeholder magic
        var new_objectGraphicData           = new ObjectGraphicData();
        new_objectGraphicData.id            = itemData.objectId;
        new_objectGraphicData.objectPath    = "Objects/Item/" + data.id;
        new_objectGraphicData.iconPath      = "Textures/Objects/Icons/" + data.id;

        objectGraphicData_list.Add(new_objectGraphicData);
        //

        objectGraphicData = (from objectGraphic in objectGraphicData_list
                             select new ObjectGraphicData()
                             {
                                 id = objectGraphic.id,
                                 objectPath = objectGraphic.objectPath,
                                 iconPath = objectGraphic.iconPath

                             }).ToList().FirstOrDefault();
    }
}
