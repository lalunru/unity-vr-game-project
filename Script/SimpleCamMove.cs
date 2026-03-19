using UnityEngine;

public class SimpleCamMove : MonoBehaviour
{
    [Header("설정")]
    public float moveSpeed = 5f;        // 이동 속도
    public float mouseSensitivity = 2f; // 마우스 감도 (시점 회전)
    public bool enableFly = false;      // 켜면 중력 무시하고 날아다님 (디버그용)

    private float verticalRotation = 0f;
    private CharacterController characterController;
    private Camera playerCamera;

    void Start()
    {
        // 1. 마우스 커서를 화면 중앙에 고정하고 숨김
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 2. 컴포넌트 찾기
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

        if (characterController == null)
            Debug.LogError("캐릭터 컨트롤러가 없습니다! Add Component 해주세요.");
    }

    void Update()
    {
        // === 1. 시점 회전 (마우스) ===
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 좌우 회전: 몸통 전체를 돌림
        transform.Rotate(0, mouseX, 0);

        // 위아래 회전: 카메라만 돌림
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f); // 위아래 90도 제한
        if (playerCamera != null)
        {
            playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        }

        // === 2. 이동 (키보드 WASD) ===
        float moveX = Input.GetAxis("Horizontal"); // A, D
        float moveZ = Input.GetAxis("Vertical");   // W, S

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // 중력 적용 (날기 모드가 아닐 때만)
        if (!enableFly)
        {
            move.y = -9.81f * Time.deltaTime; 
        }
        else
        {
            // 날기 모드일 때는 Q, E로 위아래 이동 가능
            if (Input.GetKey(KeyCode.E)) move.y = 1f;
            if (Input.GetKey(KeyCode.Q)) move.y = -1f;
        }

        if (characterController != null)
        {
            characterController.Move(move * moveSpeed * Time.deltaTime);
        }

        // === 3. 마우스 커서 탈출 (ESC 키) ===
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}