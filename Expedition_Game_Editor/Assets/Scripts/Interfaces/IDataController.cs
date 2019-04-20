using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IDataController
{
    ICollection data_list { get; set; }
    Enums.DataType data_type { get; }

    void GetData();
    void InitializeController();
}
