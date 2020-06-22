using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GeneralDataElement : GeneralData, IDataElement
{
    public DataElement DataElement { get; set; }

    public GeneralDataElement() : base()
    {
        DataType = Enums.DataType.None;
    }

    public void Update() { }

    public void UpdateSearch() { }

    public void UpdateIndex() { }

    public void SetOriginalValues() { }

    public void GetOriginalValues() { }

    public void ClearChanges() { }

    public bool Changed { get { return false; } }

    public IDataElement Clone()
    {
        var dataElement = new GeneralDataElement();

        CloneGeneralData(dataElement);
        
        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        SetOriginalValues();
    }
}
