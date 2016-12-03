using UnityEngine;
using System.Collections;
using Windows.Kinect;
using System;
using System.Collections.Generic;


public class DetectSkeleton : MonoBehaviour
{

    public GameObject BodySrcManager;

    public int sliding_window_size;

    public Transform rightHandObj = null;
    public Transform leftHandObj = null;
    public Transform leftFootObj = null;
    public Transform rightFootObj = null;

    // Default variable for source manager. Takes care of connection to kinect.
    private BodySourceManager b_src_man;
    // Field for all bodies. Gets field by each frame of the kinect.
    private Body[] bodies;
    // This holds the JointTypes and the according game objects for the Inverse Kinematics.
    private Dictionary<JointType, Transform> descriptors;
    private Dictionary<JointType, SlidingWindow> sliders;
    // Use this to hold the scale. Set Once.
    Vector3 scal_overall_v = Vector3.zero;
    // Use a vector for the initial kinect hip position
    Vector3 initial_hip_k = Vector3.zero;
    // Globla Array to hold sliding window frame.
    private SlidingWindow sliding_window;


    // Use this for initialization
    void Start()
    {

        // get the default body source manager
        b_src_man = BodySrcManager.GetComponent<BodySourceManager>();

        descriptors = new Dictionary<JointType, Transform>()
            {
                {JointType.WristRight, rightHandObj },
                {JointType.WristLeft, leftHandObj },
                {JointType.AnkleRight, rightFootObj },
                {JointType.AnkleLeft, leftFootObj }
            };

        sliders = new Dictionary<JointType, SlidingWindow>()
            {
                {JointType.WristRight, new SlidingWindow(sliding_window_size) },
                {JointType.WristLeft, new SlidingWindow(sliding_window_size) },
                {JointType.AnkleRight, new SlidingWindow(sliding_window_size) },
                {JointType.AnkleLeft, new SlidingWindow(sliding_window_size) }
            };

        sliding_window = new SlidingWindow(sliding_window_size);

    }


    // Update is called once per frame
    void Update()
    {

        if (b_src_man == null)
        {
            Debug.LogError("BodySrcManager not found");
            return;
        }
        bodies = b_src_man.GetData();

        if (bodies == null)
        {
            return;
        }

        foreach (var body in bodies)
        {

            if (body == null)
            {
                continue;
            }

            // Compute scale only upon first frame with actual numbers:
            if (scal_overall_v == Vector3.zero &&
                cameraSpacePointtoVector(body.Joints[JointType.Head].Position) != Vector3.zero &&
                cameraSpacePointtoVector(body.Joints[JointType.AnkleRight].Position) != Vector3.zero &&
                cameraSpacePointtoVector(body.Joints[JointType.HipLeft].Position) != Vector3.zero &&
                cameraSpacePointtoVector(body.Joints[JointType.HipRight].Position) != Vector3.zero)
            {
                Debug.Log("Inital Setup");
                // MAGIC NUMBER
                float scal_avatar = 7.5f;
                float scal_kinect = body.Joints[JointType.Head].Position.Y - body.Joints[JointType.AnkleRight].Position.Y;

                // Compute scale from kinect coordiante system to unity coordiante system.
                float scal_overall = scal_avatar / scal_kinect;
                scal_overall_v = new Vector3(scal_overall, scal_overall, -scal_overall);

                initial_hip_k = new Vector3(
                    (body.Joints[JointType.HipLeft].Position.X + body.Joints[JointType.HipRight].Position.X) / 2,
                    (body.Joints[JointType.HipLeft].Position.Y + body.Joints[JointType.HipRight].Position.Y) / 2,
                    (body.Joints[JointType.HipLeft].Position.Z + body.Joints[JointType.HipRight].Position.Z) / 2
                    );
            }
            else if (scal_overall_v == Vector3.zero)
            {
                continue;
            }
            
            // Get the coordinate for the Hip form the kinect
            var hip_coordinates_k = new Vector3(
                (body.Joints[JointType.HipLeft].Position.X + body.Joints[JointType.HipRight].Position.X) / 2,
                (body.Joints[JointType.HipLeft].Position.Y + body.Joints[JointType.HipRight].Position.Y) / 2,
                (body.Joints[JointType.HipLeft].Position.Z + body.Joints[JointType.HipRight].Position.Z) / 2
                );

            // First, re-compute the position of the hip and move avatar
            if (hip_coordinates_k == Vector3.zero)
            {
                continue;
            }
            GameObject.Find("female").transform.position =  hip_coordinates_k - initial_hip_k;
            Debug.Log(hip_coordinates_k - initial_hip_k);
            Debug.Log("Pure kinrect: " + hip_coordinates_k);

            // Get the coordinate for the Hip from unity
            var hip_coordinates_u = new Vector3(
                GameObject.Find("hips").transform.position.x,
                GameObject.Find("hips").transform.position.y,
                GameObject.Find("hips").transform.position.z
                );
            // Iterate over the dictionary
            foreach (KeyValuePair<JointType, Transform> pair in descriptors)
            {
                // get the current position from the kinect
                var p_k = body.Joints[pair.Key].Position;
                var coordinates_k = new Vector3(p_k.X, p_k.Y, p_k.Z);
                // if there are no new frames, continue
                if (coordinates_k == Vector3.zero)
                {
                    continue;
                }
                else
                {
                    // THIS IS WHERE THE MAGIC HAPPENS!
                    /*
                     * We simply use the body scale from unity and map the distance vector from kinect_hip to kinect_hand positions
                     * and add this vector to the unity hip position.
                     * Easy as fuck.
                     */
                    //sliders[pair.Key].push(coordinates_k);
                    sliders[pair.Key].push(hip_coordinates_u + vec3_multiply(scal_overall_v, (coordinates_k - hip_coordinates_k)));
                    //pair.Value.transform.position = hip_coordinates_u + vec3_multiply(scal_overall_v, (sliders[pair.Key].pop() - hip_coordinates_k));
                    pair.Value.transform.position = sliders[pair.Key].pop();
                }


            }
        }
    }

    private Vector3 vec3_multiply(Vector3 one, Vector3 two)
    {
        return new Vector3(
            one.x * two.x,
            one.y * two.y,
            one.z * two.z);
    }

    private Vector3 cameraSpacePointtoVector(CameraSpacePoint pos)
    {
        return new Vector3(pos.X, pos.Y, pos.Z);
    }
}
