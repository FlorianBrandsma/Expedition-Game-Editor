using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//OLD SCRIPT - REVIEW

public class Controls : MonoBehaviour
{
    public Camera UICamera;

    public RawImage joystick;
    public RawImage direction;

    public Vector2 startPos;
    public Vector2 newPos;

    float sensitivity = 100f;
    float resistance = 7.5f;
    float speed = 5f;

    OverworldScript overworld;

    void Awake()
    {
        overworld = Camera.main.GetComponent<OverworldScript>();
    }

	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos.x = Input.mousePosition.x;
            startPos.y = Input.mousePosition.y;

            joystick.transform.localScale = new Vector3(1 + (resistance / 10), 1 + (resistance / 10), 1);       
            joystick.transform.position = new Vector3(UICamera.ScreenToWorldPoint(Input.mousePosition).x, UICamera.ScreenToWorldPoint(Input.mousePosition).y, joystick.transform.position.z);
            
            joystick.gameObject.SetActive(true);
        }
        if (Input.GetMouseButton(0))
        {
            newPos.x = Mathf.Clamp(((Input.mousePosition.x - startPos.x) / Screen.width) * (sensitivity / resistance), -1, 1);
            newPos.y = Mathf.Clamp(((Input.mousePosition.y - startPos.y) / Screen.height) * (sensitivity / resistance), -1, 1);

            //Lerp de speed een klein beetje

            transform.Translate(new Vector3((newPos.x / 10) * speed, 0, (newPos.y / 10) * speed));

            RotateDirection();
            
            overworld.UpdateGrid();
        }
        if (Input.GetMouseButtonUp(0))
        {
            joystick.gameObject.SetActive(false);
        }
    }

    void RotateDirection()
    {
        Vector3 screenPos = UICamera.WorldToScreenPoint(direction.transform.position);

        float angle = Mathf.Atan2(Input.mousePosition.x - screenPos.x, Input.mousePosition.y - screenPos.y) / (2 * Mathf.PI) * 360;

        direction.transform.eulerAngles = new Vector3(0, 0, -angle);

        float newDist = Mathf.Clamp01(Vector3.Distance(screenPos, Input.mousePosition) / 65);
        
        direction.transform.localScale = new Vector3(0.5f + newDist / 2, 0.5f + newDist / 2, 1);
    }
}
