using UnityEngine;
using System.Collections.Generic;
using simProcess;

public class UnityMultiLink : MonoBehaviour {

    public List<SElement> inputs;
    public List<SElement> outputs;
	public int mode = 0;

	MultiLink mLink;

	// Use this for initialization
	void Start () {
		UnitySimClock.instance.mLinks.Add (this);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void connectSim()
	{
		mLink = new MultiLink(mode);
		foreach (SElement sElem in inputs) {
			mLink.connectInput(sElem.getElement());
		}
		foreach (SElement sElem in outputs) {
			mLink.connectOutput(sElem.getElement());
		}
	}
}
