using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class TaskElementTransformEditSegment : MonoBehaviour, ISegment
{
    private SegmentController segmentController { get { return GetComponent<SegmentController>(); } }
    private IDataController dataController { get { return GetComponent<IDataController>(); } }
    public IEditor dataEditor { get; set; }

    private TaskDataElement task_data;
    private RegionDataElement region_data;
    //private RegionDataElement region_data;

    #region UI
    public SelectionElement editor_button;
    #endregion

    #region Data Methods
    private void InitializeData()
    {
        //region_data = segmentController.editorController.pathController.route.path.FindLastRoute("Region").data.Cast<RegionDataElement>().FirstOrDefault();
        region_data = new RegionDataElement();

        region_data.id = 0;
        region_data.table = "Region";
        region_data.type = (int)Enums.RegionType.Task;
    }
    #endregion

    #region Segment
    public void InitializeSegment()
    {
        dataEditor = segmentController.editorController.pathController.dataEditor;

        InitializeData();

        SelectionElement element = editor_button.GetComponent<SelectionElement>();

        element.route = segmentController.editorController.pathController.route.Copy();
        element.route.property = SelectionManager.Property.Open;

        //Data could be initialized here

        element.route.data = new Data(dataController, new[] { region_data });
    }

    public void OpenSegment()
    {
        //editor_button.GetComponentInChildren<Text>().text = "Open " + region_data.name;
        gameObject.SetActive(true);
    }

    public void ApplySegment()
    {
        
    }

    public void CloseSegment()
    {
        gameObject.SetActive(false);
    }

    public void SetSearchResult(SearchElement searchElement)
    {

    }
    #endregion
}
