using UnityEngine;

public class TerrainGeneralAtmosphereSegment : MonoBehaviour, ISegment
{
    public DataElement editButton;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }
    
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData() { }

    public void InitializeSegment()
    {
        InitializeEditButton();
    }

    private void InitializeEditButton()
    {
        //Take the data from the last selected terrain
        var terrainRoute = SegmentController.Path.FindLastRoute(Enums.DataType.Terrain);

        editButton.Id = terrainRoute.id;
        editButton.Data = terrainRoute.data;

        //Cut the path to the region
        editButton.Path = SegmentController.Path.TrimToLastType(Enums.DataType.Region);

        editButton.InitializeElement();
    }

    public void OpenSegment() { }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
