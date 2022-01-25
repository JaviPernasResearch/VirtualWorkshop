using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using simProcess;

public abstract class SElement : MonoBehaviour {

	public SElement nextElement;

    public UnityMultiLink myPreviousLink;
    public UnityMultiLink myNextLink;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public abstract void initializeSim();
	public abstract void startSim();

    public abstract void restartSim();

    //public abstract string getDisplayInfo();

    public virtual void connectSim()
    {
    	if (nextElement != null)
    		SimpleLink.createLink (getElement(), nextElement.getElement ());

        if (myPreviousLink != null)
            myPreviousLink.outputs.Add(this);
        if (myNextLink != null)
            myNextLink.inputs.Add(this);



    }
	/*
    public void connectSim()
    {
        if (myPreviousLink != null)
            myPreviousLink.outputs.Add(this);
        if (myNextLink != null)
            myNextLink.inputs.Add(this);

    }*/
    public abstract Element getElement();


}
