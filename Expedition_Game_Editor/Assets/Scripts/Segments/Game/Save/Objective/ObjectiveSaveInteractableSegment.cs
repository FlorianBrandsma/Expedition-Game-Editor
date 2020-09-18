using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class ObjectiveSaveInteractableSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public void InitializeDependencies() { }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.WorldInteractable);

        var searchParameters = searchProperties.searchParameters.Cast<Search.WorldInteractable>().First();
        searchParameters.requestType = Search.WorldInteractable.RequestType.GetQuestAndObjectiveWorldInteractables;

        var questSaveData = (QuestSaveElementData)SegmentController.Path.FindLastRoute(Enums.DataType.QuestSave).ElementData;
        searchParameters.questId = new List<int>() { questSaveData.QuestId };

        var objectiveSaveData = (ObjectiveSaveElementData)SegmentController.Path.FindLastRoute(Enums.DataType.ObjectiveSave).ElementData;
        searchParameters.objectiveId = new List<int>() { objectiveSaveData.ObjectiveId };

        SegmentController.DataController.GetData(searchProperties);
    }

    public void InitializeSegment()
    {
        InitializeData();
    }
    
    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(IElementData elementData) { }

    public void CloseSegment() { }
}
