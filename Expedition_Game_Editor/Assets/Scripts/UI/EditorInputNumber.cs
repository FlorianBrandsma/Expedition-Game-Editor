using UnityEngine;
using UnityEngine.UI;

public class EditorInputNumber : MonoBehaviour
{
    private int inputValue;

    public UnitManager.Unit unit;

    public InputField inputField;

    public bool enableLimit;
    public int min, max;

    public int Value
    {
        get { return inputValue; }
        set
        {
            inputValue = value;
            inputField.text = inputValue.ToString();
        }
    }

    private void Awake()
    {
        if (enableLimit)
            inputField.placeholder.GetComponent<Text>().text = min + "-" + max;
    }

    public void ChangeValue(int value)
    {
        inputValue += value;

        inputField.text = inputValue.ToString();

        OnValueChanged();
    }

    public void OnValueChanged()
    {
        if (inputField.text.Length == 0) return;

        //Allow writing negative numbers
        if (inputField.text.Length == 1 && inputField.text == "-") return;

        if (enableLimit)
        {
            if (int.Parse(inputField.text) < min)
                inputField.text = min.ToString();

            if (int.Parse(inputField.text) > max)
                inputField.text = max.ToString();
        }

        inputValue = int.Parse(inputField.text);
    }

    public void OnEndEdit()
    {
        if (inputField.text.Length == 0) return;

        inputValue = int.Parse(inputField.text);
        inputField.text = inputValue.ToString();
    }
}
