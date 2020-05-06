using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOrganizer : MonoBehaviour, IOrganizer
{
    private List<IPoolable> poolObjects = new List<IPoolable>();

    private IDisplayManager DisplayManager      { get { return GetComponent<IDisplayManager>(); } }
    private CameraManager CameraManager         { get { return (CameraManager)DisplayManager; } }

    private CameraProperties CameraProperties   { get { return (CameraProperties)DisplayManager.Display; } }
    private ObjectProperties ObjectProperties   { get { return (ObjectProperties)DisplayManager.Display.Properties; } }

    private IDataController DataController      { get { return DisplayManager.Display.DataController; } }

    public void InitializeOrganizer() { }

    public void SelectData() { }

    public void SetData()
    {
        SetData(DataController.DataList);
    }

    public void SetData(List<IDataElement> list)
    {

    }

    public void UpdateData()
    {
        SetData();
    }

    public void ResetData(List<IDataElement> filter)
    {
        CloseOrganizer();
        SetData();
    }

    public void ClearOrganizer()
    {

    }

    public void CloseOrganizer()
    {
        DestroyImmediate(this);
    }
}
