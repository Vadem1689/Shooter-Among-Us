using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class JoostickMove : MonoBehaviour
{
    public Image joystickImage; // Image для джостика
    public float joystickRadius = 50f; // Радиус джостика
    public float maxDistance = 200f; // Максимальное расстояние перетаскивания

    private Vector2 startPosition; // Начальная позиция джостика
    private bool isDragging = false; // Флаг для отслеживания перетаскивания

    // Создаем InputAction для левой кнопки мыши
    private InputAction leftClickAction;

    void Awake()
    {
        // Получаем InputAction для левой кнопки мыши
        leftClickAction = new InputAction("LeftClick", binding: "<Mouse>/leftButton");

        // Активируем InputAction
        leftClickAction.Enable();
    }

    void Start()
    {
        // Привязка событий к InputAction
        leftClickAction.started += OnPointerDown;
        leftClickAction.canceled += OnPointerUp;
        leftClickAction.performed += OnDrag;
    }

    private void OnPointerDown(InputAction.CallbackContext context)
    {
        // Установка начальной позиции джостика
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
            // Вычисление смещения от начальной позиции
            Vector2 offset = Mouse.current.position.ReadValue() - startPosition;

            // Ограничение смещения
            if (offset.magnitude > maxDistance)
            {
                offset = offset.normalized * maxDistance;
            }

            // Перемещение джостика
            joystickImage.rectTransform.anchoredPosition = offset;
        }
    }

    void Update()
    {
        // Проверка клика левой кнопки мыши
        if (leftClickAction.WasPressedThisFrame())
        {
            // Получение позиции клика в пикселях
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            // Проверка, находится ли клик в левой части экрана
            if (mousePosition.x < Screen.width / 2)
            {
                // Перемещение джостика в точку клика
                joystickImage.rectTransform.anchoredPosition = mousePosition;
            }
        }
    }

    void OnDestroy()
    {
        // Отключаем InputAction перед уничтожением скрипта
        leftClickAction.Disable();
    }
}
