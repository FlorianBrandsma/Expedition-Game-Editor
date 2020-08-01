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
	private float scaleMultiplier;
	#endregion
	
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

	public void UpdatePosition()
	{
		InitializeData();
		
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
			case Enums.DataType.WorldInteractable:  InitializeWorldInteractableData();  break;
			case Enums.DataType.Interaction:        InitializeInteractionData();        break;
			case Enums.DataType.WorldObject:        InitializeWorldObjectData();        break;
			case Enums.DataType.Phase:              InitializePhaseData();              break;

			default: Debug.Log("CASE MISSING: " + targetElementData.DataType); break;
		}
	}

	private void InitializeWorldInteractableData()
	{
		var worldInteractableData = (WorldInteractableElementData)targetElementData;

		height = worldInteractableData.height * worldInteractableData.scaleMultiplier;
		width  = worldInteractableData.width  * worldInteractableData.scaleMultiplier;
		depth  = worldInteractableData.depth  * worldInteractableData.scaleMultiplier;
	}

	private void InitializeInteractionData()
	{
		var interactionData = (InteractionElementData)targetElementData;

		height = interactionData.height * 1;
		width  = interactionData.width  * 1;
		depth  = interactionData.depth  * 1;
	}

	private void InitializeWorldObjectData()
	{
		var worldObjectData = (WorldObjectElementData)targetElementData;

		height = worldObjectData.height * worldObjectData.ScaleMultiplier;
		width  = worldObjectData.width  * worldObjectData.ScaleMultiplier;
		depth  = worldObjectData.depth  * worldObjectData.ScaleMultiplier;
	}

	private void InitializePhaseData()
	{
		var phaseData = (PhaseElementData)targetElementData;

		height = phaseData.height   * phaseData.scaleMultiplier;
		width = phaseData.width     * phaseData.scaleMultiplier;
		depth = phaseData.depth     * phaseData.scaleMultiplier;
	}

	private void UpdateSelectionPosition()
	{
		screenPos = cam.WorldToScreenPoint(new Vector3(target.position.x,
													   target.position.y + height,
													   cam.transform.position.z < target.position.z ? target.position.z : cam.transform.position.z)) * statusIconManager.Multiplier;

		var widthCap = statusIconManager.WidthCap;
		var heightCap = statusIconManager.HeightCap;
		
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
		screenPos = cam.WorldToScreenPoint(new Vector3(target.position.x,
													   target.position.y + (height / 2),
													   cam.transform.position.z < target.position.z ? target.position.z : cam.transform.position.z)) * statusIconManager.Multiplier;

		transform.localPosition = new Vector3(-15 + screenPos.x - (parentRect.rect.width / 2), screenPos.y - (parentRect.rect.height / 2), 0);
	}

	public void ClosePoolable()
	{
		gameObject.SetActive(false);
	}
}
