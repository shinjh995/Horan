using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDataBind
{
    public void BindData();
}
public class DefaultDataLoader //CoreManager�� �� �ϳ��� ������ ���� Ŭ����
{
    public Dictionary<int, Data.Stat_Player> playerStatDict { get; private set; } = new Dictionary<int, Data.Stat_Player>();
    public Dictionary<string, Data.DataSet_Monster> DataCache_Monsters { get; private set; } = new Dictionary<string, Data.DataSet_Monster>();
    public Dictionary<int, Data.DataSet_Group> DataCache_Groups { get; private set; } = new Dictionary<int, Data.DataSet_Group>();


    //����ó�� �ڵ� �߰� ����...
    public void DefaultDataLoad()
    {
        playerStatDict = LoadData<Data.Stat_PlayerDataSeparator, int, Data.Stat_Player>("PlayerStatData").MakeDict();

        DataCache_Monsters = LoadData<Data.Separator_MonsterTable, string, Data.DataSet_Monster>("MonsterTable").MakeDict();

        DataCache_Groups = LoadData<Data.Separator_GroupTable, int, Data.DataSet_Group>("GroupTable").MakeDict();
    }

    //����ó�� �ڵ� �߰� ����...
    public DataDict LoadData<DataDict, Key, Value>(string DataFileName) where DataDict : Data.IDataSeparator<Key, Value>
    {
        TextAsset textasset = Resources.Load<TextAsset>($"Data/{DataFileName}");
        
        DataDict data = JsonUtility.FromJson<DataDict>(textasset.text);
       
        return JsonUtility.FromJson<DataDict>(textasset.text);
    }

}
