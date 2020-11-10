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
            
            outcomeDataList.Add(outcome);
        }
    }

    private static void GetInteractionData()
    {
        var searchParameters = new Search.Interaction();
        searchParameters.id = outcomeDataList.Select(x => x.InteractionId).Distinct().ToList();

        interactionDataList = DataManager.GetInteractionData(searchParameters);
    }

    private static void GetTaskData()
    {
        var searchParameters = new Search.Task();
        searchParameters.id = interactionDataList.Select(x => x.TaskId).Distinct().ToList();

        taskDataList = DataManager.GetTaskData(searchParameters);
    }

    private static void GetWorldInteractableData()
    {
        var searchParameters = new Search.WorldInteractable();
        searchParameters.id = taskDataList.Select(x => x.WorldInteractableId).Distinct().ToList();

        worldInteractableDataList = DataManager.GetWorldInteractableData(searchParameters);
    }

    private static void GetInteractableData()
    {
        var searchParameters = new Search.Interactable();
        searchParameters.id = worldInteractableDataList.Select(x => x.InteractableId).Distinct().ToList();

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
