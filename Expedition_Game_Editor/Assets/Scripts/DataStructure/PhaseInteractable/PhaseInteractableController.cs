using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhaseInteractableController : MonoBehaviour, IDataController
{
    public Search.PhaseInteractable searchParameters;

    private PhaseInteractableDataManager phaseInteractableDataManager = new PhaseInteractableDataManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.PhaseInteractable; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.PhaseInteractable>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        phaseInteractableDataManager.InitializeManager(this);
    }

    public List<IDataElement> GetData(IEnumerable searchParameters)
    {
        return phaseInteractableDataManager.GetQuestInteractableDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, SelectionElement.Data resultData) { }

    public void ToggleElement(IDataElement dataElement)
    {
        var phaseInteractablesData = (PhaseInteractableDataElement)dataElement;
        var questData = (QuestDataElement)SegmentController.editorController.PathController.route.data.dataElement;

        switch(phaseInteractablesData.elementStatus)
        {
            case Enums.ElementStatus.Enabled:

                phaseInteractablesData.QuestId = 0;
                phaseInteractablesData.elementStatus = Enums.ElementStatus.Disabled;

                break;

            case Enums.ElementStatus.Disabled:

                phaseInteractablesData.QuestId = questData.id;
                phaseInteractablesData.elementStatus = Enums.ElementStatus.Enabled;

                break;
        }

        SegmentController.Segment.DataEditor.UpdateEditor();

        dataElement.SelectionElement.SetOverlay(phaseInteractablesData.elementStatus); 
    }
}