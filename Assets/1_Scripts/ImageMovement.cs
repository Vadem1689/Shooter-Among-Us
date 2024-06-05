using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageMovement : MonoBehaviour
{
    public Image image; // ������ �� �������� (UI �������)
    public float speed = 5f; // �������� �����������
    public float duplicationThreshold = 0.5f; // ����� ��� ������������ (0-1)

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
        print(mouseDelta);
        // ���������� �������� � ��� �� �����������
        imageRect.anchoredPosition += mouseDelta ;

        // ���������, ����� �� ����������� ��������
        if (CheckDuplication())
        {
            DuplicateImage();
        }
    }

    // ��������, ����� �� ����������� ��������
    private bool CheckDuplication()
    {
        // ���������, ��������� �� ���� �����, ��� "duplicationThreshold" � ���� ��������
        return (imageRect.anchoredPosition.x < initialPosition.x - imageWidth * duplicationThreshold ||
                imageRect.anchoredPosition.x > initialPosition.x + imageWidth * duplicationThreshold ||
                imageRect.anchoredPosition.y < initialPosition.y - imageHeight * duplicationThreshold ||
                imageRect.anchoredPosition.y > initialPosition.y + imageHeight * duplicationThreshold);
    }

    // ������������ ��������
    private void DuplicateImage()
    {
        // ������� �������� ��������
        Image newImage = Instantiate(image);
        // ������������� ������� ���������
        newImage.rectTransform.anchoredPosition = imageRect.anchoredPosition;
        // ������������� �������� ���������
        newImage.transform.SetParent(image.transform.parent);

        // ��������� ��������� �������
        initialPosition = imageRect.anchoredPosition;
    }
}
