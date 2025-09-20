using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotate : MonoBehaviour
{
    public float speed = 20;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0); // y 축 회전만 진행    

        // 중요한 point . 해당 rotate 이 Network 상에 동기화 되려면 마찬가지로
        // 해당 Script 를 장착한 Weapon Prefab 에 Photon Transform View Component 이 있어야 한다
    }
}
