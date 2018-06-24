using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IOrganizer
{
    void InitializeOrganizer(Path select_path, Path edit_path);
    void SetProperties(ListProperties listProperties);
    void SetListSize(float new_size);
    void SetRows(List<int> id_list);
    void ResetRows(List<int> filter);
    void SelectElement(int id);
    Vector2 GetListSize(List<int> id_list, bool exact);
    void CloseList();
}
