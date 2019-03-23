using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class RegionStructureComponent : MonoBehaviour, IComponent
{
	private RegionManager.Type type;

	public RegionDisplayManager default_display;

	private PathController pathController { get { return GetComponent<PathController>(); } }

	public EditorComponent component;
	private List<DataList> structure_dataList = new List<DataList>();

	public List<GeneralData> structure_list;

	private Route region;
	private Route structure;

	private Path active_path;

	public void InitializeComponent(Path path)
	{
		active_path = path;

		region = active_path.FindLastRoute("Region").Copy();
		type = (RegionManager.Type)region.GeneralData().type;

		InitializeData();

		if (path.route.Count < (pathController.route.path.route.Count + 1))
		{
			//The region route gets added at the end when the component is initialized.
			//It tries to add another route every time it gets opened, causing the selection to appear.
			//This attempt gets blocked by using the max_length variable

			int index = (int)RegionDisplayManager.active_display;

			if (index > (pathController.controllers.Length - 1))
				index = (pathController.controllers.Length - 1);

			region.controller = index;

			path.Add(region);
		}     
	}

	private void InitializeData()
	{
		if (active_path.type != Path.Type.New) return;

		if(type == RegionManager.Type.Task)
			RegionDisplayManager.active_display = 0;

		structure_dataList.Clear();

		InitializeStructureData();
		InitializeRegionData();

		if (region.GeneralData().id == 0)
			region.GeneralData().id = 1;
	}

	private void InitializeStructureData()
	{
		Route previous_route = null;
		int target_id = 0;

		for (int i = 0; i < structure_list.Count; i++)
		{
			Route structure_route = active_path.FindLastRoute(structure_list[i].table);
			target_id = structure_route.GeneralData().id;

			string sql = "";

			sql += "SELECT * FROM " + structure_route.GeneralData().table;

			if (previous_route != null)
				sql += " WHERE " + previous_route.GeneralData().table + "Id = " + target_id;

			DataList dataList = GetData(sql, structure_route.GeneralData());

			structure_dataList.Add(dataList);

			previous_route = structure_route;
		}
	}

	private void InitializeRegionData()
	{
		Route structure_route = active_path.FindLastRoute("Region");
		int target_id = structure_route.GeneralData().id;

		string sql = "";

		sql += "SELECT * FROM Region ";

		if(type != RegionManager.Type.Base)
			sql += "WHERE Phase" + "Id = " + target_id;

		DataList dataList = GetData(sql, structure_route.GeneralData());

		structure_dataList.Add(dataList);        
	}

	public void SetComponent(Path new_path)
	{
		for (int i = 0; i < structure_dataList.Count; i++)
		{
			SetStructureComponent(i);
		}
	}

	private void SetStructureComponent(int index)
	{
		DataList dataList = structure_dataList[index];
		
		Dropdown dropdown = ComponentManager.componentManager.AddDropdown(component);

		dropdown.options.Clear();
		dropdown.onValueChanged.RemoveAllListeners();
		
		for (int i = 0; i < dataList.list.Count; i++)
		{
			dropdown.options.Add(new Dropdown.OptionData(dataList.data.table + " " + i));
		}

		List<GeneralData> new_list = structure_list.ToList();
		new_list.Add(region.GeneralData());

		Route structure_route = active_path.FindLastRoute(new_list[index].table);

		int selected_index = dataList.list.FindIndex(x => x.id == structure_route.GeneralData().id);

		dropdown.captionText.text = dataList.list[selected_index].table + " " + selected_index;
		dropdown.value = selected_index;

		Path path = pathController.route.path;
		dropdown.onValueChanged.AddListener(delegate { OpenPath(path, dataList.list[dropdown.value], (index + 1)); });
	}

	private DataList GetData(string sql, GeneralData generalData)
	{
		DataList dataList = new DataList();

		dataList.data = generalData; //placeholder. Will be made using sql (FROM)
		dataList.id_count = 15; //Placeholder

		dataList.GetData(sql);

		return dataList;
	}

	private void OpenPath(Path path, GeneralData generalData, int index)
	{
		//path.ReplaceAllRoutes(generalData);

		SetStructureData(path, generalData, index);

		EditorManager.editorManager.OpenPath(path);
	}

	private void SetStructureData(Path path, GeneralData data, int index)
	{
		for (int i = index; i < structure_dataList.Count; i++)
		{
			GeneralData structure_data = structure_dataList[i].data;

			string sql = "";

			if(structure_data.table == "Region")
			{
				sql += "SELECT * FROM Region WHERE Phase" + "Id = " + data.id;
			} else {
				sql += "SELECT * FROM " + structure_data.table + " WHERE " + data.table + "Id = " + data.id;
			}

			structure_dataList[i] = GetData(sql, structure_data);

			//(TASK belongs to an ELEMENT which is placed on a TERRAIN which belongs to a REGION and a PHASE)
			//TASK: load ELEMENT > TERRAIN > REGION
			data = structure_dataList[i].list[0];

			//path.ReplaceAllRoutes(data);
		}
	}

	public void CloseComponent() { }
}
