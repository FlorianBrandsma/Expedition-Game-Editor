using UnityEngine;
using System.Collections;

public interface IDisplayManager
{
    SelectionElement SelectedElement { get; set; }
    RectTransform RectTransform { get; }
    IDisplay Display { get; }

    void UpdateData();
    void CorrectPosition(SelectionElement selectionElement);
}
