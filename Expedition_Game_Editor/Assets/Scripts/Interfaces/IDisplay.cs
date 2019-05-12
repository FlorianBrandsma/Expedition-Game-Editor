using UnityEngine;

public interface IDisplay
{
    void ClearDisplay();
    void CloseDisplay();

    IDataController DataController { get; set; }
}
