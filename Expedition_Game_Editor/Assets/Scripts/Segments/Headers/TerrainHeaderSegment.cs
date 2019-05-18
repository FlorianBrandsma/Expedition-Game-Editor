using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class TerrainHeaderSegment : MonoBehaviour, ISegment
{
    private SegmentController segmentController;

    private TerrainDataElement terrainData;

    public IEditor DataEditor { get; set; }

    #region UI

    public SelectionElement selectionElement;
    public InputField inputField;
    public Text idText;

    #endregion

    #region Data Variables

    private int id;
    private int index;
    private string name;
    private string icon;

    #endregion

    #region Data Properties
    public string Name
    {
        get { return name; }
        set
        {
            name = value;

            terrainData.Name = value;
        }
    }
    #endregion

    #region Data Methods
    public void UpdateName()
    {
        Name = inputField.text;
        DataEditor.UpdateEditor();
    }
    #endregion

    #region Segment
    public void InitializeSegment()
    {
        segmentController = GetComponent<SegmentController>();
        DataEditor = segmentController.editorController.pathController.dataEditor;

        InitializeTaskData();
    }

    private void InitializeTaskData()
    {
        terrainData = DataEditor.Data.ElementData.Cast<TerrainDataElement>().FirstOrDefault();

        id = terrainData.id;
        index = terrainData.Index;
        name = terrainData.Name;
    }

    public void OpenSegment()
    {
        idText.text = id.ToString();

        inputField.text = name;

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
