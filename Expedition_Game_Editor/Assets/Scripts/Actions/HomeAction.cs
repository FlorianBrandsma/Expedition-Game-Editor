using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HomeAction : MonoBehaviour, IAction
{
    public ActionProperties actionProperties;

    public Texture2D icon;
    
    private PathController PathController { get { return GetComponent<PathController>(); } }

    public void InitializeAction(Path path) { }

    public void SetAction(Path path)
    {
        InitializeButton();
    }
    
    private void InitializeButton()
    {
        var button = ActionManager.actionManager.AddButton(actionProperties);

        button.GetComponent<EditorButton>().icon.texture = icon;

        button.onClick.AddListener(delegate { InitializePath(new PathManager.Main().Initialize()); });
    }

    public void InitializePath(Path path)
    {
        EditorManager.editorManager.Render(path);
    }

    public void CloseAction() { }
}
