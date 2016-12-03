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


            foreach(var j in body.Joints)
            {
                
                //Debug.Log(j.Key.ToString());
                joint_name = j.Key.ToString();

                if ( joint_name == "WristRight")
                {
                    x = j.Value.Position.Z;
                    y = j.Value.Position.Y;
                    z = j.Value.Position.X;

                    if (x != 0 && y != 0 && y != 0)
                    {
                        
                        Debug.Log("New Wrist Right has position: (" + x + "," + y + "," + z + ")");
                        g_x = x;
                        g_y = y;
                        g_z = z;
                        rightHandObj.transform.position = new Vector3(g_x * scalingFactor, g_y * scalingFactor, g_z * scalingFactor);
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
