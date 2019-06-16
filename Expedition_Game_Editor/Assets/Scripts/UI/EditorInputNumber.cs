using UnityEngine;
using UnityEngine.UI;

public class EditorInputNumber : MonoBehaviour, IEditorElement
{
    private int inputValue;

    public UnitManager.Unit unit;

    public InputField inputField;
    public Button minusButton;
    public Button plusButton;

    public bool enableLimit;
    public int min, max;

    public Image Image { get { return GetComponent<Image>(); } }

    public EditorElement EditorElement { get { return GetComponent<EditorElement>(); } }

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

    public void EnableElement(bool enable)
    {
        Image.sprite = enable ? EditorElement.enabledImage : EditorElement.disabledImage;

        inputField.interactable = false;

        minusButton.interactable = enable;
        plusButton.interactable = enable;
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
