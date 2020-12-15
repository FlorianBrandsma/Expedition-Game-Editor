using UnityEngine;
using UnityEngine.UI;

public class ExJoystick : MonoBehaviour, IPoolable
{
    public RawImage direction;

    public RectTransform RectTransform      { get { return GetComponent<RectTransform>(); } }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return Enums.ElementType.Joystick; } }
    public int Id                           { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

    public IPoolable Instantiate()
    {
        return Instantiate(this);
    }
    
    public void Rotate(Vector3 screenPoint, float angle)
    {
        //65?
        var pointDistance = Mathf.Clamp01(Vector3.Distance(screenPoint, Input.mousePosition) / 65);

        direction.transform.eulerAngles = new Vector3(0, 0, -angle);
        direction.transform.localScale = new Vector3(0.5f + pointDistance / 2, 0.5f + pointDistance / 2, 1);
    }

    public void ClosePoolable()
    {
        direction.transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }
}
