using UnityEngine;

public interface IDisplay
{
    void ClearDisplay();
    void CloseDisplay();

    IProperties Properties { get; }
    IDataController DataController { get; set; }

    SelectionManager.Property SelectionProperty { get; }
    SelectionManager.Type SelectionType { get; }
}
