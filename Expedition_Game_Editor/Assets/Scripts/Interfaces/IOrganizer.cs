﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IOrganizer
{
    void InitializeOrganizer();
    void SetProperties(ListProperties listProperties);
    void SetListSize(float new_size);
    void SetRows(List<ElementData> data_list);
    void ResetRows(List<ElementData> filter);
    SelectionElement GetElement(int index);
    Vector2 GetListSize(List<ElementData> data_list, bool exact);

    void CloseList();
}