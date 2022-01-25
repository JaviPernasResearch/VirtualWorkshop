using UnityEngine;
using System.Collections;
using simProcess;
using System.Collections.Generic;

public class UnityAssembler : SElement, VElement    {

    PoissonProcess cycleTime;
    Assembler theAssembler;
    public GameObject itemPrefab;

    public Transform itemPosition;
    List<GameObject> gItem;
    public float separation = 1f;

    public string elementName = "AS";
    public int requirements;

    public double cTime = 2.0;

    // Use this for initialization
    void Awake()
    {
    }

    void Start()
    {
        UnitySimClock.instance.elements.Add(this);

        gItem = new List<GameObject>(requirements);

        gItem.Clear();
    }

    override public void initializeSim()
    {
        cycleTime = new PoissonProcess(cTime);

        theAssembler = new Assembler(elementName, UnitySimClock.instance.clock, cycleTime, requirements);

        theAssembler.vElement = this;
    }
    override public void startSim()
    {
        if (itemPosition == null)
        {
            itemPosition = this.transform;
        }


        theAssembler.start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    override public Element getElement()
    {
        return theAssembler;
    }


    void VElement.reportState(string msg)
    {
    }

    object VElement.generateItem(int type)
    {
        foreach (GameObject obj in gItem)
        {
            Destroy(obj);
        }
        gItem.Clear();

        GameObject newItem = Instantiate(itemPrefab) as GameObject;

        newItem.transform.position = itemPosition.position + new Vector3(0f, (float)separation, 0f);

        return newItem;
    }

    void VElement.loadItem(Item vItem)
    {
        GameObject mItem;

        mItem = (GameObject)vItem.vItem;

        gItem.Add((GameObject)mItem);

        mItem.transform.position = itemPosition.position + new Vector3(0f, (float)separation * (gItem.Count), 0f);
    }

	void VElement.unloadItem(Item vItem)
	{
	}

    public override void restartSim()
    {
        List<Item> items = theAssembler.getItem().getSubItems();
        int i = 0;

        foreach (Item it in items)
        {
            GameObject.Destroy((GameObject)it.vItem);
            i++;
        }

        this.startSim();
    }

}

