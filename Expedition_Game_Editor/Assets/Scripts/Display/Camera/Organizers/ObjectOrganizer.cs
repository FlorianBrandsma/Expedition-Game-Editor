using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class ObjectOrganizer : MonoBehaviour, IOrganizer
{
    private List<IPoolable> poolObjects = new List<IPoolable>();

    private CameraManager cameraManager { get { return GetComponent<CameraManager>(); } }
    private ObjectProperties properties;

    private IDataController dataController;

    public void InitializeOrganizer()
    {
        dataController = cameraManager.cameraProperties.DataController;
    }

    public void InitializeProperties()
    {
        properties = cameraManager.cameraProperties.GetComponent<ObjectProperties>();
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

            ObjectGraphic graphic = (ObjectGraphic)ObjectManager.SpawnObject(objectGraphicData.id, prefab.PoolType, prefab);

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
                                                        properties.pivotPosition[(int)graphic.pivot]);

        graphic.gameObject.SetActive(true);
    }

    private void ClearCamera()
    {
        foreach (IPoolable poolObject in poolObjects)
        {
            switch(poolObject.PoolType)
            {
                case ObjectManager.PoolType.ObjectGraphic: ((ObjectGraphic)poolObject).gameObject.SetActive(false); break;
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
