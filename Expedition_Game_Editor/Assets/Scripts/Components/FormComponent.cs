using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FormComponent : MonoBehaviour
{
    public EditorComponent component;

    //Initialize once, never reset
    //If not constant, form will close with component
    public bool constant;

    //If the button is pressed, the form hides or closes
    public bool hide;
    private bool hidden;

    public EditorForm editorForm;
    private LayoutManager layoutManager;
    public ComponentManager componentManager;
    //public Texture2D texture;


    private void Awake()
    {
        layoutManager = GetComponent<LayoutManager>();
    }

    public void SetComponent()
    {
        SetButton();

        if (constant && editorForm.active) return;
            Debug.Log("Set: " + editorForm);
    }

    private void SetButton()
    {
        Button new_button = componentManager.AddFormButton();

        Button button = GetComponent<Button>();

        //new_element.data = element.data;
        //new_element.selectionType = element.selectionType;
        //new_element.listType = element.listType;

        new_button.onClick.AddListener(delegate { ClickComponent(); });
    }

    public void ClickComponent()
    {
        Debug.Log("Click!");

        //If active...
        //If hide...
        //If hidden...

    }

    public void OpenForm()
    {

    }

    public void CloseForm()
    {

    }

    public void CloseComponent()
    {
        if (!constant)
            CloseForm();
    }
}
