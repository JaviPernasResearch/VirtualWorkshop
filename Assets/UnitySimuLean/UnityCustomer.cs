using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using simProcess;
using System.Collections.Generic;

public class UnityCustomer  : SElement, VElement
{
	public bool automaticDemand = true;

    //public string 
    public int capacity = 1;

	public int MinQuantity = 1;
	public int MaxQuantity = 10;

	public double xSeparation = 1.0, ySeparation = 1.0, zSeparation = 1.0;
	public int yLevels = 2, zLevels = 5;


    public Transform storagePosition;

    CustomerSink theCustomerSink;

    double lastSimTime = 0;

    //UI
    public Text displayPending;
    public Text capacityInputField;
    public Text displayStock;

    // Use this for initialization
    void Start ()
	{
		UnitySimClock.instance.elements.Add (this);
	}
	
	// Update is called once per frame
	void Update ()
	{
        //displayPending.text = "H";

        if (theCustomerSink != null && displayPending!= null && displayStock != null)
        {
            double deltaT = UnitySimClock.instance.clock.getSimulationTime() - lastSimTime;
            if (deltaT > 0)
            {
                SimCosts.addCost(deltaT * SimCosts.pendingOrderUnitCost * theCustomerSink.getPendingOrders());
                lastSimTime = UnitySimClock.instance.clock.getSimulationTime();
            }

            //Debug.Log("Peding orders" + theCustomerSink.getPendingOrders().ToString());
            displayPending.text = theCustomerSink.getPendingOrders().ToString();
            displayStock.text = theCustomerSink.getNumberItems().ToString();

            Debug.Log("Demand: " + theCustomerSink.totalDemand.ToString());
        }

    }

    public void shipTruck()
    {
        theCustomerSink.shipTruck();
    }

	override public void initializeSim()
	{
        if (capacityInputField != null)
            capacity = int.Parse(capacityInputField.text);
        
        SimCosts.addCost(SimCosts.costPerInventiryCapacity * capacity);

        // Create the element
        theCustomerSink = new CustomerSink (capacity, name, UnitySimClock.instance.clock, MinQuantity, MaxQuantity) ;

		// Asign the vElement
		theCustomerSink.vElement = this;
	}
	override public void startSim()
	{
        if (storagePosition == null)
        {
            storagePosition = this.gameObject.transform;
        }

        if (capacityInputField != null)
        {
            theCustomerSink.setCapacity(int.Parse(capacityInputField.text));
        }

        // Start the element
        theCustomerSink.start ();
    }

	override public Element getElement()
	{
		return theCustomerSink;
	}


	void VElement.reportState(string msg)
	{
		int xL, yL, zL;
		GameObject gItem;
		Queue<Item> items = theCustomerSink.getItems();
		int i = 0;

		foreach (Item it in items) {
			gItem = (GameObject) it.vItem;

			yL = i % yLevels;
			zL = (i / yLevels) % zLevels;
			xL = i / (yLevels * zLevels);

            if (gItem != null)
            {
                gItem.transform.position = storagePosition.position + new Vector3((float)(xL * xSeparation), (float)(yL * ySeparation), (float)(zL * zSeparation));

            }
			

			i++;
		}
	}

	object VElement.generateItem(int type)
	{
		return null;
	}

	void VElement.loadItem(Item vItem)
	{
		VElement vElem = this;
		vElem.reportState ("");
	}

	void VElement.unloadItem(Item vItem)
	{
		VElement vElem = this;

		GameObject.Destroy ((GameObject) vItem.vItem);

		vElem.reportState ("");
	}

    public override void restartSim()
    {
        Queue<Item> items = theCustomerSink.getItems();
        int i = 0;

        foreach (Item it in items)
        {
            GameObject.Destroy((GameObject)it.vItem);
            i++;
        }

        this.startSim();
    }
}

