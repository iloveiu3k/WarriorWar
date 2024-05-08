using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Photon.Pun.UtilityScripts;
using UnityEngine.UI;
using ExitGames.Client.Photon.StructWrapping;

public class UISelected : MonoBehaviourPunCallbacks
{
    public static UISelected instance;
    private void Awake()
    {
        instance = this;
    }
    [SerializeField] GameObject playerLocal;
    [SerializeField] GameObject playerRival;
    [SerializeField] Transform p;
    [SerializeField] TMP_Text TimeCountDown;
    [SerializeField] GameObject txtMatchStart;
    [SerializeField] GameObject TimeStart;
    [SerializeField] string Map;
    [SerializeField] AudioSource au;
    [SerializeField] Image mineSprite;
    [SerializeField] Image otherSprite;
    [SerializeField] List<Sprite> sprites;
    Image image;
    private float countdownTime = 120f;
    private float currentTime;
    private bool allPlayerReady=false;
    private bool loadMap = false;
    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            ZoneSelect();
            currentTime = countdownTime;
            PhotonNetwork.AutomaticallySyncScene = true;
        }
    }
    private void ZoneSelect()
    {
        playerLocal.GetComponentInChildren<TMP_Text>().text = PhotonNetwork.LocalPlayer.NickName;
        playerRival.GetComponentInChildren<TMP_Text>().text = PhotonNetwork.PlayerListOthers[0].NickName;
        mineSprite.sprite = DataManager.Instance.avatar;
        photonView.RPC("SetAvatarSelect", RpcTarget.Others, mineSprite.sprite.name);
    }
    [PunRPC]
    private void SetAvatarSelect(string name)
    {
        foreach(Sprite sprite in sprites)
        {
            if(sprite.name== name)
            {
                otherSprite.sprite = sprite;
                break;
            }
        }
    }
    private void Update()
    {
        if (allPlayerReady)
        {
            if (countdownTime > 1)
            {
                countdownTime -= Time.deltaTime;
                int seconds = Mathf.FloorToInt(countdownTime);
                TimeStart.GetComponent<TMP_Text>().text = seconds.ToString();
            }
            else
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    if (loadMap == false)
                    {
                        PhotonNetwork.LoadLevel(Map);
                        loadMap = true;
                    }
                }
            }
        }
        else
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                int seconds = Mathf.FloorToInt(currentTime);
                TimeCountDown.text = seconds.ToString();
            }
            else if (currentTime <= 1f)
            {
                TimeCountDown.text = "Ready";
            }
        }
    }
    private void StartGame()
    {
        PhotonNetwork.LoadLevel(Map);
    }
    public void SetCurrentTime(float time)
    {
        this.currentTime=time;
    }
    public void SetAllReady()
    {
        countdownTime = 10;
        txtMatchStart.SetActive(true);
        TimeStart.SetActive(true);
        TimeCountDown.gameObject.SetActive(false);
        allPlayerReady = true;
        au.Play();
    }
}
