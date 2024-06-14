using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageMovement : MonoBehaviour
{
    public Image image; // Ссылка на картинку (UI элемент)
    public float speed = 5f; // Скорость перемещения
    public float maxOffset = 30f; // Максимальное смещение за рамку

    private RectTransform imageRect;
    private Vector2 initialPosition;
    private float imageWidth;
    private float imageHeight;

    void Start()
    {
        // Получаем RectTransform картинки
        imageRect = image.rectTransform;
        // Сохраняем начальную позицию
        initialPosition = imageRect.anchoredPosition;
        // Получаем ширину и высоту картинки
        imageWidth = imageRect.rect.width;
        imageHeight = imageRect.rect.height;
    }

    void Update()
    {
        // Получаем изменение позиции мыши
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        // Масштабируем скорость
        mouseDelta *= speed * Time.deltaTime;

        // Ограничиваем смещение за рамки
        float newX = Mathf.Clamp(imageRect.anchoredPosition.x + mouseDelta.x,
                                initialPosition.x - maxOffset,
                                initialPosition.x + maxOffset);
        float newY = Mathf.Clamp(imageRect.anchoredPosition.y + mouseDelta.y,
                                initialPosition.y - maxOffset,
                                initialPosition.y + maxOffset);

        // Устанавливаем новую позицию
        imageRect.anchoredPosition = new Vector2(newX, newY);
    }
}
