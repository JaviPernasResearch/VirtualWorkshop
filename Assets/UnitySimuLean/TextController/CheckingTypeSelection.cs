using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckingTypeSelection : MonoBehaviour {

    public GameObject continuousMenu;
    public GameObject periodicMenu;
    public Dropdown mySelection; 

	// Use this for initialization
	void Start () {
        if (continuousMenu == null || periodicMenu == null || mySelection == null)
            Debug.LogError("Checking setup not configured");

        continuousMenu.SetActive(false);
        periodicMenu.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setupMenu()
    {
        if (mySelection.value == 0)
        {
            continuousMenu.SetActive(true);
        }
        else
            periodicMenu.SetActive(true);
    }
}
