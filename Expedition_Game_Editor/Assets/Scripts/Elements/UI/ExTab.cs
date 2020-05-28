using UnityEngine;
using UnityEngine.UI;

public class ExTab : MonoBehaviour, IPoolable
{
    public Enums.ElementType elementType;

    public Text label;
    public Sprite tabActive;
    public Sprite tabInactive;

    public RectTransform LabelRect          { get { return label.GetComponent<RectTransform>(); } }
    public Image Image                      { get { return GetComponent<Image>(); } }
    public Button Button                    { get { return GetComponent<Button>(); } }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return elementType; } }
    public int Id                           { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

    public IPoolable Instantiate()
    {
        return Instantiate(this);
    }

    public void ClosePoolable()
    {
        Button.onClick.RemoveAllListeners();

        gameObject.SetActive(false);
    }
}
