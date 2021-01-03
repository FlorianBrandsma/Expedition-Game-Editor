using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class IconDataManager
{
    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(Search.Icon searchParameters)
    {
        GetIconData(searchParameters);

        if (iconDataList.Count == 0) return new List<IElementData>();
        
        var list = (from iconData in iconDataList
                    select new IconElementData()
                    {
                        Id = iconData.Id,

                        Path = iconData.Path,
                        BaseIconPath = ""

                    }).OrderBy(x => x.Id).ToList();
        
        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetIconData(Search.Icon searchParameters)
    {
        iconDataList = new List<IconBaseData>();

        foreach (IconBaseData icon in Fixtures.iconList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(icon.Id)) continue;
            if (searchParameters.category.Count > 0 && !searchParameters.category.Contains(icon.Category)) continue;

            iconDataList.Add(icon);
        }
    }
}
