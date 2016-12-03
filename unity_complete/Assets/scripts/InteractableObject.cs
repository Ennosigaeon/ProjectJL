using UnityEngine;
using System.Collections;

public class InteractableObject : MonoBehaviour
{
	public GameObject body = null;
    private bool isGrabbed = false;
    private Transform originalPosition;
    private int lastTouch = 0;
    private int lastSwungIn = 0;

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
        Debug.Log("Touch");
        lastTouch = Time.frameCount;
	}

	void OnCollisionExit(Collision other){
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

    private void releaseObject(GameObject hand)
    {
        Debug.Log("Releassing object");
        this.gameObject.transform.SetParent(null);
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        this.gameObject.GetComponent<Rigidbody>().velocity = hand.GetComponent<Rigidbody>().velocity;
        this.gameObject.GetComponent<Rigidbody>().angularVelocity = hand.GetComponent<Rigidbody>().angularVelocity;
        isGrabbed = false;
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
        if (Time.frameCount - lastTouch < 20)
        {
            isGrabbed = true;
            Debug.Log("Grabbed object");
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            this.gameObject.transform.SetParent(body.gameObject.transform);
            
        }
    }

    private void swingInObject()
    {
        if (isGrabbed)
        {
            lastSwungIn = Time.frameCount;
        }
    }

    private void swingOutObject(GameObject hand)
    {
        if (Time.frameCount - lastSwungIn < 200)
        {
            releaseObject(hand);
        }
    }

    private void resetTransform()
    {
        this.transform.position = originalPosition.position;
        this.transform.rotation = originalPosition.rotation;
    }

}
