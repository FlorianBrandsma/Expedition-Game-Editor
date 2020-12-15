using UnityEngine;
using UnityEngine.UI;

public class ExSpeechTextBox : MonoBehaviour, IPoolable
{
	public Text nameText;
	public Text speechText;

	public GameObject moreIcon;

    public Sprite speechTextBoxSpeakSprite;
    public Sprite speechTextBoxShoutSprite;

    public RectTransform RectTransform      { get { return GetComponent<RectTransform>(); } }
    public Image Image                      { get { return GetComponent<Image>(); } }

	public Transform Transform              { get { return GetComponent<Transform>(); } }
	public Enums.ElementType ElementType    { get { return Enums.ElementType.SpeechTextBox; } }
	public int Id                           { get; set; }
	public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

	public IPoolable Instantiate()
	{
		return Instantiate(this);
	}
    
    public void SetSpeechTextBox(Enums.SpeechMethod speechMethod)
    {
        switch(speechMethod)
        {
            case Enums.SpeechMethod.Speak: Image.sprite = speechTextBoxSpeakSprite; break;
            case Enums.SpeechMethod.Shout: Image.sprite = speechTextBoxShoutSprite; break;

            default:
                Image.sprite = speechTextBoxSpeakSprite;
                Debug.Log("CASE MISSING: " + speechMethod);
                break;
        }
    }

	public void ClosePoolable()
	{
		gameObject.SetActive(false);
	}
}
