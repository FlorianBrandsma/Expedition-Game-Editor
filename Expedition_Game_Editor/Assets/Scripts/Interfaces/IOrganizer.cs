using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IOrganizer
{
    void InitializeOrganizer();

    void SetProperties();
    void UpdateData();
    void SetData();
    void ResetData(ICollection filter);
    void ClearOrganizer();
    void CloseOrganizer();   
}
