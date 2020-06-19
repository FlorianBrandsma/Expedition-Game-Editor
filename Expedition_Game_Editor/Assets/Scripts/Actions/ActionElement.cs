using UnityEngine;
using System.Collections;

public class ActionElement : MonoBehaviour
{
	public ActionProperties ActionProperties { get; set; }
	
	public void SetElement(ActionProperties actionProperties)
	{
		ActionProperties = actionProperties;
	}
}
