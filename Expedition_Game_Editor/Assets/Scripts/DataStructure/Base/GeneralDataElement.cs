using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GeneralDataElement : GeneralData, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public GeneralDataElement() : base() { }

    public int Id { get; set; }

    public void Update()
    {
        SetOriginalValues();
    }

    public void UpdateSearch() { }

    public void SetOriginalValues()
    {
        ClearChanges();
    }

    public new void GetOriginalValues() { }

    public void ClearChanges() { }

    public bool Changed { get { return false; } }
}
