using UnityEngine;
using System;
using System.Collections;

public class GeneralData
{
    private Enums.SelectionStatus selectionStatus;
    private Enums.DataType dataType;

    private int id;
    private int index;

    //Changed
    public bool changedIndex;

    public Enums.SelectionStatus SelectionStatus
    {
        get { return selectionStatus; }
        set { selectionStatus = value; }
    }
    public Enums.DataType DataType
    {
        get { return dataType; }
        set { dataType = value; }
    }

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public int Index
    {
        get { return index; }
        set
        {
            if (value == index) return;

            changedIndex = true;

            index = value;
        }
    }
    
    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    public GeneralData() { }

    public bool Equals(GeneralData data)
    {
        if (DataType != data.DataType)
            return false;

        if (Id != data.Id)
            return false;

        return true;
    }

    public void CloneGeneralData(IDataElement dataElement)
    {
        dataElement.SelectionStatus = SelectionStatus;
        dataElement.DataType = DataType;

        dataElement.Id = Id;
        dataElement.Index = Index;
    }

    public virtual void Copy(IDataElement dataSource)
    {
        var generalDataSource = (GeneralDataElement)dataSource;

        selectionStatus = generalDataSource.selectionStatus;
        dataType = generalDataSource.dataType;

        id = generalDataSource.id;
        index = generalDataSource.index;
    }
}
