using UnityEngine;
using UnityEngine.UI;

public class ExRatingStar : MonoBehaviour
{
    public Image filling;

    public void SetStar(float value)
    {
        filling.fillAmount = value;
    }
}
