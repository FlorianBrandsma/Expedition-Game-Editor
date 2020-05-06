using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HomeAction : MonoBehaviour, IAction
{
    public ActionProperties actionProperties;

    public Texture2D icon;

    private ExButton actionButton;

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public void InitializeAction(Path path) { }

    public void SetAction(Path path)
    {
        InitializeButton();
    }
    
    private void InitializeButton()
    {
        actionButton = ActionManager.instance.AddButton(actionProperties);

        actionButton.icon.texture = icon;

        actionButton.Button.onClick.AddListener(delegate { InitializePath(new PathManager.Editor().Initialize()); });
    }

    public void InitializePath(Path path)
    {
        RenderManager.Render(path);
    }

    public void CloseAction()
    {
        actionButton.Button.onClick.RemoveAllListeners();
    }
}
