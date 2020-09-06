using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemEditor : MonoBehaviour, IEditor
{
    public ItemData itemData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataList.Where(x => x.Id == itemData.Id).FirstOrDefault(); } }

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

    public float ModelHeight
    {
        get { return itemData.Height; }
        set
        {
            itemData.Height = value;

            DataList.ForEach(x => ((ItemElementData)x).Height = value);
        }
    }

    public float ModelWidth
    {
        get { return itemData.Height; }
        set
        {
            itemData.Height = value;

            DataList.ForEach(x => ((ItemElementData)x).Width = value);
        }
    }

    public float ModelDepth
    {
        get { return itemData.Height; }
        set
        {
            itemData.Height = value;

            DataList.ForEach(x => ((ItemElementData)x).Depth = value);
        }
    }

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

        ModelHeight = itemData.Height;
        ModelWidth = itemData.Width;
        ModelDepth = itemData.Depth;
    }
    
    public void UpdateEditor()
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
