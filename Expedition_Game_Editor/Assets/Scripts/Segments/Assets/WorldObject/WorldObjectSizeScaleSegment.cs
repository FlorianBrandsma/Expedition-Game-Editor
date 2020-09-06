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
    private float scale;
    
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public float Scale
    {
        get { return scale; }
        set
        {
            scale = value;

            var worldObjectDataList = DataEditor.DataList.Cast<WorldObjectElementData>().ToList();
            worldObjectDataList.ForEach(worldObjectData =>
            {
                worldObjectData.Scale = value;
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

        var worldObjectData = (WorldObjectElementData)DataEditor.ElementData;

        height  = worldObjectData.Height;
        width   = worldObjectData.Width;
        depth   = worldObjectData.Depth;

        scale   = worldObjectData.Scale;
    }

    public void InitializeSegment()
    {
        SetSizeValues();
    }

    public void SetSizeValues()
    {
        heightText.text = (height * scale).ToString();
        widthText.text  = (width * scale).ToString();
        depthText.text  = (depth * scale).ToString();
    }

    public void UpdateScale()
    {
        Scale = inputField.Value;

        SetSizeValues();

        DataEditor.UpdateEditor();
    }

    public void OpenSegment()
    {
        SetSizeValues();

        inputField.Value = Scale;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
