using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IDataController
{
    ICollection dataList { get; set; }
    Enums.DataType data_type { get; }

    void GetData(List<int> id_list);
    void InitializeController();
}
