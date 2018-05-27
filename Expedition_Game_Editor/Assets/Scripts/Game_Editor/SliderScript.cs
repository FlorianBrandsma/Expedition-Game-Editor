using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderScript : MonoBehaviour
{
    public void UpdateSlider(Slider slider)
    {
        slider.value = Mathf.Clamp(GetComponent<ScrollRect>().verticalNormalizedPosition, 0, 1);
    }
}
