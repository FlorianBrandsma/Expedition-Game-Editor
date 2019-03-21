using UnityEngine;

public interface IDisplay
{
    //DataList dataList { get; set; }

    void InitializeProperties();
    void SetDisplay();
    void CloseDisplay();

    IDataController dataController { get; }
}
