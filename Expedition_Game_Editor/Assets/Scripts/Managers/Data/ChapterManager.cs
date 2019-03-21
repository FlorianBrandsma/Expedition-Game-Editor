using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterManager
{
    private ChapterController dataController;
    private List<ChapterData> chapterData_list;

    public List<ChapterDataElement> GetChapterDataElements(ChapterController dataController)
    {
        this.dataController = dataController;

        GetChapterData();
        //GetIconData()?

        var list = (from oCore in chapterData_list
                    select new ChapterDataElement()
                    {
                        id = oCore.id,
                        table = oCore.table,
                        type = oCore.type,
                        index = oCore.index,
                        name = oCore.name,
                        description = oCore.description,

                        icon = "Textures/Characters/1"

                    }).OrderBy(x => x.index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetChapterData()
    {
        chapterData_list = new List<ChapterData>();
        
        //Temporary
        for (int i = 0; i < dataController.temp_id_count; i++)
        {
            var chapterData = new ChapterData();

            chapterData.id = (i + 1);
            chapterData.table = "Chapter";
            chapterData.index = i;

            chapterData.name = "Chapter " + (i + 1);
            chapterData.description = "This is a pretty regular sentence. The structure is something you'd expect. Nothing too long though!";

            chapterData_list.Add(chapterData);
        }
    }

    internal class ChapterData : GeneralData
    {
        public int index;
        public string name;
        public string description;      
    }
}
