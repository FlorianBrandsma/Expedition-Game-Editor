using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SegmentController : MonoBehaviour
{
    public GameObject dataControllerParent;

    public bool loadOnce;

    public bool disableToggle;
    public Toggle toggle;
    public string segmentName;
    public Text header;
    public GameObject content;

    public SegmentController[] segmentGroup;
    public SegmentController siblingSegment;
    
    [HideInInspector]
    public EditorController editorController;

    [HideInInspector]
    public bool loaded;

    [HideInInspector]
    public Path path;

    public ISegment Segment { get { return GetComponent<ISegment>(); } }

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
        foreach(SegmentController segment in segmentGroup)
        {
            if (segment.toggle.isOn != toggle.isOn)
                segment.toggle.isOn  = toggle.isOn;
        }

        content.SetActive(toggle.isOn);
    }

    public void InitializeDependencies(EditorController editorController)
    {
        this.editorController = editorController;

        if (Segment != null)
            Segment.InitializeDependencies();
    }

    public void InitializeSegment(EditorController editorController)
    {
        this.editorController = editorController;

        path = editorController.pathController.route.path;

        if (DataController != null)
            DataController.InitializeController();

        if (GetComponent<SearchController>() != null)
            GetComponent<SearchController>().InitializeController();

        if (GetComponent<ISegment>() != null)
            GetComponent<ISegment>().InitializeSegment();

        if (siblingSegment != null)
            siblingSegment.Segment.InitializeData();

        loaded = true; 
    }

    public void OpenSegment()
    {
        if (GetComponent<ISegment>() != null)
            GetComponent<ISegment>().OpenSegment();
    }

    public void CloseSegment()
    {
        if (Segment != null)
            Segment.CloseSegment();

        if (Display != null)
            Display.CloseDisplay();

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
