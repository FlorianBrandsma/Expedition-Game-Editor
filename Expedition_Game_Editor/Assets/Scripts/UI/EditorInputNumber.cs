using UnityEngine;
using UnityEngine.UI;

public class EditorInputNumber : MonoBehaviour
{
    public InputField inputField;

    public int input = 0;

    public int min, max;
    private bool limit_enabled;

    private void Awake()
    {
        if (max > 0)
            limit_enabled = false;
    }

    public void ChangeValue(int value)
    {
        input += value;

        inputField.text = input.ToString();

        OnValueChanged();
    }

    public void OnValueChanged()
    {
        if (inputField.text.Length == 0) return;
        
        if (int.Parse(inputField.text) < min)
            inputField.text = min.ToString();

        if (int.Parse(inputField.text) > max)
            inputField.text = max.ToString();

        input = int.Parse(inputField.text);
    }

    public void OnEndEdit()
    {
        input = int.Parse(inputField.text);
        inputField.text = input.ToString();
    }
}
