using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    [SerializeField] GameObject HomePanel;
    [SerializeField] GameObject Gold;
    [SerializeField] GameObject Diamond;
    [SerializeField] GameObject Setting;
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject ModePlay;
    [SerializeField] GameObject Avatar;
    private void Start()
    {
        NomalForm();
    }
    public void NomalForm()
    {
        HomePanel.SetActive(true);
        Gold.SetActive(true);
        Diamond.SetActive(true);
        MainMenu.SetActive(true);
        Setting.SetActive(true);
        ModePlay.SetActive(true);
    }
    public void ChooseAvatar()
    {
        Avatar.SetActive(true);
        ModePlay.SetActive(false);
    }
    public void TurnOffAvatar()
    {
        NomalForm();
        Avatar.SetActive(false);
    }
}
