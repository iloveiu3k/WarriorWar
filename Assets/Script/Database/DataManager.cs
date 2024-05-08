using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    private static DataManager _instance;
    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DataManager();
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }
    public string email="";
    public string pass = "";
    public string nickName = "";
    public string idPlayer = "";
    public Sprite avatar ;
    public void SetNickName(string nick)
    {
        nickName = nick;
    }
    public void SetIdPlayer(string id)
    {
        idPlayer = id;
    }
    public void SetAvatar(Sprite ava)
    {
        avatar = ava;
    }
}
