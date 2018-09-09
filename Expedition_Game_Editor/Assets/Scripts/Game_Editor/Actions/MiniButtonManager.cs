using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MiniButtonManager : MonoBehaviour
{
    public List<int>    select_path;
    public string       type;

    public SectionManager sectionManager;

    public void SetButtons()
    {
        Button button = GetComponent<EditorController>().actionManager.AddMiniButton();
        button.GetComponentInChildren<RawImage>().texture = Resources.Load<Texture2D>("Textures/Icons/" + type);

        button.onClick.AddListener(delegate { OpenPath();});
    }

    void OpenPath()
    {
        Path path = new Path(sectionManager, new List<int>(), new List<ElementData>());

        sectionManager.OpenPath(path.CreateEdit(select_path));
    }
}
