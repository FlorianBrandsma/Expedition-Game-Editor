using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class CameraFilterDataManager
{
    private static List<CameraFilterBaseData> cameraFilterDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.CameraFilter>().First();

        GetCameraFilterData(searchParameters);

        if (searchParameters.includeRemoveElement)
            cameraFilterDataList.Add(new CameraFilterBaseData());

        if (cameraFilterDataList.Count == 0) return new List<IElementData>();
        
        var list = (from cameraFilterData in cameraFilterDataList
                    select new CameraFilterElementData()
                    {
                        Id = cameraFilterData.Id,

                        Path = cameraFilterData.Path,
                        IconPath = cameraFilterData.IconPath,

                        Name = cameraFilterData.Name

                    }).OrderBy(x => x.Id).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
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
