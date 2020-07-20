using UnityEngine;
using UnityEngine.UI;

public class ExInputNumber : MonoBehaviour, IEditorElement, IPoolable
{
    private float inputValue;

    public Text valueType;

    public InputField inputField;
    public Button minusButton;
    public Button plusButton;
    public GameObject glow;

    public bool enableLimit;
    public bool invertOnOverextension;
    public float min, max;

    public Sprite enabledImage;
    public Sprite disabledImage;

    public Image Image                      { get { return GetComponent<Image>(); } }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return Enums.ElementType.InputNumber; } }
    public int Id                           { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

    public IPoolable Instantiate()
    {
        return Instantiate(this);
    }

    public bool InputInvalid
    {
        set
        {
            glow.SetActive(value);
        }
    }

    public float Value
    {
        get { return inputValue; }
        set
        {
            inputValue = value;
            inputField.text = inputValue.ToString();
        }
    }
    
    private void OnEnable()
    {
        InitializeElement();
    }

    public void InitializeElement()
    {
        if (enableLimit)
            inputField.placeholder.GetComponent<Text>().text = min + "-" + max;
    }

    public void EnableElement(bool enable)
    {
        Image.sprite = enable ? enabledImage : disabledImage;

        inputField.interactable = enable;

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
        if (inputField.text.Length == 0)
        {
            if (enableLimit)
                inputField.text = min.ToString();
            else
                inputField.text = "0";
        }

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

    public void ClosePoolable()
    {
        gameObject.SetActive(false);
    }
}
