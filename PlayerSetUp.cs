using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetUp : MonoBehaviour, IPunInstantiateMagicCallback
{
    // void Start()
    void OnEnable()
    {

        // Scene �� Instantiate �Ǿ� �ִ� ���� �÷��̾�� �߿��� ����  �����ϴ� �÷��̾��� 
        if (this.gameObject.GetComponent<PhotonView>().IsMine == false)
        {
            // ���� �����ϴ� �÷��̾ �ƴ϶��, �� �ٸ� ����� �����ϴ� �÷��̾���
            // enable false �� ���� �ٸ� ��� �Ÿ� control ���� ���ϰ� �Ѵ�
            enabled = false;

            // �׷��ٸ� ���� movement �� �ǽð� ����ȭ�Ǿ �ٸ� ����鵵 �� �� �ְ� �Ϸ���
            // ��� �ؾ� �ұ� ?
            // PhotonView Component �� �ϴ� �߰��Ѵ�. �ش� Component ��
            // ���� ������Ʈ�� �������� ��Ʈ��ũ �� ����ȭ ��Ų��.
            // ���� ���� server ���� ������ �����ϰ��� �ϴ� ��� object �� PhotonView Component �� �־�� �Ѵ�
            // ex) Rig ������ update �Ǹ� �ش� ������ ����ȭ�ȴ�.
            // �׸��� �̷��� �߰��� Photon Transform View Component �� Photon View Component ��
            // Observed Components ����Ʈ�� �ڵ����� �߰��ȴ�

            // ����, Obeservable �� Observable Search �ɼ��� Manual �� �ٲ۴�. �׸���
            // ���� Component �� Animator �� �߰��ϴ� ������ �� ���� �ִ�
            // �̷��� Animator Component �� �߰��ϸ�, Photon Animator View Component �� �ڵ����� �߰��ȴ�
            // �׸��� �츮�� Animator �� �߰��ص� BlendVertical, BlendHorizontal �� �ڵ����� �߰��Ѵ�

            // ����, �̷��� Prefab �� ������ �����ϸ�, ���� Scene ���� �ش� object �� �����ϴ� ������
            // ����� �� �����Ƿ� �ٽ� �������ָ� �ȴ�.

        }
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        // info.Sender: �� ������Ʈ�� ������ �÷��̾�
        // info.photonView: �� ������Ʈ�� PhotonView
        // ����: Player�� TagObject�� �� ������Ʈ�� ����
        info.Sender.TagObject = this.gameObject;

        // �߰��� �ʿ��� �ʱ�ȭ �ڵ� �ۼ�
        Debug.Log($"�÷��̾ ������Ʈ Ǯ�� ������Ʈ�� �����߽��ϴ�.");
    }
}
