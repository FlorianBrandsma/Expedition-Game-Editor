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
                        table = iconData.table,

                        Index = iconData.index,
                        Name = iconData.name

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetIconData(Search.Icon searchParameters)
    {
        iconDataList = new List<IconData>();

        for (int i = 0; i < searchParameters.temp_id_count; i++)
        {
            var iconData = new IconData();

            iconData.id = (i + 1);
            iconData.table = "Icon";
            iconData.index = i;

            iconData.name = "Icon " + (i + 1);

            iconDataList.Add(iconData);
        }
    }

    internal class IconData : GeneralData
    {
        public int index;
        public string name;
    }
}
