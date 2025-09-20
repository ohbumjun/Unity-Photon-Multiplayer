using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 해당 스크립트는 Lobby Scene 에서 Button 들에 들어있다.
public class Descriptions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject dropDown;

    // 마우스가 해당 오브젝트 위에 올라갔을 때 호출되는 함수
    public void OnPointerEnter(PointerEventData eventData)
    {
        dropDown.SetActive(true); // 처음에는 설명 UI 를 켠다
    }
    // 마우스가 오브젝트에서 벗어났을 때 호출되는 함수
    public void OnPointerExit(PointerEventData eventData)
    {
        dropDown.SetActive(false); // 처음에는 설명 UI 를 끈다
    }

    // Start is called before the first frame update
    void Start()
    {
        dropDown.SetActive(false); // 처음에는 설명 UI 를 끈다
    }

}
