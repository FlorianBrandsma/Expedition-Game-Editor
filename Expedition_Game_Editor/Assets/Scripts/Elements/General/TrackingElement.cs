using UnityEngine;
using UnityEngine.UI;

public class TrackingElement : MonoBehaviour
{
	public TrackingElementOverlay TrackingElementOverlay    { get; set; }
	public Enums.TrackingElementType TrackingElementType    { get; set; }

	public Camera cam;
	public IElementData targetElementData;
	public Transform target;
	public Vector3 screenPos;
	public RectTransform parentRect;
	
	#region Data Variables
	private float height, width, depth;
	private float scale;
	#endregion

	private float widthCap;
	private float heightCap;

	public float HorizontalOffset       { get; set; }
	public float VerticalOffset         { get; set; }

	public RectTransform RectTransform  { get { return GetComponent<RectTransform>(); } }
	
	public ExSpeechBubble SpeechBubble  { get { return GetComponent<ExSpeechBubble>(); } }
	
	public void InitializeElement()
	{
		var cameraManager = TrackingElementOverlay.CameraManager;
		
		widthCap    = ((cameraManager.displayRect.rect.width / 2) - (RectTransform.rect.width / 2));
		heightCap   = ((cameraManager.displayRect.rect.height / 2) - (RectTransform.rect.height / 2));

		transform.SetParent(cameraManager.overlayManager.layer[0]);
		transform.localEulerAngles = Vector3.zero;

		cam = cameraManager.cam;

		parentRect = cameraManager.displayRect;

		if (cameraManager.overlayManager.vertical_min != null)
		{
			HorizontalOffset    = cameraManager.overlayManager.vertical_min.rect.width / 2;
			VerticalOffset      = cameraManager.overlayManager.horizontal_min.rect.height / 2;
		}
	}

	public void SetTrackingTarget(DataElement dataElement)
	{
		if (dataElement == null) return;

		targetElementData = dataElement.ElementData;
		target = dataElement.transform;

		InitializeData();

		gameObject.SetActive(true);
	}

	public void UpdateTrackingElement()
	{
		if (target == null) return;

		InitializeData();
		UpdatePosition();
	}

	public void UpdatePosition()
	{
		if (target == null) return;
		
		switch (TrackingElementType)
		{
			case Enums.TrackingElementType.Limited:    UpdateLimitedPosition();    break;
			case Enums.TrackingElementType.Free:       UpdateFreePosition();       break;
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
			case Enums.DataType.SceneActor:             InitializeSceneActorData();             break;
			case Enums.DataType.SceneProp:              InitializeScenePropData();              break;

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
		width  = phaseData.Width    * phaseData.Scale;
		depth  = phaseData.Depth    * phaseData.Scale;
	}

	private void InitializeSceneActorData()
	{
		var sceneActorData = (SceneActorElementData)targetElementData;

		height = sceneActorData.Height * sceneActorData.Scale;
		width  = sceneActorData.Width  * sceneActorData.Scale;
		depth  = sceneActorData.Depth  * sceneActorData.Scale;
	}

	private void InitializeScenePropData()
	{
		var scenePropData = (ScenePropElementData)targetElementData;

		height = scenePropData.Height * scenePropData.Scale;
		width  = scenePropData.Width  * scenePropData.Scale;
		depth  = scenePropData.Depth  * scenePropData.Scale;
	}

	private void InitializeGameWorldInteractable()
	{
		var gameWorldInteractableData = (GameWorldInteractableElementData)targetElementData;

		height = gameWorldInteractableData.Height   * gameWorldInteractableData.Scale;
		width  = gameWorldInteractableData.Width    * gameWorldInteractableData.Scale;
		depth  = gameWorldInteractableData.Depth    * gameWorldInteractableData.Scale;
	}

	private void UpdateLimitedPosition()
	{
		var maxHeight = 12;
		var heightOffset = 0.5f;

		var totalHeight = height + heightOffset;

		var fixedHeight = (totalHeight > maxHeight ? maxHeight : totalHeight);
		
		screenPos = cam.WorldToScreenPoint(new Vector3(target.position.x,
													   target.position.y + fixedHeight,
													   target.position.z)) * TrackingElementOverlay.Multiplier;

		if (screenPos.z < 0)
		{
			//When the target is behind the camera, the WorldToScreenPoint values get messed up.
			//The target is always in the bottom of the screen, so only its horizontal position relative to the
			//camera matters
			screenPos = (target.transform.position - cam.transform.position) * parentRect.rect.width;
		}
		
		transform.localPosition = new Vector3(-HorizontalOffset + screenPos.x - (parentRect.rect.width / 2), 
											  -VerticalOffset   + screenPos.y - (parentRect.rect.height / 2), 
											  0);

		if (SpeechBubble != null)
			SpeechBubble.SetSpeechBubble(true);

		if (transform.localPosition.x > widthCap - HorizontalOffset)
		{
			transform.localPosition = new Vector3(widthCap - HorizontalOffset, transform.localPosition.y, 0);

			if (SpeechBubble != null)
				SpeechBubble.SetSpeechBubble(false);
		}

		if (transform.localPosition.x < -widthCap + HorizontalOffset)
		{
			transform.localPosition = new Vector3(-widthCap + HorizontalOffset, transform.localPosition.y, 0);

			if (SpeechBubble != null)
				SpeechBubble.SetSpeechBubble(false);
		}

		if (transform.localPosition.y > heightCap - VerticalOffset)
		{
			transform.localPosition = new Vector3(transform.localPosition.x, heightCap - VerticalOffset, 0);

			if (SpeechBubble != null)
				SpeechBubble.SetSpeechBubble(false);
		}
		
		if (transform.localPosition.y < -heightCap - VerticalOffset)
		{
			transform.localPosition = new Vector3(transform.localPosition.x, -heightCap + VerticalOffset, 0);

			if (SpeechBubble != null)
				SpeechBubble.SetSpeechBubble(true);
		}
	}

	private void UpdateFreePosition()
	{
		screenPos = cam.WorldToScreenPoint(new Vector3(target.position.x,
													   target.position.y + (height / 2),
													   target.position.z)) * TrackingElementOverlay.Multiplier;

		transform.localPosition = new Vector3(-HorizontalOffset + screenPos.x - (parentRect.rect.width / 2), screenPos.y - (parentRect.rect.height / 2), 0);
	}

	public void CloseTrackingElement()
	{
		HorizontalOffset = 0;

		gameObject.SetActive(false);
	}
}
