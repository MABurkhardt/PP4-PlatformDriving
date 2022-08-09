using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRollFrontAxel : MonoBehaviour
{
    WheelCollider WheelL;
    WheelCollider WheelR;
    float AntiRoll = 5000;

    private void FixedUpdate()
    {
        WheelHit wheelHit;
        float travelL = 1.0f;
        float travelR = 1.0f;

        bool groundedL = WheelL.GetGroundHit(out wheelHit);
    }
}