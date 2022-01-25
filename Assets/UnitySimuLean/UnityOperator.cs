using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using simProcess;
using System;
using System.Collections.Generic;

public class UnityOperator: SElement, VElement	{

    public Animator anim;
    NavMeshAgent agent;
    Operator theOperator;

    GameObject carryingItem;

    public bool start, wayBack;
    bool isOff;


    public int capacity = 0;

    int station;
    public float loadTime = 3.0f;
    //public float unloadTime = 3.0f;

    public Transform origin, destination, wayBackStation;
    Transform nextStation;
    public Transform itemCarriedPosition;

    void Start()
    {
        UnitySimClock.instance.elements.Add(this);
    }

    override public void initializeSim()
    {
        isOff = true;
        start = false;
        station = 0;
        agent = GetComponent<NavMeshAgent>();
        agent.destination = transform.position;
        agent.isStopped = true;

        nextStation = origin;
		
		theOperator = new Operator (name, UnitySimClock.instance.clock, capacity);

        theOperator.vElement = this;
            
    }
	
	override public void startSim()
	{
        theOperator.start ();
	}
	
    void Update()
    {
        if (start && isOff )
        {
            StartCoroutine(completeRoute(origin));
        }

        if (carryingItem != null)
        {
            carryingItem.transform.position = itemCarriedPosition.position;
        }
    }

    public IEnumerator completeRoute(Transform newPosition)
    {
        start = false;
        isOff = false;

        agent.destination = newPosition.position;

        agent.isStopped = false;
        anim.SetBool("move", true);

        do
        {
            yield return null;

        } while (!(agent.remainingDistance < 2f));


        agent.isStopped = true;
        anim.SetBool("move", false);

        yield return new WaitForSeconds(loadTime);


        isOff = true;
        start = false;
        station++;

        if (station == 1)
        {
            theOperator.atPickPoint = true;
            theOperator.pickItem(); //Once the operator is at pick point, we pick the item(s)

            do
            {
                yield return null;

            } while (theOperator.atPickPoint == true);

            StartCoroutine(completeRoute(this.destination));

            yield break;
        }
        else
        {
            theOperator.leaveItem();
            carryingItem = null;

            if (station == 2 && wayBack)
            {
                StartCoroutine(completeRoute(wayBackStation));
                yield break;
            }
        }       
        station = 0;

        yield return null;
    }
	
	override public Element getElement()
	{
		return theOperator;
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
		gItem = (GameObject) vItem.vItem;

        if (gItem != null)
        {
            gItem.transform.position = origin.transform.position;
            start = true;
        }

	}

    void VElement.unloadItem(Item vItem)
    {
        if (theOperator.atPickPoint == true)
        {
            GameObject gItem;

            foreach (Item it in vItem.getSubItems())
            {
                gItem = (GameObject)it.vItem;
                if (gItem != null)
                    gItem.transform.position = itemCarriedPosition.transform.position;
            }

            carryingItem = (GameObject)vItem.vItem;

            theOperator.atPickPoint = false;
        }
    }

    public override void restartSim()
    {
        //Queue<Item> items = theOperator.getItems();
        //int i = 0;

        //foreach (Item it in items)
        //{
        //    GameObject.Destroy((GameObject)it.vItem);
        //    i++;
        //}

        this.startSim();
    }


}
