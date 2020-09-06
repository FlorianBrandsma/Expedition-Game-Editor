using UnityEngine;
using System;

public class PlayerSaveData : PlayerSaveBaseData
{
    //public string interactableName;

    public DateTime SaveTime    { get; set; }

    //Testing purposes
    public DateTime TestTime    { get; set; }
    public TimeSpan PassedTime  { get { return SaveTime - TestTime; } }
    public float PassedSeconds  { get { return PassedTime.Seconds; } }
    //----------------

    public override void GetOriginalValues(PlayerSaveData originalData)
    {
        SaveTime = originalData.SaveTime;

        TestTime = originalData.TestTime;
        
        base.GetOriginalValues(originalData);
    }

    public PlayerSaveData Clone()
    {
        var data = new PlayerSaveData();

        data.SaveTime = SaveTime;

        data.TestTime = TestTime;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(PlayerSaveElementData elementData)
    {
        elementData.SaveTime = SaveTime;

        elementData.TestTime = TestTime;

        base.Clone(elementData);
    }
}
