using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemEditor : MonoBehaviour, IEditor
{
    private ItemData itemData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == itemData.Id).FirstOrDefault(); } }

    private PathController PathController           { get { return GetComponent<PathController>(); } }
    public List<SegmentController> EditorSegments   { get; } = new List<SegmentController>();

    public bool Loaded { get; set; }

    public List<IElementData> DataList
    {
        get { return new List<IElementData>() { EditData }; }
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
        get { return itemData.Id; }
    }
    
    public int ModelId
    {
        get { return itemData.ModelId; }
        set
        {
            itemData.ModelId = value;

            DataList.ForEach(x => ((ItemElementData)x).ModelId = value);
        }
    }

    public int Index
    {
        get { return itemData.Index; }
    }

    public string Name
    {
        get { return itemData.Name; }
        set
        {
            itemData.Name = value;

            DataList.ForEach(x => ((ItemElementData)x).Name = value);
        }
    }

    public string ModelPath
    {
        get { return itemData.ModelPath; }
        set
        {
            itemData.ModelPath = value;

            DataList.ForEach(x => ((ItemElementData)x).ModelPath = value);
        }
    }

    public string ModelIconPath
    {
        get { return itemData.ModelIconPath; }
        set
        {
            itemData.ModelIconPath = value;

            DataList.ForEach(x => ((ItemElementData)x).ModelIconPath = value);
        }
    }

    public float Height
    {
        get { return itemData.Height; }
        set
        {
            itemData.Height = value;

            DataList.ForEach(x => ((ItemElementData)x).Height = value);
        }
    }

    public float Width
    {
        get { return itemData.Width; }
        set
        {
            itemData.Width = value;

            DataList.ForEach(x => ((ItemElementData)x).Width = value);
        }
    }

    public float Depth
    {
        get { return itemData.Depth; }
        set
        {
            itemData.Depth = value;

            DataList.ForEach(x => ((ItemElementData)x).Depth = value);
        }
    }
    #endregion

    public void InitializeEditor()
    {
        itemData = (ItemData)ElementData.Clone();
    }

    public void OpenEditor()
    {
        ModelId = itemData.ModelId;

        Name = itemData.Name;
        
        ModelPath = itemData.ModelPath;
        ModelIconPath = itemData.ModelIconPath;

        Height = itemData.Height;
        Width = itemData.Width;
        Depth = itemData.Depth;
    }
    
    public void UpdateEditor()
    {
        PathController.layoutSection.SetActionButtons();
    }

    public bool Changed()
    {
        return ElementDataList.Any(x => x.Changed);
    }

    public void ApplyChanges(DataRequest dataRequest)
    {
        ApplyItemChanges(dataRequest);
    }

    private void ApplyItemChanges(DataRequest dataRequest)
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
                AddItem(dataRequest);
                break;

            case Enums.ExecuteType.Update:
                UpdateItem(dataRequest);
                break;

            case Enums.ExecuteType.Remove:
                RemoveItem(dataRequest);
                break;
        }
    }

    private void AddItem(DataRequest dataRequest)
    {
        var tempData = EditData;

        EditData.Add(dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
            itemData.Id = tempData.Id;
    }

    private void UpdateItem(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);
    }

    private void RemoveItem(DataRequest dataRequest)
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
                UpdateEditor();
                break;
        }
    }

    public void CancelEdit()
    {
        ElementDataList.ForEach(x =>
        {
            x.ExecuteType = Enums.ExecuteType.Update;
            x.ClearChanges();
        });
        
        Loaded = false;
    }

    public void CloseEditor() { }
}
