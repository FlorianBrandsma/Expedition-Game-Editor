using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DefaultHeader : MonoBehaviour, ISegment
{
    private SegmentController segmentController;
    public IEditor dataEditor { get; set; }

    #region UI
    public IndexSwitch index_switch;

    public SelectionElement main_element;
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

            switch (dataEditor.data.controller.data_type)
            {
                case Enums.DataType.Chapter:

                    ChapterDataElement chapterData = dataEditor.data.element.Cast<ChapterDataElement>().FirstOrDefault();
                    chapterData.name = value;

                    break;

                case Enums.DataType.Phase:

                    PhaseDataElement phaseData = dataEditor.data.element.Cast<PhaseDataElement>().FirstOrDefault();
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
        dataEditor.UpdateEditor();
    }
    #endregion

    #region Segment
    public void InitializeSegment()
    {
        segmentController = GetComponent<SegmentController>();
        dataEditor = segmentController.editorController.pathController.dataEditor;

        switch (dataEditor.data.controller.data_type)
        {
            case Enums.DataType.Chapter: InitializeChapterData(); break;
            case Enums.DataType.Phase: InitializePhaseData(); break;
        }

        if (index_switch != null)
            index_switch.InitializeSwitch(this, _index, dataEditor.data.controller.data_list.Count - 1); 
    }

    private void InitializeChapterData()
    {
        ChapterDataElement chapterData = dataEditor.data.element.Cast<ChapterDataElement>().FirstOrDefault();

        _id = chapterData.id;
        _index = chapterData.index;
        _name = chapterData.name;
        _icon = chapterData.icon;
    }

    private void InitializePhaseData()
    {
        PhaseDataElement phaseData = dataEditor.data.element.Cast<PhaseDataElement>().FirstOrDefault();

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
    #endregion
}
