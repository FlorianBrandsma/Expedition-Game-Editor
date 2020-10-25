using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SceneDataManager : MonoBehaviour
{
    private static List<SceneBaseData> sceneDataList;

    private static List<RegionBaseData> regionDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Scene>().First();

        GetSceneData(searchParameters);

        if (sceneDataList.Count == 0) return new List<IElementData>();

        GetRegionData();

        var list = (from sceneData  in sceneDataList
                    join regionData in regionDataList on sceneData.RegionId equals regionData.Id
                    select new SceneElementData()
                    {
                        Id = sceneData.Id,

                        OutcomeId = sceneData.OutcomeId,
                        RegionId = sceneData.RegionId,

                        Index = sceneData.Index,

                        Name = sceneData.Name,

                        FreezeTime = sceneData.FreezeTime,
                        FreezeMovement = sceneData.FreezeMovement,
                        AutoContinue = sceneData.AutoContinue,

                        SceneDuration = sceneData.SceneDuration,
                        ShotDuration = sceneData.ShotDuration,

                        PublicNotes = sceneData.PublicNotes,
                        PrivateNotes = sceneData.PrivateNotes,

                        RegionName = regionData.Name

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetSceneData(Search.Scene searchParameters)
    {
        sceneDataList = new List<SceneBaseData>();

        foreach (SceneBaseData scene in Fixtures.sceneList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(scene.Id)) continue;
            if (searchParameters.outcomeId.Count    > 0 && !searchParameters.outcomeId.Contains(scene.OutcomeId)) continue;

            var outcomeData = new SceneBaseData();

            outcomeData.Id = scene.Id;

            outcomeData.OutcomeId = scene.OutcomeId;
            outcomeData.RegionId = scene.RegionId;

            outcomeData.Index = scene.Index;

            outcomeData.Name = scene.Name;

            outcomeData.FreezeTime = scene.FreezeTime;
            outcomeData.FreezeMovement = scene.FreezeMovement;
            outcomeData.AutoContinue = scene.AutoContinue;

            outcomeData.SceneDuration = scene.SceneDuration;
            outcomeData.ShotDuration = scene.ShotDuration;

            outcomeData.PublicNotes = scene.PublicNotes;
            outcomeData.PrivateNotes = scene.PrivateNotes;

            sceneDataList.Add(outcomeData);
        }
    }

    private static void GetRegionData()
    {
        var regionSearchParameters = new Search.Region();
        regionSearchParameters.id = sceneDataList.Select(x => x.RegionId).Distinct().ToList();

        regionDataList = DataManager.GetRegionData(regionSearchParameters);
    }

    public static void UpdateData(SceneElementData elementData)
    {
        var data = Fixtures.sceneList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (elementData.ChangedName)
            data.Name = elementData.Name;

        if (elementData.ChangedFreezeTime)
            data.FreezeTime = elementData.FreezeTime;

        if (elementData.ChangedFreezeMovement)
            data.FreezeMovement = elementData.FreezeMovement;

        if (elementData.ChangedAutoContinue)
            data.AutoContinue = elementData.AutoContinue;

        if (elementData.ChangedSceneDuration)
            data.SceneDuration = elementData.SceneDuration;

        if (elementData.ChangedShotDuration)
            data.ShotDuration = elementData.ShotDuration;

        if (elementData.ChangedPublicNotes)
            data.PublicNotes = elementData.PublicNotes;

        if (elementData.ChangedPrivateNotes)
            data.PrivateNotes = elementData.PrivateNotes;
    }

    static public void UpdateIndex(SceneElementData elementData)
    {
        var data = Fixtures.sceneList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        data.Index = elementData.Index;
    }
}
