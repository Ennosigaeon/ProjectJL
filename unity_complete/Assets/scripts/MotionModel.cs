using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class MotionModel : MonoBehaviour
{

    protected Animator animator;

    public bool ikActive = false;
    public Transform rightHandObj = null;
    public Transform leftHandObj = null;
    public Transform leftFootObj = null;
    public Transform rightFootObj = null;

    public Transform rightKneeObj = null;
    public Transform leftKneeObj = null;
    public Transform rightElbowObj = null;
    public Transform leftElbowObj = null;

    public Transform lookObj = null;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if (animator)
        {

            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive)
            {

                // Set the look target position, if one has been assigned
                if (lookObj != null)
                {
                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(lookObj.position);
                }

                // Set the right hand target position and rotation, if one has been assigned
                if (rightHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
                    if ( rightElbowObj != null)
                    {
                        animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 0.5f);
                        animator.SetIKHintPosition(AvatarIKHint.RightElbow, rightElbowObj.position);
                    }
                }
                if (leftHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
                    if (leftElbowObj != null)
                    {
                        animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 0.5f);
                        animator.SetIKHintPosition(AvatarIKHint.LeftElbow, leftElbowObj.position);
                    }
                }
                if (rightFootObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
                    animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootObj.position);
                    animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootObj.rotation);
                    if (rightKneeObj != null)
                    {
                        animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, 0.05f);
                        animator.SetIKHintPosition(AvatarIKHint.RightKnee, rightKneeObj.position);
                    }
                }
                if (leftFootObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
                    animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootObj.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootObj.rotation);
                    if (leftKneeObj != null)
                    {
                        animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, 0.05f);
                        animator.SetIKHintPosition(AvatarIKHint.LeftKnee, leftKneeObj.position);
                    }
                }
            }

            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                animator.SetLookAtWeight(0);
            }
        }
    }
}
