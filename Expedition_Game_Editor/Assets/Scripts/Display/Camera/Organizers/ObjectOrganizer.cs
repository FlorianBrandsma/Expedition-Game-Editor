using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Source;

public class ObjectOrganizer : MonoBehaviour, IOrganizer
{
    private CameraManager cameraManager { get { return GetComponent<CameraManager>(); } }
    private ObjectProperties properties;

    static public List<ObjectGraphic> graphicList = new List<ObjectGraphic>();

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
            
            ObjectGraphic graphicPrefab = Resources.Load<ObjectGraphic>(objectGraphicData.Path);

            if (graphicPrefab == null) continue;

            graphicPrefab.id = objectGraphicData.id;

            ObjectGraphic graphic = cameraManager.SpawnGraphic(graphicList, graphicPrefab);
            cameraManager.graphicList.Add(graphic);

            //data.SelectionElement = element;
            graphic.route.data = new Data(dataController, data);

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
        cameraManager.ClearGraphics();
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
