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
    }

    public void SetForm()
    {
        if (closedManually) return;

        if (autoOpen)
        {
            if (!editorForm.activeInPath)
                EditorManager.editorManager.Render(new PathManager.Form(editorForm).Initialize());
        }
        
        closedManually = false;
    }

    private void InitializeButton()
    {
        var button = ActionManager.actionManager.AddFormButton(actionProperties);

        button.GetComponent<EditorButton>().icon.texture = editorForm.activeInPath ? openIcon : closeIcon;

        button.onClick.AddListener(delegate { Interact(); });
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
    }

    public void OpenForm()
    {
        Path path = (new PathManager.Form(editorForm).Initialize());

        EditorManager.editorManager.Render(path);
    }

    public void CloseAction()
    {
        activeInPath = false;
    }
}
