using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ItemDataManager
{
    private static List<ItemBaseData> itemDataList;

    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(Search.Item searchParameters)
    {
        GetItemData(searchParameters);

        if (searchParameters.includeAddElement)
            itemDataList.Add(DefaultData(searchParameters.type.First()));

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

    public static ItemElementData DefaultData(int type)
    {
        return new ItemElementData()
        {
            Id = -1,

            ModelId = 1,

            Type = type
        };
    }

    public static void SetDefaultAddValues(List<ItemElementData> list)
    {
        var addElementData = list.Where(x => x.Id == -1).First();

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

            elementData.SetOriginalValues();

        } else { }
    }

    public static void UpdateData(ItemElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.itemList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedModelId)
            {
                data.ModelId = elementData.ModelId;
            }

            if (elementData.ChangedName)
            {
                data.Name = elementData.Name;
            }

            elementData.SetOriginalValues();

        } else { }
    }

    static public void UpdateIndex(ItemElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.ChangedIndex) return;

        var data = Fixtures.itemList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            data.Index = elementData.Index;

            elementData.OriginalData.Index = elementData.Index;

        } else { }
    }

    public static void RemoveData(ItemElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.itemList.RemoveAll(x => x.Id == elementData.Id);

            elementData.RemoveIndex(dataRequest);

        } else { }
    }

    public static void RemoveIndex(ItemElementData elementData, DataRequest dataRequest)
    {
        var itemSearchParameters = new Search.Item()
        {
            type = new List<int>() { elementData.Type }
        };

        var itemDataList = DataManager.GetItemData(itemSearchParameters);

        itemDataList.Where(x => x.Index > elementData.Index).ToList().ForEach(itemData =>
        {
            var itemElementData = new ItemElementData()
            {
                Id = itemData.Id,
                Index = itemData.Index
            };

            itemElementData.SetOriginalValues();

            itemElementData.Index--;

            itemElementData.UpdateIndex(dataRequest);
        });
    }
}
