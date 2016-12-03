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

            float scal_avatar = 7.5f;
            float scal_kinect = body.Joints[JointType.Head].Position.Y - body.Joints[JointType.FootRight].Position.Y;

            Debug.Log("We got scal_avatar with " + scal_avatar + " and scal_kinect with " + scal_kinect);
            float scal_overall = scal_avatar / scal_kinect;
            // - 
            var hip_x = (body.Joints[JointType.HipLeft].Position.X + body.Joints[JointType.HipRight].Position.X) / 2;
            var hip_y = (body.Joints[JointType.HipLeft].Position.Y + body.Joints[JointType.HipRight].Position.Y) / 2;
            var hip_z = (body.Joints[JointType.HipLeft].Position.Z + body.Joints[JointType.HipRight].Position.Z) / 2;

            Debug.Log(GameObject.Find("head").transform.position);
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
                        
                        
                        g_x = (GameObject.Find("hips").transform.position.x + scal_overall * (x - hip_x));
                        g_y = (GameObject.Find("hips").transform.position.y + scal_overall * (y - hip_y)); ;
                        g_z = (GameObject.Find("hips").transform.position.z + (-scal_overall) * (z - hip_z)); ;
                        rightHandObj.transform.position = new Vector3(g_x, g_y, g_z);
                        Debug.Log("New Wrist Right has position: (" + (g_x) + "," + (g_y) + "," + (g_z ) + ")");
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
