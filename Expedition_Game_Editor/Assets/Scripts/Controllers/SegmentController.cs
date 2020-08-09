using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class SegmentController : MonoBehaviour
{
    public bool Loaded { get { return EditorController.PathController.layoutSection.Loaded; } }

    public GameObject dataControllerParent;

    public string segmentName;
    public Text header;

    public List<EditorElement> editorElements;

    public EditorController EditorController { get; set; }

    public ListProperties ListProperties { get { return GetComponent<ListProperties>(); } }

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
    }
    
    public void EnableSegment(bool enable)
    {
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
        if (ListProperties != null)
        {  
            if (ListProperties.selectionType == SelectionManager.Type.Automatic)
            {
                ListProperties.AutoSelectElement();

                return true;
            }            
        }

        return false;
    }
}
