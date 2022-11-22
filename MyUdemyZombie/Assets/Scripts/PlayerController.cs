using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // InputSystem �� �����ϰ� ���ִ� ����Ƽ �̺�Ʈ�ý���

public class PlayerController : MonoBehaviour
{
    public Transform cameraContainer; // acess to transform value of our camera container
                                      // ī�޶� �����̳��� Ʈ�������� �����ϴ� ��
    public float minXLook, maxXLook; // min&max amount that we can look down or up to avoid spin around completely
                                     // �����ٺ��ų� �÷��ٺ� �� �ִ� �ּ� �ִ� ��
    private float camCurrentXRotation; // get access to camera rotation
                                       // ī�޶� ȸ��
    public float looksensitivity; // how fast we are able to look around
                                  // ������ �ѷ��� �� �ִ�, �ΰ��� ����
    private Vector2 mouseDelta;
    // public float looksensitivity;

    private void Start()
    {
        // locked the cursor and make it also invisible
        // Ŀ���� ��װ� ������ �ʰ� �մϴ�.(�ۺ��� ���� ����ϴ�.)
        Cursor.lockState = CursorLockMode.Locked;

    }

    // context contains all information we need to know such as we pressed the button or not, the value or...
    // ���ؽ�Ʈ���� �츮�� �˾ƾ� �� ��� ������ ���ԵǾ� �ֽ��ϴ�. ���� ��� ��ư�� �������� ����, �� �Ǵ�...
    public void OnLookInput(InputAction.CallbackContext context)
    {
        // mouse delta is whatever is the value of vector2
        // ���콺 ��Ÿ�� �����̵��� vector2�� ���Դϴ�.(�Ƹ� name�� �����̵��� vector2���� ���� �ҷ��� �� �ִٶ�� ��?)        
        mouseDelta = context.ReadValue<Vector2>();
    }

    private void LateUpdate()
    {
        // we want to rotate camera after player has moved
        // �÷��̾ �̵��� �� ī�޶� ȸ���ϴ� �޼���
        CameraLook();
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
