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

        var questInteractableList = SegmentController.DataController.DataList.Cast<WorldInteractableElementData>().ToList();
        questInteractableList.ForEach(x => QuestEditor.worldInteractableDataList.Add(x));
    }

    private void SetStatus(List<IElementData> dataList)
    {
        dataList.ForEach(x =>
        {
            var worldInteractableElementData = (WorldInteractableElementData)x;

            if (worldInteractableElementData.QuestId == QuestEditor.QuestData.Id)
                worldInteractableElementData.elementStatus = Enums.ElementStatus.Enabled;
            else if (worldInteractableElementData.QuestId == 0)
                worldInteractableElementData.elementStatus = Enums.ElementStatus.Disabled;
            else
                worldInteractableElementData.elementStatus = Enums.ElementStatus.Locked;
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

    public void SetSearchResult(DataElement dataElement)
    {
        DataEditor.UpdateEditor();

        SetSearchParameters();
    }
}
