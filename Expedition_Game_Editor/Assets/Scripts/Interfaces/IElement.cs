using UnityEngine;
using System.Collections;

public interface IElement
{
    Color ElementColor { set; }

    void InitializeElement();
    void UpdateElement();
    void SetElement();
    void CloseElement();
}
