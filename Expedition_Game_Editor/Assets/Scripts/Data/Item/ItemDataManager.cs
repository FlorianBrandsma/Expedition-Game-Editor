using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ItemDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<ItemData> itemDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;
    
    public ItemDataManager(ItemController itemController)
    {
        DataController = itemController;
    }

    public List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Item>().First();

        GetItemData(searchParameters);

        if (itemDataList.Count == 0) return new List<IElementData>();

        GetObjectGraphicData();
        GetIconData();

        var list = (from itemData           in itemDataList
                    join objectGraphicData  in objectGraphicDataList    on itemData.objectGraphicId equals objectGraphicData.id
                    join iconData           in iconDataList             on objectGraphicData.iconId equals iconData.id
                    select new ItemElementData()
                    {
                        Id = itemData.id,
                        Index = itemData.index,

                        Type = itemData.type,

                        ObjectGraphicId = itemData.objectGraphicId,

                        Name = itemData.name,

                        objectGraphicPath = objectGraphicData.path,
                        objectGraphicIconPath = iconData.path,
                        
                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    internal void GetItemData(Search.Item searchParameters)
    {
        itemDataList = new List<ItemData>();

        foreach(Fixtures.Item item in Fixtures.itemList)
        {
            if (searchParameters.id.Count   > 0 && !searchParameters.id.Contains(item.id)) continue;
            if (searchParameters.type.Count > 0 && !searchParameters.type.Contains(item.type)) continue;

            var itemData = new ItemData();

            itemData.id = item.id;
            itemData.index = item.index;

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
        var iconSearchParameters = new Search.Icon();
        iconSearchParameters.id = objectGraphicDataList.Select(x => x.iconId).Distinct().ToList();

        iconDataList = dataManager.GetIconData(iconSearchParameters);
    }

    internal class ItemData
    {
        public int id;
        public int index;

        public int type;

        public string name;

        public int objectGraphicId;
    }
}
