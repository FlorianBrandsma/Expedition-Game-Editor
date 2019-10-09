using UnityEngine;
using System.Collections;

public interface IElement
{
    Color ElementColor { set; }

    void InitializeElement();
    void SetElement();
    void CloseElement();
}
