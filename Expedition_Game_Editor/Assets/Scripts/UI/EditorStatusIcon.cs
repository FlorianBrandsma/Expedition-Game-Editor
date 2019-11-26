using UnityEngine;
using System.Collections;

public class EditorStatusIcon : MonoBehaviour
{
	public Camera cam;
	public Transform target;

	public Vector3 screenPos;

    public RectTransform parentRect;
	
	void Update ()
	{
		if (target == null) return;

		screenPos = cam.WorldToScreenPoint(new Vector3(target.position.x, target.position.y + 1f, target.position.z));

		transform.localPosition = new Vector3(-30 + screenPos.x - (parentRect.rect.width / 2), 30 + screenPos.y - (parentRect.rect.height / 2), 0);
	}
}
