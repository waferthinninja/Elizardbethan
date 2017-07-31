using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    public Transform DeadFlyParent; // this will be the collider object that we attach flies to when they are caught
    public Collider2D TongueCollider;
    public float MinAngle = -45; // furthest the head can rotate, in degrees
    public float MaxAngle = 45; // furthest the head can rotate, in degrees
    public float RotationSpeed = 200; // Only relevant in "axis" aiming mode

    private Animator _animator;
    public bool IsFiring { get; private set; }

    private bool _mouseAiming = false;

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
        IsFiring = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsFiring)
        {
            if (_mouseAiming)
            {
                LookAtMouse();
            }
            else
            {
                // key input
                float yAxis = Input.GetAxis("Vertical");
                float angle = transform.eulerAngles.z + (yAxis * RotationSpeed * Time.deltaTime);
                if (angle > 90) angle -= 360; // turn angle like 345 to be -15                
                angle = Mathf.Clamp(angle, MinAngle, MaxAngle);
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }

            if (Input.GetButton("Fire1"))
            {
                ShootTongue();
            }
        }
        
        if (Input.GetKeyDown("m"))
        {
            _mouseAiming = !_mouseAiming;
            Debug.Log(string.Format("Switching to {0} aiming", _mouseAiming ? "mouse" : "axis"));
        }

    }

    private void LookAtMouse()
    {
        // look at the mouse        
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, MinAngle, MaxAngle);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }


    private void ShootTongue()
    {
        IsFiring = true;
        TongueCollider.enabled = true;
        _animator.Play("TongueLash");
    }

    public void ConsumeFlies()
    {
        foreach (Transform t in DeadFlyParent)
        {
            Destroy(t.gameObject); // replace with pooling at some point
                                   
            // do other stuff - give score, energy or whatever
            GameManager.Instance.ConsumeFly();
        }

        // turn off the collider
        TongueCollider.enabled = false;

        // reactivate head movement
        IsFiring = false;

        
    }
}
