using UnityEngine;
using System.Collections;
using Windows.Kinect;
using System;


public class DetectSkeleton : MonoBehaviour {

    public GameObject BodySrcManager;

    public float scalingFactor;


    public Transform rightHandObj = null;
    public Transform leftHandObj = null;
    public Transform leftFootObj = null;
    public Transform rightFootObj = null;

    // Default variable for source manager. Takes care of connection to kinect.
    private BodySourceManager b_src_man;
    // Field for all bodies. Gets field by each frame of the kinect.
    private Body[] bodies;

    // Use this for initialization
    void Start () {

        // get the default body source manager
        b_src_man = BodySrcManager.GetComponent<BodySourceManager>();

    }

    float g_x = 0, g_y = 0, g_z = 0;

    // Update is called once per frame
    void Update()
    {
        string joint_name;
        float x, y, z;

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

            var hip_x = (body.Joints[JointType.HipLeft].Position.X + body.Joints[JointType.HipRight].Position.X) / 2;
            var hip_y = (body.Joints[JointType.HipLeft].Position.Y + body.Joints[JointType.HipRight].Position.Y) / 2;
            var hip_z = (body.Joints[JointType.HipLeft].Position.Z + body.Joints[JointType.HipRight].Position.Z) / 2;


            foreach (var j in body.Joints)
            {
                
                //Debug.Log(j.Key.ToString());
                joint_name = j.Key.ToString();

                if ( joint_name == "WristRight")
                {
                    x = j.Value.Position.X;
                    y = j.Value.Position.Y;
                    z = j.Value.Position.Z;

                    if (x != 0 && y != 0 && y != 0)
                    {
                        
                        
                        g_x = x;
                        g_y = y;
                        g_z = z;
                        rightHandObj.transform.position = new Vector3(g_x - hip_x, g_y - hip_y, -1* (g_z - hip_z));
                        Debug.Log("New Wrist Right has position: (" + (g_x - hip_x) + "," + (g_y - hip_y) + "," + (-1 * (g_z - hip_z)) + ")");
                        //model_to_move.transform.position = new Vector3(g_x * scalingFactor, g_y * scalingFactor, g_z * scalingFactor);
                    }
                    
                }
                // FootLeft, FootRight
                // WristLeft, WristRight
                // HipLeft, HipRight --> Mittelpunkt
            }


        }
    }
}
