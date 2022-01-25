using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldController : MonoBehaviour {

	InputField theField;

    public int minValue = 0;
    public int maxValue = 10000;

    public bool shipmentPanel = false;
    public Text maxStock;
    public Text currentStock;
    public Text pendingStock;

    // Use this for initialization
    void Start () {
		theField = this.GetComponentInChildren<InputField> ();
		if (theField==null) {
			Debug.Log ("InputField configuration failed");
		}

	}
	
	// Update is called once per frame
	void Update () {

        if (shipmentPanel == true)
        {
            maxValue = int.Parse(maxStock.text) - int.Parse(currentStock.text)- int.Parse(pendingStock.text);

            if (maxValue < 0)
            {
                maxValue = 0;
            }
        }

        if (theField.text != "")
        {
            try
            {
                if (int.Parse(theField.text) < minValue )
                {
                    theField.text = minValue.ToString();

                }
                else if (int.Parse(theField.text) > maxValue)
                {
                    theField.text = maxValue.ToString();
                }
            }
            catch (System.FormatException)
            {
                theField.text = minValue.ToString();
            }
        }
        else if(!theField.isFocused)
        {
            theField.text = minValue.ToString();
        }
       
    }

	public void modifyQuantity(int quantity)
	{
        if (quantity > 0)
        {
            theField.text = (int.Parse(theField.text) + quantity).ToString();

        }
        else
        {
            if (int.Parse(theField.text) > 0 && int.Parse(theField.text) > -quantity)
            {
                theField.text = (int.Parse(theField.text) + quantity).ToString();
            }
            else
            {
                theField.text = minValue.ToString();
                Debug.Log("Capacity can't be a null quantity");
            }

        }

	}
}
