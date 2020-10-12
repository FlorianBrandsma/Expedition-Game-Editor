using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ExStatusIcon : MonoBehaviour, IPoolable
{
	public StatusIconOverlay statusIconManager;

	public StatusIconOverlay.StatusIconType statusIconType;

    public Camera cam;
	public IElementData targetElementData;
	public Transform target;
	public Vector3 screenPos;
	public RectTransform parentRect;
    
	#region Data Variables
	private float height, width, depth;
	private float scale;
    #endregion

    public float HorizontalOffset           { get; set; }

    public RectTransform RectTransform      { get { return GetComponent<RectTransform>(); } }
	public RawImage RawImage                { get { return GetComponent<RawImage>(); } }

	public Transform Transform              { get { return GetComponent<Transform>(); } }
	public Enums.ElementType ElementType    { get { return Enums.ElementType.StatusIcon; } }
	public int Id                           { get; set; }
	public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

	public IPoolable Instantiate()
	{
		return Instantiate(this);
	}

	public void SetIconTarget(DataElement dataElement)
	{
		targetElementData = dataElement.ElementData;
		target = dataElement.transform;

		InitializeData();
	}

	public void UpdateIcon()
	{
		if (target == null) return;

		InitializeData();
		UpdatePosition();
	}

	public void UpdatePosition()
	{
		if (target == null) return;
		
		switch (statusIconType)
		{
			case StatusIconOverlay.StatusIconType.Selection:    UpdateSelectionPosition();  break;
			case StatusIconOverlay.StatusIconType.Lock:         UpdateLockPosition();       break;
		}
	}

	private void InitializeData()
	{
		switch(targetElementData.DataType)
		{
			case Enums.DataType.WorldInteractable:      InitializeWorldInteractableData();      break;
			case Enums.DataType.InteractionDestination: InitializeInteractionDestinationData(); break;
			case Enums.DataType.WorldObject:            InitializeWorldObjectData();            break;
			case Enums.DataType.Phase:                  InitializePhaseData();                  break;

			case Enums.DataType.GameWorldInteractable:  InitializeGameWorldInteractable();      break;

			default: Debug.Log("CASE MISSING: " + targetElementData.DataType); break;
		}
	}

	private void InitializeWorldInteractableData()
	{
		var worldInteractableData = (WorldInteractableElementData)targetElementData;

		height = worldInteractableData.Height * worldInteractableData.Scale;
		width  = worldInteractableData.Width  * worldInteractableData.Scale;
		depth  = worldInteractableData.Depth  * worldInteractableData.Scale;
	}

	private void InitializeInteractionDestinationData()
	{
		var interactionDestinationData = (InteractionDestinationElementData)targetElementData;

		height = interactionDestinationData.Height * 1;
		width  = interactionDestinationData.Width  * 1;
		depth  = interactionDestinationData.Depth  * 1;
	}

	private void InitializeWorldObjectData()
	{
		var worldObjectData = (WorldObjectElementData)targetElementData;

		height = worldObjectData.Height * worldObjectData.Scale;
		width  = worldObjectData.Width  * worldObjectData.Scale;
		depth  = worldObjectData.Depth  * worldObjectData.Scale;
	}

	private void InitializePhaseData()
	{
		var phaseData = (PhaseElementData)targetElementData;

		height = phaseData.Height   * phaseData.Scale;
		width = phaseData.Width     * phaseData.Scale;
		depth = phaseData.Depth     * phaseData.Scale;
	}

	private void InitializeGameWorldInteractable()
	{
		var gameWorldInteractableData = (GameWorldInteractableElementData)targetElementData;

		height = gameWorldInteractableData.Height   * gameWorldInteractableData.Scale;
		width = gameWorldInteractableData.Width     * gameWorldInteractableData.Scale;
		depth = gameWorldInteractableData.Depth     * gameWorldInteractableData.Scale;
	}

	private void UpdateSelectionPosition()
	{
		screenPos = cam.WorldToScreenPoint(new Vector3(target.position.x,
													   target.position.y + height,
                                                       cam.transform.position.z < target.position.z ? target.position.z : cam.transform.position.z)) * statusIconManager.Multiplier;
        
		var widthCap = statusIconManager.WidthCap;
		var heightCap = statusIconManager.HeightCap;
		
		transform.localPosition = new Vector3(-HorizontalOffset + screenPos.x - (parentRect.rect.width / 2), screenPos.y - (parentRect.rect.height / 2), 0);

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
		screenPos = cam.WorldToScreenPoint(new Vector3(target.position.x,
													   target.position.y + (height / 2),
                                                       cam.transform.position.z < target.position.z ? target.position.z : cam.transform.position.z)) * statusIconManager.Multiplier;

		transform.localPosition = new Vector3(-15 + screenPos.x - (parentRect.rect.width / 2), screenPos.y - (parentRect.rect.height / 2), 0);
	}

	public void ClosePoolable()
	{
        HorizontalOffset = 0;

		gameObject.SetActive(false);
	}
}
