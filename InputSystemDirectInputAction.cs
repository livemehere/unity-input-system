using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    Animator anim;
    public InputAction movement;
    public InputAction jump;
    public InputAction run;

    Transform foot;
    public float speed;
    public float jumpForce;
    public float turnSpeed;
    public float runSpeed;
    public bool isGrounded = true;

    void Start()
    {
        speed = 3f;
        jumpForce = 300f;
        turnSpeed = 10f;
        runSpeed = 5f;
        rb = GetComponent<Rigidbody>();
        foot = transform.Find("Foot");
        anim = transform.Find("Body").GetComponent<Animator>();

        movement.Enable();
        jump.Enable();
        run.Enable();
        jump.performed += ctx => Jump();
    }

    void Update(){
        if(Physics.Raycast(foot.position, Vector3.down, 0.1f)){
            isGrounded = true;
        }
    }

    private void FixedUpdate()
    {
        Vector3 dir = Vector3.zero;
        Vector2 input = movement.ReadValue<Vector2>();
        dir.z = input.y;
        dir.x = input.x;
        float curSpeed = run.IsPressed() ? runSpeed : speed;
        rb.MovePosition(rb.position + dir * curSpeed * Time.deltaTime);

        if (dir != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, dir, turnSpeed * Time.deltaTime);
            anim.SetFloat("speed", curSpeed == runSpeed ? 5 : 1);
        }else{
            anim.SetFloat("speed", 0);
        }
    }

    void Jump(){
        if(!isGrounded) return;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }
}
