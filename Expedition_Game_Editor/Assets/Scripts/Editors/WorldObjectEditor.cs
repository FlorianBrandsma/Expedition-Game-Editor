using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WorldObjectEditor : MonoBehaviour, IEditor
{
    private WorldObjectData worldObjectData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == worldObjectData.Id).FirstOrDefault(); } }

    private PathController PathController           { get { return GetComponent<PathController>(); } }
    public List<SegmentController> EditorSegments   { get; } = new List<SegmentController>();

    public bool Loaded                              { get; set; }

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
        get { return worldObjectData.Id; }
    }

    public int ModelId
    {
        get { return worldObjectData.ModelId; }
        set
        {
            worldObjectData.ModelId = value;

            DataList.ForEach(x => ((WorldObjectElementData)x).ModelId = value);
        }
    }

    public int TerrainId
    {
        get { return worldObjectData.TerrainId; }
        set
        {
            worldObjectData.TerrainId = value;

            DataList.ForEach(x => ((WorldObjectElementData)x).TerrainId = value);
        }
    }

    public int TerrainTileId
    {
        get { return worldObjectData.TerrainTileId; }
        set
        {
            worldObjectData.TerrainTileId = value;

            DataList.ForEach(x => ((WorldObjectElementData)x).TerrainTileId = value);
        }
    }

    public float PositionX
    {
        get { return worldObjectData.PositionX; }
        set
        {
            worldObjectData.PositionX = value;

            DataList.ForEach(x => ((WorldObjectElementData)x).PositionX = value);
        }
    }

    public float PositionY
    {
        get { return worldObjectData.PositionY; }
        set
        {
            worldObjectData.PositionY = value;

            DataList.ForEach(x => ((WorldObjectElementData)x).PositionY = value);
        }
    }

    public float PositionZ
    {
        get { return worldObjectData.PositionZ; }
        set
        {
            worldObjectData.PositionZ = value;

            DataList.ForEach(x => ((WorldObjectElementData)x).PositionZ = value);
        }
    }

    public int RotationX
    {
        get { return worldObjectData.RotationX; }
        set
        {
            worldObjectData.RotationX = value;

            DataList.ForEach(x => ((WorldObjectElementData)x).RotationX = value);
        }
    }

    public int RotationY
    {
        get { return worldObjectData.RotationY; }
        set
        {
            worldObjectData.RotationY = value;

            DataList.ForEach(x => ((WorldObjectElementData)x).RotationY = value);
        }
    }

    public int RotationZ
    {
        get { return worldObjectData.RotationZ; }
        set
        {
            worldObjectData.RotationZ = value;

            DataList.ForEach(x => ((WorldObjectElementData)x).RotationZ = value);
        }
    }

    public float Scale
    {
        get { return worldObjectData.Scale; }
        set
        {
            worldObjectData.Scale = value;

            DataList.ForEach(x => ((WorldObjectElementData)x).Scale = value);
        }
    }

    public string ModelName
    {
        get { return ((WorldObjectData)EditData).ModelName; }
        set
        {
            worldObjectData.ModelName = value;

            DataList.ForEach(x => ((WorldObjectElementData)x).ModelName = value);
        }
    }

    public string ModelPath
    {
        get { return ((WorldObjectData)EditData).ModelPath; }
        set
        {
            worldObjectData.ModelPath = value;

            DataList.ForEach(x => ((WorldObjectElementData)x).ModelPath = value);
        }
    }

    public string ModelIconPath
    {
        get { return ((WorldObjectData)EditData).ModelIconPath; }
        set
        {
            worldObjectData.ModelIconPath = value;

            DataList.ForEach(x => ((WorldObjectElementData)x).ModelIconPath = value);
        }
    }

    public float Height
    {
        get { return ((WorldObjectData)EditData).Height; }
        set
        {
            worldObjectData.Height = value;

            DataList.ForEach(x => ((WorldObjectElementData)x).Height = value);
        }
    }

    public float Width
    {
        get { return ((WorldObjectData)EditData).Width; }
        set
        {
            worldObjectData.Width = value;

            DataList.ForEach(x => ((WorldObjectElementData)x).Width = value);
        }
    }

    public float Depth
    {
        get { return ((WorldObjectData)EditData).Depth; }
        set
        {
            worldObjectData.Depth = value;

            DataList.ForEach(x => ((WorldObjectElementData)x).Depth = value);
        }
    }
    #endregion

    public void InitializeEditor()
    {
        worldObjectData = (WorldObjectData)ElementData.Clone();
    }
    
    public void ResetEditor()
    {
        ModelId         = worldObjectData.ModelId;
        TerrainId       = worldObjectData.TerrainId;
        TerrainTileId   = worldObjectData.TerrainTileId;

        PositionX       = worldObjectData.PositionX;
        PositionY       = worldObjectData.PositionY;
        PositionZ       = worldObjectData.PositionZ;

        RotationX       = worldObjectData.RotationX;
        RotationY       = worldObjectData.RotationY;
        RotationZ       = worldObjectData.RotationZ;

        Scale           = worldObjectData.Scale;

        ModelName       = worldObjectData.ModelName;

        ModelPath       = worldObjectData.ModelPath;
        ModelIconPath   = worldObjectData.ModelIconPath;

        Height          = worldObjectData.Height;
        Width           = worldObjectData.Width;
        Depth           = worldObjectData.Depth;
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

    public bool Addable()
    {
        return true;
    }

    public bool Applicable()
    {
        return ElementDataList.Any(x => x.Changed);
    }

    public bool Removable()
    {
        return true;
    }

    public void ApplyChanges(DataRequest dataRequest)
    {
        ApplyWorldObjectChanges(dataRequest);
    }

    private void ApplyWorldObjectChanges(DataRequest dataRequest)
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
                AddWorldObject(dataRequest);
                break;

            case Enums.ExecuteType.Update:
                UpdateWorldObject(dataRequest);
                break;

            case Enums.ExecuteType.Remove:
                RemoveWorldObject(dataRequest);
                break;
        }
    }

    private void AddWorldObject(DataRequest dataRequest)
    {
        var tempData = EditData;

        EditData.Add(dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
            worldObjectData.Id = tempData.Id;
    }

    private void UpdateWorldObject(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);
    }

    private void RemoveWorldObject(DataRequest dataRequest)
    {
        EditData.Remove(dataRequest);
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
                ElementDataList.Where(x => x != EditData).ToList().ForEach(x => x.SetOriginalValues());
                ResetExecuteType();
                UpdateEditor();
                break;
        }
    }

    private void ResetExecuteType()
    {
        ElementDataList.Where(x => x.Id != -1).ToList().ForEach(x => x.ExecuteType = Enums.ExecuteType.Update);
    }

    public void CancelEdit()
    {
        ResetExecuteType();

        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}
