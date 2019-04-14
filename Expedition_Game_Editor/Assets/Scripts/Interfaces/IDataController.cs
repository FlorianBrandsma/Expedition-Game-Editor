using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IDataController
{
    SegmentController segmentController { get; }

    ICollection data_list { get; set; }
    Enums.DataType data_type { get; }

    IDisplay display { get; }

    void InitializeController();
}
