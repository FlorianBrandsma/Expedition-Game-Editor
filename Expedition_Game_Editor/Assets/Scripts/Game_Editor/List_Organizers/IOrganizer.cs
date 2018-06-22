using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IOrganizer
{
    void InitializeOrganizer(Path select_path, Path edit_path);

    void SetProperties(ListProperties listProperties);

    void SetListSize(float new_size);

    void SetRows();

    void SelectElement(int id);

    Vector2 GetListSize(bool exact);

    void CloseList();
}
