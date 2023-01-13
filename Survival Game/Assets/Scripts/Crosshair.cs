using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private float gunAccuracy; //크로스헤어상태에따른총의정확도

    [SerializeField]
    private GameObject go_CrosshairHUD; //크로스헤어비활성화를위한부모객체


    public void WalkingAnimation(bool _flag)
    {
        animator.SetBool("Walking", _flag);
    }
    public void RunningAnimation(bool _flag)
    {
        animator.SetBool("Running", _flag);
    }
    public void CrouchingAnimation(bool _flag)
    {
        animator.SetBool("Crouching", _flag);
    }
 
}
