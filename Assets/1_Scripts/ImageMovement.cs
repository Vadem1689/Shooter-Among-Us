using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageMovement : MonoBehaviour
{
    public Image image; // ������ �� �������� (UI �������)
    public float speed = 5f; // �������� �����������
    public float maxOffset = 30f; // ������������ �������� �� �����

    private RectTransform imageRect;
    private Vector2 initialPosition;
    private float imageWidth;
    private float imageHeight;

    void Start()
    {
        // �������� RectTransform ��������
        imageRect = image.rectTransform;
        // ��������� ��������� �������
        initialPosition = imageRect.anchoredPosition;
        // �������� ������ � ������ ��������
        imageWidth = imageRect.rect.width;
        imageHeight = imageRect.rect.height;
    }

    void Update()
    {
        // �������� ��������� ������� ����
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        // ������������ ��������
        mouseDelta *= speed * Time.deltaTime;

        // ������������ �������� �� �����
        float newX = Mathf.Clamp(imageRect.anchoredPosition.x + mouseDelta.x,
                                initialPosition.x - maxOffset,
                                initialPosition.x + maxOffset);
        float newY = Mathf.Clamp(imageRect.anchoredPosition.y + mouseDelta.y,
                                initialPosition.y - maxOffset,
                                initialPosition.y + maxOffset);

        // ������������� ����� �������
        imageRect.anchoredPosition = new Vector2(newX, newY);
    }
}
