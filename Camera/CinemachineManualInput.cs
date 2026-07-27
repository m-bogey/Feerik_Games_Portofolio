using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CinemachineManualInput : MonoBehaviour
{
    private CinemachineOrbitalFollow _orbital;   // FreeLook en Cinemachine 3 = OrbitalFollow

    [Header("Sensitivity")]
    [SerializeField] private float horizontalSpeed = 180f;  // degrés/sec (manette)
    [SerializeField] private float verticalSpeed = 90f;
    [SerializeField] private float mouseMult = 0.1f;  // multiplicateur souris

    private Vector2 _lookInput;
    private bool _isMouse;

    void Awake()
    {
        _orbital = GetComponent<CinemachineOrbitalFollow>();
        if (_orbital == null)
            _orbital = GetComponentInChildren<CinemachineOrbitalFollow>();

        if (_orbital == null)
            Debug.LogError($" Pas de CinemachineOrbitalFollow ");
    }

    void Update()
    {
        if (_orbital == null) return;

        float h = _lookInput.x;
        float v = _lookInput.y;

        if (_isMouse)
        {
            // Souris : delta brut = petit multiplicateur
            h *= mouseMult;
            v *= mouseMult;
        }
        else
        {
            // Manette : valeur normalisée = degrés/sec
            h *= horizontalSpeed * Time.deltaTime;
            v *= verticalSpeed * Time.deltaTime;
        }

        // Injecte dans l'OrbitalFollow
        _orbital.HorizontalAxis.Value += h;
        _orbital.VerticalAxis.Value =
            Mathf.Clamp(
                _orbital.VerticalAxis.Value - v,
                _orbital.VerticalAxis.Range.x,
                _orbital.VerticalAxis.Range.y
            );
    }

    // Appel par Unity Event du PlayerInput = Look 
    public void OnLook(InputAction.CallbackContext ctx)
    {
        _lookInput = ctx.ReadValue<Vector2>();

        // Detecte si c'est souris ou stick
        _isMouse = ctx.control?.device is Mouse;
    }
}