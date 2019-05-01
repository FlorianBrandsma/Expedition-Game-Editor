using UnityEngine;

public interface IDisplay
{
    void InitializeProperties();
    void SetDisplay();
    void ClearDisplay();
    void CloseDisplay();

    SegmentController SegmentController { get; }
}
