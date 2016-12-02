using UnityEngine;
using System.Collections;

public class interactableObject : MonoBehaviour
{
    private bool isTouched = false;

    void OnEnable()
    {
        MyoPoseBaseController.onDoubleTap += doOnDoubleTap;
        MyoPoseBaseController.onFingerSpread += doOnFingerSpread;
        MyoPoseBaseController.onFist += doOnFist;
        MyoPoseBaseController.onRest += doOnRest;
        MyoPoseBaseController.onWaveIn += doOnWaveIn;
        MyoPoseBaseController.onWaveOut += doOnWaveOut;
    }

    void doOnDoubleTap(GameObject body)
    {
        Debug.Log("DoubleTab detected with " + this.gameObject);

    }

    void doOnFingerSpread(GameObject body)
    {
        Debug.Log("FingerSpread detected with " + this.gameObject);
        this.gameObject.transform.SetParent(null);
        //		this.attachedRigidbody.isKinematic = false;
        //TODO body als richtiges Argument uebergeben
        //		tossObject(this.attachedRigidbody, body.gameObject);
    }

    void doOnFist(GameObject body)
    {
        Debug.Log("Fist detected with " + this.gameObject);
        if (isTouched)
        {
            //			this.attachedRigidbody.isKinematic = true;
            //			this.gameObject.transform.SetParent(body.gameObject.transform);
            //TODO hier Element neu erzeugen oder durch andere Geste?
            Instantiate(this.gameObject);
        }
    }

    void doOnRest(GameObject body)
    {
        Debug.Log("Rest detected with " + this.gameObject);

    }

    void doOnWaveIn(GameObject body)
    {
        Debug.Log("WaveIn detected with " + this.gameObject);

    }

    void doOnWaveOut(GameObject body)
    {
        Debug.Log("WaveOut detected with " + this.gameObject);

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

    private void tossObject(Rigidbody rigidbody, GameObject parent)
    {
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
}
