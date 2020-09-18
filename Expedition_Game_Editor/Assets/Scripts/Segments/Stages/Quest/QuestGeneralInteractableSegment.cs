using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuestGeneralInteractableSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private QuestEditor QuestEditor             { get { return (QuestEditor)DataEditor; } }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData()
    {
        if (DataEditor.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.WorldInteractable);

        var searchParameters = searchProperties.searchParameters.Cast<Search.WorldInteractable>().First();
        searchParameters.phaseId = new List<int>() { QuestEditor.PhaseId };

        SegmentController.DataController.GetData(searchProperties);

        SetStatus(SegmentController.DataController.Data.dataList);

        var questInteractableList = SegmentController.DataController.Data.dataList.Cast<WorldInteractableElementData>().ToList();
        questInteractableList.ForEach(x => QuestEditor.worldInteractableDataList.Add(x));
    }

    private void SetStatus(List<IElementData> dataList)
    {
        dataList.ForEach(x =>
        {
            var worldInteractableElementData = (WorldInteractableElementData)x;

            if (worldInteractableElementData.QuestId == QuestEditor.Id)
                worldInteractableElementData.ElementStatus = Enums.ElementStatus.Enabled;
            else if (worldInteractableElementData.QuestId == 0)
                worldInteractableElementData.ElementStatus = Enums.ElementStatus.Disabled;
            else
                worldInteractableElementData.ElementStatus = Enums.ElementStatus.Locked;
        });
    }

    public void InitializeSegment() { }
    
    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(IElementData elementData) { }

    public void CloseSegment() { }
}
