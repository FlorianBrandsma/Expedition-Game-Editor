using UnityEngine;
using System;

public class IconElementData : IconData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public IconData OriginalData                    { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Icon; } }
    
    public Enums.SelectionStatus SelectionStatus    { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedPath
    {
        get { return Path != OriginalData.Path; }
    }

    public bool Changed
    {
        get
        {
            return ChangedPath;
        }
    }
    #endregion

    public void Update() { }

    public void UpdateSearch() { }

    public void SetOriginalValues()
    {
        OriginalData = base.Clone();

        ClearChanges();
    }

    public void ClearChanges()
    {
        if (!Changed) return;

        GetOriginalValues();
    }

    public void GetOriginalValues()
    {
        base.GetOriginalValues(OriginalData);
    }

    public new IElementData Clone()
    {
        var data = new IconElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();
        
        base.Clone(data);

        return data;
    }
}