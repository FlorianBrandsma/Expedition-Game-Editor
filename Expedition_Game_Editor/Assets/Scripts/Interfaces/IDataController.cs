﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IDataController
{
    SegmentController SegmentController { get; }

    ICollection DataList            { get; set; }
    Enums.DataType DataType         { get; }
    IEnumerable SearchParameters    { get; set; }

    void InitializeController();
    void GetData(IEnumerable searchParameters);
    void SetData(SelectionElement searchElement, Data resultData);
}
