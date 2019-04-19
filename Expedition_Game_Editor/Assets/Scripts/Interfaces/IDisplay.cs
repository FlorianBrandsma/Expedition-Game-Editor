using UnityEngine;

public interface IDisplay
{
    void InitializeProperties();
    void SetDisplay();
    void CloseDisplay();

    SegmentController segmentController { get; }
}
