using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccelerometerRotate : MonoBehaviour {

    public float speed = 10.0f;
    public float maxAngle = 24f;
    private Vector3 v3To;
    public Vector3 v3Current;
    public Text textLabel;

    void Start()
    {
        v3To = new Vector3(0, 0, 0);
        v3Current = new Vector3(0, 0, 0);
        transform.eulerAngles = v3Current;
    }

    // Update is called once per frame
    void Update () {
        //if(SystemInfo.supportsAccelerometer)
        //{
        //    v3To.z = -Input.acceleration.x;
        //}
        //else
        //{
        //    float h = Input.GetAxis("Horizontal");
        //    v3To.z = h;
        //}
        //GetComponent<Rigidbody2D>().AddTorque(v3To.z, ForceMode2D.Force);
        
        if(SystemInfo.supportsAccelerometer)
        {
            if(Mathf.Abs(Input.acceleration.x) >= 0.2f)
            {
                if(v3To.z > -maxAngle && v3To.z < maxAngle)
                    v3To.z += Input.acceleration.x;
            }
            else if (v3To.z != 0.0f)
            {
                v3To = Vector3.Lerp(v3To, Vector3.zero, Time.deltaTime * speed/4f);
            }
        }
        else
        {
            float h = Input.GetAxis("Horizontal");
            if (h != 0)
            {
                if (v3To.z > -maxAngle && v3To.z < maxAngle)
                    v3To.z += h;
            }
            else if (v3To.z != 0.0f)
            {
                v3To = Vector3.Lerp(v3To, Vector3.zero, Time.deltaTime * speed/4f);
            }
        }
        
        v3Current = Vector3.Lerp(v3Current, v3To, Time.deltaTime * speed);
        transform.eulerAngles = v3Current;
        textLabel.text = "S: " + SystemInfo.supportsAccelerometer.ToString() + " XYZ: " + Input.acceleration.ToString() + " current: " + v3To.ToString();
    }
}
