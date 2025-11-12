using System.Collections;
using Photon.Pun;
using UnityEngine;

public class Woman : MonoBehaviourPun
{
    public float speed = 5f;   
    private Rigidbody rb;
    private Animator animator;
    private bool jump;
    private float jumpForce = 5f;
    private float jumpCooldown = 0.25f;
    private bool canJump = true;
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        //내 포톤뷰 아니면 실행 금지
        if(!photonView.IsMine)
            return;
        Move();
        Jump();
    }

    //이동 메서드
    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(h, 0f, v);
        
        if (direction != Vector3.zero)
        {
            var q = Quaternion.LookRotation(direction);
            transform.rotation = q;
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            animator.SetInteger("State", 1);
        }
        else
        {
            animator.SetInteger("State", 0);
        }
    }
    
    //점프 메서드
    void Jump()
    {
        jump = Input.GetKeyDown(KeyCode.Space);

        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //animator.SetInteger("Jump", 1);
            Debug.Log("점프!");
            canJump = false;
            StartCoroutine(JumpCooldown());
        }
    }
    
    //점프 쿨타임
    private IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(jumpCooldown);
        //animator.SetFloat("Move", vertical);
    }
    
    //착지 
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
            //animator.SetInteger("Jump", 0);
            //animator.SetFloat("Move", vertical);
        }
    }
    
}
