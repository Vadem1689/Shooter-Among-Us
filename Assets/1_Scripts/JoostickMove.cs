using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class JoostickMove : MonoBehaviour
{
    public Image joystickImage; // Image ��� ��������
    public float joystickRadius = 50f; // ������ ��������
    public float maxDistance = 200f; // ������������ ���������� ��������������

    private Vector2 startPosition; // ��������� ������� ��������
    private bool isDragging = false; // ���� ��� ������������ ��������������

    // ������� InputAction ��� ����� ������ ����
    private InputAction leftClickAction;

    void Awake()
    {
        // �������� InputAction ��� ����� ������ ����
        leftClickAction = new InputAction("LeftClick", binding: "<Mouse>/leftButton");

        // ���������� InputAction
        leftClickAction.Enable();
    }

    void Start()
    {
        // �������� ������� � InputAction
        leftClickAction.started += OnPointerDown;
        leftClickAction.canceled += OnPointerUp;
        leftClickAction.performed += OnDrag;
    }

    private void OnPointerDown(InputAction.CallbackContext context)
    {
        // ��������� ��������� ������� ��������
        startPosition = Mouse.current.position.ReadValue();
        isDragging = true;
    }

    private void OnPointerUp(InputAction.CallbackContext context)
    {
        isDragging = false;
    }

    private void OnDrag(InputAction.CallbackContext context)
    {
        if (isDragging)
        {
            // ���������� �������� �� ��������� �������
            Vector2 offset = Mouse.current.position.ReadValue() - startPosition;

            // ����������� ��������
            if (offset.magnitude > maxDistance)
            {
                offset = offset.normalized * maxDistance;
            }

            // ����������� ��������
            joystickImage.rectTransform.anchoredPosition = offset;
        }
    }

    void Update()
    {
        // �������� ����� ����� ������ ����
        if (leftClickAction.WasPressedThisFrame())
        {
            // ��������� ������� ����� � ��������
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            // ��������, ��������� �� ���� � ����� ����� ������
            if (mousePosition.x < Screen.width / 2)
            {
                // ����������� �������� � ����� �����
                joystickImage.rectTransform.anchoredPosition = mousePosition;
            }
        }
    }

    void OnDestroy()
    {
        // ��������� InputAction ����� ������������ �������
        leftClickAction.Disable();
    }
}
