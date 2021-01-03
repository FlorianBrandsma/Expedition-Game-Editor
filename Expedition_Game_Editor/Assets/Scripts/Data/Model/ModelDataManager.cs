using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ModelDataManager
{
    private static List<ModelBaseData> modelDataList;

    private static List<IconBaseData> iconDataList;
    
    public static List<IElementData> GetData(Search.Model searchParameters)
    {
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
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(model.Id))         continue;
            if (searchParameters.excludeId.Count    > 0 && searchParameters.excludeId.Contains(model.Id))   continue;

            modelDataList.Add(model);
        }
    }

    private static void GetIconData()
    {
        var searchParameters = new Search.Icon();
        searchParameters.id = modelDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(searchParameters);
    }
}
