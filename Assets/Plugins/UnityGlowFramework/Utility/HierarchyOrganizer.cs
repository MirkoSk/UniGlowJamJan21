using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unparents all children at runtime.
/// </summary>
public class HierarchyOrganizer : MonoBehaviour 
{
	#region Unity Event Functions
	private void Awake () 
	{
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            transform.GetChild(i).SetParent(null);
        }
	}
	#endregion
}

