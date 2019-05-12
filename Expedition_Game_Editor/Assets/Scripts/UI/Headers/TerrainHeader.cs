using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class TerrainHeader : MonoBehaviour, ISegment
{
    private SegmentController segmentController { get { return GetComponent<SegmentController>(); } }
    private IDataController dataController { get { return GetComponent<IDataController>(); } }
    public IEditor DataEditor { get; set; }

    private TerrainDataElement terrain_data;
    private RegionDataElement region_data;

    #region UI
    public Button editor_button;
    public Text id;
    #endregion

    #region Data Variables
    private int _id;
    #endregion

    #region Data Methods
    private void InitializeData()
    {
        terrain_data = DataEditor.data.ElementData.Cast<TerrainDataElement>().FirstOrDefault();
        region_data = segmentController.editorController.pathController.route.path.FindLastRoute("Region").data.ElementData.Cast<RegionDataElement>().FirstOrDefault();

        _id = terrain_data.id;
    }
    #endregion

    #region Segment
    public void InitializeSegment()
    {
        DataEditor = segmentController.editorController.pathController.dataEditor;

        InitializeData();

        SelectionElement element = editor_button.GetComponent<SelectionElement>();

        element.route = segmentController.editorController.pathController.route.Copy();
        element.route.property = SelectionManager.Property.Open;

        element.route.data = new Data(dataController, new[] { region_data });
    }

    public void OpenSegment()
    {
        id.text = _id.ToString();
        editor_button.GetComponentInChildren<Text>().text = "Open " + region_data.name;
        gameObject.SetActive(true);
    }

    public void ApplySegment()
    {

    }

    public void CloseSegment()
    {
        gameObject.SetActive(false);
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {

    }
    #endregion
}
