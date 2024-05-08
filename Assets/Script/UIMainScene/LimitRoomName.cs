using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LimitRoomName : MonoBehaviour
{
    public int maxCharacterCount = 6; 
    public TMP_InputField inputField;
    private void Start()
    {
        inputField.characterLimit = maxCharacterCount;
    }


}
