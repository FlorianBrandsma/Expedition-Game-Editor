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

    private void SetGraphic(ObjectGraphic objectGraphic)
    {
        objectGraphic.transform.localPosition = new Vector2(objectGraphic.transform.localPosition.x, 
                                                            objectProperties.pivotPosition[(int)objectGraphic.pivot]);

        objectGraphic.transform.localEulerAngles = objectGraphic.previewRotation;
        objectGraphic.transform.localScale = objectGraphic.previewScale;

        //objectGraphic.transform.localEulerAngles = new Vector3(90, 0, 0);

        //objectGraphic.transform.localScale = new Vector3(70, 70, 70);

        objectGraphic.gameObject.SetActive(true);
    }
    
    private void CloseGraphic(ObjectGraphic objectGraphic)
    {
        objectGraphic.transform.localPosition = new Vector3(0, 0, 0);

        objectGraphic.transform.localEulerAngles = new Vector3(0, 0, 0);

        objectGraphic.transform.localScale = new Vector3(1, 1, 1);
    }

    private void ClearCamera()
    {
        poolObjects.ForEach(x => CloseGraphic((ObjectGraphic)x));

        poolObjects.ForEach(x => x.ClosePoolable());

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
