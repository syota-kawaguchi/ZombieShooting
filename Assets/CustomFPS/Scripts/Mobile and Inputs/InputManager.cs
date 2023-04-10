using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonMonoBehaviour<InputManager> {
    public bool _useMobileInput;

    [SerializeField]
    public static bool useMobileInput;

    [Header("Movement keys")]
    public KeyCode Crouch;
    public KeyCode Run;
    public KeyCode Jump;
    public KeyCode LeanLeft;
    public KeyCode LeanRight;

    [Header("Gameplay keys")]
    public KeyCode Fire;
    public KeyCode Aim;
    public KeyCode Use;
    public KeyCode DropWeapon;
    public KeyCode Reload;
    public KeyCode FiremodeSingle;
    public KeyCode FiremodeAuto;
    public KeyCode Inventory;

    public static Vector2 joystickInputVector;
    public static Vector2 touchPanelLook;

    //public UnityEngine.UI.Button[] mobileEquipmentButtons;

    private float playerBodyMovementSmoothness = 5f;

    new private void Awake() {
        useMobileInput = _useMobileInput;

        DontDestroyOnLoad(gameObject);

        /*
        if (useMobileInput)
        {
            foreach(UnityEngine.UI.Button button in mobileEquipmentButtons)
            {
                button.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (UnityEngine.UI.Button button in mobileEquipmentButtons)
            {
                button.gameObject.SetActive(false);
            }
        }*/
    }

    private void Update() {
        MovementDirection();
    }

    public static float horizontalInput;
    public static float verticalInput;
    public static float horizontalFactor;
    public static float verticalFactor;

    public void MovementDirection() {
        if (Application.isEditor) {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }
        //on VR
        else {
            horizontalInput = Input.GetAxis("Vertical");
            verticalInput = -Input.GetAxis("Horizontal");
        }
        Debug.Log($"Input ({horizontalInput}, {verticalInput})");
        horizontalFactor = Mathf.Lerp(horizontalFactor, horizontalInput, Time.deltaTime * playerBodyMovementSmoothness);
        verticalFactor = Mathf.Lerp(verticalFactor, verticalInput, Time.deltaTime * playerBodyMovementSmoothness);
    }

    public bool IsRunning() {
        return Input.GetKey(Run);
    }
}
