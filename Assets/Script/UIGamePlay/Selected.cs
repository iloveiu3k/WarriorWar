using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selected : MonoBehaviour
{
    private GameObject SpriteName;
    public GameObject Unit;
    public void OnButtonClick()
    {
        SpriteName = transform.Find("warrior").GetComponentInChildren<Image>().gameObject;
        
        AllWarrior allWarrior = transform.parent.GetComponent<AllWarrior>();
        
        if (allWarrior != null)
        {
            allWarrior.OnButtonClick(SpriteName, Unit);
        }
    }
}
