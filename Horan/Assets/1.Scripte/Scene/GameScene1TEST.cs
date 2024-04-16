using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene1TEST : BaseScene
{
   
    [SerializeField]
    GameObject player; //�� ������ �ӽ�, ���� �÷��̾� ���� ���� ���� ���� 

    protected override void Init()
    {
        Debug.Log("GameScene1TEST");
        SceneName = "GameTest1";
        //Managers.UIManager.ShowSceneUI<LobbyUI>("CharacterHUD");
    }

    void Start()
    {
        //1. �ش� �ʿ� Ŭ����� ȣ��� �Լ��� ���ε�
        //2. �÷��̾� ĳ���� + ���� ĳ���� ����[����]
        //3. ī�޶� ����

        Managers.ContentsManager.OnWaveClear -= QuitLobby;
        Managers.ContentsManager.OnWaveClear += QuitLobby;
        
        //�÷��̾� ĳ���� ���� �ڵ� �ʿ�
        Camera.main.GetComponent<CameraComponent>().SetPlayer(player);
    }

    public override void Clear()
    {

    }

    void QuitLobby()
    { Managers.MySceneManager.LoadScene("Lobby"); }
}
