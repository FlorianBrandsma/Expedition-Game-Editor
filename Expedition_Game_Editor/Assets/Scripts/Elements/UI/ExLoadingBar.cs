using UnityEngine;
using UnityEngine.UI;

public class ExLoadingBar : MonoBehaviour, IPoolable
{
    public Image barFilling;
    public Text methodText;

    public RectTransform RectTransform      { get { return GetComponent<RectTransform>(); } }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return Enums.ElementType.LoadingBar; } }
    public int PoolId                       { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }
    
    public IPoolable Instantiate()
    {
        return Instantiate(this);
    }
    
    public void UpdateFiller(float value)
    {
        barFilling.fillAmount = value;
    }

    public void ClosePoolable()
    {
        gameObject.SetActive(false);
    }
}
