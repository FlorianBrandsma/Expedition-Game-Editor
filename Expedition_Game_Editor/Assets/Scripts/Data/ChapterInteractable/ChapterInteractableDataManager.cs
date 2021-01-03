using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ChapterInteractableDataManager
{
    private static List<ChapterInteractableBaseData> chapterInteractableDataList;

    private static List<InteractableBaseData> interactableDataList;
    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(Search.ChapterInteractable searchParameters)
    {
        GetChapterInteractableData(searchParameters);

        if (searchParameters.includeAddElement)
            chapterInteractableDataList.Add(DefaultData());

        if (chapterInteractableDataList.Count == 0) return new List<IElementData>();
        
        GetInteractableData();
        GetModelData();
        GetIconData();

        var list = (from chapterInteractableData in chapterInteractableDataList

                    join leftJoin in (from interactableData in interactableDataList
                                      join modelData        in modelDataList    on interactableData.ModelId equals modelData.Id
                                      join iconData         in iconDataList     on modelData.IconId         equals iconData.Id
                                      select new { interactableData, iconData }) on chapterInteractableData.InteractableId equals leftJoin.interactableData.Id into interactableData

                    select new ChapterInteractableElementData()
                    {
                        Id = chapterInteractableData.Id,

                        ChapterId = chapterInteractableData.ChapterId,
                        InteractableId = chapterInteractableData.InteractableId,

                        InteractableName = interactableData.FirstOrDefault() != null ? interactableData.FirstOrDefault().interactableData.Name : "",
                        ModelIconPath = interactableData.FirstOrDefault() != null ? interactableData.FirstOrDefault().iconData.Path : ""

                    }).OrderBy(x => x.Id).ToList();
        
        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static ChapterInteractableElementData DefaultData()
    {
        return new ChapterInteractableElementData();
    }

    public static void SetDefaultAddValues(List<ChapterInteractableElementData> list)
    {
        var addElementData = list.Where(x => x.Id == 0).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;
    }

    private static void GetChapterInteractableData(Search.ChapterInteractable searchParameters)
    {
        chapterInteractableDataList = new List<ChapterInteractableBaseData>();

        foreach (ChapterInteractableBaseData chapterInteractable in Fixtures.chapterInteractableList)
        {
            if (searchParameters.id.Count               > 0 && !searchParameters.id.Contains(chapterInteractable.Id))                           continue;
            if (searchParameters.chapterId.Count        > 0 && !searchParameters.chapterId.Contains(chapterInteractable.ChapterId))             continue;
            if (searchParameters.interactableId.Count   > 0 && !searchParameters.interactableId.Contains(chapterInteractable.InteractableId))   continue;

            chapterInteractableDataList.Add(chapterInteractable);
        }
    }

    private static void GetInteractableData()
    {
        var searchParameters = new Search.Interactable();
        searchParameters.id = chapterInteractableDataList.Select(x => x.InteractableId).Distinct().ToList();

        interactableDataList = DataManager.GetInteractableData(searchParameters);
    }

    private static void GetModelData()
    {
        var searchParameters = new Search.Model();
        searchParameters.id = interactableDataList.Select(x => x.ModelId).Distinct().ToList();

        modelDataList = DataManager.GetModelData(searchParameters);
    }

    private static void GetIconData()
    {
        var searchParameters = new Search.Icon();
        searchParameters.id = modelDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(searchParameters);
    }

    public static void AddData(ChapterInteractableElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.chapterInteractableList.Count > 0 ? (Fixtures.chapterInteractableList[Fixtures.chapterInteractableList.Count - 1].Id + 1) : 1;
            Fixtures.chapterInteractableList.Add(((ChapterInteractableData)elementData).Clone());

            elementData.SetOriginalValues();

            AddDependencies(elementData, dataRequest);

        } else {

            AddDependencies(elementData, dataRequest);
        }
    }

    private static void AddDependencies(ChapterInteractableElementData elementData, DataRequest dataRequest)
    {
        AddWorldInteractable(elementData, dataRequest);
    }

    private static void AddWorldInteractable(ChapterInteractableElementData elementData, DataRequest dataRequest)
    {
        //Phase
        var phaseSearchParameters = new Search.Phase()
        {
            chapterId = new List<int>() { elementData.ChapterId }
        };

        var phaseDataList = DataManager.GetPhaseData(phaseSearchParameters);

        if (phaseDataList.Count == 0) return;

        phaseDataList.ForEach(phaseData =>
        {
            var worldInteractableElementData = new WorldInteractableElementData()
            {
                PhaseId = phaseData.Id,
                ChapterInteractableId = elementData.Id,
                InteractableId = elementData.InteractableId,
                Type = (int)Enums.InteractableType.Agent
            };

            WorldInteractableDataManager.AddData(worldInteractableElementData, dataRequest, true);
        });
    }

    public static void UpdateData(ChapterInteractableElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.chapterInteractableList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedInteractableId)
            {
                data.InteractableId = elementData.InteractableId;
            }

            if (elementData.ChangedInteractableId)
                UpdateWorldInteractableData(elementData, dataRequest);

            elementData.SetOriginalValues();
            
        } else {

            if (elementData.ChangedInteractableId)
                UpdateWorldInteractableData(elementData, dataRequest);
        }
    }
    
    private static void UpdateWorldInteractableData(ChapterInteractableElementData elementData, DataRequest dataRequest)
    {
        var worldInteractableSearchParameters = new Search.WorldInteractable()
        {
            chapterInteractableId = new List<int>() { elementData.Id }
        };

        var worldInteractableDataList = DataManager.GetWorldInteractableData(worldInteractableSearchParameters);

        if (worldInteractableDataList.Count == 0) return;

        worldInteractableDataList.ForEach(worldInteractableData =>
        {
            var worldInteractableElementData = new WorldInteractableElementData()
            {
                Id = worldInteractableData.Id
            };

            worldInteractableElementData.SetOriginalValues();

            worldInteractableElementData.InteractableId = elementData.InteractableId;

            worldInteractableElementData.Update(dataRequest);
        });
    }

    public static void RemoveData(ChapterInteractableElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            RemoveDependencies(elementData, dataRequest);

            Fixtures.chapterInteractableList.RemoveAll(x => x.Id == elementData.Id);
            
        } else {

            RemoveDependencies(elementData, dataRequest);
        }
    }

    private static void RemoveDependencies(ChapterInteractableElementData elementData, DataRequest dataRequest)
    {
        RemoveWorldInteractableData(elementData, dataRequest);
    }

    private static void RemoveWorldInteractableData(ChapterInteractableElementData elementData, DataRequest dataRequest)
    {
        var worldInteractableSearchParameters = new Search.WorldInteractable()
        {
            chapterInteractableId = new List<int>() { elementData.Id }
        };

        var worldInteractableDataList = DataManager.GetWorldInteractableData(worldInteractableSearchParameters);
        
        if (worldInteractableDataList.Count == 0) return;
        
        worldInteractableDataList.ForEach(worldInteractableData =>
        {
            var worldInteractableElementData = new WorldInteractableElementData()
            {
                Id = worldInteractableData.Id
            };

            worldInteractableElementData.Remove(dataRequest);
        });
    }
}
