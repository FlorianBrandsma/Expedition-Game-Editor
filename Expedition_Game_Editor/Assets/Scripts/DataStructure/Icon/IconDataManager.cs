using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class IconDataManager
{
    private IconController iconController;
    private List<IconData> iconDataList;

    public IconDataManager(IconController iconController)
    {
        this.iconController = iconController;
    }

    public List<IDataElement> GetIconDataElements(IEnumerable searchParameters)
    {
        var iconSearchData = searchParameters.Cast<Search.Icon>().FirstOrDefault();

        GetIconData(iconSearchData);

        var list = (from iconData in iconDataList
                    select new IconDataElement()
                    {
                        DataType = Enums.DataType.Icon,

                        Id = iconData.Id,
                        Index = iconData.Index,

                        Path = iconData.path,
                        baseIconPath = ""

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetIconData(Search.Icon searchParameters)
    {
        iconDataList = new List<IconData>();

        foreach (Fixtures.Icon icon in Fixtures.iconList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(icon.Id)) continue;
            if (searchParameters.category.Count > 0 && !searchParameters.category.Contains(icon.category)) continue;

            var iconData = new IconData();

            iconData.Id = icon.Id;
            iconData.Index = icon.Index;

            iconData.path = icon.path;

            iconDataList.Add(iconData);
        }
    }

    internal class IconData : GeneralData
    {
        public string path;
    }
}
