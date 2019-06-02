using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhaseElementController : MonoBehaviour, IDataController
{
    public Search.PhaseElement searchParameters;

    private PhaseElementDataManager phaseElementDataManager = new PhaseElementDataManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.PhaseElement; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.PhaseElement>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        phaseElementDataManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = phaseElementDataManager.GetQuestElementDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {

    }

    public void ToggleElement(IDataElement dataElement)
    {
        var phaseElementData = (PhaseElementDataElement)dataElement;
        var questData = (QuestDataElement)SegmentController.editorController.pathController.route.data.DataElement;

        switch(phaseElementData.elementStatus)
        {
            case Enums.ElementStatus.Enabled:

                phaseElementData.QuestId = 0;
                phaseElementData.elementStatus = Enums.ElementStatus.Disabled;

                break;

            case Enums.ElementStatus.Disabled:

                phaseElementData.QuestId = questData.id;
                phaseElementData.elementStatus = Enums.ElementStatus.Enabled;

                break;
        }

        SegmentController.Segment.DataEditor.UpdateEditor();

        dataElement.SelectionElement.SetOverlay(phaseElementData.elementStatus); 
    }
}