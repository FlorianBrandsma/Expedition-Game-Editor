using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ScenePropEditor : MonoBehaviour, IEditor
{
    private ScenePropData scenePropData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataList.Where(x => x.Id == scenePropData.Id).FirstOrDefault(); } }

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

    public int ModelId
    {
        get { return scenePropData.ModelId; }
        set
        {
            scenePropData.ModelId = value;

            DataList.ForEach(x => ((ScenePropElementData)x).ModelId = value);
        }
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

    public float Scale
    {
        get { return scenePropData.Scale; }
        set
        {
            scenePropData.Scale = value;

            DataList.ForEach(x => ((ScenePropElementData)x).Scale = value);
        }
    }

    public string ModelPath
    {
        get { return ((ScenePropData)EditData).ModelPath; }
        set
        {
            scenePropData.ModelPath = value;

            DataList.ForEach(x => ((ScenePropElementData)x).ModelPath = value);
        }
    }

    public string ModelIconPath
    {
        get { return ((ScenePropData)EditData).ModelIconPath; }
        set
        {
            scenePropData.ModelIconPath = value;

            DataList.ForEach(x => ((ScenePropElementData)x).ModelIconPath = value);
        }
    }

    public string ModelName
    {
        get { return ((ScenePropData)EditData).ModelName; }
        set
        {
            scenePropData.ModelName = value;

            DataList.ForEach(x => ((ScenePropElementData)x).ModelName = value);
        }
    }
    
    public float Height
    {
        get { return ((ScenePropData)EditData).Height; }
        set
        {
            scenePropData.Height = value;

            DataList.ForEach(x => ((ScenePropElementData)x).Height = value);
        }
    }

    public float Width
    {
        get { return ((ScenePropData)EditData).Width; }
        set
        {
            scenePropData.Width = value;

            DataList.ForEach(x => ((ScenePropElementData)x).Width = value);
        }
    }

    public float Depth
    {
        get { return ((ScenePropData)EditData).Depth; }
        set
        {
            scenePropData.Depth = value;

            DataList.ForEach(x => ((ScenePropElementData)x).Depth = value);
        }
    }
    #endregion

    public void InitializeEditor()
    {
        scenePropData = (ScenePropData)ElementData.Clone();
    }
    
    public void ResetEditor()
    {
        ModelId         = scenePropData.ModelId;
        TerrainId       = scenePropData.TerrainId;
        TerrainTileId   = scenePropData.TerrainTileId;

        PositionX       = scenePropData.PositionX;
        PositionY       = scenePropData.PositionY;
        PositionZ       = scenePropData.PositionZ;

        RotationX       = scenePropData.RotationX;
        RotationY       = scenePropData.RotationY;
        RotationZ       = scenePropData.RotationZ;

        Scale           = scenePropData.Scale;

        ModelPath       = scenePropData.ModelPath;
        ModelIconPath   = scenePropData.ModelIconPath;

        ModelName       = scenePropData.ModelName;

        Height          = scenePropData.Height;
        Width           = scenePropData.Width;
        Depth           = scenePropData.Depth;
    }

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

    public void ApplyChanges(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);

        ElementDataList.Where(x => x != EditData).ToList().ForEach(x => x.SetOriginalValues());

        if (SelectionElementManager.SelectionActive(EditData.DataElement))
            EditData.DataElement.UpdateElement();

        UpdateEditor();
    }

    public void FinalizeChanges()
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
            case Enums.ExecuteType.Remove:
                RenderManager.PreviousPath();
                break;
            case Enums.ExecuteType.Update:
                ResetExecuteType();
                UpdateEditor();
                break;
        }
    }

    private void ResetExecuteType()
    {
        ElementDataList.Where(x => x.Id != 0).ToList().ForEach(x => x.ExecuteType = Enums.ExecuteType.Update);
    }

    public void CancelEdit()
    {
        ResetExecuteType();

        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}
