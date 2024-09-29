using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPanelUI : MonoBehaviour
{
	public GameObject panel;
	public void OpenPanel()
	{
		panel.SetActive(!panel.activeInHierarchy);
	}
}
