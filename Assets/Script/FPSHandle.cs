using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine.Windows;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class FPSHandle : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public int Fps =60;
    public TMP_Text fpsText;
    public float deltaTime;
    private bool flag = true;
    [SerializeField] GameObject Setting;
    [SerializeField] GameObject guidance;
    [SerializeField] GameObject House1;
    [SerializeField] GameObject House2;
    [SerializeField] GameObject WinLose;
    [SerializeField] GameObject Win;
    [SerializeField] GameObject Lose;
    [SerializeField] GameObject btnOut;
    bool checkWin = false;
    void Start()    
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = Fps;
    }
    void Update()
    {
        if (flag)
        {
            StartCoroutine(ShowFPS());
        }
        if (UnityEngine.Input.GetKey(KeyCode.Tab))
        {
            guidance.SetActive(true);
        }
        else if (UnityEngine.Input.GetKeyUp(KeyCode.Tab))
        {
            guidance.SetActive(false);
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {
            Setting.SetActive(true);
        }
        if (House1 != null&House2!=null)
        {
            if (House1.GetComponent<Character>().getHeal() <= 0f && !checkWin)
            {
                WinLose.SetActive(true);
                checkWin = true;
                if (PhotonNetwork.IsMasterClient)
                {
                    Lose.SetActive(true);
                }
                else
                {
                    Win.SetActive(true);
                }
            }
            else if (House2.GetComponent<Character>().getHeal() <= 0f && !checkWin)
            {
                WinLose.SetActive(true);
                checkWin = true;
                if (PhotonNetwork.IsMasterClient)
                {
                    Win.SetActive(true);
                }
                else
                {
                    Lose.SetActive(true);
                }
            }
        }

    }
    public void TurnOff()
    {
        Setting.SetActive(false);
    }
    private IEnumerator ShowFPS()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        flag = false;
        yield return new WaitForSeconds(2);
        flag = true;
        if (fpsText != null)
        {
            fpsText.text = "Fps: " + Mathf.Ceil(fps).ToString();
        }
    }
    public void OutGame()
    {
        btnOut.SetActive(true);
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(1);
        base.OnLeftRoom();
    }
    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
}
