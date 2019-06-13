using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class IconDataManager
{
    private IconController iconController;
    private List<IconData> iconDataList;

    public void InitializeManager(IconController iconController)
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
                        id = iconData.id,
                        table = "Icon",
                        index = iconData.index,

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
            if (searchParameters.category.Count > 0 && !searchParameters.category.Contains(icon.category)) continue;

            var iconData = new IconData();

            iconData.id = icon.id;
            iconData.index = icon.index;

            iconData.path = icon.path;

            iconDataList.Add(iconData);
        }
    }

    internal class IconData : GeneralData
    {
        public string path;
    }
}
