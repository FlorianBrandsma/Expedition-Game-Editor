using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ItemDataManager
{
    private static List<ItemBaseData> itemDataList;

    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Item>().First();

        GetItemData(searchParameters);

        if (itemDataList.Count == 0) return new List<IElementData>();

        GetModelData();
        GetIconData();

        var list = (from itemData   in itemDataList
                    join modelData  in modelDataList    on itemData.ModelId equals modelData.Id
                    join iconData   in iconDataList     on modelData.IconId equals iconData.Id
                    select new ItemElementData()
                    {
                        Id = itemData.Id,
                        Index = itemData.Index,

                        Type = itemData.Type,

                        ModelId = itemData.ModelId,

                        Name = itemData.Name,

                        ModelPath = modelData.Path,
                        ModelIconPath = iconData.Path,
                        
                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetItemData(Search.Item searchParameters)
    {
        itemDataList = new List<ItemBaseData>();

        foreach(ItemBaseData item in Fixtures.itemList)
        {
            if (searchParameters.id.Count   > 0 && !searchParameters.id.Contains(item.Id)) continue;
            if (searchParameters.type.Count > 0 && !searchParameters.type.Contains(item.Type)) continue;

            itemDataList.Add(item);
        }
    }

    private static void GetModelData()
    {
        var searchParameters = new Search.Model();
        searchParameters.id = itemDataList.Select(x => x.ModelId).Distinct().ToList();

        modelDataList = DataManager.GetModelData(searchParameters);
    }

    private static void GetIconData()
    {
        var searchParameters = new Search.Icon();
        searchParameters.id = modelDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(searchParameters);
    }

    public static void UpdateData(ItemElementData elementData)
    {
        var data = Fixtures.itemList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (elementData.ChangedModelId)
            data.ModelId = elementData.ModelId;

        if (elementData.ChangedName)
            data.Name = elementData.Name;
    }

    static public void UpdateIndex(ItemElementData elementData)
    {
        var data = Fixtures.itemList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        data.Index = elementData.Index;
    }
}
