using UnityEngine;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;

public class MyoPoseController : MonoBehaviour {

    public GameObject myoGameObject = null;
    public float strechFactor = 1f;
    public float minStrechFactor = 0.5f;
    public float maxStrechFactor = 2.5f;
    public KeyCode resetKey = KeyCode.Space;

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
                onDoubleTap();
                break;
            case Pose.FingersSpread:
                onFingerSpread();
                break;
            case Pose.Fist:
                onFist();
                break;
            case Pose.Rest:
                onRest();
                break;
            case Pose.Unknown:
                onUnknown();
                break;
            case Pose.WaveIn:
                onWaveIn();
                break;
            case Pose.WaveOut:
                onWaveOut();
                break;
        }
    }

    private void reset() {
        Vector3 position = Vector3.zero;
        position.y = 1;
        transform.position = position;
        transform.localScale = Vector3.one;
    } 

    private void onDoubleTap() {
    }

    private void onFingerSpread() {
        float stretch = System.Math.Min(gameObject.transform.localScale.x + strechFactor, maxStrechFactor);
        transform.localScale = new Vector3(stretch, stretch, stretch);
    }

    private void onFist() {
        float stretch = System.Math.Max(gameObject.transform.localScale.x - strechFactor, minStrechFactor);
        transform.localScale = new Vector3(stretch, stretch, stretch);
    }

    private void onRest() {
    }

    private void onUnknown() {
    }

    private void onWaveIn() {
        Vector3 position = gameObject.transform.position;
        position.x -= position.x <= -maxOffset ? 0 : speed;
        transform.position = position;
    }

    private void onWaveOut() {
        Vector3 position = gameObject.transform.position;
        position.x += position.x >= maxOffset ? 0 : speed;
        transform.position = position;
    }

}
