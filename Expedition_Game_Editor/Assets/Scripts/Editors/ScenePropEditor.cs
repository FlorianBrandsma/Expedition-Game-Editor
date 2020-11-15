using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ScenePropEditor : MonoBehaviour, IEditor
{
    private ScenePropData scenePropData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == scenePropData.Id).FirstOrDefault(); } }

    private PathController PathController           { get { return GetComponent<PathController>(); } }
    public List<SegmentController> EditorSegments   { get; } = new List<SegmentController>();

    public bool Loaded { get; set; }

    public List<IElementData> DataList
    {
        get { return SelectionElementManager.FindElementData(EditData).Concat(new[] { EditData }).Distinct().ToList(); }
    }

    public List<IElementData> ElementDataList
    {
        get
        {
            var list = new List<IElementData>();

            DataList.ForEach(x => { if (x != null) list.Add(x); });

            return list;
        }
    }

    #region Data properties
    public int Id
    {
        get { return scenePropData.Id; }
    }
    
    public int TerrainId
    {
        get { return scenePropData.TerrainId; }
        set
        {
            scenePropData.TerrainId = value;

            DataList.ForEach(x => ((ScenePropElementData)x).TerrainId = value);
        }
    }

    public int TerrainTileId
    {
        get { return scenePropData.TerrainTileId; }
        set
        {
            scenePropData.TerrainTileId = value;

            DataList.ForEach(x => ((ScenePropElementData)x).TerrainTileId = value);
        }
    }

    public float PositionX
    {
        get { return scenePropData.PositionX; }
        set
        {
            scenePropData.PositionX = value;

            DataList.ForEach(x => ((ScenePropElementData)x).PositionX = value);
        }
    }

    public float PositionY
    {
        get { return scenePropData.PositionY; }
        set
        {
            scenePropData.PositionY = value;

            DataList.ForEach(x => ((ScenePropElementData)x).PositionY = value);
        }
    }

    public float PositionZ
    {
        get { return scenePropData.PositionZ; }
        set
        {
            scenePropData.PositionZ = value;

            DataList.ForEach(x => ((ScenePropElementData)x).PositionZ = value);
        }
    }

    public int RotationX
    {
        get { return scenePropData.RotationX; }
        set
        {
            scenePropData.RotationX = value;

            DataList.ForEach(x => ((ScenePropElementData)x).RotationX = value);
        }
    }

    public int RotationY
    {
        get { return scenePropData.RotationY; }
        set
        {
            scenePropData.RotationY = value;

            DataList.ForEach(x => ((ScenePropElementData)x).RotationY = value);
        }
    }

    public int RotationZ
    {
        get { return scenePropData.RotationZ; }
        set
        {
            scenePropData.RotationZ = value;

            DataList.ForEach(x => ((ScenePropElementData)x).RotationZ = value);
        }
    }

    public string ModelName
    {
        get { return ((ScenePropData)EditData).ModelName; }
    }

    public string ModelIconPath
    {
        get { return ((ScenePropData)EditData).ModelIconPath; }
    }
    #endregion

    public void InitializeEditor()
    {
        scenePropData = (ScenePropData)ElementData.Clone();
    }
    
    public void OpenEditor() { }

    public void UpdateEditor()
    {
        ElementDataList.Where(x => SelectionElementManager.SelectionActive(x.DataElement)).ToList().ForEach(x => x.DataElement.UpdateElement());

        SetEditor();
    }

    public void SetEditor()
    {
        PathController.layoutSection.SetActionButtons();
    }

    public bool Changed()
    {
        return ElementDataList.Any(x => x.Changed);
    }

    public void ApplyChanges()
    {
        EditData.Update();
        
        ElementDataList.Where(x => x != EditData).ToList().ForEach(x => x.SetOriginalValues());

        if (SelectionElementManager.SelectionActive(EditData.DataElement))
            EditData.DataElement.UpdateElement();

        UpdateEditor();
    }

    public void CancelEdit()
    {
        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}
