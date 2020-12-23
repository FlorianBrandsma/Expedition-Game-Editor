using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectionElement
{
    IElement Element { get; }
    void InitializeElement();
    void InitializeElement(SelectionManager.Type selectionType, SelectionManager.Property selectionProperty, SelectionManager.Property addProperty, bool uniqueSelection);
    void UpdateElement();
    void CancelSelection();
}
