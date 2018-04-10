using UnityEngine;
using System.Collections;

public class InteractIcon : MonoBehaviour
{
    public Transform target;

    public Vector3 screenPos;

	void Update ()
    {
        screenPos = Camera.main.WorldToScreenPoint(new Vector3(target.position.x, target.position.y + 3.75f, target.position.z));

        transform.localPosition = new Vector3(screenPos.x - (Screen.width / 2), screenPos.y - (Screen.height / 2), 0);
	}
}
