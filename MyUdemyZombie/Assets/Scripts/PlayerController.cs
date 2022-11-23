using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // InputSystem �� �����ϰ� ���ִ� ����Ƽ �̺�Ʈ�ý���

public class PlayerController : MonoBehaviour
{

    [Header("Movement")]
    public float moveSpeed = 6f; // how fast our player should move
                                 // �÷��̾ �󸶳� ���� �������� �ϴ���. �̵��ӵ�
    private Vector2 currentMovementInput; 

    [Header("Camera Look")]
    public Transform cameraContainer; // acess to transform value of our camera container
                                      // ī�޶� �����̳� Ʈ�������� �����ϴ� ��
    public float minXLook, maxXLook; // min&max amount that we can look down or up to avoid spin around completely
                                     // �����ٺ��ų� �÷��ٺ� �� �ִ� �ּ� �ִ� ��
    private float camCurrentXRotation; // get access to camera rotation
                                       // ī�޶� ȸ���� ����
    public float looksensitivity; // how fast we are able to look around
                                  // ������ �ѷ��� �� �ִ�, �ΰ��� ����
    private Vector2 mouseDelta; // player �� �̵��ϰų� Ŭ���� �� ���콺Ŀ���� ���Դϴ�. �׷���
                                // private �� �����Ͽ� Ŀ���� ��޴ϴ�.
    // public Vector2 mouseDelta;
    private Rigidbody myRig; // Rigidbody �� �����ϰ� ���ݴϴ�.

    private void Awake()
    {
        // ���� Rigidbody�� myRig �� �Ҵ��մϴ�.
        myRig = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // locked the cursor and make it also invisible
        // Ŀ���� ��װ� ������ �ʰ� �մϴ�.(public Vector2 mouseDelta; �� ���� ����ϴ�.)
        Cursor.lockState = CursorLockMode.Locked;

    }

    // LateUpdate �� Update �� ���� �� �����Ӵ� �� �� ȣ��˴ϴ�.
    // Update ���� ����� ��� ����� LateUpdate �� ������ �� �Ϸ�˴ϴ�.
    // LateUpdate is called after all Update funtions have been called
    private void LateUpdate()
    {
        // we want to rotate camera after player has moved
        // �÷��̾ �̵��� �� ī�޶� ȸ���ϴ� �޼���
        CameraLook();
    }

    // FixedUpdate will be called 50 time per second (around 0.02 second)
    // FixedUpdate�� �ʴ� 50�� ȣ��˴ϴ�(�� 0.02��).
    // Rigidbody �� �ٷ� ���� Update ��� FixedUpdate �� ����ؾ� �մϴ� . 
    // ���� ��� rigidbody�� ���� �߰��� ��,
    // Update ������ ��� ������ ��� FixedUpdate ������ ��� ���� �����ӿ� ���� �����ؾ� �մϴ�.
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // store our movement value to move direction
        // �̵� ���� �����Ͽ� �÷��̾ �ٶ󺸴� �������� �̵��մϴ�.
        Vector3 moveDirection = transform.forward * currentMovementInput.y + 
                                transform.right * currentMovementInput.x;

        // => moveDirection = moveDirection * moveSpeed;
        // moveDirection * moveSpeed �� �� ������ 0.02�ʸ��� ���Ѵ�. 
        // move player with the speed value of our moveSpeed
        // moveSpeed�� �ӵ� ������ �÷��̾ �̵��մϴ�.
        moveDirection *= moveSpeed;
        moveDirection.y = myRig.velocity.y;
        myRig.velocity = moveDirection;

    }

    // context contains all information we need to know such as we pressed the button or not, the value or...
    // ���ؽ�Ʈ���� �츮�� �˾ƾ� �� ��� ������ ���ԵǾ� �ֽ��ϴ�. ���� ��� ��ư�� �������� ����, �� �Ǵ�...
    public void OnLookInput(InputAction.CallbackContext context)
    {
        // mouse delta is whatever is the value of vector2
        // ���콺 ��Ÿ�� �����̵��� vector2�� ���Դϴ�.(�Ƹ��� Input Actions �� �����ؼ� Action property �� �����ϴ� �������.)        
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMovementInput(InputAction.CallbackContext context)
    {
        // check if we pressing the button and if its true
        // ��ư�� �������� Ȯ���ϰ� ������� Ȯ���մϴ�.
        if (context.phase == InputActionPhase.Performed)
        {
            // move the player
            // ����� Ȯ�εǸ� player �� �̵��Ѵ�.
            currentMovementInput = context.ReadValue<Vector2>();
        }
        // check if we stop pressing the button and if its true
        // ��ư�� ������ ���� ���߸� �װ��� ������� Ȯ���մϴ�.
        else if (context.phase == InputActionPhase.Canceled)
        {
            // stop moving the player
            // �÷��̾� �̵��� �����մϴ�.
            currentMovementInput = Vector2.zero;
        }

    }


    private void CameraLook()
    {
        // add value to camera x rotation depend on our mouse delta y value then multiply it sensitivity value
        // ī�޶� x ȸ���� ���� �߰��ϰ� ���콺 ��Ÿ y ���� �ΰ��� ���� ���մϴ�.
        camCurrentXRotation += mouseDelta.y * looksensitivity;
        // limiting to looking up and down
        // ���Ʒ��� ���� ���� �����մϴ�.
        camCurrentXRotation = Mathf.Clamp(camCurrentXRotation, minXLook, maxXLook);
        // apply cam Current x Rotation to our camera container
        // ī�޶� �����̳ʿ� camCurrentXRotation ����
        cameraContainer.localEulerAngles = new Vector3(-camCurrentXRotation, 0, 0);
        // rotate only along Y axis for the player Container
        // player Container�� Y������ ȸ��
        transform.eulerAngles += new Vector3(0, mouseDelta.x * looksensitivity, 0);

    }

}
