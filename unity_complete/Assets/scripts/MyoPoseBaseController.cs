using UnityEngine;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;

public abstract class MyoPoseBaseController : MonoBehaviour {

    public GameObject myoGameObject = null;
    public float strechFactor = 1f;
    public float minStrechFactor = 0.5f;
    public float maxStrechFactor = 2.5f;
    public KeyCode resetKey = KeyCode.Space;

	//==
	public delegate void MyoAction();
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
        if (Input.GetKey(resetKey))
            reset();

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
					onDoubleTap();
				}
            	break;
           	case Pose.FingersSpread:
            	if (onDoubleTap != null) {
					onDoubleTap();
				}
				break;
            case Pose.Fist:
				if (onDoubleTap != null) {
					onDoubleTap();
				}
            	break;
            case Pose.Rest:
				if (onDoubleTap != null) {
					onDoubleTap();
				}
            	break;
            case Pose.Unknown:
                onUnknown();
            	break;
            case Pose.WaveIn:
				if (onDoubleTap != null) {
					onDoubleTap();
				}
                break;
            case Pose.WaveOut:
				if (onDoubleTap != null) {
					onDoubleTap();
				}
            	break;
        }
    }
}
