using UnityEngine;
using System;

public class UserElementData : UserData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public UserData OriginalData                    { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.User; } }
    
    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedIconId
    {
        get { return IconId != OriginalData.IconId; }
    }
    
    public bool ChangedUsername
    {
        get { return Username != OriginalData.Username; }
    }

    public bool ChangedEmail
    {
        get { return Email != OriginalData.Email; }
    }

    public bool ChangedPassword
    {
        get { return Password != OriginalData.Password; }
    }

    public bool Changed
    {
        get { return ChangedIconId || ChangedUsername || ChangedEmail || ChangedPassword; }
    }
    #endregion

    public UserElementData() { }

    public void Add(DataRequest dataRequest)
    {
        UserDataManager.AddData(this, dataRequest);
    }

    public void Update(DataRequest dataRequest)
    {
        UserDataManager.UpdateData(this, dataRequest);
    }
    
    public void Remove(DataRequest dataRequest)
    {
        UserDataManager.RemoveData(this, dataRequest);
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
        var data = new UserElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        data.ExecuteType = ExecuteType;

        base.Clone(data);

        return data;
    }
}
