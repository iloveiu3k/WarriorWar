using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ManagerSoundInGame : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TMP_Text mainSound;
    [SerializeField] TMP_Text musicSound;
    [SerializeField] TMP_Text gameSound;

    [SerializeField] Slider mainSoundSC;
    [SerializeField] Slider musicSoundSC;
    [SerializeField] Slider gameSoundSC;
    public void SetMainSound()
    {
        mainSound.text = Mathf.RoundToInt(mainSoundSC.value * 100f).ToString() + "%";
    }
    public void SetMusicSound()
    {
        musicSound.text = Mathf.RoundToInt(musicSoundSC.value * 100f).ToString() + "%";

    }
    public void SetGameSound()
    {
        int soundPercentage = Mathf.RoundToInt(gameSoundSC.value * 100f);
        gameSound.text = soundPercentage.ToString() + "%";
    }
}
