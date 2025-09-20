using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCharacters : MonoBehaviour
{
    public GameObject character; // Prefab ���� ���� Resource/Folder �� YBot ���� �����Ѵ� 
    public Transform[] spawnPoints; // �̰͵� ���� ������ Transform Component �� ���� SpanwPoint ���� �巡�� ����ؼ� ����

    public GameObject[] weapons; // ����� 
    public Transform[] weaponSpawnPoints; // ���� spawn ��ġ��

    public float weaponRespawnTime = 10.0f; // ���� respawn �ð�


    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            // Scene Load �� Charactor Spawning ���̿� ������� ������ �ξ�� �Ѵ� 
            // player, sever lag �� �߻��� �� �ִ�
            StartCoroutine(WaitToSpawn());
        }
        else
        {
            Debug.Log("Not Connected");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(1.0f); // �� 2�ʶ�� �ð��� ���Ƿ� ������ ���̴�

        // Network �� ����� ��쿡�� Character ���� Spawn ��ų ���̴�.
        // Photon Network �� YBot Prefab �� ���� Scene �� Instantiate ��ų ���̴�
        // �̶� �츮�� Instantiate ��Ű���� �ϴ� Prefab ���� Photon View Component �� ������ �����̴�
        // ��, �Ϲ� Instantiate �Լ��� �����ϴ�. �ֳ��ϸ� �װ��� ���� ���� local computer ������ instantiate
        // ��Ű�� �Ǵ� ���̱� �����̴�

        // �̶� character.name �� ã�� ������, Resource Folder �� ����
        // �ش� name �� ��Ȯ�� ��ġ�ϴ� character �� ã�� ���̴�

        // Photon Network ������ Character �� ������ ������ count �� �ö󰣴�. �ش� ���� ����Ѵ�
        Debug.Log($"CountOfPlayers : {PhotonNetwork.CountOfPlayers}");
        Debug.Log($"CurrentRoom.PlayerCount : {PhotonNetwork.CurrentRoom.PlayerCount}");

        // �Ʒ� �ڵ��� ��� Room �� 1����� �ǵ��� ��� ����. ������ ���� Room �� ��쿡�� Room ���� CountOfPlayers �� ����ؾ� �Ѵ�.
        // CountOfPlayers �� Server ��ü�� Player ���� ��Ÿ���� �����̴�
        // PhotonNetwork.Instantiate(character.name,
        //     spawnPoints[PhotonNetwork.CountOfPlayers - 1].position,
        //     spawnPoints[PhotonNetwork.CountOfPlayers - 1].rotation);

        PhotonNetwork.Instantiate(character.name,
            spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount - 1].position,
            spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount - 1].rotation);

        Debug.Log("Connected Instantiate");
    }

    public void SpwanWeaponsStart()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            GameObject weapon = PhotonNetwork.Instantiate(weapons[i].name,
                weaponSpawnPoints[i].position,
                weaponSpawnPoints[i].rotation);
        }
    }
}
