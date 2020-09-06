using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class WorldObjectRotationDegreeSegment : MonoBehaviour, ISegment
{
    public ExInputNumber xInputField, yInputField, zInputField;

    private int rotationX, rotationY, rotationZ;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public int RotationX
    {
        get { return rotationX; }
        set
        {
            rotationX = value;

            var worldObjectDataList = DataEditor.DataList.Cast<WorldObjectElementData>().ToList();
            worldObjectDataList.ForEach(worldObjectData =>
            {
                worldObjectData.RotationX = value;
            });
        }
    }

    public int RotationY
    {
        get { return rotationY; }
        set
        {
            rotationY = value;

            var worldObjectDataList = DataEditor.DataList.Cast<WorldObjectElementData>().ToList();
            worldObjectDataList.ForEach(worldObjectData =>
            {
                worldObjectData.RotationY = value;
            });
        }
    }

    public int RotationZ
    {
        get { return rotationZ; }
        set
        {
            rotationZ = value;

            var worldObjectDataList = DataEditor.DataList.Cast<WorldObjectElementData>().ToList();
            worldObjectDataList.ForEach(worldObjectData =>
            {
                worldObjectData.RotationZ = value;
            });
        }
    }

    public void UpdateRotationX()
    {
        RotationX = (int)xInputField.Value;

        DataEditor.UpdateEditor();
    }

    public void UpdateRotationY()
    {
        RotationY = (int)yInputField.Value;

        DataEditor.UpdateEditor();
    }

    public void UpdateRotationZ()
    {
        RotationZ = (int)zInputField.Value;

        DataEditor.UpdateEditor();
    }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment() { }

    public void InitializeData()
    {
        if (DataEditor.Loaded) return;

        var worldObjectData = (WorldObjectElementData)DataEditor.ElementData;

        rotationX = worldObjectData.RotationX;
        rotationY = worldObjectData.RotationY;
        rotationZ = worldObjectData.RotationZ;
    }

    public void OpenSegment()
    {
        xInputField.Value = RotationX;
        yInputField.Value = RotationY;
        zInputField.Value = RotationZ;

        gameObject.SetActive(true);
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
