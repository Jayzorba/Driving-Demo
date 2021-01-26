using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

    private float m_VerticalInput;
    private float m_HorizontalInput;
    private float m_steeringAngle;

    private WheelFrictionCurve resetwfc;
    private WheelFrictionCurve wfcSideways;


    public WheelCollider frontLeftW, frontRightW;
    public WheelCollider backLeftW, backRightW;
    public Transform frontLeftT, frontRightT;
    public Transform backLeftT, backRightT;
    public float maxSteerAngle = 45;
    public float motorForce = 1000;
    public float driftTime = 0f;
    public float driftComplete = 0.5f;
    public bool drifting = false;


    private void Start()
    {
        wfcSideways = new WheelFrictionCurve();
        wfcSideways.extremumSlip = 2f;
        wfcSideways.extremumValue = 2f;
        wfcSideways.asymptoteSlip = 1f;
        wfcSideways.asymptoteValue = 2f;
        wfcSideways.stiffness = 0.45f;

        resetwfc = new WheelFrictionCurve();
        resetwfc.extremumSlip = 1f;
        resetwfc.extremumValue = 1f;
        resetwfc.asymptoteSlip = 0.5f;
        resetwfc.asymptoteValue = 1f;
        resetwfc.stiffness = 2.5f;
    }
    public void GetInput()
    {

        m_HorizontalInput = Input.GetAxis("Horizontal");
        m_VerticalInput = Input.GetAxis("Vertical");

    }

    private void Steer()
    {

        m_steeringAngle = maxSteerAngle * m_HorizontalInput;
        frontRightW.steerAngle = m_steeringAngle;
        frontLeftW.steerAngle = m_steeringAngle;




      //  if (m_HorizontalInput > 0 )

            if(Input.GetKey(KeyCode.LeftShift))
        {
            //driftTime = 0f;
            backLeftW.sidewaysFriction = wfcSideways;
            backRightW.sidewaysFriction = wfcSideways;
            motorForce = 800;
            drifting = true;
        }




       // else if (driftTime >= driftComplete)

        else/*(Input.GetKeyUp(KeyCode.LeftShift))*/
        {
            backLeftW.sidewaysFriction = resetwfc;
            backRightW.sidewaysFriction = resetwfc;
            motorForce = 2500;
            new WaitForSeconds(5f);
            motorForce = 1000;
           // driftTime = 0f;
            drifting = false;
        }




    }

    private void Accelerate()
    {

         backRightW.motorTorque = m_VerticalInput * motorForce;
         backLeftW.motorTorque = m_VerticalInput * motorForce;

    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontRightW, frontRightT);
        UpdateWheelPose(frontLeftW, frontLeftT);
        UpdateWheelPose(backRightW, backRightT);
        UpdateWheelPose(backLeftW, backLeftT);
    }

    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;


    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();
        driftTime += Time.deltaTime;
    }
}
