using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ChapterDataManager
{
    private static List<ChapterBaseData> chapterDataList;

    public static List<IElementData> GetData(Search.Chapter searchParameters)
    {
        GetChapterData(searchParameters);

        if (searchParameters.includeAddElement)
            chapterDataList.Add(DefaultData());

        if (chapterDataList.Count == 0) return new List<IElementData>();
        
        var list = (from chapterData in chapterDataList
                    select new ChapterElementData()
                    {
                        Id = chapterData.Id,
                        Index = chapterData.Index,

                        Name = chapterData.Name,

                        TimeSpeed = chapterData.TimeSpeed,

                        PublicNotes = chapterData.PublicNotes,
                        PrivateNotes = chapterData.PrivateNotes

                    }).OrderBy(x => x.Id > 0).ThenBy(x => x.Index).ToList();

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());
        
        return list.Cast<IElementData>().ToList();
    }

    public static ChapterElementData DefaultData()
    {
        return new ChapterElementData()
        {
            Id = -1
        };
    }

    public static void SetDefaultAddValues(List<ChapterElementData> list)
    {
        var addElementData = list.Where(x => x.Id == -1).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;

        addElementData.Index = list.Count - 1;
    }

    private static void GetChapterData(Search.Chapter searchParameters)
    {
        chapterDataList = new List<ChapterBaseData>();

        foreach(ChapterBaseData chapter in Fixtures.chapterList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(chapter.Id)) continue;

            chapterDataList.Add(chapter);
        }
    }

    public static void AddData(ChapterElementData elementData, DataRequest dataRequest)
    {
        if(dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.chapterList.Count > 0 ? (Fixtures.chapterList[Fixtures.chapterList.Count - 1].Id + 1) : 1;
            Fixtures.chapterList.Add(((ChapterData)elementData).Clone());

            elementData.SetOriginalValues();

            AddDependencies(elementData, dataRequest);

            PlayerSaveDataManager.UpdateReferences(dataRequest);

        } else {

            CheckDuplicateName(elementData, dataRequest);
        }
    }

    private static void AddDependencies(ChapterElementData elementData, DataRequest dataRequest)
    {
        AddChapterSaveData(elementData, dataRequest);

        if (!dataRequest.includeDependencies) return;

        AddPhaseData(elementData, dataRequest);
    }

    private static void AddChapterSaveData(ChapterElementData elementData, DataRequest dataRequest)
    {
        //Save
        var saveSearchParameters = new Search.Save();

        var saveDataList = DataManager.GetSaveData(saveSearchParameters);

        if (saveDataList.Count == 0) return;

        saveDataList.ForEach(saveData =>
        {
            var chapterSaveElementData = ChapterSaveDataManager.DefaultData(saveData.Id, elementData.Id);
            chapterSaveElementData.Add(dataRequest);
        });
    }

    private static void AddPhaseData(ChapterElementData elementData, DataRequest dataRequest)
    {
        var phaseElementData = PhaseDataManager.DefaultData(elementData.Id);

        phaseElementData.Name = "Default name";

        phaseElementData.Add(dataRequest);
    }

    public static void UpdateData(ChapterElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.chapterList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedName)
            {
                data.Name = elementData.Name;
            }

            if (elementData.ChangedTimeSpeed)
            {
                data.TimeSpeed = elementData.TimeSpeed;
            }

            if (elementData.ChangedPublicNotes)
            {
                data.PublicNotes = elementData.PublicNotes;
            }

            if (elementData.ChangedPrivateNotes)
            {
                data.PrivateNotes = elementData.PrivateNotes;
            }

            elementData.SetOriginalValues();

        } else {

            if (elementData.ChangedName)
            {
                //Let's imagine the chapter name is unique...
                CheckDuplicateName(elementData, dataRequest);
            }
        }   
    }

    private static void CheckDuplicateName(ChapterElementData elementData, DataRequest dataRequest)
    {
        var chapterList = Fixtures.chapterList.Where(x => x.Id != elementData.Id).ToList();

        if (chapterList.Any(x => x.Name == elementData.Name))
            dataRequest.errorList.Add("This name totally exists already");
    }

    static public void UpdateIndex(ChapterElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.ChangedIndex) return;

        var data = Fixtures.chapterList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            data.Index = elementData.Index;

            elementData.OriginalData.Index = elementData.Index;

            PlayerSaveDataManager.UpdateReferences(dataRequest);

        } else { }
    }
    
    public static void RemoveData(ChapterElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            RemoveDependencies(elementData, dataRequest);

            Fixtures.chapterList.RemoveAll(x => x.Id == elementData.Id);

            elementData.RemoveIndex(dataRequest);

            PlayerSaveDataManager.UpdateReferences(dataRequest);

        } else {

            RemoveDependencies(elementData, dataRequest);
        }
    }

    private static void RemoveDependencies(ChapterElementData elementData, DataRequest dataRequest)
    {
        RemoveWorldInteractableData(elementData, dataRequest);
        RemoveChapterInteractableData(elementData, dataRequest);
        RemoveChapterRegionData(elementData, dataRequest);

        RemovePhaseData(elementData, dataRequest);
        RemoveChapterSaveData(elementData, dataRequest);
    }
    
    private static void RemoveWorldInteractableData(ChapterElementData elementData, DataRequest dataRequest)
    {
        var worldInteractableSearchParameters = new Search.WorldInteractable()
        {
            chapterId = new List<int>() { elementData.Id }
        };

        var worldInteractableDataList = DataManager.GetWorldInteractableData(worldInteractableSearchParameters);

        worldInteractableDataList.ForEach(worldInteractableData =>
        {
            var worldInteractableElementData = new WorldInteractableElementData()
            {
                Id = worldInteractableData.Id,

                ChapterId = elementData.Id,

                Type = worldInteractableData.Type
            };

            worldInteractableElementData.Remove(dataRequest);
        });
    }

    private static void RemoveChapterInteractableData(ChapterElementData elementData, DataRequest dataRequest)
    {
        var chapterInteractableSearchParameters = new Search.ChapterInteractable()
        {
            chapterId = new List<int>() { elementData.Id }
        };

        var chapterInteractableDataList = DataManager.GetChapterInteractableData(chapterInteractableSearchParameters);

        chapterInteractableDataList.ForEach(chapterInteractableData =>
        {
            var chapterInteractableElementData = new ChapterInteractableElementData()
            {
                Id = chapterInteractableData.Id,
                ChapterId = elementData.Id
            };

            chapterInteractableElementData.Remove(dataRequest);
        });
    }

    private static void RemoveChapterRegionData(ChapterElementData elementData, DataRequest dataRequest)
    {
        var chapterRegionSearchParameters = new Search.ChapterRegion()
        {
            chapterId = new List<int>() { elementData.Id }
        };

        var chapterRegionDataList = DataManager.GetChapterRegionData(chapterRegionSearchParameters);

        chapterRegionDataList.ForEach(chapterRegionData =>
        {
            var chapterRegionElementData = new ChapterRegionElementData()
            {
                Id = chapterRegionData.Id,
                ChapterId = elementData.Id
            };

            chapterRegionElementData.Remove(dataRequest);
        });
    }

    private static void RemovePhaseData(ChapterElementData elementData, DataRequest dataRequest)
    {
        var phaseSearchParameters = new Search.Phase()
        {
            chapterId = new List<int>() { elementData.Id }
        };

        var phaseDataList = DataManager.GetPhaseData(phaseSearchParameters);

        phaseDataList.ForEach(worldInteractableData =>
        {
            var phaseElementData = new PhaseElementData()
            {
                Id = worldInteractableData.Id
            };

            phaseElementData.Remove(dataRequest);
        });
    }

    private static void RemoveChapterSaveData(ChapterElementData elementData, DataRequest dataRequest)
    {
        var chapterSaveSearchParameters = new Search.ChapterSave()
        {
            chapterId = new List<int>() { elementData.Id }
        };

        var chapterSaveDataList = DataManager.GetChapterSaveData(chapterSaveSearchParameters);

        chapterSaveDataList.ForEach(chapterSaveData =>
        {
            var chapterSaveElementData = new ChapterSaveElementData()
            {
                Id = chapterSaveData.Id
            };

            chapterSaveElementData.Remove(dataRequest);
        });
    }

    public static void RemoveIndex(ChapterElementData elementData, DataRequest dataRequest)
    {
        var chapterSearchParameters = new Search.Chapter();

        var chapterDataList = DataManager.GetChapterData(chapterSearchParameters);

        chapterDataList.Where(x => x.Index > elementData.Index).ToList().ForEach(chapterData =>
        {
            var chapterElementData = new ChapterElementData()
            {
                Id = chapterData.Id,
                Index = chapterData.Index
            };

            chapterElementData.SetOriginalValues();

            chapterElementData.Index--;

            chapterElementData.UpdateIndex(dataRequest);
        });
    }
}
