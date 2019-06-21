using UnityEngine;
using System;
using System.Collections;

public class GeneralData
{
    public Enums.DataType dataType;

    public int id;
    public int index;
 
    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), dataType); } }

    public GeneralData()
    {
        dataType = Enums.DataType.None;

        id = 0;
        index = 0;
    }

    public GeneralData(Enums.DataType dataType, int id, int index)
    {
        this.dataType = dataType;

        this.id = id;
        this.index = index;
    }

    public bool Equals(GeneralData data)
    {
        if (dataType != data.dataType)
            return false;

        if (id != data.id)
            return false;

        return true;
    }

    public GeneralData Copy()
    {
        return new GeneralData(dataType, id, index);
    }
}
