using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageMovement : MonoBehaviour
{
    public Image image; // Ссылка на картинку (UI элемент)
    public float speed = 5f; // Скорость перемещения
    public float duplicationThreshold = 0.5f; // Порог для дублирования (0-1)

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
        print(mouseDelta);
        // Перемещаем картинку в том же направлении
        imageRect.anchoredPosition += mouseDelta ;

        // Проверяем, нужно ли дублировать картинку
        if (CheckDuplication())
        {
            DuplicateImage();
        }
    }

    // Проверка, нужно ли дублировать картинку
    private bool CheckDuplication()
    {
        // Проверяем, находится ли мышь ближе, чем "duplicationThreshold" к краю картинки
        return (imageRect.anchoredPosition.x < initialPosition.x - imageWidth * duplicationThreshold ||
                imageRect.anchoredPosition.x > initialPosition.x + imageWidth * duplicationThreshold ||
                imageRect.anchoredPosition.y < initialPosition.y - imageHeight * duplicationThreshold ||
                imageRect.anchoredPosition.y > initialPosition.y + imageHeight * duplicationThreshold);
    }

    // Дублирование картинки
    private void DuplicateImage()
    {
        // Создаем дубликат картинки
        Image newImage = Instantiate(image);
        // Устанавливаем позицию дубликата
        newImage.rectTransform.anchoredPosition = imageRect.anchoredPosition;
        // Устанавливаем родителя дубликата
        newImage.transform.SetParent(image.transform.parent);

        // Обновляем начальную позицию
        initialPosition = imageRect.anchoredPosition;
    }
}
