using UnityEngine;
using System;
using UnityEngine.UI;
using simProcess;
using System.Collections.Generic;

public class UnityMultiServer : SElement, VElement  {

	PoissonProcess[] cycleTime;
	MultiServer theWorkstation;

	public Transform itemPosition;

    public string elementName = "WS";

	public double cTime = 2.0;
    public int capacity = 1;
    public float separation = 1f;

    //UI
    public Text capacityInputField;
    public Dropdown modelType;

    void Start () {

        UnitySimClock.instance.elements.Add (this);
	}

	override public void initializeSim()
	{
        //if (modelType.value == 0)
        //{
        //    capacity = 1; //Standard
        //}
        //else
        //    capacity = 2; //Upgrade

        if (capacityInputField != null)
            capacity = int.Parse(capacityInputField.text);

        SimCosts.addCost(SimCosts.costPerWorkstationCapacity * capacity);

        cycleTime = new PoissonProcess[capacity];
        for (int i = 0; i < capacity; i++)
        {
            cycleTime[i] = new PoissonProcess(cTime);
        }


        theWorkstation = new MultiServer (cycleTime, elementName, UnitySimClock.instance.clock);
		
		theWorkstation.vElement = this;
	}
	override public void startSim()
	{
		if (itemPosition == null) {
			itemPosition = this.transform;
		}

        if (capacityInputField != null)
        {
            theWorkstation.setCapacity(int.Parse(capacityInputField.text));
        }
    

		theWorkstation.start ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	override public Element getElement()
	{
		return theWorkstation;
	}

	
	void VElement.reportState(string msg)
	{
        GameObject gItem;
        Queue<Item> items = theWorkstation.getItems();
        int i = 0;

        foreach (Item it in items)
        {
            gItem = (GameObject)it.vItem;
            if (gItem != null)
            {
                gItem.transform.position = itemPosition.position + new Vector3(0f, (float)separation * i, 0f);
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
		GameObject gItem;

		gItem = (GameObject) vItem.vItem;

        if (gItem != null)
        {
            gItem.transform.position = itemPosition.position + new Vector3(0f, (float)separation * (theWorkstation.getQueueLength() - 1), 0f);
        }
		
	}

	void VElement.unloadItem(Item vItem)
	{
	}

    // UI Methods

    //public override string getDisplayInfo()
    //{
    //    SigFigures = dropdown.value + 2;
    //    return "In the queue:" + " " + Math.Round(this.getOccRate(), SigFigures);
    //}

    //public double getOccRate()
    //{
    //    return theWorkstation.MovingMean();
    //}

    public override void restartSim()
    {
        Queue<Item> items = theWorkstation.getItems();
        int i = 0;

        foreach (Item it in items)
        {
            GameObject.Destroy((GameObject)it.vItem);
            i++;
        }

        this.startSim();
    }
}
