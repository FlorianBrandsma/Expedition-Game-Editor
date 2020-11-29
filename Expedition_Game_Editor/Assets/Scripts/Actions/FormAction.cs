using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FormAction : MonoBehaviour, IAction
{
    public ActionProperties actionProperties;

    [HideInInspector]
    public bool activeInPath;

    private bool closedManually;

    public bool autoOpen;
    public bool resetPath;
    public int controllerIndex;

    private ExButton actionButton;
    public EditorForm editorForm;

    public Texture2D openIcon;
    public Texture2D closeIcon;

    public void InitializeAction(Path path)
    {
        editorForm.formAction = this;

        activeInPath = true;
    }

    public void SetAction(Path path)
    {
        InitializeButton();

        if (resetPath)
        {
            RenderManager.loadType = Enums.LoadType.Reload;
            editorForm.CloseForm();
        }
    }

    public void SetForm()
    {
        if (closedManually) return;

        if (autoOpen)
        {
            if (!editorForm.activeInPath)
                RenderManager.Render(new PathManager.Form(editorForm, controllerIndex).Initialize());
        }

        SetButton();

        closedManually = false;
    }

    private void InitializeButton()
    {
        actionButton = ActionManager.instance.AddFormButton(actionProperties);
        
        actionButton.Button.onClick.AddListener(delegate { Interact(); });
    }

    private void SetButton()
    {
        actionButton.icon.texture = editorForm.activeInPath ? closeIcon : openIcon;
    }

    public void Interact()
    {
        if (editorForm.gameObject.activeInHierarchy)
        {
            closedManually = true;
            editorForm.CloseForm();

        } else {

            closedManually = false;
            OpenForm();
        }

        SetButton();
    }

    public void OpenForm()
    {
        Path path = (new PathManager.Form(editorForm, controllerIndex).Initialize());
        
        RenderManager.Render(path);
    }

    public void CloseAction()
    {
        activeInPath = false;
    }
}
