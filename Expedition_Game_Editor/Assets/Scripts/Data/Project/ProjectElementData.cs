using UnityEngine;
using System;

public class ProjectElementData : ProjectData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public ProjectData OriginalData                 { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Project; } }
    
    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedIconId
    {
        get { return IconId != OriginalData.IconId; }
    }
    
    public bool ChangedName
    {
        get { return Name != OriginalData.Name; }
    }

    public bool ChangedDescription
    {
        get { return Description != OriginalData.Description; }
    }

    public bool Changed
    {
        get { return ChangedIconId || ChangedName || ChangedDescription; }
    }
    #endregion

    public ProjectElementData() { }

    public void Add(DataRequest dataRequest)
    {
        ProjectDataManager.AddData(this, dataRequest);
    }

    public void Update(DataRequest dataRequest)
    {
        ProjectDataManager.UpdateData(this, dataRequest);
    }
    
    public void Remove(DataRequest dataRequest)
    {
        ProjectDataManager.RemoveData(this, dataRequest);
    }
    
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
        var data = new ProjectElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        data.ExecuteType = ExecuteType;

        base.Clone(data);

        return data;
    }
}
