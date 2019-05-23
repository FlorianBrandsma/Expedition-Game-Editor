using UnityEngine;
using System.Collections;
using Source;

public class BackgroundManager : MonoBehaviour
{
    public GameObject shadow;

    private CameraManager cameraManager;

    private ObjectProperties objectProperties;

    public void InitializeBackground(CameraManager cameraManager)
    {
        this.cameraManager = cameraManager;
        
        objectProperties = cameraManager.cameraProperties.GetComponent<ObjectProperties>();
    }

    public void SetBackground(ObjectGraphic graphic)
    {
        if (objectProperties == null) return;

        if (objectProperties.castShadow && graphic.id > 0)
            shadow.SetActive(true);
    }

    public void CloseBackground()
    {
        if (objectProperties == null) return;

        //if (objectProperties.castShadow)
        shadow.SetActive(false);
    }
}
