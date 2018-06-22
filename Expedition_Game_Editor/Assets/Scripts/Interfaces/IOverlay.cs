using UnityEngine;
using System.Collections;

public interface IOverlay
{
    void InitializeOverlay(RectTransform main_list, RectTransform parent_list);
    void SetOverlay();
    void UpdateOverlay();
    void CloseOverlay();
}
