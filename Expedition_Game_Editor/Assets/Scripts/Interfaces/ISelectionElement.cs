using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectionElement
{
    void InitializeElement();
    void InitializeElement(SelectionManager.Type selectionType, SelectionManager.Property selectionProperty);
    void UpdateElement();
    void CancelSelection();
}
