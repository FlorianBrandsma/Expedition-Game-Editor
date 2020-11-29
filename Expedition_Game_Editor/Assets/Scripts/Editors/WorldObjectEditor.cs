using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WorldObjectEditor : MonoBehaviour, IEditor
{
    private WorldObjectData worldObjectData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataList.Where(x => x.Id == worldObjectData.Id).FirstOrDefault(); } }

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
        get { return worldObjectData.Id; }
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
    }

    public string ModelIconPath
    {
        get { return ((WorldObjectData)EditData).ModelIconPath; }
    }

    public float Height
    {
        get { return ((WorldObjectData)EditData).Height; }
    }

    public float Width
    {
        get { return ((WorldObjectData)EditData).Width; }
    }

    public float Depth
    {
        get { return ((WorldObjectData)EditData).Depth; }
    }
    #endregion

    public void InitializeEditor()
    {
        worldObjectData = (WorldObjectData)ElementData.Clone();
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
