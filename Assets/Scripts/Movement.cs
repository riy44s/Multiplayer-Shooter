using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 14f;
    [SerializeField] private float maxVelocityChange = 10f;

    [Space]
    [SerializeField] float airControl = 0.5f;

    [Space]
    [SerializeField] float jumpHeight = 10f;

    private Vector2 input;
    private Rigidbody rb;

    private bool sprinting;
    private bool jumping;

    private bool grounded = false;

    public AudioSource leftFoot;
    public AudioSource rightFoot;
    public AudioClip[] footStepSounds;
    public float footStepIntervel = 0.5f;
    private float nextFootStepTime;
    private bool isLeftFootStep = true;

    public bool mobile;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
  
    void Update()
    {
        if (mobile)
        {
            input = new Vector2(SimpleInput.GetAxisRaw("Horizontal"),SimpleInput.GetAxisRaw("Vertical"));
        }
        else
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        input.Normalize();


        sprinting = Input.GetButton("Sprint");

        jumping = Input.GetButton("Jump");

        Jump();
      

        if (grounded && rb.velocity.magnitude > 0.1f && Time.time >= nextFootStepTime)
        {
            PlayerFootStepSound();
            nextFootStepTime = Time.time + footStepIntervel;
        }
    }

    public void Jump()
    {
        jumping = SimpleInput.GetButton("Jump");
    }

    private void OnTriggerStay(Collider other)
    {
        grounded = true;
    }

    void FixedUpdate()
    {
        if (grounded)
        {
            if (jumping)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
            }

            else if (input.magnitude > 0.5f)
            {
                rb.AddForce(CalculateMovement(sprinting ? sprintSpeed : walkSpeed), ForceMode.VelocityChange);
            }
            else
            {
                var velocity1 = rb.velocity;
                velocity1 = new Vector3(x: velocity1.x * 0.2f * Time.fixedDeltaTime, velocity1.y, velocity1.z * 0.2f * Time.fixedDeltaTime);
                rb.velocity = velocity1;
            }
        }
        else
        {
            if (input.magnitude > 0.5f)
            {
                rb.AddForce(CalculateMovement(sprinting ? sprintSpeed * airControl : walkSpeed * airControl), ForceMode.VelocityChange);
            }
            else
            {
                var velocity1 = rb.velocity;
                velocity1 = new Vector3(x: velocity1.x * 0.2f * Time.fixedDeltaTime, velocity1.y, velocity1.z * 0.2f * Time.fixedDeltaTime);
                rb.velocity = velocity1;
            }
        }

        grounded = false;
    }

    Vector3 CalculateMovement(float speed)
    {
        Vector3 targetVelocity = new Vector3(input.x, 0, input.y);
        targetVelocity = transform.TransformDirection(targetVelocity);

        targetVelocity *= speed;

        Vector3 velocity = rb.velocity;

        if (input.magnitude > 0.5f)
        {
            Vector3 velocityChange = targetVelocity - velocity;

            velocityChange.x = Mathf.Clamp(velocityChange.x,-maxVelocityChange,maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);

            velocityChange.y = 0;

            return velocityChange;
        }
        else
        {
            return new Vector3();
        }
    }

    void PlayerFootStepSound()
    {
        AudioClip foolStepClip = footStepSounds[Random.Range(0, footStepSounds.Length)];

        if (isLeftFootStep)
        {
            leftFoot.PlayOneShot(foolStepClip);
        }
        else
        {
            rightFoot.PlayOneShot(foolStepClip);
        }
        isLeftFootStep = !isLeftFootStep;
    }
}
