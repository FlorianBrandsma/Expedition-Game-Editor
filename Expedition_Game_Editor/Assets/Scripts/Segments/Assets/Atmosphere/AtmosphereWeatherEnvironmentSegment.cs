using UnityEngine;
using UnityEngine.UI;

public class AtmosphereWeatherEnvironmentSegment : MonoBehaviour, ISegment
{
    public Text terrainText;
    public Text regionText;

    public RawImage icon;
    public RawImage baseIcon;

    public SegmentController SegmentController      { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                       { get; set; }

    private AtmosphereEditor AtmosphereEditor       { get { return (AtmosphereEditor)DataEditor; } }

    #region Data properties
    private string IconPath
    {
        get { return AtmosphereEditor.IconPath; }
    }

    private string BaseTilePath
    {
        get { return AtmosphereEditor.BaseTilePath; }
    }

    private string TerrainName
    {
        get { return AtmosphereEditor.TerrainName; }
    }

    private string RegionName
    {
        get { return AtmosphereEditor.RegionName; }
    }
    #endregion

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData() { }

    public void InitializeSegment()
    {
        icon.texture = Resources.Load<Texture2D>(IconPath);
        baseIcon.texture = Resources.Load<Texture2D>(BaseTilePath);

        terrainText.text = TerrainName;
        regionText.text = RegionName;

    }
    
    public void OpenSegment() { }

    public void SetSearchResult(IElementData elementData) { }

    public void CloseSegment() { }
}
