using UnityEngine;
using System;
using System.Collections;

public class GeneralData
{
    private Enums.DataType dataType;
    private int id;
    private int index;

    public bool changedIndex;

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

    public GeneralData()
    {
        DataType = Enums.DataType.None;

        Id = 0;
        Index = 0;
    }

    public GeneralData(Enums.DataType dataType, int Id, int index)
    {
        this.DataType = dataType;

        this.Id = Id;
        Index = index;
    }

    public bool Equals(GeneralData data)
    {
        if (DataType != data.DataType)
            return false;

        if (Id != data.Id)
            return false;

        return true;
    }

    public GeneralData Copy()
    {
        return new GeneralData(DataType, Id, Index);
    }
}
