using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Xml;

public class AllWarrior : MonoBehaviourPunCallbacks
{

    [SerializeField] GameObject P1;
    [SerializeField] GameObject P2;
    Warrior warr, warr2;
    private int count=1;
    private int countOther = 1;
    private GameObject lastClickedSpriteName=null;
    private bool doneSelect =false;
    private GameObject Unit;
    private Sprite spriteUnit;
    private List<string> Units = new List<string>();
    private List<string> spriteUnits = new List<string>();
    private void Start()
    {
        warr = P1.GetComponentInChildren<Warrior>();
    }
    public void OnButtonClick(GameObject spriteName,GameObject unit)
    {
        if (spriteName != lastClickedSpriteName)
        {
            lastClickedSpriteName = spriteName;
            Unit = unit;
            spriteUnit = spriteName.GetComponent<Image>().sprite;
            ChangeWarrior();
        }
        else
        {
            lastClickedSpriteName = null;
        }
    }
    public void ChangeWarrior()
    {
        if (count <= 6)
        {
            if (lastClickedSpriteName != null)
            {
                warr.GetWarrior(count - 1).transform.Find("warrior").gameObject.SetActive(true);
                warr.GetWarrior(count - 1).transform.Find("warrior").GetComponentInChildren<Image>().sprite = lastClickedSpriteName.GetComponent<Image>().sprite;
                if (PhotonNetwork.IsConnected)
                {
                    photonView.RPC("UpdateOtherPlayer", RpcTarget.OthersBuffered, lastClickedSpriteName.GetComponent<Image>().sprite.name);
                }
            }
        }
    }
    [PunRPC]
    public void UpdateOtherPlayer(string name)
    {
        warr2 = P2.GetComponentInChildren<Warrior>();
        Sprite sprite = SpriteOtherPlayer(warr2.ImageWarriors(), name);
        if (warr2 != null&&sprite!=null)
        {
            //Debug.Log(sprite.name);
            warr2.GetWarrior(countOther - 1).transform.Find("warrior").gameObject.SetActive(true);
            warr2.GetWarrior(countOther - 1).transform.Find("warrior").GetComponentInChildren<Image>().sprite = sprite;
        }
    }
    [PunRPC]
    public void UpdateBoxCount(int i)
    {
        countOther = i;
    }
    private Sprite SpriteOtherPlayer(List<Sprite> sprites,string name)
    {
        foreach(Sprite sprite in sprites)
        {
            if (sprite.name == name)
            {
                return sprite;
            }
        }
        return null;
    }
    public void Selected()
    {
        if (lastClickedSpriteName != null && count<=6 && Unit != null)
        {
            //Debug.Log(Unit.name);
            count+=1;
            Units.Add(Unit.name);
            spriteUnits.Add(spriteUnit.name);
            lastClickedSpriteName.SetActive(false);
            lastClickedSpriteName.transform.parent.GetComponent<Button>().enabled=false;
            lastClickedSpriteName = null;
            Unit = null;
            spriteUnit = null;
            photonView.RPC("UpdateBoxCount", RpcTarget.OthersBuffered, count);
        }
        if (count == 7)
        {
            doneSelect = true;
            photonView.RPC("SelectDone", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber, doneSelect);
        }
    }
    [PunRPC]
    public void SelectDone(int actorNumber,bool done)
    {
        Player player = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        player.CustomProperties["IsReadySelected"] = done;
        //Debug.Log(player.NickName);
        CheckAllPlayerReady();
    }
    private void Update()
    {
        if (count>6)
        {
            transform.root.GetComponent<UISelected>().SetCurrentTime(0f);
        }
    }
    private void CheckAllPlayerReady()
    {
        Player[] players = PhotonNetwork.PlayerList;
        //Debug.Log(players.Length);
        bool allPlayersReady = true;
        foreach (Player player in players)
        {
            if (!player.CustomProperties.ContainsKey("IsReadySelected") || !(bool)player.CustomProperties["IsReadySelected"])
            {
                //Debug.Log(player.NickName);
                allPlayersReady = false;
                break;
            }
            if (player.CustomProperties.ContainsKey("IsReadySelected"))
            {
                Debug.Log(player.NickName);
            }
        }
        if (allPlayersReady)
        {
            SaveStringList(Units, "Units");
            SaveStringList(spriteUnits, "SpriteUnits");
            transform.root.GetComponent<UISelected>().SetAllReady();
        }
    }
    [PunRPC]
    private void AllPlayerReady()
    {
    }
    private void SaveStringList(List<string> stringList, string key)
    {
        string serializedList = string.Join(",", stringList.ToArray());
        Debug.Log(serializedList);
        PlayerPrefs.SetString(key, serializedList);
    }
}
