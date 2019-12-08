using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EditorStatusIcon : MonoBehaviour
{
    public StatusIconManager.StatusIconType statusIconType;

	public Camera cam;
    public IDataElement targetDataElement;
    public Transform target;
    public Vector3 screenPos;
    public RectTransform parentRect;
    float multiplier;

    #region Data Variables
    private float height, width, depth;
    private float scaleMultiplier;
    #endregion
    
    public RectTransform RectTransform { get { return GetComponent<RectTransform>(); } }
	public RawImage RawImage { get { return GetComponent<RawImage>(); } }
	
	public void UpdatePosition()
	{
        InitializeData();

        multiplier = (((parentRect.rect.width / (float)Screen.width)) * 2f) / ((parentRect.rect.width / EditorManager.UI.rect.width) * 2);
        
        switch (statusIconType)
        {
            case StatusIconManager.StatusIconType.Selection:    UpdateSelectionPosition();  break;
            case StatusIconManager.StatusIconType.Lock:         UpdateLockPosition();       break;
        }
    }

    private void InitializeData()
    {
        switch(targetDataElement.DataType)
        {
            case Enums.DataType.SceneInteractable:  InitializeSceneInteractableData();  break;
            case Enums.DataType.Interaction:        InitializeInteractionData();        break;
            case Enums.DataType.SceneObject:        InitializeSceneObjectData();        break;
        }
    }

    private void InitializeSceneInteractableData()
    {
        var sceneInteractableData = (SceneInteractableDataElement)targetDataElement;

        height = sceneInteractableData.height * sceneInteractableData.scaleMultiplier;
        width  = sceneInteractableData.width  * sceneInteractableData.scaleMultiplier;
        depth  = sceneInteractableData.depth  * sceneInteractableData.scaleMultiplier;
    }

    private void InitializeInteractionData()
    {
        var interactionData = (InteractionDataElement)targetDataElement;

        height = interactionData.height * interactionData.ScaleMultiplier;
        width  = interactionData.width  * interactionData.ScaleMultiplier;
        depth  = interactionData.depth  * interactionData.ScaleMultiplier;
    }

    private void InitializeSceneObjectData()
    {
        var sceneObjectData = (SceneObjectDataElement)targetDataElement;

        height = sceneObjectData.height * sceneObjectData.ScaleMultiplier;
        width  = sceneObjectData.width  * sceneObjectData.ScaleMultiplier;
        depth  = sceneObjectData.depth  * sceneObjectData.ScaleMultiplier;
    }

    private void UpdateSelectionPosition()
    {
        screenPos = cam.WorldToScreenPoint(new Vector3(target.position.x, target.position.y + height, cam.transform.position.z < target.position.z ? target.position.z : cam.transform.position.z)) * multiplier;
        
        var heightCap = (parentRect.rect.height / 2 - RectTransform.rect.height);
        var widthCap  = (parentRect.rect.width  / 2 - RectTransform.rect.width);
        
        transform.localPosition = new Vector3(-15 + screenPos.x - (parentRect.rect.width / 2), screenPos.y - (parentRect.rect.height / 2), 0);

        if (transform.localPosition.x > widthCap)
            transform.localPosition = new Vector3(widthCap, transform.localPosition.y, 0);

        if (transform.localPosition.x < -widthCap)
            transform.localPosition = new Vector3(-widthCap, transform.localPosition.y, 0);

        if (transform.localPosition.y > heightCap)
            transform.localPosition = new Vector3(transform.localPosition.x, heightCap, 0);

        if (transform.localPosition.y < -heightCap)
            transform.localPosition = new Vector3(transform.localPosition.x, -heightCap, 0);
    }

    private void UpdateLockPosition()
    {
        screenPos = cam.WorldToScreenPoint(new Vector3(target.position.x, target.position.y + (height / 2), cam.transform.position.z < target.position.z ? target.position.z : cam.transform.position.z)) * multiplier;

        transform.localPosition = new Vector3(-15 + screenPos.x - (parentRect.rect.width / 2), screenPos.y - (parentRect.rect.height / 2), 0);
    }
}
