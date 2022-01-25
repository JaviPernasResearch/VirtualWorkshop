using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class positionStock : MonoBehaviour {


    int positionStockNumber;
    public Text myDisplay;

    public UnityProvider theProvider;
    public UnityCustomer theInventory;

    // Use this for initialization
    void Start () {
        if (theInventory == null ||  theProvider == null)
        {
            Debug.Log("Position stock can not be displayed");
        }
        myDisplay = this.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        positionStockNumber = int.Parse(theProvider.pendingArrivals.text) + int.Parse(theInventory.displayStock.text);
        if (theInventory != null && theProvider != null && myDisplay != null)
        {
            myDisplay.text = positionStockNumber.ToString();
        }
	}
}
