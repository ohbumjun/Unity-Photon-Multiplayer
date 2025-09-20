using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Floor Layout Scene ���� Canvas -> NamesBG �Ʒ��� �ִ� ������ ������ �����Ѵ�.
// �翬�� NamesBG �� �ش� Script �� �پ��־�� �Ѵ�.
public class NicknameScript : MonoBehaviour
{
    public Text[] nickNames;
    public Image[] healthBars;

    private void Start()
    {
        // �� ó������ ��� �׸��� �ŵδٰ� ���� player �� �����ϸ� �׶� �ٽ� �ش� ������ �Ҵ� => DisplayColor AssignColor() ����
        for (int i = 0; i < nickNames.Length; i++)
        {
            nickNames[i].gameObject.SetActive(false);
            healthBars[i].gameObject.SetActive(false);
        }
    }
}
