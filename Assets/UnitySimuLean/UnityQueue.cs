using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using simProcess;

public class UnityQueue : SElement, VElement {

	ItemsQueue theQueue;

	public int capacity = 1;

	public float separation = 1f;

    public Transform itemPosition;

    //UI Variables

    int SigFigures;
    public Dropdown dropdown;

	void Start () {
		UnitySimClock.instance.elements.Add (this);
		if (itemPosition == null) {
			itemPosition = this.transform;
		}
	}
	
	override public void initializeSim()
	{	
		theQueue = new ItemsQueue (capacity, name, UnitySimClock.instance.clock);
		
		theQueue.vElement = this;
	}
	override public void startSim()
	{
		theQueue.start ();
	}
	
	

    override public Element getElement()
	{
		return theQueue;
	}

	void VElement.reportState(string msg)
	{
		GameObject gItem;
		Queue<Item> items = theQueue.getItems();
		int i = 1;

		foreach (Item it in items) {
			gItem = (GameObject) it.vItem;

            if (gItem != null)
                gItem.transform.position = itemPosition.position + new Vector3(0f, (float) separation * i, 0f);

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
            gItem.transform.position = itemPosition.position + new Vector3((float)separation * theQueue.getItems().Count, 0f, 0f);
        //gItem.transform.position = itemPosition.position + new Vector3(0f, (float) separation * theQueue.getItems().Count, 0f);
    }

    void VElement.unloadItem(Item vItem)
	{
	}

    public override void restartSim()
    {
        Queue<Item> items = theQueue.getItems();
        int i = 0;

        foreach (Item it in items)
        {
            GameObject.Destroy((GameObject)it.vItem);
            i++;
        }

        this.startSim();
    }

    //UI Methods

    //public override string getDisplayInfo()
    //{
    //    SigFigures = dropdown.value + 2;
    //    return "In the queue:" + " " + Math.Round(this.getOccRate(), SigFigures);

    //}


    //public int getItemsNumber()
    //{
    //    if (theQueue == null)
    //        return 0;
    //    else
    //        return theQueue.itemsQ.Count;
    //}

    //public double getOccRate()
    //{
    //        return theQueue.MovingMean();
    //}
}
