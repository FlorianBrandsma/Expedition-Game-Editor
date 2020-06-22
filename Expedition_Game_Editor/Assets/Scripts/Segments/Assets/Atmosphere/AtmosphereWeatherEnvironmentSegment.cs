using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class AtmosphereWeatherEnvironmentSegment : MonoBehaviour, ISegment
{
    private AtmosphereElementData AtmosphereData { get { return (AtmosphereElementData)DataEditor.Data.elementData; } }

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public Text terrainText;
    public Text regionText;
    
    public RawImage icon;
    public RawImage baseIcon;
    #endregion

    #region Data Methods
    #endregion

    #region Segment
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment()
    {
        icon.texture = Resources.Load<Texture2D>(AtmosphereData.iconPath);
        baseIcon.texture = Resources.Load<Texture2D>(AtmosphereData.baseTilePath);

        terrainText.text = AtmosphereData.terrainName;
        regionText.text = AtmosphereData.regionName;

    }

    public void InitializeData() { }

    public void OpenSegment() { }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
    #endregion
}
