using UnityEngine;
using System.Collections;

public interface IDisplayManager
{
    RectTransform RectTransform { get; }
    IDisplay Display { get; }

    void UpdateData();
    void CorrectPosition(IDataElement data);
}
