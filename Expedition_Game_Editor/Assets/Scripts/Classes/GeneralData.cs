using UnityEngine;
using System;
using System.Collections;

public class GeneralData
{
    public Enums.DataType dataType;

    private int id;
    private int index;

    public bool changedIndex;

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
    
    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), dataType); } }

    public GeneralData()
    {
        dataType = Enums.DataType.None;

        Id = 0;
        Index = 0;
    }

    public GeneralData(Enums.DataType dataType, int id, int index)
    {
        this.dataType = dataType;

        this.Id = id;
        Index = index;
    }

    public bool Equals(GeneralData data)
    {
        if (dataType != data.dataType)
            return false;

        if (Id != data.Id)
            return false;

        return true;
    }

    public GeneralData Copy()
    {
        return new GeneralData(dataType, Id, Index);
    }
}
