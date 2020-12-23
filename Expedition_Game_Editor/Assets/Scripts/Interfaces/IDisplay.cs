using UnityEngine;

public interface IDisplay
{
    void ClearDisplay();
    void CloseDisplay();
    
    Enums.DisplayType DisplayType               { get; }

    IDisplayManager DisplayManager              { get; }
    IProperties Properties                      { get; }
    IDataController DataController              { get; set; }

    SelectionManager.Type SelectionType         { get; }
    SelectionManager.Property SelectionProperty { get; }
    SelectionManager.Property AddProperty       { get; }
    
    bool UniqueSelection                        { get; }
}
