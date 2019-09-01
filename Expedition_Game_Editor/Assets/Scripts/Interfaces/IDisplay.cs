using UnityEngine;

public interface IDisplay
{
    void SetDisplay();
    void ClearDisplay();
    void CloseDisplay();

    IProperties Properties { get; }
    IDataController DataController { get; set; }
}
