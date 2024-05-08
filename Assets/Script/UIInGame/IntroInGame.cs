using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class IntroInGame : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private string team;
    private Transform pointTarget;
    private Transform pointTeamBlue;
    private Transform pointTeamRed;
    private GameObject panel;
    private GameObject uiUnit;
    private GameObject re;
    private float speed = 10f;
    private bool start=true;

    void Start()
    {
        if (photonView.IsMine)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Start");
            foreach(GameObject obj in objs)
            {
                if(obj.name== "WelCome")
                {
                    panel = obj;
                }else if(obj.name == "UIUnits")
                {
                    uiUnit = obj;
                }
            }
            re = GameObject.FindGameObjectWithTag("Resource");
            pointTeamBlue = GameObject.Find("pointBlue").transform;
            pointTeamRed = GameObject.Find("pointRed").transform;
            transform.GetComponent<CameraController>().enabled = false;
            panel.SetActive(true);
            uiUnit.SetActive(false);
            re.SetActive(false);
            if (PhotonNetwork.IsMasterClient)
            {
                pointTarget = pointTeamBlue;
            }
            else
            {
                pointTarget = pointTeamRed;
            }
        }
    }
    void Update()
    {
        if (photonView.IsMine)
        {
            StartGame();
        }
    }
    void StartGame()
    {
        if (photonView.IsMine)
        {
            if (start)
            {
                Vector3 direction = pointTarget.position - transform.position;
                direction.Normalize();
                transform.Translate(direction * speed * Time.deltaTime);
            }
            else
            {
                transform.GetComponent<CameraController>().enabled = true;
                panel.SetActive(false);
                uiUnit.SetActive(true);
                re.SetActive(true);
            }
            if (Vector3.Distance( transform.position ,pointTarget.position)<=1 && start)
            {
                start = false;
            }
        }
    }
    
}
