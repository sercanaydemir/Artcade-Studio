using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{
    [Header("Değişkenler")]
    public float force;
    public float brakeForce;
    public float dragSensitive;
    public float maxV;
    private float radius;
    private Rigidbody rb;
    private bool isFinished;

    [Space]
    public Transform groundCheck;
    [Space]
    public LayerMask finishLayer;

    [Header("Wheel Colliders")]
    public WheelCollider w1;
    public WheelCollider w2;
    public WheelCollider w3;
    public WheelCollider w4;
    //-----WheelObject-------------------------
    [Header("WheelGFX Objesi")]
    public GameObject t1;
    public GameObject t2;
    public GameObject t3;
    public GameObject t4;

    public Transform ForceCenter;

    //-----Rotater Vector----------------------
    private Vector3 t1Vec, t2Vec, t3Vec, t4Vec;
    //-----Scale Vector------------------------
    private Vector3 t1Scale, t2Scale, t3Scale, t4Scale;

    private RaycastHit hit;

    [Header("Enemy Değişkenleri")]
    [Space]
    public Transform rayPointLeft, rayPointRight, rayPointTop;
    public float rayDistance;
    private float timer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = GameObject.FindGameObjectWithTag("GroundCheck").transform;
        
        t1Scale = t1.transform.localScale;
        t2Scale = t2.transform.localScale;
        t3Scale = t3.transform.localScale;
        t4Scale = t4.transform.localScale;

    }

    private void FixedUpdate()
    {
        Finish();
        Controller();
        Movement();

    }

    private void Finish()
    {
        if (Physics.Raycast(groundCheck.position, new Vector3(transform.position.x, transform.position.y - 10, transform.position.z),
            out hit,10,finishLayer))
        {
            Debug.Log("Finish");
            isFinished = true;
        }
    }

    private void Movement()
    {
        if (!isFinished)
        {
            //w1.motorTorque = force;
            //w2.motorTorque = force;
            w3.motorTorque = force;
            w4.motorTorque = force;

            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, Mathf.Clamp(rb.velocity.z, 0, maxV));
            
            
            //-------Wheel Rotater------------------------------
            Quaternion rot;
            Vector3 pos;

            w1.GetWorldPose(out pos, out rot);
            t1.transform.position = pos;
            t1.transform.rotation = rot;

            w2.GetWorldPose(out pos, out rot);
            t2.transform.position = pos;
            t2.transform.rotation = rot;

            w3.GetWorldPose(out pos, out rot);
            t3.transform.position = pos;
            t3.transform.rotation = rot;

            w4.GetWorldPose(out pos, out rot);
            t4.transform.position = pos;
            t4.transform.rotation = rot;

            

            //---------Scale Changer------------------------
            
            float wRadius = w1.radius = w2.radius = w3.radius = w4.radius;
            float tYScale = t1.transform.localScale.y;
            float tZScale = t1.transform.localScale.z;

            if (!(tYScale == wRadius*2) && !(tZScale == wRadius*2))
            {
                tYScale = wRadius * 2;
                tZScale = wRadius * 2;

                t1Scale.x = t2Scale.x = t3Scale.x = t4Scale.x = 1;
                t1Scale.y = t2Scale.y = t3Scale.y = t4Scale.y = tYScale;
                t1Scale.z = t2Scale.z = t3Scale.z = t4Scale.z = tZScale;

                t1.transform.localScale = t1Scale;
                t2.transform.localScale = t2Scale;
                t3.transform.localScale = t3Scale;
                t4.transform.localScale = t4Scale;

                
            }

        }
        else if (isFinished)
        {
            rb.drag = 0.8f;
            w1.brakeTorque = brakeForce ;
            w2.brakeTorque = brakeForce ;
            //w3.brakeTorque = brakeForce;
            //w4.brakeTorque = brakeForce;
            rb.constraints = RigidbodyConstraints.None;
        }
            
    }

    private void Controller()
    {
        //-----------Player Controller---------------------
        if (!isFinished && gameObject.tag == "Player")
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (TouchPhase.Moved == touch.phase)
                {
                    Vector3 deltaPosition = touch.deltaPosition * dragSensitive;

                    radius = deltaPosition.x * Time.deltaTime;

                    w1.radius = w2.radius = w3.radius = w4.radius += radius;

                    float wRadius = w1.radius = w2.radius = w3.radius = w4.radius;

                    wRadius = Mathf.Clamp(wRadius, 0.4f, 0.9f);
                    
                    w1.radius = w2.radius = w3.radius = w4.radius = wRadius;
                    
                }

            }
        }

        //--------Enemy Controller---------------------------
        else if(!isFinished && gameObject.tag == "Enemy")
        {
            if (Physics.Raycast(rayPointLeft.position, Vector3.forward,out hit , rayDistance))
            {
                timer = 0;

                radius = hit.distance * Time.deltaTime;

                w1.radius = w2.radius = w3.radius = w4.radius += radius;

                float wRadius = w1.radius = w2.radius = w3.radius = w4.radius;

                wRadius = Mathf.Clamp(wRadius, 0.4f, 0.9f);

                w1.radius = w2.radius = w3.radius = w4.radius = wRadius;

                Debug.Log("Workk");
            }

            else if (!Physics.Raycast(rayPointLeft.position, Vector3.forward, out hit, rayDistance))
            {
                timer += Time.deltaTime;
                if (timer>=2)
                {
                    radius = -rayDistance * Time.deltaTime;

                    w1.radius = w2.radius = w3.radius = w4.radius += radius;

                    float wRadius = w1.radius = w2.radius = w3.radius = w4.radius;

                    wRadius = Mathf.Clamp(wRadius, 0.4f, 0.9f);

                    w1.radius = w2.radius = w3.radius = w4.radius = wRadius;

                    Debug.Log("Workk");
                }
                
            }
        }

        if (!isFinished && gameObject.tag == "Enemy")
        {
            if (Physics.Raycast(rayPointTop.position, Vector3.forward, out hit, rayDistance))
            {
                radius = hit.distance * Time.deltaTime;
                
                w1.radius = w2.radius = w3.radius = w4.radius -= radius;

                float wRadius = w1.radius = w2.radius = w3.radius = w4.radius;

                wRadius = Mathf.Clamp(wRadius, 0.4f, 0.9f);

                w1.radius = w2.radius = w3.radius = w4.radius = wRadius;
            }
        }
       
    }

    public void Restart()
    {
        SceneManager.LoadScene("Gameplay");

    }
  

}
