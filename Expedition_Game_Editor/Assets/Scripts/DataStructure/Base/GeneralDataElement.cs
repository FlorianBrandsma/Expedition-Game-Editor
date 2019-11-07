using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GeneralDataElement : GeneralData, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public GeneralDataElement() : base() { }

    public void Update() { }

    public void UpdateSearch() { }

    public void UpdateIndex() { }

    public void SetOriginalValues() { }

    public void GetOriginalValues() { }

    public void ClearChanges() { }

    public bool Changed { get { return false; } }
}
