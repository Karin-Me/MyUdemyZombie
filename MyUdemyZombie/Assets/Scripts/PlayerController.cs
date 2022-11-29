using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // InputSystem 에 접근하게 해주는 유니티 이벤트시스템

public class PlayerController : MonoBehaviour
{
    [Header("Movement Sound")]
    private AudioSource audioS;
    public AudioClip[] stepSounds;
    public float footStepRate, footStepThresHold;
    private float lastStepTime;

    [Header("Jumping")]
    public float jumpForce;
    public LayerMask groundLayer;


    [Header("Movement")]
    public float moveSpeed = 6f; // how fast our player should move
                                 // 플레이어가 얼마나 빨리 움직여야 하는지. 이동속도
    private Vector2 currentMovementInput;

    [Header("Camera Look")]
    public Transform cameraContainer; // acess to transform value of our camera container
                                      // 카메라 컨테이너 트랜스폼에 접근하는 값
    public float minXLook, maxXLook; // min&max amount that we can look down or up to avoid spin around completely
                                     // 내려다보거나 올려다볼 수 있는 최소 최대 양
    private float camCurrentXRotation; // get access to camera rotation
                                       // 카메라 회전에 접근
    public float looksensitivity; // how fast we are able to look around
                                  // 빠르게 둘러볼 수 있는, 민감도 조절
    private Vector2 mouseDelta; // player 가 이동하거나 클릭할 때 마우스커서가 보입니다. 그래서
                                // private 로 변경하여 커서를 잠급니다.
                                // public Vector2 mouseDelta;
    private Rigidbody myRig; // Rigidbody 에 접근하게 해줍니다.
   [HideInInspector]
    public bool canLook = true;
    private void Awake()
    {
        // 나의 Rigidbody를 myRig 에 할당합니다.
        myRig = GetComponent<Rigidbody>();
        audioS = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // locked the cursor and make it also invisible
        // 커서를 잠그고 보이지 않게 합니다.(public Vector2 mouseDelta; 도 같이 남깁니다.)
        Cursor.lockState = CursorLockMode.Locked;

    }

    // LateUpdate 는 Update 가 끝난 후 프레임당 한 번 호출됩니다.
    // Update 에서 수행된 모든 계산은 LateUpdate 가 시작할 때 완료됩니다.
    // LateUpdate is called after all Update funtions have been called
    private void LateUpdate()
    {
        if (canLook == true)
        {
            // we want to rotate camera after player has moved
            // 플레이어가 이동할 때 카메라 회전하는 메서드
            CameraLook();
        }
        
    }

    // FixedUpdate will be called 50 time per second (around 0.02 second)
    // FixedUpdate는 초당 50번 호출됩니다(약 0.02초).
    // Rigidbody 를 다룰 때는 Update 대신 FixedUpdate 를 사용해야 합니다 . 
    // 예를 들어 rigidbody에 힘을 추가할 때,
    // Update 내부의 모든 프레임 대신 FixedUpdate 내부의 모든 고정 프레임에 힘을 적용해야 합니다.
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // store our movement value to move direction
        // 이동 값을 저장하여 플레이어가 바라보는 방향으로 이동합니다.
        Vector3 moveDirection = transform.forward * currentMovementInput.y +
                                transform.right * currentMovementInput.x;

        // => moveDirection = moveDirection * moveSpeed;
        // moveDirection * moveSpeed 를 매 프레임 0.02초마다 곱한다. 
        // move player with the speed value of our moveSpeed
        // moveSpeed의 속도 값으로 플레이어를 이동합니다.
        moveDirection *= moveSpeed;
        moveDirection.y = myRig.velocity.y;
        myRig.velocity = moveDirection;

        // The velocity vector of the rigidbody. It represents the rate of change of Rigidbody position.
        // rigidbody의 속도 벡터. Rigidbody 위치의 변화율을 나타냅니다.

        // Time.time simply gives you a numeric value which is equal to
        // the number of seconds which have elapsed since the project started playing.
        // Time.time은 단순히 프로젝트 재생이 시작된 이후 경과된 초 수와 동일한 숫자 값을 제공합니다.

        // check if rigidbody speed bigger than our footstepthreshhold
        if (moveDirection.magnitude > footStepThresHold && IsGrounded())
        {
            if (Time.time - lastStepTime > footStepRate)
            {
                lastStepTime = Time.time;
                audioS.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)]);
            }
        }


    }

    // context contains all information we need to know such as we pressed the button or not, the value or...
    // 컨텍스트에는 우리가 알아야 할 모든 정보가 포함되어 있습니다. 예를 들어 버튼을 눌렀는지 여부, 값 또는...
    public void OnLookInput(InputAction.CallbackContext context)
    {
        // mouse delta is whatever is the value of vector2
        // 마우스 델타가 무엇이든지 vector2의 값입니다.(아마도 Input Actions 에 접근해서 Action property 에 접근하는 방법같다.)        
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMovementInput(InputAction.CallbackContext context)
    {
        // check if we pressing the button and if its true
        // 버튼을 눌렀는지 확인하고 사실인지 확인합니다.
        if (context.phase == InputActionPhase.Performed)
        {
            // move the player
            // 사실이 확인되면 player 를 이동한다.
            currentMovementInput = context.ReadValue<Vector2>();
        }
        // check if we stop pressing the button and if its true
        // 버튼을 누르는 것을 멈추면 그것이 사실인지 확인합니다.
        else if (context.phase == InputActionPhase.Canceled)
        {
            // stop moving the player
            // 플레이어 이동을 중지합니다.
            currentMovementInput = Vector2.zero;
        }

    }


    private void CameraLook()
    {
        // add value to camera x rotation depend on our mouse delta y value then multiply it sensitivity value
        // 카메라 x 회전에 값을 추가하고 마우스 델타 y 값에 민감도 값을 곱합니다.
        camCurrentXRotation += mouseDelta.y * looksensitivity;
        // limiting to looking up and down
        // 위아래를 보는 것을 제한합니다.
        camCurrentXRotation = Mathf.Clamp(camCurrentXRotation, minXLook, maxXLook);
        // apply cam Current x Rotation to our camera container
        // 카메라 컨테이너에 camCurrentXRotation 적용
        cameraContainer.localEulerAngles = new Vector3(-camCurrentXRotation, 0, 0);
        // rotate only along Y axis for the player Container
        // player Container가 Y축으로 회전
        transform.eulerAngles += new Vector3(0, mouseDelta.x * looksensitivity, 0);

    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        // is it the first frame that we held the button down
        // down button을 눌렀는지 확인한다.
        if (context.phase == InputActionPhase.Started)
        {
            // add force toward upside
            // 위로 힘을 가한다.
            if (IsGrounded())
            {
                myRig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            // 광선의 표현.
            new Ray(transform.position + (transform.forward* 0.2f) + (Vector3.up * 0.02f), Vector3.down),
            new Ray(transform.position + (-transform.forward* 0.2f) + (Vector3.up * 0.02f), Vector3.down),
            new Ray(transform.position + (transform.right* 0.2f) + (Vector3.up * 0.02f), Vector3.down),
            new Ray(transform.position + (-transform.right* 0.2f) + (Vector3.up * 0.02f), Vector3.down)
        };

        // Statement 1 sets a variable before the loop starts (int i = 0).
        // Statement 1 루프가 시작되기 전에 변수를 설정합니다. (int i = 0).

        // Statement 2 defines the condition for the loop to run.If the condition is true, the loop will start over again, if it is false, the loop will end.
        // Statement 2 루프가 실행되는 조건을 정의합니다.
        // 조건이 참이면 반복문을 다시 시작하고 거짓이면 반복문을 종료합니다.

        // Statement 3 increases a value(i++) each time the code block in the loop has been executed.
        // Statement 3 루프의 코드 블록이 실행될 때마다 value(i++)가 증가합니다.
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayer))
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + (transform.forward * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.forward * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (transform.right * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.right * 0.2f), Vector3.down);
    }

    public void ToggleCurcor(bool toggle)
    {
        // check if toggle is true then lockstate is none lockstate is locked  
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        // canLook is become opposite of toggle
        canLook = !toggle;
    }
}
