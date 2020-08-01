using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class WorldObjectSizeScaleSegment : MonoBehaviour, ISegment
{
    public ExInputNumber inputField;
    public Text heightText, widthText, depthText;
    
    private float height;
    private float width;
    private float depth;
    private float scaleMultiplier;
    
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public float ScaleMultiplier
    {
        get { return scaleMultiplier; }
        set
        {
            scaleMultiplier = value;

            var worldObjectDataList = DataEditor.DataList.Cast<WorldObjectElementData>().ToList();
            worldObjectDataList.ForEach(worldObjectData =>
            {
                worldObjectData.ScaleMultiplier = value;
            });
        }
    }
    
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }
    
    public void InitializeData()
    {
        InitializeDependencies();

        if (DataEditor.Loaded) return;

        var worldObjectData = (WorldObjectElementData)DataEditor.Data.elementData;

        height = worldObjectData.height;
        width = worldObjectData.width;
        depth = worldObjectData.depth;

        scaleMultiplier = worldObjectData.ScaleMultiplier;
    }

    public void InitializeSegment()
    {
        SetSizeValues();
    }

    public void SetSizeValues()
    {
        heightText.text = (height * scaleMultiplier).ToString();
        widthText.text = (width * scaleMultiplier).ToString();
        depthText.text = (depth * scaleMultiplier).ToString();
    }

    public void UpdateScaleMultiplier()
    {
        ScaleMultiplier = inputField.Value;

        SetSizeValues();

        DataEditor.UpdateEditor();
    }

    private void SetSearchParameters() { }

    public void OpenSegment()
    {
        SetSizeValues();

        inputField.Value = ScaleMultiplier;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
