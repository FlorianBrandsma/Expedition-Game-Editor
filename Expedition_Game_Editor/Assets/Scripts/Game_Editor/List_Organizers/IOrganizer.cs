using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IOrganizer
{
    void InitializeOrganizer();
    void SetProperties(ListProperties listProperties);
    void SetListSize(float new_size);
    void SetRows(List<int> id_list);
    void ResetRows(List<int> filter);
    SelectionElement GetElement(int index);
    Vector2 GetListSize(List<int> id_list, bool exact);
    void CloseList();
}
