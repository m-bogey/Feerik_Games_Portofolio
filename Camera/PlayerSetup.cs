using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerSetup : MonoBehaviour
{
    private PlayerInput playerInput;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private CinemachineCamera cmCamera;
    [SerializeField] private Transform cameraTarget;
    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    
    void Start()
    {
        SetupCamera();
    }
    
    void SetupCamera()
    {
        int index = playerInput.playerIndex;
        // SPLIT SCREEN
        if (index == 0)
        {
            playerCamera.rect = new Rect(0f, 0f, 0.5f, 1f);
        }
        else
        {
            playerCamera.rect = new Rect(0.5f, 0f, 0.5f, 1f);
        }
        // FOLLOW
        cmCamera.Follow = cameraTarget;
        cmCamera.LookAt = cameraTarget;
    }
}