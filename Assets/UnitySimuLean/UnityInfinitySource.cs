using UnityEngine;
using System.Collections;
using simProcess;
using UnityEngine.UI;

public class UnityInfinitySource : SElement, VElement  {

	InfiniteSource theSource;

	public GameObject itemPrefab;

    public float separation = 1f;

    public Transform itemPosition;


    override public void initializeSim()
	{
		theSource = new InfiniteSource (this.name, UnitySimClock.instance.clock);
		theSource.vElement = this;

	}
	override public void startSim()
	{
		theSource.start ();
        
	}


    // Use this for initialization
    void Start () {
		UnitySimClock.instance.elements.Add (this);

    }
	 
    override public Element getElement()
	{
		return theSource;
	}
	
	
	void VElement.reportState(string msg)
	{
        
    }
	
	object VElement.generateItem(int type)
	{

		GameObject newItem = Instantiate (itemPrefab) as GameObject;

		newItem.SetActive (true);

		newItem.transform.position = itemPosition.position + new Vector3(0f, (float)separation, 0f); ;
        newItem.gameObject.name = newItem.gameObject.name + theSource.getNumberItems();
        return newItem;
    }
	
	void VElement.loadItem(Item vItem)
	{
		GameObject gItem;
		
		gItem = (GameObject) vItem.vItem;
		
		gItem.transform.position = itemPosition.position;
        
        
    }

	void VElement.unloadItem(Item vItem)
	{
	}

    public override void restartSim()
    {

        this.startSim();
    }


    //UI Methods

    //public override string getDisplayInfo()
    //{
    //    return "Items " + this.name + ": " + this.getItemsNumber();
    //}

    //public int getItemsNumber()
    //{
    //    if (theSource == null)
    //        return 0;
    //    else
    //        return theSource.numberIterms;
    //}

}
