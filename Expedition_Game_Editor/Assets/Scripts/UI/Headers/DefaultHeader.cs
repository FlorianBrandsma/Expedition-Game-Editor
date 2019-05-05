using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class DefaultHeader : MonoBehaviour, ISegment
{
    private SegmentController segmentController;
    public IEditor DataEditor { get; set; }

    #region UI
    public IndexSwitch index_switch;

    public InputField input_field;
    public Text id;
    #endregion

    #region Data Variables
    private int _id;
    private int _index;
    private string _name;
    private string _icon;
    #endregion

    #region Data Properties
    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;

            switch (DataEditor.data.controller.DataType)
            {
                case Enums.DataType.Chapter:

                    ChapterDataElement chapterData = DataEditor.data.element.Cast<ChapterDataElement>().FirstOrDefault();
                    chapterData.Name = value;

                    break;

                case Enums.DataType.Phase:

                    PhaseDataElement phaseData = DataEditor.data.element.Cast<PhaseDataElement>().FirstOrDefault();
                    phaseData.name = value;

                    break; 
            }
        }
    }
    #endregion

    #region Data Methods
    public void UpdateName()
    {
        Name = input_field.text;
        DataEditor.UpdateEditor();
    }
    #endregion

    #region Segment
    public void InitializeSegment()
    {
        segmentController = GetComponent<SegmentController>();
        DataEditor = segmentController.editorController.pathController.dataEditor;

        switch (DataEditor.data.controller.DataType)
        {
            case Enums.DataType.Chapter: InitializeChapterData(); break;
            case Enums.DataType.Phase: InitializePhaseData(); break;
        }

        if (index_switch != null)
            index_switch.InitializeSwitch(this, _index, DataEditor.data.controller.DataList.Count - 1); 
    }

    private void InitializeChapterData()
    {
        ChapterDataElement chapterData = DataEditor.data.element.Cast<ChapterDataElement>().FirstOrDefault();

        _id = chapterData.id;
        _index = chapterData.Index;
        _name = chapterData.Name;
    }

    private void InitializePhaseData()
    {
        PhaseDataElement phaseData = DataEditor.data.element.Cast<PhaseDataElement>().FirstOrDefault();

        _id = phaseData.id;
        _index = phaseData.index;
        _name = phaseData.name;
        _icon = phaseData.icon;
    }

    public void OpenSegment()
    {
        if (index_switch != null)
            index_switch.Activate();

        id.text = _id.ToString();

        input_field.text = _name;

        gameObject.SetActive(true);
    }

    public void ApplySegment()
    {

    }

    public void CloseSegment()
    {
        if (index_switch != null)
            index_switch.Deactivate();

        gameObject.SetActive(false);
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {

    }
    #endregion
}
