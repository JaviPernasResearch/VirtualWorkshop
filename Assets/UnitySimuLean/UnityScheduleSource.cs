using UnityEngine;
using System.Collections;
using simProcess;
using System.Collections.Generic;

public class UnityScheduleSource : SElement, VElement   {

    ScheduleSource theSource;

    public GameObject itemPrefab;

    public string myName;
    public string fileName;


    override public void initializeSim()
    {
        theSource = new ScheduleSource(this.myName, UnitySimClock.instance.clock, fileName);
        theSource.vElement = this;
    }
    override public void startSim()
    {
        theSource.start();
    }

    // Use this for initialization
    void Start()
    {
        UnitySimClock.instance.elements.Add(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    override public Element getElement()
    {
        return theSource;
    }


    void VElement.reportState(string msg)
    {
    }

    object VElement.generateItem(int myId)
    {
        GameObject newItem = Instantiate(/*this.getPrefab(idSb)*/ itemPrefab) as GameObject;

        newItem.SetActive(true);

        newItem.transform.position = this.transform.position;

        return newItem;
    }

    void VElement.loadItem(Item vItem)
    {
        GameObject gItem;

        gItem = (GameObject)vItem.vItem;

        gItem.transform.position = this.transform.position;
    }

	void VElement.unloadItem(Item vItem)
	{
	}

    public override void restartSim()
    {
        Queue<Item> items = theSource.getItems();
        int i = 0;

        foreach (Item it in items)
        {
            GameObject.Destroy((GameObject)it.vItem);
            i++;
        }

        this.startSim();
    }

}

