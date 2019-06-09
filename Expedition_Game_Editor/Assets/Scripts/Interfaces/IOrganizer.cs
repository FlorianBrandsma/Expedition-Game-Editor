using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IOrganizer
{
    void InitializeOrganizer();

    void InitializeProperties();
    void UpdateData();
    void SetData();
    void ResetData(List<IDataElement> filter);
    void ClearOrganizer();
    void CloseOrganizer();   
}
