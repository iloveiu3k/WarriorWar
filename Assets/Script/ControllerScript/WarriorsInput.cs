using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorsInput : MonoBehaviour
{
    private List<string> Units = new List<string>();
    private List<string> spriteUnits = new List<string>();
    void Start()
    {
        Units = LoadStringList("Units");
        spriteUnits = LoadStringList("spriteUnits");
    }
    void Update()
    {
        
    }
    private List<string> LoadStringList(string key)
    {
        string serializedList = PlayerPrefs.GetString(key);
        List<string> stringList = new List<string>(serializedList.Split(','));
        return stringList;
    }
}
