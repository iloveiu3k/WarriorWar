using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class HealthBar : MonoBehaviourPunCallbacks
{
    private Vector3 desiredRotation = new Vector3(-60f, 180f, 0f);    
    private void Start()
    {
        if (transform.root.CompareTag("Unit"))
        {
            if (!photonView.IsMine)
            {
                transform.Find("Handle").GetComponentInChildren<Image>().color = Color.red;
            }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient && transform.root.CompareTag("Team2"))
            {
                transform.Find("Handle").GetComponentInChildren<Image>().color = Color.red;
                Vector3 currentRotation = transform.rotation.eulerAngles;
                transform.rotation = Quaternion.Euler(desiredRotation);
            }
            else if (!PhotonNetwork.IsMasterClient && transform.root.CompareTag("Team1"))
            {
                transform.Find("Handle").GetComponentInChildren<Image>().color = Color.red;
                Vector3 currentRotation = transform.rotation.eulerAngles;
                transform.rotation = Quaternion.Euler(desiredRotation);
            }
        }
    }
    void Update()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(desiredRotation);
    }

}
