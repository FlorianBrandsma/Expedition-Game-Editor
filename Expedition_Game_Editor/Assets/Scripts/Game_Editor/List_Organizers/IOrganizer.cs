using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IOrganizer
{
    void SetListSize(float new_size);

    void SetRows();

    void SelectElement(int id);

    Vector2 GetListSize();

    void CloseList();
}
