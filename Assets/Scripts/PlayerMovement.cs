using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static readonly int HashMove = Animator.StringToHash("Move");

    public float moveSpeed = 5f;
    public float rotateSpeed = 100f;

    private Animator playerAnimator;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;


    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerAnimator.SetFloat(HashMove, playerInput.Move);

        
    }



    private void FixedUpdate()  

    {
        float angle = playerInput.Rotate * rotateSpeed * Time.deltaTime;
        playerRigidbody.MoveRotation(playerRigidbody.rotation * Quaternion.Euler(0f, angle, 0f));   

        Vector3 delta = playerInput.Move * transform.forward * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + delta);
    }


}
