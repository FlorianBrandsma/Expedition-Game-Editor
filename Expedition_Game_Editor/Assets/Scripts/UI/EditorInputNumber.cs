using UnityEngine;
using UnityEngine.UI;

public class EditorInputNumber : MonoBehaviour, IEditorElement
{
    private float inputValue;

    public string valueType;
    public string valueUnit;

    public InputField inputField;
    public Button minusButton;
    public Button plusButton;
    public Text valueTypeText;
    public Text valueUnitText;

    public bool enableLimit;
    public bool invertOnOverextension;
    public float min, max;

    public Image Image { get { return GetComponent<Image>(); } }

    public EditorElement EditorElement { get { return GetComponent<EditorElement>(); } }

    public float Value
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
        if (valueType.Length > 0)
        {
            valueTypeText.text = valueType;

            Vector2 inputFieldOffset = inputField.GetComponent<RectTransform>().offsetMin;
            inputField.GetComponent<RectTransform>().offsetMin = new Vector2(inputFieldOffset.x + valueTypeText.rectTransform.sizeDelta.x / 2, inputFieldOffset.y);
        }

        if(valueUnit.Length > 0)
        {
            valueUnitText.text = valueUnit;

            Vector2 inputFieldOffset = inputField.GetComponent<RectTransform>().offsetMax;
            inputField.GetComponent<RectTransform>().offsetMax = new Vector2(inputFieldOffset.x - valueUnitText.rectTransform.sizeDelta.x / 2, inputFieldOffset.y);
        }

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
            if (float.Parse(inputField.text) < min)
                inputField.text = invertOnOverextension ? max.ToString() : min.ToString();
            if (float.Parse(inputField.text) > max)
                inputField.text = invertOnOverextension ? min.ToString() : max.ToString();
        }

        inputValue = float.Parse(inputField.text);
    }

    public void OnEndEdit()
    {
        if (inputField.text.Length == 0) return;

        inputValue = float.Parse(inputField.text);
        inputField.text = inputValue.ToString();
    }
}
