using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class continuousMode : MonoBehaviour {

    public Button orderButton;
    public Text positionStock;
    public Text orderPoint;

    public Text cep;
    public Text pp;
    public Text displayCEP, displayPP;
    // Use this for initialization
    void Start () {
        if (orderButton == null || positionStock == null || orderPoint == null)
            Debug.LogError("Error: Initializing failed");
	}

    // Update is called once per frame
    void Update() {
        if (int.Parse(positionStock.text) > int.Parse(orderPoint.text))
        {
            orderButton.interactable = false;
        }
        else
            orderButton.interactable = true;
	}

    public void setParameters()
    {
        displayCEP.text = cep.text;
        displayPP.text = pp.text;
    }
}
