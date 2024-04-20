using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    Animator anim;

    Transform foot;
    public float speed;
    public float jumpForce;
    public float turnSpeed;
    public float runSpeed;
    public bool isGrounded = true;
    bool isRunning = false;
    Vector3 movement;
    PlayerInput input;

    void Start()
    {
        speed = 3f;
        jumpForce = 300f;
        turnSpeed = 10f;
        runSpeed = 5f;
        rb = GetComponent<Rigidbody>();
        foot = transform.Find("Foot");
        anim = transform.Find("Body").GetComponent<Animator>();

        input = GetComponent<PlayerInput>();
        input.actions["Run"].performed += ctx => isRunning = true;
        input.actions["Run"].canceled += ctx => isRunning = false;
    }

    void Update(){
        if(Physics.Raycast(foot.position, Vector3.down, 0.1f)){
            isGrounded = true;
        }
    }

    private void FixedUpdate()
    {
        float curSpeed = isRunning ? runSpeed : speed;
        rb.MovePosition(rb.position + movement * curSpeed * Time.deltaTime);

        if (movement != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward,movement, turnSpeed * Time.deltaTime);
            anim.SetFloat("speed", curSpeed == runSpeed ? 5 : 1);
        }else{
            anim.SetFloat("speed", 0);
        }
    }


    void OnJump(){
        if(!isGrounded) return;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    void OnMove(InputValue value){
        Vector3 dir = Vector3.zero;
        Vector2 input = value.Get<Vector2>();
        dir.z = input.y;
        dir.x = input.x;
        movement = dir;
    }
}
