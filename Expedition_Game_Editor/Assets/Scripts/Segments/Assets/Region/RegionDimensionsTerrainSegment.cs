using UnityEngine;
using UnityEngine.UI;

public class RegionDimensionsTerrainSegment : MonoBehaviour, ISegment
{
    private RegionDataElement regionDataElement;

    #region UI

    public EditorInputNumber sizeInputNumber;
    public Text heightText;

    #endregion

    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    public void UpdateSize()
    {
        regionDataElement.TerrainSize = (int)sizeInputNumber.Value;

        heightText.text = regionDataElement.TerrainSize.ToString();

        DataEditor.UpdateEditor();
    }

    public void ApplySegment() { }

    public void CloseSegment() { }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.dataEditor;
    }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        regionDataElement = (RegionDataElement)DataEditor.Data.dataElement;
    }

    private void SetSearchParameters() { }

    public void OpenSegment()
    {
        SegmentController.EnableSegment(false);

        sizeInputNumber.Value = regionDataElement.TerrainSize;

        heightText.text = regionDataElement.TerrainSize.ToString();
    }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
