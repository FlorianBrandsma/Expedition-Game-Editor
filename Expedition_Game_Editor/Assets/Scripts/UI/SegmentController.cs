using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SegmentController : MonoBehaviour
{
    public GameObject dataControllerParent;
    
    public bool autoLoad;
    public bool loadOnce;

    public bool disableToggle;
    public Toggle toggle;
    public string segmentName;
    public Text header;
    public GameObject content;

    public SegmentController[] siblingSegments;

    [HideInInspector]
    public EditorController editorController;

    [HideInInspector]
    public bool loaded;

    [HideInInspector]
    public Path path;

    private ISegment Segment { get { return GetComponent<ISegment>(); } }

    public IDataController DataController
    {
        get
        {
            if (dataControllerParent != null)
                return dataControllerParent.GetComponent<IDataController>();
            else
                return GetComponent<IDataController>();
        }
    }

    public IDisplay Display { get { return GetComponent<IDisplay>(); } }

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
        if(Segment != null)
            Segment.CloseSegment();
    }

    public void InitializeSegment(EditorController editorController)
    {
        this.editorController = editorController;

        path = editorController.pathController.route.path;

        if (GetComponent<ISegment>() != null)
            GetComponent<ISegment>().InitializeSegment();

        if (!loaded && !editorController.pathController.loaded && autoLoad)
        {
            if (GetComponent<SearchController>() != null)
                GetComponent<SearchController>().InitializeController();

            if (DataController != null)
            {
                DataController.InitializeController();
                loaded = true;
            }
        }     
    }

    public void FilterRows(List<GeneralData> list)
    {
        if (GetComponent<ListProperties>() != null)
        {
            GetComponent<ListProperties>().CloseDisplay();
            GetComponent<ListProperties>().SegmentController.DataController.DataList = new List<GeneralData>(list);
        }

        SetSegmentDisplay();
    }

    public void InitializeSegmentDisplay()
    {
        if (Display != null)
            Display.InitializeProperties();
    }

    public void SetSegmentDisplay()
    {
        if (Display != null && autoLoad)
            Display.SetDisplay();

        //This block might belong in a non-display method
        if (GetComponent<ISegment>() != null)
            GetComponent<ISegment>().OpenSegment();
    }

    public void CloseSegmentDisplay()
    {
        if (Display != null)
            Display.CloseDisplay();

        if (GetComponent<SearchController>() != null)
            GetComponent<SearchController>().CloseController();

        if (!loadOnce)
            loaded = false;
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
