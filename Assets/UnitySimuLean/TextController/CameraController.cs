using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class CameraController : MonoBehaviour {

    public GameObject firstPerson;
    public GameObject overheadPerson;
    FirstPersonController theFps;

    public bool FpsPriority = false;

    // Use this for initialization
    void Awake () {
        theFps = firstPerson.GetComponent<FirstPersonController>();
        if (theFps != null)
        {
            theFps.enabled = false;
        }
        changeCamera();
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void changeCamera()
    {
        firstPerson.SetActive( FpsPriority);
        overheadPerson.SetActive(!FpsPriority);
        FpsPriority = !FpsPriority;
    }
}
