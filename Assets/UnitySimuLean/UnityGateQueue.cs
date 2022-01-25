using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using simProcess;
using System.Collections.Generic;

public class UnityGateQueue  : SElement, VElement
{
	public string name = "GateQueue";

	public int capacity = 1;

	public double xSeparation = 1.0, ySeparation = 1.0, zSeparation = 1.0;
	public int yLevels = 2, zLevels = 5;

    public Transform storagePosition;

	GateQueue myGateQueue;

    //UI
    public Text releaseQuantity;
    public Text pendingItems;
    public Text stockedItems;
    public Text capacityInputField;

    //Right production orders
    public UnityMultiAssembler [] Assemblers;
    public int assemblerInput;
    int assemblerMultiplier = 0;

    // Use this for initialization
    void Start ()
	{
		UnitySimClock.instance.elements.Add (this);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (pendingItems != null && myGateQueue!= null && assemblerMultiplier != 0)
		{
			pendingItems.text = (myGateQueue.getPendingItems()/ assemblerMultiplier).ToString() ;
        }
        if (stockedItems != null && myGateQueue != null)
        {
            stockedItems.text = myGateQueue.getQueueLength().ToString();
        }

	}

	public void release() {
		int q;

		q = int.Parse (releaseQuantity.text) * assemblerMultiplier;

        Debug.Log("Release " + q.ToString());

		myGateQueue.release (q);
	}

	override public void initializeSim()
	{
        if(capacityInputField != null)
            capacity = int.Parse(capacityInputField.text);

        SimCosts.addCost(SimCosts.costPerInventiryCapacity * capacity);

        // Create the element
        myGateQueue = new GateQueue (capacity, name, UnitySimClock.instance.clock);

		// Asign the vElement
		myGateQueue.vElement = this;
	}
	override public void startSim()
	{
        if (storagePosition == null)
        {
            storagePosition = this.gameObject.transform;
        }

        if (Assemblers.Length!=0)
        {
            foreach (UnityMultiAssembler assem in Assemblers)
            {
                MultiAssembler myAssembler = (MultiAssembler) assem.getElement();

                if (myAssembler.getInputsCount() > 0)
                {
                    assemblerMultiplier += myAssembler.getInput(assemblerInput).getCapacity();
                }
                else
                    assemblerMultiplier += myAssembler.getInput(0).getCapacity();
            }
        }


        if (capacityInputField != null)
        {
            myGateQueue.setCapacity(int.Parse(capacityInputField.text));
        }

        // Start the element
        myGateQueue.start ();
	}

	override public Element getElement()
	{
		return myGateQueue;
	}



	void VElement.reportState(string msg)
	{
		int xL, yL, zL;
		GameObject gItem;
		Queue<Item> items = myGateQueue.getItems();
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
		vElem.reportState ("");
	}

    public override void restartSim()
    {
        Queue<Item> items = myGateQueue.getItems();
        int i = 0;

        foreach (Item it in items)
        {
            GameObject.Destroy((GameObject)it.vItem);
            i++;
        }

        this.startSim();
    }
}

