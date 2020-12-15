using UnityEngine;
using UnityEngine.UI;

public class ExCameraFilter : MonoBehaviour, IPoolable
{
    public RawImage RawImage                { get { return GetComponent<RawImage>(); } }

    public RectTransform RectTransform      { get { return GetComponent<RectTransform>(); } }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return Enums.ElementType.CameraFilter; } }
    public int Id                           { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }
    
    public IPoolable Instantiate()
    {
        return Instantiate(this);
    }

    public void SetAlpha(float alpha)
    {
        RawImage.color = new Color(1, 1, 1, alpha);
    }

    private void ResetColor()
    {
        RawImage.color = new Color(1, 1, 1, 0);
    }

    public void ClosePoolable()
    {
        ResetColor();

        gameObject.SetActive(false);
    }
}
