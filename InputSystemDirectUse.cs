using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    Animator anim;
    Keyboard keyboard;
    Transform foot;
    public float speed = 5f;
    public float jumpForce = 30f;
    public float turnSpeed = 10f;
    public float runSpeed = 10f;
    public bool isGrounded = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        keyboard = Keyboard.current;
        foot = transform.Find("Foot");
        anim = transform.Find("Body").GetComponent<Animator>();
    }

    void Update(){
        if(keyboard.spaceKey.isPressed && isGrounded){
            Jump();
            isGrounded = false;
        } 
        
        if(Physics.Raycast(foot.position, Vector3.down, 0.1f)){
            isGrounded = true;
        }
    }

    private void FixedUpdate()
    {
        if (keyboard == null) return;

        Vector3 movement = Vector3.zero;
        float curSpeed = keyboard.leftShiftKey.isPressed ? runSpeed : speed;
        movement.z = keyboard.wKey.isPressed ? 1 : keyboard.sKey.isPressed ? -1 : 0;
        movement.x = keyboard.dKey.isPressed ? 1 : keyboard.aKey.isPressed ? -1 : 0;
        rb.MovePosition(rb.position + movement * curSpeed * Time.deltaTime);

        if (movement != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, movement, turnSpeed * Time.deltaTime);
            anim.SetFloat("speed", curSpeed == runSpeed ? 5 : 1);
        }else{
            anim.SetFloat("speed", 0);
        }
    }

    void Jump(){
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
