using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    Animator anim;
    PlayerController controls;


    Transform foot;
    public float speed;
    public float jumpForce;
    public float turnSpeed;
    public float runSpeed;
    public bool isGrounded = true;

    private void Awake()
    {
        // Actions Asset must be assigned in Awake not in Start
        controls = new PlayerController();
    }

    void Start()
    {
        speed = 3f;
        jumpForce = 300f;
        turnSpeed = 10f;
        runSpeed = 5f;
        rb = GetComponent<Rigidbody>();
        foot = transform.Find("Foot");
        anim = transform.Find("Body").GetComponent<Animator>();
    }

    void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Jump.performed += ctx => Jump();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    void Update(){
        if(Physics.Raycast(foot.position, Vector3.down, 0.1f)){
            isGrounded = true;
        }
    }

    private void FixedUpdate()
    {
        Vector3 dir = Vector3.zero;
        Vector2 input = controls.Player.Move.ReadValue<Vector2>();
        dir.z = input.y;
        dir.x = input.x;
        float curSpeed = controls.Player.Run.IsPressed() ? runSpeed : speed;
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
