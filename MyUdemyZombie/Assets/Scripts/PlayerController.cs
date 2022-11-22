using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // InputSystem 에 접근하게 해주는 유니티 이벤트시스템

public class PlayerController : MonoBehaviour
{
    public Transform cameraContainer; // acess to transform value of our camera container
                                      // 카메라 컨테이너의 트랜스폼에 접근하는 값
    public float minXLook, maxXLook; // min&max amount that we can look down or up to avoid spin around completely
                                     // 내려다보거나 올려다볼 수 있는 최소 최대 양
    private float camCurrentXRotation; // get access to camera rotation
                                       // 카메라 회전
    public float looksensitivity; // how fast we are able to look around
                                  // 빠르게 둘러볼 수 있는, 민감도 조절
    private Vector2 mouseDelta;
    // public float looksensitivity;

    private void Start()
    {
        // locked the cursor and make it also invisible
        // 커서를 잠그고 보이지 않게 합니다.(퍼블릭도 같이 남깁니다.)
        Cursor.lockState = CursorLockMode.Locked;

    }

    // context contains all information we need to know such as we pressed the button or not, the value or...
    // 컨텍스트에는 우리가 알아야 할 모든 정보가 포함되어 있습니다. 예를 들어 버튼을 눌렀는지 여부, 값 또는...
    public void OnLookInput(InputAction.CallbackContext context)
    {
        // mouse delta is whatever is the value of vector2
        // 마우스 델타가 무엇이든지 vector2의 값입니다.(아마 name이 무엇이든지 vector2에서 값을 불러올 수 있다라는 뜻?)        
        mouseDelta = context.ReadValue<Vector2>();
    }

    private void LateUpdate()
    {
        // we want to rotate camera after player has moved
        // 플레이어가 이동할 때 카메라 회전하는 메서드
        CameraLook();
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

}
