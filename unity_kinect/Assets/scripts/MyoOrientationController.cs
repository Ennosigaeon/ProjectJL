using UnityEngine;
using System.Collections;

public class MyoOrientationController : MonoBehaviour
{

    public GameObject myoGameObject = null;
    public KeyCode resetKey = KeyCode.Space;
    private ThalmicMyo myo = null;
    private Quaternion antiYaw = Quaternion.identity;
    private float referenceRoll = 0.0f;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Myo Orientation Controller start");
        myo = myoGameObject.GetComponent<ThalmicMyo>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(resetKey))
            reset();

        Quaternion orientation = getOrientation();
        processOrientation(orientation);
    }

    void processOrientation(Quaternion orientation)
    {
        transform.rotation = orientation;
    }

    private void reset()
    {
        antiYaw = Quaternion.FromToRotation(
            new Vector3(myo.transform.forward.x, 0, myo.transform.forward.z),
            new Vector3(0, 0, 1)
        );
        Vector3 referenceZeroRoll = computeZeroRollVector();
        referenceRoll = rollFromZero(referenceZeroRoll, myo.transform.forward, myo.transform.up);
    }

    private Quaternion getOrientation()
    {
        if (myo == null)
            return Quaternion.identity;

        Vector3 zeroRoll = computeZeroRollVector();
        float roll = rollFromZero(zeroRoll, myo.transform.forward, myo.transform.up);

        float relativeRoll = roll - referenceRoll;
        if (relativeRoll > 180.0f)
            relativeRoll -= 360.0f;
        if (relativeRoll < -180.0f)
            relativeRoll += 360.0f;

        Debug.LogError(relativeRoll);

        Quaternion antiRoll = Quaternion.AngleAxis(relativeRoll, myo.transform.forward);
        return antiYaw * antiRoll * Quaternion.LookRotation(myo.transform.forward);
    }

    private Vector3 computeZeroRollVector()
    {
        Vector3 antigravity = Vector3.up;
        Vector3 m = Vector3.Cross(myo.transform.forward, antigravity);
        Vector3 roll = Vector3.Cross(m, myo.transform.forward);

        return roll.normalized;
    }

    private float rollFromZero(Vector3 zeroRoll, Vector3 forward, Vector3 up)
    {
        float cosine = Vector3.Dot(up, zeroRoll);
        Vector3 cp = Vector3.Cross(up, zeroRoll);
        float directionCosine = Vector3.Dot(forward, cp);
        float sign = directionCosine < 0.0f ? 1.0f : -1.0f;
        return sign * Mathf.Rad2Deg * Mathf.Acos(cosine);
    }
}
