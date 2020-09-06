using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class ObjectOrganizer : MonoBehaviour, IOrganizer
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
        if(DataController != null)
            SetData(DataController.Data.dataList);
    }

    public void SetData(List<IElementData> list)
    {
        foreach (IElementData elementData in list)
        {
            var modelData = (ModelElementData)elementData;

            if (modelData.Id == 1) continue;

            var prefab = Resources.Load<Model>(modelData.Path);
            var model = (Model)PoolManager.SpawnObject(prefab, modelData.Id);

            poolObjects.Add(model);

            model.transform.SetParent(CameraManager.content, false);

            //Debugging
            model.name = elementData.DebugName + elementData.Id;

            SetModel(model);
        }
    }

    public void UpdateData()
    {
        SetData();
    }

    public void ResetData(List<IElementData> filter)
    {
        CloseOrganizer();
        SetData();
    }

    private void SetModel(Model model)
    {
        model.transform.localPosition = new Vector2(model.transform.localPosition.x, 
                                                    ObjectProperties.pivotPosition[(int)model.pivot]);

        model.transform.localEulerAngles = model.previewRotation;
        model.transform.localScale = model.previewScale;
        
        model.gameObject.SetActive(true);
    }
    
    private void CloseModel(Model model)
    {
        model.transform.localPosition = new Vector3(0, 0, 0);
        model.transform.localEulerAngles = new Vector3(0, 0, 0);
        model.transform.localScale = new Vector3(1, 1, 1);
    }

    private void ClearCamera()
    {
        poolObjects.ForEach(x => CloseModel((Model)x));

        poolObjects.ForEach(x => x.ClosePoolable());

        poolObjects.Clear();
    }

    private void CloseCamera() { }

    public void ClearOrganizer()
    {
        poolObjects.ForEach(x => PoolManager.ClosePoolObject(x));
        ClearCamera();
    }

    public void CloseOrganizer()
    {
        CloseCamera();

        DestroyImmediate(this);
    }
}
