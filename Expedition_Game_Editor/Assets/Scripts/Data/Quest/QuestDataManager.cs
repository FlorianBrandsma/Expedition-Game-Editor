using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class QuestDataManager
{
    private static List<QuestBaseData> questDataList;

    public static List<IElementData> GetData(Search.Quest searchParameters)
    {
        GetQuestData(searchParameters);

        if (searchParameters.includeAddElement)
            questDataList.Add(DefaultData(searchParameters.phaseId.First()));

        if (questDataList.Count == 0) return new List<IElementData>();

        var list = (from questData in questDataList
                    select new QuestElementData()
                    {
                        Id = questData.Id,
                        Index = questData.Index,

                        PhaseId = questData.PhaseId,

                        Name = questData.Name,

                        PublicNotes = questData.PublicNotes,
                        PrivateNotes = questData.PrivateNotes

                    }).OrderBy(x => x.Id > 0).ThenBy(x => x.Index).ToList();

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static QuestElementData DefaultData(int phaseId)
    {
        return new QuestElementData()
        {
            PhaseId = phaseId
        };
    }

    public static void SetDefaultAddValues(List<QuestElementData> list)
    {
        var addElementData = list.Where(x => x.Id == 0).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;

        addElementData.Index = list.Count - 1;
    }

    private static void GetQuestData(Search.Quest searchParameters)
    {
        questDataList = new List<QuestBaseData>();

        foreach(QuestBaseData quest in Fixtures.questList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(quest.Id)) continue;
            if (searchParameters.phaseId.Count  > 0 && !searchParameters.phaseId.Contains(quest.PhaseId)) continue;

            questDataList.Add(quest);
        }
    }

    public static void AddData(QuestElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.questList.Count > 0 ? (Fixtures.questList[Fixtures.questList.Count - 1].Id + 1) : 1;
            Fixtures.questList.Add(((QuestData)elementData).Clone());
            
            elementData.SetOriginalValues();

            AddDependencies(elementData, dataRequest);

        } else { }
    }

    private static void AddDependencies(QuestElementData elementData, DataRequest dataRequest)
    {
        AddQuestSaveData(elementData, dataRequest);

        if (!dataRequest.includeDependencies) return;
    }

    private static void AddQuestSaveData(QuestElementData elementData, DataRequest dataRequest)
    {
        //Save
        var saveSearchParameters = new Search.Save();

        var saveDataList = DataManager.GetSaveData(saveSearchParameters);

        if (saveDataList.Count == 0) return;

        saveDataList.ForEach(saveData =>
        {
            var questSaveElementData = QuestSaveDataManager.DefaultData(saveData.Id, elementData.Id);
            questSaveElementData.Add(dataRequest);
        });
    }

    public static void UpdateData(QuestElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.questList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedName)
            {
                data.Name = elementData.Name;
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

        } else { }
    }

    static public void UpdateIndex(QuestElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.ChangedIndex) return;

        var data = Fixtures.questList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            data.Index = elementData.Index;

            elementData.OriginalData.Index = elementData.Index;

        } else { }
    }

    static public void RemoveData(QuestElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            RemoveReferences(elementData, dataRequest);
            RemoveDependencies(elementData, dataRequest);

            Fixtures.questList.RemoveAll(x => x.Id == elementData.Id);

            elementData.RemoveIndex(dataRequest);

        } else {

            RemoveDependencies(elementData, dataRequest);
        }
    }

    private static void RemoveReferences(QuestElementData elementData, DataRequest dataRequest)
    {
        RemoveWorldInteractableReference(elementData, dataRequest);
    }

    private static void RemoveWorldInteractableReference(QuestElementData elementData, DataRequest dataRequest)
    {
        var worldInteractableParameters = new Search.WorldInteractable()
        {
            questId = new List<int>() { elementData.Id }
        };

        var worldInteractableDataList = DataManager.GetWorldInteractableData(worldInteractableParameters);

        worldInteractableDataList.ForEach(worldInteractableData =>
        {
            var worldInteractableElementData = new WorldInteractableElementData()
            {
                Id = worldInteractableData.Id,
                QuestId = elementData.Id
            };

            worldInteractableElementData.SetOriginalValues();

            worldInteractableElementData.QuestId = 0;

            worldInteractableElementData.Update(dataRequest);
        });
    }

    private static void RemoveDependencies(QuestElementData elementData, DataRequest dataRequest)
    {
        RemoveObjectiveData(elementData, dataRequest);
        RemoveQuestSaveData(elementData, dataRequest);
    }

    private static void RemoveObjectiveData(QuestElementData elementData, DataRequest dataRequest)
    {
        var objectiveSearchParameters = new Search.Objective()
        {
            questId = new List<int>() { elementData.Id }
        };

        var objectiveDataList = DataManager.GetObjectiveData(objectiveSearchParameters);

        objectiveDataList.ForEach(objectiveData =>
        {
            var objectiveElementData = new ObjectiveElementData()
            {
                Id = objectiveData.Id
            };

            objectiveElementData.Remove(dataRequest);
        });
    }

    private static void RemoveQuestSaveData(QuestElementData elementData, DataRequest dataRequest)
    {
        var questSaveSearchParameters = new Search.QuestSave()
        {
            questId = new List<int>() { elementData.Id }
        };

        var questSaveDataList = DataManager.GetQuestSaveData(questSaveSearchParameters);

        questSaveDataList.ForEach(questSaveData =>
        {
            var questSaveElementData = new QuestSaveElementData()
            {
                Id = questSaveData.Id
            };

            questSaveElementData.Remove(dataRequest);
        });
    }

    public static void RemoveIndex(QuestElementData elementData, DataRequest dataRequest)
    {
        var questSearchParameters = new Search.Quest()
        {
            phaseId = new List<int>() { elementData.PhaseId }
        };

        var questDataList = DataManager.GetQuestData(questSearchParameters);

        questDataList.Where(x => x.Index > elementData.Index).ToList().ForEach(questData =>
        {
            var questElementData = new QuestElementData()
            {
                Id = questData.Id,
                Index = questData.Index
            };

            questElementData.SetOriginalValues();

            questElementData.Index--;

            questElementData.UpdateIndex(dataRequest);
        });
    }
}
