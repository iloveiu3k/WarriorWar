using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PlayerSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerSpawner instance;
    private void Awake()
    {
        instance = this;
    }
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform spawnPoint1;
    [SerializeField] Transform spawnPoint2;
    [SerializeField] Transform PositionTeam1;
    [SerializeField] Transform PositionTeam2;
    [SerializeField] GameObject Team1;
    [SerializeField] GameObject Team2;
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            SpawnPlayer();
        }
    }

    public void SpawnPlayer()
    {
        string playerName = PhotonNetwork.LocalPlayer.NickName;
        GameObject player;
        if (PhotonNetwork.IsMasterClient)
        {
            player=PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPoint1.position, spawnPoint1.rotation);
        }
        else
        {
            player=PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint2.position, spawnPoint2.rotation);
        }

    }
}
