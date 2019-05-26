using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class ChapterHeaderSegment : MonoBehaviour, ISegment
{
    ChapterDataElement chapterData;

    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    #region UI
    public IndexSwitch indexSwitch;
    public SelectionElement selectionElement;
    public InputField inputField;
    public Text idText;
    #endregion

    #region Data Variables
    private int id;
    private int index;
    private int elementId;
    private string chapterName;
    private string objectGraphicIcon;
    #endregion

    #region Methods
    public void UpdateName()
    {
        chapterData.Name = inputField.text;
        DataEditor.UpdateEditor();
    }

    public void UpdateElement(ElementDataElement elementDataElement)
    {
        chapterData.ElementId = elementDataElement.id;
        chapterData.objectGraphicIcon = elementDataElement.objectGraphicIcon;

        DataEditor.UpdateEditor();
    }
    #endregion

    #region Segment
    public void InitializeSegment()
    {
        DataEditor = SegmentController.editorController.pathController.dataEditor;

        InitializeChapterData();

        selectionElement.InitializeElement();

        if (indexSwitch != null)
            indexSwitch.InitializeSwitch(this, index, DataEditor.Data.DataController.DataList.Count - 1);
    }

    private void InitializeChapterData()
    {
        ChapterDataElement chapterData = DataEditor.Data.ElementData.Cast<ChapterDataElement>().FirstOrDefault();

        id = chapterData.id;
        index = chapterData.Index;
        chapterName = chapterData.Name;

        elementId = chapterData.ElementId;
        objectGraphicIcon = chapterData.objectGraphicIcon;
    }

    public void OpenSegment()
    {
        if (indexSwitch != null)
            indexSwitch.Activate();

        idText.text = id.ToString();

        inputField.text = chapterName;

        var elementDataElement = new ElementDataElement();

        elementDataElement.id = elementId;
        elementDataElement.objectGraphicIcon = objectGraphicIcon;

        selectionElement.SetElement(new[] { elementDataElement });

        gameObject.SetActive(true);
    }

    public void ApplySegment()
    {

    }

    public void CloseSegment()
    {
        if (indexSwitch != null)
            indexSwitch.Deactivate();

        gameObject.SetActive(false);
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {
        switch (selectionElement.route.data.DataController.DataType)
        {
            case Enums.DataType.Element:

                var elementDataElement = selectionElement.route.data.ElementData.Cast<ElementDataElement>().FirstOrDefault();

                UpdateElement(elementDataElement);

                break;

            default: Debug.Log("CASE MISSING"); break;
        }
    }

    #endregion
}
