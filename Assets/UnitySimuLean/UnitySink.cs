using UnityEngine;
using System.Collections;
using simProcess;

public class UnitySink : SElement, VElement {

	Sink theSink;



	// Use this for initialization
	void Start () {
		UnitySimClock.instance.elements.Add (this);
	}

	override public void initializeSim()
	{
		theSink = new Sink (this.name, UnitySimClock.instance.clock);
		
		theSink.vElement = this;
	}
	override public void startSim()
	{
		theSink.start ();
	}

	override public Element getElement()
	{
		return theSink;
	}
	
	
	void VElement.reportState(string msg)
	{
	}
	
	object VElement.generateItem(int type)
	{
		return null;
	}

   

    void VElement.loadItem(Item vItem)
	{
		GameObject gItem;

        gItem = (GameObject)vItem.vItem;

        if (gItem != null)
        {
            GameObject.Destroy(gItem);
        }	

	}

	void VElement.unloadItem(Item vItem)
	{
	}

    public override void restartSim()
    {

        this.startSim();
    }

    //UI methods

    //public override string getDisplayInfo()
    //{
    //    return "Done:" + " " + this.getItemsNumber();
    //}

    //public int getItemsNumber()
    //{
    //    if (theSink == null)
    //        return 0;
    //    else
    //        return theSink.numberIterms;
    //}

}
