using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class OutcomeDataManager
{
    private static List<OutcomeBaseData> outcomeDataList;

    private static List<InteractionBaseData> interactionDataList;
    private static List<TaskBaseData> taskDataList;
    private static List<WorldInteractableBaseData> worldInteractableDataList;
    private static List<InteractableBaseData> interactableDataList;
    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Outcome>().First();

        GetOutcomeData(searchParameters);

        if (outcomeDataList.Count == 0) return new List<IElementData>();

        GetInteractionData();
        GetTaskData();
        GetWorldInteractableData();
        GetInteractableData();
        GetModelData();
        GetIconData();

        var list = (from outcomeData in outcomeDataList
                    join interactionData        in interactionDataList          on outcomeData.InteractionId            equals interactionData.Id
                    join taskData               in taskDataList                 on interactionData.TaskId               equals taskData.Id
                    join worldInteractableData  in worldInteractableDataList    on taskData.WorldInteractableId         equals worldInteractableData.Id
                    join interactableData       in interactableDataList         on worldInteractableData.InteractableId equals interactableData.Id
                    join modelData              in modelDataList                on interactableData.ModelId             equals modelData.Id
                    join iconData               in iconDataList                 on modelData.IconId                     equals iconData.Id
                    select new OutcomeElementData()
                    {
                        Id = outcomeData.Id,

                        InteractionId = outcomeData.InteractionId,

                        Type = outcomeData.Type,

                        CompleteTask = outcomeData.CompleteTask,
                        ResetObjective = outcomeData.ResetObjective,

                        PublicNotes = outcomeData.PublicNotes,
                        PrivateNotes = outcomeData.PrivateNotes,

                        ModelIconPath = iconData.Path,

                        DefaultInteraction = interactionData.Default,

                        InteractionStartTime = interactionData.StartTime,
                        InteractionEndTime = interactionData.EndTime,

                        TaskName = taskData.Name
                        
                    }).OrderBy(x => x.Type).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetOutcomeData(Search.Outcome searchParameters)
    {
        outcomeDataList = new List<OutcomeBaseData>();

        foreach (OutcomeBaseData outcome in Fixtures.outcomeList)
        {
            if (searchParameters.id.Count               > 0 && !searchParameters.id.Contains(outcome.Id)) continue;
            if (searchParameters.interactionId.Count    > 0 && !searchParameters.interactionId.Contains(outcome.InteractionId)) continue;
            
            var outcomeData = new OutcomeBaseData();

            outcomeData.Id = outcome.Id;
            
            outcomeData.InteractionId = outcome.InteractionId;

            outcomeData.Type = outcome.Type;

            outcomeData.CompleteTask = outcome.CompleteTask;
            outcomeData.ResetObjective = outcome.ResetObjective;

            outcomeData.PublicNotes = outcome.PublicNotes;
            outcomeData.PrivateNotes = outcome.PrivateNotes;

            outcomeDataList.Add(outcomeData);
        }
    }

    private static void GetInteractionData()
    {
        var interactionSearchParameters = new Search.Interaction();

        interactionSearchParameters.id = outcomeDataList.Select(x => x.InteractionId).Distinct().ToList();

        interactionDataList = DataManager.GetInteractionData(interactionSearchParameters);
    }

    private static void GetTaskData()
    {
        var taskSearchParameters = new Search.Task();

        taskSearchParameters.id = interactionDataList.Select(x => x.TaskId).Distinct().ToList();

        taskDataList = DataManager.GetTaskData(taskSearchParameters);
    }

    private static void GetWorldInteractableData()
    {
        var worldInteractableSearchParameters = new Search.WorldInteractable();

        worldInteractableSearchParameters.id = taskDataList.Select(x => x.WorldInteractableId).Distinct().ToList();

        worldInteractableDataList = DataManager.GetWorldInteractableData(worldInteractableSearchParameters);
    }

    private static void GetInteractableData()
    {
        var interactableSearchParameters = new Search.Interactable();

        interactableSearchParameters.id = worldInteractableDataList.Select(x => x.InteractableId).Distinct().ToList();

        interactableDataList = DataManager.GetInteractableData(interactableSearchParameters);
    }

    private static void GetModelData()
    {
        var modelSearchParameters = new Search.Model();

        modelSearchParameters.id = interactableDataList.Select(x => x.ModelId).Distinct().ToList();

        modelDataList = DataManager.GetModelData(modelSearchParameters);
    }

    private static void GetIconData()
    {
        var iconSearchParameters = new Search.Icon();
        iconSearchParameters.id = modelDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(iconSearchParameters);
    }

    public static void UpdateData(OutcomeElementData elementData)
    {
        var data = Fixtures.outcomeList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (elementData.ChangedCompleteTask)
            data.CompleteTask = elementData.CompleteTask;

        if (elementData.ChangedResetObjective)
            data.ResetObjective = elementData.ResetObjective;

        if (elementData.ChangedPublicNotes)
            data.PublicNotes = elementData.PublicNotes;

        if (elementData.ChangedPrivateNotes)
            data.PrivateNotes = elementData.PrivateNotes;
    }
}
