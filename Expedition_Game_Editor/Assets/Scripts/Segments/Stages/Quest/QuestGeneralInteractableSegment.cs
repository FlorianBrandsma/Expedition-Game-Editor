using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuestGeneralInteractableSegment : MonoBehaviour, ISegment
{
    private QuestEditor QuestEditor { get { return (QuestEditor)DataEditor; } }

    private DataManager dataManager = new DataManager();

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }
    
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment() { }

    public void InitializeData()
    {
        if (DataEditor.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.WorldInteractable);

        var searchParameters = searchProperties.searchParameters.Cast<Search.WorldInteractable>().First();
        searchParameters.phaseId = new List<int>() { QuestEditor.QuestData.PhaseId };

        SegmentController.DataController.DataList = RenderManager.GetData(SegmentController.DataController, searchProperties);

        SetStatus(SegmentController.DataController.DataList);

        var questInteractableList = SegmentController.DataController.DataList.Cast<WorldInteractableDataElement>().ToList();
        questInteractableList.ForEach(x => QuestEditor.worldInteractableDataList.Add(x));
    }

    private void SetStatus(List<IDataElement> dataList)
    {
        dataList.ForEach(x =>
        {
            var worldInteractableDataElement = (WorldInteractableDataElement)x;

            if (worldInteractableDataElement.QuestId == QuestEditor.QuestData.Id)
                worldInteractableDataElement.elementStatus = Enums.ElementStatus.Enabled;
            else if (worldInteractableDataElement.QuestId == 0)
                worldInteractableDataElement.elementStatus = Enums.ElementStatus.Disabled;
            else
                worldInteractableDataElement.elementStatus = Enums.ElementStatus.Locked;
        });
    }

    private void SetSearchParameters() { }

    public void OpenSegment()
    {
        SetSearchParameters();

        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement selectionElement)
    {
        DataEditor.UpdateEditor();

        SetSearchParameters();
    }
}
