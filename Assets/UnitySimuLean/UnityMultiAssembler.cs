using UnityEngine;
using System.Collections;
using simProcess;
using System.Collections.Generic;
using UnityEngine.UI;

public class UnityMultiAssembler  : SElement, VElement
{
	//public string name;

	public int capacity = 1;


    public SElement [] myInputs;
    public int [] requirements = { 1 };

	public double meanDelay = 60.0;

    public GameObject assembledItemPrefab;

	public Transform[] itemLoadPosition;

    public Transform itemProcessPosition;

	public float separation = 0f;


	MultiAssembler myMultiAssembler;

    //UI
    public Text capacityInputField;
    public Dropdown modelType;

    // Use this for initialization
    void Start ()
	{
		UnitySimClock.instance.elements.Add (this);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

    override public void connectSim()
    {
        if (myInputs.Length != requirements.Length)
        {
            Debug.LogError("Distintos requerimientos y entradas");
        }

        for (int i=0; i<myInputs.Length; i++)
        {
            SimpleLink.createLink(myInputs[i].getElement(), myMultiAssembler.getInput(i));
        }

        base.connectSim();
    }



    override public void initializeSim()
	{
        if (capacityInputField != null)
            capacity = int.Parse(capacityInputField.text);

        SimCosts.addCost(SimCosts.costPerAssemblerCapacity * capacity);

        // Create the element
        myMultiAssembler = new MultiAssembler (capacity, requirements, new PoissonProcess(meanDelay), name, UnitySimClock.instance.clock) ;



		// Asign the vElement
		myMultiAssembler.vElement = this;
	}
	override public void startSim()
	{
        if (capacityInputField != null)
        {
            myMultiAssembler.setCapacity(int.Parse(capacityInputField.text));
        }

        // Start the element
        myMultiAssembler.start ();
	}

	override public Element getElement()
	{
		return myMultiAssembler;
	}


	void VElement.reportState(string msg)
	{
		GameObject gItem;
		Queue<Item> items = myMultiAssembler.getItems();
		int i = 0;

		foreach (Item it in items)
		{
			gItem = (GameObject)it.vItem;
            if (gItem != null)
            {
                gItem.transform.position = itemProcessPosition.position + new Vector3(0f, separation * i, 0f);

            }

            i++;
		}
	}

	object VElement.generateItem(int type)
	{

        GameObject vItem;
        if (assembledItemPrefab == null)
        {
            return null;
        }
        else
        {
            vItem = Instantiate(assembledItemPrefab) as GameObject;
            vItem.SetActive(true);

            vItem.transform.position = gameObject.transform.position + new Vector3(0f, 0f, 0f); ;
            vItem.gameObject.name = "Item" + this.myMultiAssembler.getCompletedItems();
            return vItem;
        }
	}

	void VElement.loadItem(Item vItem)
	{
		GameObject gItem;

		gItem = (GameObject) vItem.vItem;

        if (itemLoadPosition.GetLength(0) ==myInputs.GetLength(0) && gItem != null)
            gItem.transform.position = itemLoadPosition[vItem.getConstrainedInput()].position;
        else
            Debug.LogError("Item positions have not been correctly configured");
	}

	void VElement.unloadItem(Item vItem)
	{
		GameObject.Destroy ((GameObject) vItem.vItem);
	}

    public override void restartSim()
    {
        Queue<Item> items = myMultiAssembler.getItems();
        int i = 0;

        foreach (Item it in items)
        {
            GameObject.Destroy((GameObject)it.vItem);
            i++;
        }

        for (int j = 0; j < myMultiAssembler.getInputsCount(); j++)
        {
            items = myMultiAssembler.getInput(j).getItems();
            i = 0;

            foreach (Item it in items)
            {
                GameObject.Destroy((GameObject)it.vItem);
                i++;
            }
        }     

        this.startSim();
    }
}

