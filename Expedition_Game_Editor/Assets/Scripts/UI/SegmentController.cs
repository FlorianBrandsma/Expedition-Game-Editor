using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SegmentController : MonoBehaviour
{
    private ISegment segment { get { return GetComponent<ISegment>(); } }

    public GameObject dataControllerParent;

    public IDataController dataController
    {
        get
        {
            if (dataControllerParent != null)
                return dataControllerParent.GetComponent<IDataController>();
            else
                return GetComponent<IDataController>();
        }
    }

    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public EditorController editorController { get; set; }

    public bool autoLoad = true;
    public bool loaded { get; set; }

    public bool disableToggle;
    public Toggle toggle;
    public string segmentName;
    public Text header;
    public GameObject content;

    public SegmentController[] siblingSegments;

    public Path path { get; set; }

    private void Awake()
    {
        if (header == null) return;

        header.text = segmentName;

        if (disableToggle)
            DisableToggle();
    }

    private void DisableToggle()
    {
        toggle.interactable = false;
        toggle.isOn = true;
        toggle.targetGraphic.color = Color.gray;
    }

    public void ActivateSegment()
    {
        foreach(SegmentController segment in siblingSegments)
        {
            if (segment.toggle.isOn != toggle.isOn)
                segment.toggle.isOn  = toggle.isOn;
        }

        content.SetActive(toggle.isOn);
    }

    public void CloseSegment()
    {
        if(segment != null)
            segment.CloseSegment();
    }

    public void InitializeSegment(EditorController editorController)
    {
        this.editorController = editorController;

        path = editorController.pathController.route.path;

        if (GetComponent<ISegment>() != null)
            GetComponent<ISegment>().InitializeSegment();

        if (!editorController.pathController.loaded && autoLoad)
        {
            if (dataController != null)
                dataController.InitializeController();    
        }     
    }

    public void FilterRows(List<GeneralData> list)
    {
        if (GetComponent<ListProperties>() != null)
        {
            GetComponent<ListProperties>().CloseDisplay();
            GetComponent<ListProperties>().segmentController.dataController.dataList = new List<GeneralData>(list);
        }

        SetSegmentDisplay();
    }

    public void InitializeSegmentDisplay()
    {
        if (display != null)
            display.InitializeProperties();
    }

    public void SetSegmentDisplay()
    {
        if (display != null && autoLoad)
            display.SetDisplay();

        //This block might belong in a non-display method
        if (GetComponent<ISegment>() != null)
            GetComponent<ISegment>().OpenSegment();
    }

    public void CloseSegmentDisplay()
    {
        if (display != null)
            display.CloseDisplay();      
    }

    public bool AutoSelectElement()
    {
        if (GetComponent<ListProperties>() != null)
        {
            if (GetComponent<ListProperties>().selectionType == SelectionManager.Type.Automatic)
            {
                GetComponent<ListProperties>().AutoSelectElement();

                return true;
            }            
        }

        return false;
    }
}
