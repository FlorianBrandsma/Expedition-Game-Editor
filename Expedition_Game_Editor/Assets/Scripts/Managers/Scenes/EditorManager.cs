using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EditorManager : MonoBehaviour
{
    public static EditorManager instance;

    public static ProjectElementData projectElementData;
    
    public ProjectDataController ProjectDataController { get { return GetComponent<ProjectDataController>(); } }

    private void Awake()
    {
        instance = this;

        if (!GlobalManager.loaded)
        {
            Fixtures.LoadFixtures();

            GlobalManager.programType = GlobalManager.Scenes.Editor;
            
            GlobalManager.OpenScene(GlobalManager.Scenes.Global);

            return;
        }
    }

    private void LoadDefaultProject()
    {
        var searchProperties = new SearchProperties(Enums.DataType.Project);

        ProjectDataController.GetData(searchProperties);

        projectElementData = (ProjectElementData)ProjectDataController.Data.dataList.FirstOrDefault();
    }

    private void Start()
    {
        //TEMPORARY! Move to Awake later for testing when the "open editor" button is gone
        LoadDefaultProject();

        RenderManager.Render(new PathManager.Editor().Initialize());
    }

    public void PreviousPath()
    {
        RenderManager.PreviousPath();
    }
}