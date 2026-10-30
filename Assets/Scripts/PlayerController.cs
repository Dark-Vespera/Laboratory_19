using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    public float currentSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float _mouseSensitivity = 2f;

    [SerializeField] private CharacterController _characterController;
    [SerializeField] private CinemachineCamera _cinCam;

    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrain = 20f;
    public float staminaRegen = 15f;
    public UnityEngine.UI.Image staminaBar;

    private bool isExhausted = false; 
    
    private Vector2 _move;

    private void Start() {
        if (PlayerPrefs.HasKey("Sensitivity")) {
            _mouseSensitivity = PlayerPrefs.GetFloat("Sensitivity");
        }

        currentSpeed = walkSpeed;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentStamina = maxStamina;
    }
    
    public void OnMove(InputValue val) {
        _move = val.Get<Vector2>();
    }

    public void OnSprint(InputValue val) {
        if(val.Get<float>() > 0.5f && !isExhausted) {
            currentSpeed = sprintSpeed;
        }
        else {
            currentSpeed = walkSpeed;
        }
    }

    private void Update() {
        _characterController.Move((GetForward() * _move.y + GetRight() * _move.x) * Time.deltaTime * currentSpeed);

        HandleStamina();
    }

    private Vector3 GetForward() {
        Vector3 forward = _cinCam.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }
    
    private Vector3 GetRight() {
        Vector3 right = _cinCam.transform.right;
        right.y = 0;
        return right.normalized;
    }

    void HandleStamina() {
        if (currentSpeed == sprintSpeed && currentStamina > 0) {
            currentStamina -= staminaDrain * Time.deltaTime;
            
            if (currentStamina <= 0) {
                currentStamina = 0; isExhausted = true; currentSpeed = walkSpeed;
            }
        }
        else {
            if (currentStamina < maxStamina) {
                currentStamina += staminaRegen * Time.deltaTime;
                if (currentStamina > maxStamina) currentStamina = maxStamina;
            }

            if (currentStamina > maxStamina * 0.5f) {
                isExhausted = false;
            }
        }

        if (staminaBar != null) {
            staminaBar.fillAmount = currentStamina / maxStamina;
        }
    }
}