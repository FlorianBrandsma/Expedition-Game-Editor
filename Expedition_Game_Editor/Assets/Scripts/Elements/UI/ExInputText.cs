using UnityEngine;
using UnityEngine.UI;

public class ExInputText : MonoBehaviour, IEditorElement
{
    public enum LineMode
    {
        OneLine,
        TwoLine
    }

    public LineMode lineMode;

    public RectTransform RectTransform  { get { return GetComponent<RectTransform>(); } }
    public InputField InputField        { get { return GetComponent<InputField>(); } }
    //public ExElement EditorElement  { get { return GetComponent<ExElement>(); } }

    public void InitializeElement()
    {
        switch(lineMode)
        {
            case LineMode.OneLine:

                RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, 30);

                break;

            case LineMode.TwoLine:

                RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, 42.5f);

                break;
        }
    }

    public void EnableElement(bool enable)
    {
        InputField.interactable = enable;
    }
}
