using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IndexSwitch : MonoBehaviour
{
    public Button minus_button;
    public Button plus_button;

    public Text index_number;

    public int index { get; set; }

    private void SetSwitch()
    {
        index = 0;

        SetIndexNumber();
    }

    private void SetIndexNumber()
    {
        index_number.text = index.ToString();
    }

    public void ChangeIndex(int value)
    {
        index += value;

        SetIndexNumber();
    }

    public void Activate()
    {
        SetSwitch();

        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
