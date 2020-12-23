using UnityEngine;
using UnityEngine.UI;

public class ExSelectionIcon : MonoBehaviour, IPoolable
{
	public Texture2D selectIcon;
	public Texture2D lockIcon;

    public Enums.SelectionIconType SelectionIconType { get; set; }

    public RawImage RawImage                { get { return GetComponent<RawImage>(); } }

    public TrackingElement TrackingElement  { get { return GetComponent<TrackingElement>(); } }

	public Transform Transform              { get { return GetComponent<Transform>(); } }
	public Enums.ElementType ElementType    { get { return Enums.ElementType.SelectionIcon; } }
	public int PoolId                           { get; set; }
	public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

	public IPoolable Instantiate()
	{
		return Instantiate(this);
	}

    public void SetElement()
    {
        switch (SelectionIconType)
        {
            case Enums.SelectionIconType.Select:
                RawImage.texture = selectIcon;
                RawImage.color = Color.green;
                break;

            case Enums.SelectionIconType.Lock:
                RawImage.texture = lockIcon;
                RawImage.color = Color.white;
                break;
        }
    }

	public void ClosePoolable()
	{
        TrackingElement.CloseTrackingElement();
	}
}
