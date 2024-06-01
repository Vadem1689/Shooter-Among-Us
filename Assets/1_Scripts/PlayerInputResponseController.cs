using BRAmongUS.Utils;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem;

public class PlayerInputResponseController : MonoBehaviour
{
    [SerializeField] private Vector2 JoystickSize = new Vector2(300, 300);

    [SerializeField] private FloatingJoystick Joystick;

    //[SerializeField] private Rigidbody2D playerRigidBody;

    [SerializeField] Player player;

    //[SerializeField] private float movementSpeed;

    [SerializeField] InputAction moveAction, fireAction, lookAction, interactAction;

    [SerializeField] Transform targetIcon;

    private Finger MovementFinger;

    private Vector2 MovementAmount;

    private Vector2 lookingDirection;

    private bool controlledByMouse;

    private bool isFiring = false;

    private float targetPointingRadius = 2.0f;

    public void Init(Player realPlayer)
    {
        // SetNewPlayer(realPlayer);
        
        fireAction.started += ctx => { OnFireStart(ctx); };
        fireAction.canceled += ctx => { OnFireFinished(ctx); };            
        moveAction.performed += ctx => { OnMove(ctx); };
        lookAction.performed += ctx => { OnLook(ctx); };
        // interactAction.performed += ctx => { OnInteract(ctx); };

        moveAction.Enable();
        fireAction.Enable();
        lookAction.Enable();
        interactAction.Enable();
    }
    
    public void SetNewPlayer(Player newPlayer)
    {
        player = newPlayer;
    }

    private void OnFireStart(InputAction.CallbackContext context)
    {
         //Debug.Log("Fire is started");
            isFiring = true;  
    }

    private void OnFireFinished(InputAction.CallbackContext context)
    {
        //Debug.Log("Fire is finished");
        isFiring = false;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        //Debug.Log("Moved");
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        return;
        if (context.control.device.ToString().Contains("Mouse"))
        {
            //Debug.Log("controlled by Mouse");
            controlledByMouse = true;
        }
        else
        {
            //Debug.Log("controlled by other Device");
            controlledByMouse = false;
        }
        //Debug.Log("OnLook");
        player.Look(new Vector2(player.transform.position.x, player.transform.position.y) + lookingDirection * 10);
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        //Debug.Log("Interacted");
        if (player.CanOpenAmmoBox())
        {
            player.SwitchGunFromBox();
        }
    }



    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;
        moveAction.Enable();
        fireAction.Enable();
        lookAction.Enable();
        interactAction.Enable();
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();
        moveAction.Disable();
        fireAction.Disable();
        lookAction.Disable();
        interactAction.Disable();
    }

    private void HandleFingerMove(Finger MovedFinger)
    {
        this.Log(MovementFinger.screenPosition);
        if (MovedFinger == MovementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = JoystickSize.x / 2f;
            ETouch.Touch currentTouch = MovedFinger.currentTouch;

            if (Vector2.Distance(
                    currentTouch.screenPosition,
                    Joystick.RectTransform.anchoredPosition
                ) > maxMovement)
            {
                knobPosition = (
                    currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition
                    ).normalized
                    * maxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition;
            }

            Joystick.Knob.anchoredPosition = knobPosition;
            MovementAmount = knobPosition / maxMovement;
        }
    }

    private void HandleLoseFinger(Finger LostFinger)
    {
        if (LostFinger == MovementFinger)
        {
            MovementFinger = null;
            Joystick.Knob.anchoredPosition = Vector2.zero;
            Joystick.gameObject.SetActive(false);
            MovementAmount = Vector2.zero;
        }
    }

    private void HandleFingerDown(Finger TouchedFinger)
    {
        this.Log(TouchedFinger.screenPosition);
        if (MovementFinger == null && TouchedFinger.screenPosition.x <= Screen.width / 2f)
        {
            MovementFinger = TouchedFinger;
            MovementAmount = Vector2.zero;
            Joystick.gameObject.SetActive(true);
            Joystick.RectTransform.sizeDelta = JoystickSize;
            Joystick.RectTransform.anchoredPosition = ClampStartPosition(TouchedFinger.screenPosition);
        }
    }

    private Vector2 ClampStartPosition(Vector2 StartPosition)
    {
        if (StartPosition.x < JoystickSize.x / 2)
        {
            StartPosition.x = JoystickSize.x / 2;
        }

        if (StartPosition.y < JoystickSize.y / 2)
        {
            StartPosition.y = JoystickSize.y / 2;
        }
        else if (StartPosition.y > Screen.height - JoystickSize.y / 2)
        {
            StartPosition.y = Screen.height - JoystickSize.y / 2;
        }

        return StartPosition;
    }

    private void FixedUpdate()
    {
        return;
        Vector2 movementAmount;
        if (!controlledByMouse) {
            movementAmount = Time.fixedDeltaTime * new Vector2(MovementAmount.x, MovementAmount.y);
        } else movementAmount = Time.fixedDeltaTime * moveAction.ReadValue<Vector2>();
        player.Move(movementAmount);

        if (!controlledByMouse)
        {
            lookingDirection = lookAction.ReadValue<Vector2>();
            targetIcon.position = new Vector2(player.transform.position.x, player.transform.position.y) + lookAction.ReadValue<Vector2>() * targetPointingRadius;
        }
        else
        {
            //Debug.Log(lookAction.ReadValue<Vector2>());            
            var screenPoint = new Vector3(lookAction.ReadValue<Vector2>().x, lookAction.ReadValue<Vector2>().y, 10);
            targetIcon.position = Camera.main.ScreenToWorldPoint(screenPoint);
            lookingDirection = (Camera.main.ScreenToWorldPoint(screenPoint) - new Vector3(player.transform.position.x, player.transform.position.y, 0)).normalized;
        }
    }


    private void Update()
    {
        return;
        if (isFiring) {
            player.Fire(new Vector2(player.transform.position.x, player.transform.position.y) + lookingDirection * 10);
        }
    }

    /*void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector3(player.transform.position.x, player.transform.position.y, 0), new Vector3(player.transform.position.x + lookingDirection.x, player.transform.position.y + lookingDirection.y, 0));
        }
    }*/

    private void OnGUI()
    {
        GUIStyle labelStyle = new GUIStyle()
        {
            fontSize = 24,
            normal = new GUIStyleState()
            {
                textColor = Color.white
            }
        };
        if (MovementFinger != null)
        {
            GUI.Label(new Rect(10, 35, 500, 20), $"Finger Start Position: {MovementFinger.currentTouch.startScreenPosition}", labelStyle);
            GUI.Label(new Rect(10, 65, 500, 20), $"Finger Current Position: {MovementFinger.currentTouch.screenPosition}", labelStyle);
        }
        else
        {
            GUI.Label(new Rect(10, 35, 500, 20), "No Current Movement Touch", labelStyle);
        }

        GUI.Label(new Rect(10, 10, 500, 20), $"Screen Size ({Screen.width}, {Screen.height})", labelStyle);
    }
}
