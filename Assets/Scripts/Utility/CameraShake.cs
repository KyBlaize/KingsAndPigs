using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Animator cameraAnimator;

    public void Shake()
    {
        cameraAnimator.SetTrigger("shake");
    }
}
