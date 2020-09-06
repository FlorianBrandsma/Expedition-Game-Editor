using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class ModelDataManager
{
    private static List<ModelBaseData> modelDataList;

    private static List<IconBaseData> iconDataList;
    
    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Model>().First();

        GetModelData(searchParameters);

        if (modelDataList.Count == 0) return new List<IElementData>();

        GetIconData();

        var list = (from modelData  in modelDataList
                    join iconData   in iconDataList on modelData.IconId equals iconData.Id
                    select new ModelElementData()
                    {
                        Id = modelData.Id,

                        IconId = modelData.IconId,
                        
                        Name = modelData.Name,
                        Path = modelData.Path,

                        Height = modelData.Height,
                        Width = modelData.Width,
                        Depth = modelData.Depth,

                        IconPath = iconData.Path,
                        Category = iconData.Category

                    }).OrderByDescending(x => x.Id == 1).ThenBy(x => x.Category).ThenBy(x => x.Name).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetModelData(Search.Model searchParameters)
    {
        modelDataList = new List<ModelBaseData>();

        foreach(ModelBaseData model in Fixtures.modelList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(model.Id)) continue;

            var modelData = new ModelBaseData();

            modelData.Id = model.Id;

            modelData.Name = model.Name;
            modelData.Path = model.Path;
            modelData.IconId = model.IconId;

            modelData.Height = model.Height;
            modelData.Width = model.Width;
            modelData.Depth = model.Depth;

            modelDataList.Add(modelData);
        }
    }

    private static void GetIconData()
    {
        var iconSearchParameters = new Search.Icon();
        iconSearchParameters.id = modelDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(iconSearchParameters);
    }
}
