using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RearRollBar : MonoBehaviour
{
    [SerializeField] Rigidbody carBody;

    [SerializeField] WheelCollider WheelL;
    [SerializeField] WheelCollider WheelR;
    [SerializeField] float AntiRoll = 5000;

    private void FixedUpdate()
    {
        WheelHit wheelHit;
        float travelL = 1.0f;
        float travelR = 1.0f;

        bool groundedL = WheelL.GetGroundHit(out wheelHit);
        if (groundedL)
        {
            travelL = (-WheelL.transform.InverseTransformPoint(wheelHit.point).y - WheelL.radius)
                / WheelL.suspensionDistance;
        }
        bool groundedR = WheelR.GetGroundHit(out wheelHit);
        if (groundedR)
        {
            travelR = (-WheelR.transform.InverseTransformPoint(wheelHit.point).y - WheelR.radius)
                / WheelR.suspensionDistance;
        }

        float antiRollForce = (travelL - travelR) * AntiRoll;

        if (groundedL)
        {
            carBody.AddForceAtPosition(WheelL.transform.up * -antiRollForce, WheelL.transform.position);
        }
        if (groundedR)
        {
            carBody.AddForceAtPosition(WheelR.transform.up * antiRollForce, WheelR.transform.position);
        }
    }
}
