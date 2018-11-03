using UnityEngine;
using UnityEngine.UI;

public class EditorSlider : MonoBehaviour
{
    public void UpdateSlider(Slider slider)
    {
        slider.value = Mathf.Clamp(GetComponent<ScrollRect>().verticalNormalizedPosition, 0, 1);
    }
}
