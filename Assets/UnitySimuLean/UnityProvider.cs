using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using simProcess;
using System.Collections.Generic;

public class UnityProvider  : SElement, VElement
{
	public string name = "Provider";
	public bool useSocket = false;
	public double minTime = 30.0;
	public double maxTime = 60.0;

	public GameObject itemPrefab;

    public Text orderQuantity;

    public Text pendingArrivals;

    ProviderSource theProviderSource;

	// Use this for initialization
	void Start ()
	{
		UnitySimClock.instance.elements.Add (this);
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (pendingArrivals != null && theProviderSource!= null)
        {
            pendingArrivals.text = theProviderSource.getPendingArrivals().ToString();
        }
	}

    public void order()
    {
        int q;

		q = int.Parse (orderQuantity.text);

        theProviderSource.order(q);

        SimCosts.addCost(SimCosts.orderCost + SimCosts.purchaseCost * q);
    }


	override public void initializeSim()
	{	
		

		// Create the element
		theProviderSource = new ProviderSource (useSocket, name, UnitySimClock.instance.clock, minTime, maxTime) ;



		// Asign the vElement
		theProviderSource.vElement = this;
	}
	override public void startSim()
	{
		// Start the element
		theProviderSource.start ();
	}

	override public Element getElement()
	{
		return theProviderSource;
	}


	void VElement.reportState(string msg)
	{

	}

	object VElement.generateItem(int type)
	{
		GameObject vItem;
		if (itemPrefab == null) {
			return null;
		} else {
			vItem = Instantiate (itemPrefab) as GameObject;
			vItem.SetActive (true);

			vItem.transform.position = gameObject.transform.position + new Vector3(0f, 0f, 0f); ;
			vItem.gameObject.name = "Item" + theProviderSource.getNumberItems();
			return vItem;
		}
	}

	void VElement.loadItem(Item vItem)
	{

	}

	void VElement.unloadItem(Item vItem)
	{

	}
    public override void restartSim()
    {

        this.startSim();
    }
}

