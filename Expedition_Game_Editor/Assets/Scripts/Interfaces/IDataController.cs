using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IDataController
{
    ICollection DataList    { get; set; }
    Enums.DataType DataType { get; }

    void GetData(SearchData searchData);
    void InitializeController();
}
