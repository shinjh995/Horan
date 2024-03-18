using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDataBind
{
    public void BindData();
}
public class DefaultDataLoader //CoreManager�� �� �ϳ��� ������ ���� Ŭ����
{
    /* 
     * ���� ������ ������ �޾Ƽ� ���� ��Ű�� ���·� ����
     * �ڽ��� � �����͸� �޾ƾ��ϴ��� �ĺ��ڸ� �����ؾ��� (enum����)
     * 
     * 1.�ε尡 �ʿ��� ������ ���� �˻�  
     * 2.����� �����͸� �ε�
     * 3.������ ����
     */

    public Dictionary<int, Data.Stat_Player> playerStatDict { get; private set; } = new Dictionary<int, Data.Stat_Player>();
    
    //����ó�� �ڵ� �߰� ����...
    public void DefaultDataLoad()
    {
        playerStatDict = LoadData<Data.Stat_PlayerDataSeparator, int, Data.Stat_Player>("PlayerStatData").MakeDict();
    }

    //����ó�� �ڵ� �߰� ����...
    public DataDict LoadData<DataDict, Key, Value>(string DataFileName) where DataDict : Data.IDataSeparator<Key, Value>
    {
        TextAsset textasset = Resources.Load<TextAsset>($"Data/{DataFileName}");
        
        DataDict data = JsonUtility.FromJson<DataDict>(textasset.text);
       
        return JsonUtility.FromJson<DataDict>(textasset.text);
    }

}
