using UnityEngine;

public interface IDisplay
{
    void SetDisplay();
    void ClearDisplay();
    void CloseDisplay();

    IDataController DataController { get; set; }
}
