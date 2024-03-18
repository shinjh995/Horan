using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    //������ ������ Ű���� �������� �и��Ͽ� �����ϱ� ���� �������̽�
    public interface IDataSeparator<Key, Value>
    {
        Dictionary<Key, Value> MakeDict();
    }

    #region Stats
    [Serializable] 
    public class Stat_Player
    {
        public int level;
        public int maxHp;
        public int attack;
        public int totalExp;
    }
    [Serializable]
    public class Stat_PlayerDataSeparator : IDataSeparator<int, Stat_Player> //������������ ���ӿ� �ε�� ���·� �߶� �ϴ� Ŭ����
    {
        public List<Stat_Player> playerstats = new List<Stat_Player>(); //������ ������ ����Ʈ�� �ش� ������ �̸��� �����ؾ� ��

        public Dictionary<int, Stat_Player> MakeDict()
        {
            Dictionary<int, Stat_Player> dict = new Dictionary<int, Stat_Player>();
            foreach (Stat_Player data in playerstats)
                dict.Add(data.level, data);
            return dict;
        }
    }

    [Serializable]
    public class Stat_Monster
    { }

    [Serializable]
    public class Stat_Equipment
    { }
    #endregion

    #region DataSet2 

    #endregion

}
