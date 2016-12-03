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
    }

    void doOnFingerSpread(GameObject body)
    {
        resetTransform();
    }

    void doOnFist(GameObject body)
    {
        grabObject();
    }

    void doOnRest(GameObject body)
    {
    }

    void doOnWaveIn(GameObject body)
    {
        swingInObject();
    }

    void doOnWaveOut(GameObject body)
    {
        swingOutObject(body);
    }


	void OnCollisionEnter(Collision other){
        //if (other.gameObject == body) {
            Debug.Log("Touch");
			isTouched = true;
        grabObject();
		//}
	}

	void OnCollisionExit(Collision other){
		//if (other.gameObject == body) {
			isTouched = false;
            Debug.Log("Untouch");
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
            Debug.Log("Grabbed object");
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
