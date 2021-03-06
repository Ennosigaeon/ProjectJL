﻿using UnityEngine;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;

public abstract class MyoPoseController : MonoBehaviour {

    public GameObject myoGameObject = null;
    public float strechFactor = 1f;
    public float minStrechFactor = 0.5f;
    public float maxStrechFactor = 2.5f;
    public KeyCode resetKey = KeyCode.Space;

	//==
	public delegate void MyoAction(GameObject g);
	public static event MyoAction onDoubleTap;
	public static event MyoAction onFingerSpread;
	public static event MyoAction onFist;
	public static event MyoAction onRest;
	public static event MyoAction onWaveIn;
	public static event MyoAction onWaveOut;


    public float speed = 1f;
    public bool updateOnChange = false;
    public float maxOffset = 2f;

    private ThalmicMyo myo = null;
    private Pose lastPose = Pose.Unknown;

    // Use this for initialization
    void Start() {
        Debug.LogError("Myo Pose Controller start");
        myo = myoGameObject.GetComponent<ThalmicMyo>();
        if (myo == null) {
            myo.Unlock(UnlockType.Hold);
        }
    }

    void OnApplicationQuit() {
        Debug.Log("Resetting unlock behaviour.");
        myo.Unlock(UnlockType.Timed);
    }

    // Update is called once per frame
    void Update() {
        Pose pose = myo == null ? Pose.Unknown : myo.pose;
        if (pose == lastPose && updateOnChange)
            return;

        if (pose != lastPose)
            Debug.LogError("Pose: " + pose.ToString());
        lastPose = pose;

        switch (pose) {
			case Pose.DoubleTap:
			//onDoubleTap ();
				if (onDoubleTap != null) {
					onDoubleTap(this.gameObject);
				}
            	break;
           	case Pose.FingersSpread:
            	if (onFingerSpread != null) {
					onFingerSpread(this.gameObject);
				}
				break;
            case Pose.Fist:
				if (onFist != null) {
					onFist(this.gameObject);
				}
            	break;
            case Pose.Rest:
				if (onRest != null) {
					onRest(this.gameObject);
				}
            	break;
            case Pose.Unknown:
            	break;
            case Pose.WaveIn:
				if (onWaveIn != null) {
                    onWaveIn(this.gameObject);
				}
                break;
            case Pose.WaveOut:
				if (onWaveOut != null) {
					onWaveOut(this.gameObject);
				}
            	break;
        }
    }
}
