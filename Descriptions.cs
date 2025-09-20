using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// �ش� ��ũ��Ʈ�� Lobby Scene ���� Button �鿡 ����ִ�.
public class Descriptions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject dropDown;

    // ���콺�� �ش� ������Ʈ ���� �ö��� �� ȣ��Ǵ� �Լ�
    public void OnPointerEnter(PointerEventData eventData)
    {
        dropDown.SetActive(true); // ó������ ���� UI �� �Ҵ�
    }
    // ���콺�� ������Ʈ���� ����� �� ȣ��Ǵ� �Լ�
    public void OnPointerExit(PointerEventData eventData)
    {
        dropDown.SetActive(false); // ó������ ���� UI �� ����
    }

    // Start is called before the first frame update
    void Start()
    {
        dropDown.SetActive(false); // ó������ ���� UI �� ����
    }

}
