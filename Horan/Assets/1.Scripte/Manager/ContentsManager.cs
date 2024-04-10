using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameModes { StoryMode, DefanseMode, TutorialMode }
public class ContentsManager 
{
    /*  �Ŵ����� ���
     * �� ��帶�� �ΰ��� ���뿡 ���� �����Ǿ� ����� �����͵��� �����Ͽ� ������ ����ɶ� ���̺������� ����� �C�����ִ� ����
     * + �� ���Ӹ���� ���� ����� ó���Ǿ���ϴ� ���۵��� �޼���� ����
     * 
     * ���丮��� : ���� ����, ���� ���� , ���̺� ���� , ���̺� ���� ,�� �ش��ϴ� �Լ��� ContentsManager�� ����(ȣ���� �� Scene����)
     * 
     * ���� ���� �帧)
     * ������ ���丮 ��带 ���� -> ���������� Ŭ�� -> �� �̵� -> �� ����ũ��Ʈ���� ���ӽ��� �Լ� ȣ�� -> �������� ����
     * �ᱹ �� ���� ���۰� ���������� �Ѿ�� �κ��� ���� �����ϴ�  ����ũ��Ʈ���� ���
     * ������ ���� ����Ǵ��� �ΰ��� ������ ������ �Ѿ�� �ֵ��� ������ �Ŵ������� ������ ����
     * 
     * ���� GameTest1������ ����ũ��Ʈ ����� �ű⼭ �������Ŵ����� GameStart()�� ȣ���ϰ� �ϴ� ������� ó�� 
     * 
     * �ƴ� ���� ���� �����Ǹ� �ʱ�ȭ������ OnStageClear�� �ڽ��� �׾����� ������ 
     * 
     * ������ ���� ���͸� �� ��´�? ������ ���� ��� ��������?
     * ���Ͱ� �����ɶ� �������Ŵ������� �ڽ��� ������������ �˸��� 
     * �׷� curWaveMonsterCounts���� ������Ű�� 
     * ������ ���� �ش� ���� �ϳ��� ������
     * 
     * ���� ���Ҷ� ���� �ش� ���� 0�̵Ǿ����� Ȯ���ϰ� 0�� �Ǿ����� ������ ����ȴ�. 
     * 
     * �������̽��� �ѱ��?
     * 
     */
    int curStageindex = 0;
    int waveMonsterCounts;
    public int WaveMonsterCounts 
    { 
        get { return waveMonsterCounts; } 
        set { waveMonsterCounts = value; if (waveMonsterCounts == 0) if(OnWaveClear!=null) OnWaveClear.Invoke(); } 
    }
    

    //����ũ��Ʈ���� �ش� ��������Ʈ ���ε��ؼ� Ŭ����� ���̵��ǰ� �����
    public Action OnWaveClear; 


    public void StageStart(GameModes gameMode,int stage=1) //�̰Ŵ� �κ񿡼� ���ÿ�
    {
        switch (gameMode)
        {
            case GameModes.StoryMode:
                curStageindex = stage;
                break;
            case GameModes.DefanseMode:
                break;
            case GameModes.TutorialMode:
                break;
        }
    }
    public void StageEnd()
    {

        //Application.Quit();   
    }
    public void NextWave() //������������ ���� ���������� �Ѿ�� ȣ��
    {
        
    }

}

