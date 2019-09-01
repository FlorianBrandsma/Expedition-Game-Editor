using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class ObjectOrganizer : MonoBehaviour, IOrganizer
{
    private List<IPoolable> poolObjects = new List<IPoolable>();

    private CameraManager cameraManager;
    private IDisplayManager DisplayManager { get { return GetComponent<IDisplayManager>(); } }
    private ObjectProperties objectProperties;

    private IDataController dataController;

    public void InitializeOrganizer()
    {
        cameraManager = (CameraManager)DisplayManager;

        dataController = DisplayManager.Display.DataController;
    }

    public void InitializeProperties()
    {
        objectProperties = (ObjectProperties)DisplayManager.Display.Properties;
    }

    public void SetData()
    {
        if(dataController != null)
            SetData(dataController.DataList);
    }

    public void SetData(List<IDataElement> list)
    {
        foreach (IDataElement data in list)
        {
            var objectGraphicData = (ObjectGraphicDataElement)data;

            if (objectGraphicData.id == 1) continue;

            ObjectGraphic prefab = Resources.Load<ObjectGraphic>(objectGraphicData.Path);

            ObjectGraphic graphic = (ObjectGraphic)PoolManager.SpawnObject(objectGraphicData.id, prefab.PoolType, prefab);

            poolObjects.Add(graphic);

            graphic.transform.SetParent(cameraManager.content, false);

            //Debugging
            GeneralData generalData = (GeneralData)data;
            graphic.name = generalData.DebugName + generalData.id;
            //

            SetGraphic(graphic);
        }
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

    void SetGraphic(ObjectGraphic graphic)
    {
        graphic.transform.localPosition = new Vector2(  graphic.transform.localPosition.x, 
                                                        objectProperties.pivotPosition[(int)graphic.pivot]);

        graphic.gameObject.SetActive(true);
    }

    private void ClearCamera()
    {
        foreach (IPoolable poolObject in poolObjects)
        {
            switch(poolObject.PoolType)
            {
                case PoolManager.PoolType.ObjectGraphic: ((ObjectGraphic)poolObject).gameObject.SetActive(false); break;
            }
        }

        poolObjects.Clear();
    }

    private void CloseCamera()
    {

    }

    public void ClearOrganizer()
    {
        ClearCamera();
    }

    public void CloseOrganizer()
    {
        CloseCamera();

        DestroyImmediate(this);
    }
}
