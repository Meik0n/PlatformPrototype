using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float maxSpeed = 6.0f; //Maximum movement speed
    public bool facingRight = true; //Where player faced at start, 
                                    //if he looking to right set to true,
                                    //if he looking to left, set to false
    public float jumpSpeed = 500.0f; //Jump speed
    public GameObject jumpDust; //Particles when jumping and landing
    private Vector3 moveDirection; //Moving direction
    private CharacterController characterController; //CharacterController component
    public float gravity = 10.0f; //Gravity multiplier
    public float stickToGroundForce = 10.0f; // Stick to ground force
    public AudioClip jumpSound; //Jump sound

    private Transform footTrigger;
    private Animator anim; //Animator Component
    private bool jump; //if player wants to jump
    private bool jumping; //if jump animation is playing now
    private bool attack = false;
    private AudioSource audioSource;
    private bool wasGrounded = true;
    private CollisionFlags collisionFlags;

    public Transform hit_check = null;
    public float hit_radius = 4f;
    public LayerMask button_layer;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        footTrigger = GameObject.Find("FootTrigger").transform;
        //Get Player CharacterController Component
        characterController = gameObject.GetComponent<CharacterController>();
        //Get Player Animator
        anim = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        //Check for Input
        Vector3 desiredMove = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, 0.0f);

        // get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out hitInfo,
                           characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        moveDirection.x = desiredMove.x * maxSpeed;
        moveDirection.z = 0.0f;

        if (moveDirection.x > 0.0f && !facingRight || moveDirection.x < 0.0f && facingRight)
        {
            Flip();
        }

        if (characterController.isGrounded)
        {
            moveDirection.y = -stickToGroundForce;

            if (jump)
            {
                moveDirection.y = jumpSpeed;
                if (jumpSound)
                {
                    AudioSource.PlayClipAtPoint(jumpSound, transform.position);
                }
                jump = false;
                jumping = true;
                anim.SetBool("Jump", true);
            }
        }
        else
        {
            anim.SetBool("Jump", true);
            moveDirection.y += -gravity * Time.fixedDeltaTime;
        }

        if (moveDirection.x != 0.0f)
        {
            if (characterController.isGrounded)
            {
                //Apply movement animation in Mecanim
                anim.SetBool("Walking", true);
                //Play footstep sound
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                audioSource.Stop();
            }
        }
        else
        {
            //Turn off movement animation and apply Idle animation in Mecanim
            anim.SetBool("Walking", false);

            if (characterController.isGrounded)
            {
                anim.SetBool("Jump", false);
            }
            //Stop footstep sound
            audioSource.Stop();

            if (attack)
            {
                Hit();
                anim.SetTrigger("Attack");
                attack = false;
            }
        }

        collisionFlags = characterController.Move(moveDirection * Time.fixedDeltaTime);


    }

    private void Update()
    {
        if (characterController.isGrounded && !anim.GetCurrentAnimatorStateInfo(0).IsTag("AttackAnim"))
        {
            if (!jump)
            {
                jump = Input.GetButtonDown("Jump");
            }

            if (!attack)
            {
                attack = Input.GetButtonDown("Fire1");
            }

        }

        if (!wasGrounded && characterController.isGrounded)
        {
            //When player lands, play Land animation in Mecanim
            if (jumpDust)
            {
                Instantiate(jumpDust, footTrigger.position, Quaternion.identity);
            }
            jumping = false;
            anim.SetBool("Jump", false);
            moveDirection.y = 0.0f;
        }
        if (!characterController.isGrounded && !jumping && wasGrounded)
        {
            moveDirection.y = 0.0f;
        }

        wasGrounded = characterController.isGrounded;
    }

    void Flip()
    {
        //Flip character from left to right and vice versa
        facingRight = !facingRight;
        transform.Rotate(Vector3.up, 180.0f, Space.World);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if (collisionFlags == CollisionFlags.Below)
        {
            return;
        }

        if (body == null || body.isKinematic)
        {
            return;
        }
        body.AddForceAtPosition(characterController.velocity, hit.point, ForceMode.Impulse);
    }

    public void Hit()
    {
        RaycastHit hit;
        if (Physics.SphereCast(hit_check.position, hit_radius, Vector3.right, out hit, hit_radius * 2, button_layer))
        {
            GameObject button = hit.transform.gameObject;
            Elevator elevator = button.GetComponentInParent<Elevator>();
            elevator.Elevate();
        }

    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(hit_check.position, hit_radius);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(new Ray(hit_check.position, Vector3.right));
    }
}
