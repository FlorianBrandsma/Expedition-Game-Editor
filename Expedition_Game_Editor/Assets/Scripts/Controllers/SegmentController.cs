using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class SegmentController : MonoBehaviour
{
    public bool Loaded { get { return EditorController.PathController.layoutSection.Loaded; } }

    public GameObject dataControllerParent;
    
    public bool enableToggle;

    public ExToggle editorToggle;

    public string segmentName;
    public Text header;

    public List<SegmentController> segmentGroup;
    public List<EditorElement> editorElements;

    public EditorController EditorController { get; set; }

    public Path Path        { get { return EditorController.PathController.route.path; } }
    public Path MainPath    { get { return EditorController.PathController.layoutSection.EditorForm.activePath; } }

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

    public List<IDataController> DataControllerList
    {
        get
        {
            List<IDataController> dataControllerList = GetComponents<IDataController>().ToList();

            if (dataControllerParent != null)
                dataControllerList.AddRange(dataControllerParent.GetComponents<IDataController>().ToList());

            return dataControllerList;
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
        foreach (SegmentController segment in segmentGroup)
        {
            if (segment.editorToggle.Toggle.isOn != editorToggle.Toggle.isOn)
                segment.editorToggle.Toggle.isOn = editorToggle.Toggle.isOn;
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
        this.EditorController = editorController;

        DataControllerList.ForEach(x => x.InitializeController());
        
        if (Segment != null)
            Segment.InitializeDependencies();            
    }

    public void InitializeSegment(EditorController editorController)
    {
        //Necessary for shared segments
        this.EditorController = editorController;
        
        if (GetComponent<SearchController>() != null)
            GetComponent<SearchController>().InitializeController();
        
        if (GetComponent<ISegment>() != null)
            GetComponent<ISegment>().InitializeSegment();
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
