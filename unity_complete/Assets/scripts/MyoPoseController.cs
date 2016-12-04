using UnityEngine;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;

public class MyoPoseController : MonoBehaviour {

    public GameObject myoGameObject = null;
    public KeyCode resetKey = KeyCode.Space;
    public GameObject target = null;

	//==
	public delegate void MyoAction(GameObject g);
	public static event MyoAction onDoubleTap;
	public static event MyoAction onFingerSpread;
	public static event MyoAction onFist;
	public static event MyoAction onRest;
	public static event MyoAction onWaveIn;
	public static event MyoAction onWaveOut;

    private ThalmicMyo myo = null;
    private Pose lastPose = Pose.Unknown;

    // Use this for initialization
    void Start() {
        Debug.Log("Myo Pose Controller start");
        myo = myoGameObject.GetComponent<ThalmicMyo>();
        if (myo == null) {
            myo.Unlock(UnlockType.Hold);
        }
    }

    //void OnApplicationQuit() {
    //    Debug.Log("Resetting unlock behaviour.");
    //    if (myo != null)
    //        myo.Unlock(UnlockType.Timed);
    // }

    // Update is called once per frame
    void Update() {
        Pose pose = myo == null ? Pose.Unknown : myo.pose;
        if (pose == lastPose)
            return;

        Debug.Log("Pose: " + pose.ToString());
        lastPose = pose;

        switch (pose) {
			case Pose.DoubleTap:
			//onDoubleTap ();
				if (onDoubleTap != null) {
					onDoubleTap(this.target);
				}
            	break;
           	case Pose.FingersSpread:
            	if (onFingerSpread != null) {
                    GameObject.Find("female").GetComponent<Animator>().Play("open_right_hand");
                    onFingerSpread(this.target);
				}
				break;
            case Pose.Fist:
				if (onFist != null) {
                    GameObject.Find("female").GetComponent<Animator>().Play("close_right_hand");
                    onFist(this.target);
				}
            	break;
            case Pose.Rest:
				if (onRest != null) {
					onRest(this.target);
				}
            	break;
            case Pose.Unknown:
            	break;
            case Pose.WaveIn:
				if (onWaveIn != null) {
                    onWaveIn(this.target);
				}
                break;
            case Pose.WaveOut:
				if (onWaveOut != null) {
					onWaveOut(this.target);
				}
            	break;
        }
    }
}
