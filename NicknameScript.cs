using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Floor Layout Scene 에서 Canvas -> NamesBG 아래에 있는 값들을 변수로 세팅한다.
// 당연히 NamesBG 에 해당 Script 가 붙어있어야 한다.
public class NicknameScript : MonoBehaviour
{
    public Text[] nickNames;
    public Image[] healthBars;

    private void Start()
    {
        // 맨 처음에는 모든 항목을 거두다가 실제 player 가 접속하면 그때 다시 해당 내용을 켠다 => DisplayColor AssignColor() 참고
        for (int i = 0; i < nickNames.Length; i++)
        {
            nickNames[i].gameObject.SetActive(false);
            healthBars[i].gameObject.SetActive(false);
        }
    }
}
