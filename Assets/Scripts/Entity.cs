using UnityEngine;

public class Entity : MonoBehaviour
{
    protected float maxHealth = 100f;
    [SerializeField]protected float currentHealth;

    protected float moveSpeed = 5f;
    protected float sprintSpeed = 8f;
    protected float crouchSpeed = 2.5f;
    [SerializeField]protected float jumpForce = 7f;
    protected float groundCheckDistance = 1.1f;
    [SerializeField]protected float cellingCheckDistance = 1f;
    protected float crouchHeight = 1f;
    protected float standHeight = 2f;

    [SerializeField]protected float crouchCameraPosition;
    [SerializeField]protected Transform playerCamera;

    private CapsuleCollider capsule;
    private bool isCrouching;
    private Vector3 standingCameraPosition;

    public LayerMask groundLayer;
    private Rigidbody rb;
    private bool isGrounded;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        currentHealth = maxHealth;

        standingCameraPosition = playerCamera.localPosition;
    }

    public virtual void TakeDamage(float damage)
    {
        //Debug.Log("Player took Damage: "+ damage);
        //Debug.Log("currenthealth: " + currentHealth);
        currentHealth -= damage;
         
        if (currentHealth < 0)
        {
            Die();
            Debug.Log("Player died ");
        }
    }


    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected void checkGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, Vector3.down * groundCheckDistance);
        Gizmos.DrawRay(transform.position + Vector3.up * (standHeight / 2f), Vector3.up * cellingCheckDistance);        
    }

    private void Crouch()
    {
        isCrouching = true;

        capsule.height = crouchHeight;
        capsule.center = new Vector3(0, -0.5f, 0);

        playerCamera.localPosition = new Vector3(standingCameraPosition.x, crouchCameraPosition, standingCameraPosition.z);
    }

    private void StandUp()
    {
        isCrouching = false;

        capsule.height = standHeight;
        capsule.center = Vector3.zero;

        playerCamera.localPosition = standingCameraPosition;
    }

    private void Update()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if(Input.GetKey(KeyCode.LeftControl))
        {
            if (!isCrouching)
            {
                Crouch();
            }
        }

        else
        {
            if (isCrouching)
            {
                Vector3 rayOrigin = transform.position + Vector3.up * (standHeight / 2f);
            }
           
            if (!Physics.Raycast(transform.position, Vector3.up, cellingCheckDistance))
            {
                StandUp();
            }
        }
    }

    private void FixedUpdate()
    {
        checkGround();
            
        float x = Input.GetAxis("Horizontal");

        float z = Input.GetAxis("Vertical");

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && z>0;


        float currentSpeed;

        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }

        else if (isSprinting)
        {
            currentSpeed = sprintSpeed;
        }

        else
        {
            currentSpeed = moveSpeed;
        }

        Vector3 movement = transform.right * x + transform.forward * z;

        rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);

        
    }

}
