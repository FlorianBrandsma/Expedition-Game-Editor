using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IOrganizer
{
    void InitializeOrganizer();

    void SetProperties(ListProperties listProperties);

    void SetListSize();
    Vector2 GetListSize(List<ElementData> data_list, bool exact);

    float element_size { get; set; }

    void SetRows(List<ElementData> data_list);
    void ResetRows(List<ElementData> filter);

    SelectionElement GetElement(int index);

    void CloseList();
}
