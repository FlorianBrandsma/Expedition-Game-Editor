using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExSpeechBubble : MonoBehaviour, IPoolable
{
	public Text speechText;

    public Texture2D speechBubbleTexture;
    public Texture2D speechBubbleWithArrowTexture;

    public TrackingElement TrackingElement  { get { return GetComponent<TrackingElement>(); } }

    public RawImage RawImage                { get { return GetComponent<RawImage>(); } }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
	public Enums.ElementType ElementType    { get { return Enums.ElementType.SpeechBubble; } }
	public int PoolId                           { get; set; }
	public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

	public IPoolable Instantiate()
	{
		return Instantiate(this);
	}

    public void InitializeElement()
    {
        StartCoroutine(Fade(true, 0.15f));

        SetSpeechBubble(true);
    }

    public void SetSpeechBubble(bool withArrow)
    {
        RawImage.texture = withArrow ? speechBubbleWithArrowTexture : speechBubbleTexture;
    }

    public void ClosePoolable()
	{
        StopAllCoroutines();

        TrackingElement.CloseTrackingElement();
    }

    private IEnumerator Fade(bool fadeIn, float duration)
    {
        var startAlpha = fadeIn ? 0 : 0.8f;
        var targetAlpha = fadeIn ? 0.8f : 0;

        if (duration > 0f)
        {
            var startTime = Time.time;
            var endTime = startTime + duration;

            yield return null;

            while (Time.time < endTime)
            {
                var progress = (Time.time - startTime) / duration;

                var alpha = Mathf.Lerp(startAlpha, targetAlpha, progress);

                RawImage.color = new Color(RawImage.color.r, RawImage.color.g, RawImage.color.b, alpha);

                yield return null;
            }
        }

        RawImage.color = new Color(RawImage.color.r, RawImage.color.g, RawImage.color.b, targetAlpha);
    }
}
