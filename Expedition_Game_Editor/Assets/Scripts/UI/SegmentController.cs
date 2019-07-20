using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class SegmentController : MonoBehaviour
{
    public GameObject dataControllerParent;

    public bool loadOnce;

    public bool enableToggle;

    public EditorToggle editorToggle;

    public string segmentName;
    public Text header;

    public SegmentController[] segmentGroup;
    public SegmentController siblingSegment;

    public EditorElement[] editorElements;

    [HideInInspector]
    public EditorController editorController;

    [HideInInspector]
    public bool loaded;

    public Path Path { get { return editorController.pathController.route.path; } }

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

        EnableToggle();
    }

    public void ToggleSegment()
    {
        foreach(SegmentController segment in segmentGroup)
        {
            if (segment.editorToggle.Toggle.isOn != editorToggle.Toggle.isOn)
                segment.editorToggle.Toggle.isOn  = editorToggle.Toggle.isOn;
        }

        foreach (EditorElement editorElement in editorElements)
            editorElement.GetComponent<IEditorElement>().EnableElement(editorToggle.Toggle.isOn);
    }

    private void EnableToggle()
    {
        editorToggle.EnableElement(enableToggle);
    }

    public void EnableSegment(bool enable)
    {
        editorToggle.EnableElement(enable);

        foreach (EditorElement editorElement in editorElements)
            editorElement.GetComponent<IEditorElement>().EnableElement(enable);
    }

    public void InitializeDependencies(EditorController editorController)
    {
        this.editorController = editorController;

        if (Segment != null)
            Segment.InitializeDependencies();
    }

    public void InitializeSegment(EditorController editorController)
    {
        //Necessary for shared segments
        this.editorController = editorController;

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
