using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePanelOnStart : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HidePanel()
    {
        this.gameObject.SetActive(false);
    }
}
