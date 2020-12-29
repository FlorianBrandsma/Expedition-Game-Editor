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

        if (searchParameters.includeAddElement)
            itemDataList.Add(DefaultData(searchParameters));

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
                        
                    }).OrderBy(x => x.Id > 0).ThenBy(x => x.Index).ToList();
        
        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static ItemBaseData DefaultData(Search.Item searchParameters)
    {
        return new ItemBaseData()
        {
            Type = searchParameters.type.First(),
            ModelId = 1
        };
    }

    private static void SetDefaultAddValues(List<ItemElementData> list)
    {
        var addElementData = list.Where(x => x.Id == 0).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;

        addElementData.Index = list.Count - 1;
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

    public static void AddData(ItemElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.itemList.Count > 0 ? (Fixtures.itemList[Fixtures.itemList.Count - 1].Id + 1) : 1;
            Fixtures.itemList.Add(((ItemData)elementData).Clone());
        } else { }
    }

    public static void UpdateData(ItemElementData elementData, DataRequest dataRequest)
    {
        var data = Fixtures.itemList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (elementData.ChangedModelId)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.ModelId = elementData.ModelId;
            else { }
        }

        if (elementData.ChangedName)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.Name = elementData.Name;
            else { }
        }
    }

    static public void UpdateIndex(ItemElementData elementData)
    {
        var data = Fixtures.itemList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        data.Index = elementData.Index;
    }

    public static void RemoveData(ItemElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.itemList.RemoveAll(x => x.Id == elementData.Id);
        }
        else { }
    }
}
