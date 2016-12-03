using UnityEngine;
using System.Collections;

public class InteractableObject : MonoBehaviour
{
	public GameObject body = null;
    private bool isTouched = false;
    private bool isGrabbed = false;
    private bool isSwungIn = false;
    private Transform originalPosition;

    void OnEnable()
    {
        MyoPoseController.onDoubleTap += doOnDoubleTap;
        MyoPoseController.onFingerSpread += doOnFingerSpread;
        MyoPoseController.onFist += doOnFist;
        MyoPoseController.onRest += doOnRest;
        MyoPoseController.onWaveIn += doOnWaveIn;
        MyoPoseController.onWaveOut += doOnWaveOut;
        originalPosition = this.transform;
    }

    void doOnDoubleTap(GameObject body)
    {
        Debug.Log("DoubleTab detected with " + this.gameObject);

    }

    void doOnFingerSpread(GameObject body)
    {
        Debug.Log("FingerSpread detected with " + this.gameObject);
        resetTransform();
       
    }

    void doOnFist(GameObject body)
    {
        Debug.Log("Fist detected with " + this.gameObject);
        grabObject();

    }

    void doOnRest(GameObject body)
    {
        Debug.Log("Rest detected with " + this.gameObject);

    }

    void doOnWaveIn(GameObject body)
    {
        swingInObject();
        Debug.Log("WaveIn detected with " + this.gameObject);
    }

    void doOnWaveOut(GameObject body)
    {
        swingOutObject(body);
        Debug.Log("WaveOut detected with " + this.gameObject);
    }


	void OnCollisionEnter(Collision other){
        //if (other.gameObject == body) {
        Debug.Log("IsTouched " + other.gameObject.name + " " + this.gameObject.name);
			isTouched = true;
        //this.GetComponent<BoxCollider>().enabled = false;
		//}
	}

	void OnCollisionExit(Collision other){
        //if (other.gameObject == body) {
        Debug.Log("Not IsTouched " + other.gameObject.name + " " + this.gameObject.name);
        isTouched = false;
        //this.GetComponent<BoxCollider>().enabled = true;
        //}
    }

    /*	void OnTriggerStay(Collider col)
        {
            Debug.Log("Collision with " + col.name + " and activated OnTriggerStay");
            //Signal for closing hand
            if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
            {
                Debug.Log("Collision with " + col.name + " while Grab-Signal detected");
                col.attachedRigidbody.isKinematic = true;
                col.gameObject.transform.SetParent(this.gameObject.transform);
                Instantiate(this.gameObject);
            }

            //Signal for opening hand
            if(device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                Debug.Log("Hand opened, " + col.name + " is thrown");
                col.gameObject.transform.SetParent(null);
                col.attachedRigidbody.isKinematic = false;

                tossObject(col.attachedRigidbody);
            }
        }
    */

    private void tossObject(GameObject hand)
    {
        this.gameObject.transform.SetParent(null);
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        this.gameObject.GetComponent<Rigidbody>().velocity = hand.GetComponent<Rigidbody>().velocity;
        this.gameObject.GetComponent<Rigidbody>().angularVelocity = hand.GetComponent<Rigidbody>().angularVelocity;
        /*		Transform origin = parent.origin ? parent.origin : parent.transform.parent;
                if(origin != null)
                {
                    rigidbody.velocity = origin.TransformVector(device.velocity);
                    rigidbody.angularVelocity = origin.TransformVector(device.angularVelocity);
                } else
                {
                    rigidbody.velocity = device.velocity;
                    rigidbody.angularVelocity = device.angularVelocity;
                }*/
    }

    private void grabObject()
    {
        if (isTouched)
        {
            isGrabbed = true;
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            this.gameObject.transform.SetParent(body.gameObject.transform);
            
        }
        else
        {
            resetStates();
        }
    }

    private void swingInObject()
    {
        if (isGrabbed)
        {
            isSwungIn = true;
        }
        else
        {
            resetStates();
        }
    }

    private void swingOutObject(GameObject hand)
    {
        if (isSwungIn)
        {
            tossObject(hand);
        }
        resetStates();
    }

    private void resetStates()
    {
        isSwungIn = false;
        isGrabbed = false;
        isTouched = false;
    }

    private void resetTransform()
    {
        this.transform.position = originalPosition.position;
        this.transform.rotation = originalPosition.rotation;

    }

}
