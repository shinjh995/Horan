using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySceneManager
{
    public BaseScene CurScene;
    //����ó�� �ڵ� �߰� ����
    public void LoadScene(string nextScene)
    {
        LoadingScene.LoadScene(nextScene);
    }

    public void CloseScene()
    { }
}
