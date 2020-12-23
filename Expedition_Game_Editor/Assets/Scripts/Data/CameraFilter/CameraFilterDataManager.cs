using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class CameraFilterDataManager
{
    private static List<CameraFilterBaseData> cameraFilterDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var list = new List<CameraFilterElementData>();

        var searchParameters = searchProperties.searchParameters.Cast<Search.CameraFilter>().First();

        GetCameraFilterData(searchParameters);

        if (cameraFilterDataList.Count > 0)
        {
            list = (from cameraFilterData in cameraFilterDataList
                    select new CameraFilterElementData()
                    {
                        Id = cameraFilterData.Id,

                        Path = cameraFilterData.Path,
                        IconPath = cameraFilterData.IconPath,

                        Name = cameraFilterData.Name

                    }).OrderBy(x => x.Id).ToList();
        }

        if (searchParameters.includeRemoveElement)
            AddRemoveElementData(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void AddRemoveElementData(List<CameraFilterElementData> list)
    {
        list.Insert(0, new CameraFilterElementData());
    }

    private static void GetCameraFilterData(Search.CameraFilter searchParameters)
    {
        cameraFilterDataList = new List<CameraFilterBaseData>();

        foreach (CameraFilterBaseData CameraFilter in Fixtures.cameraFilterList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(CameraFilter.Id))          continue;
            if (searchParameters.excludeId.Count    > 0 && searchParameters.excludeId.Contains(CameraFilter.Id))    continue;
            
            cameraFilterDataList.Add(CameraFilter);
        }
    }
}
