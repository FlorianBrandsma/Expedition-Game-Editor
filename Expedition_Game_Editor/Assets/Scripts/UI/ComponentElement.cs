using UnityEngine;
using System.Collections;

public class ComponentElement : MonoBehaviour
{
	public EditorComponent component { get; set; }
	
	public void SetElement(EditorComponent new_component)
	{
		component = new_component;
	}
}
