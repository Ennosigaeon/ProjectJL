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
        if (isGrabbed)
        {
            releaseObject(body);
        }
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
        //swingInObject();
    }

    void doOnWaveOut(GameObject body)
    {
        resetTransform();
    }

	void OnCollisionEnter(Collision other){
        Debug.Log("Touch");
        lastTouch = Time.frameCount;
        
	}

	void OnCollisionExit(Collision other){
    }

    private void releaseObject(GameObject hand)
    {
        Debug.Log("Releassing object");
        this.gameObject.transform.SetParent(null);
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        isGrabbed = false;
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

    private void resetTransform()
    {
        this.transform.position = originalPosition.position;
        this.transform.rotation = originalPosition.rotation;
    }

}
