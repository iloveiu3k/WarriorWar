using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineWarriorInGame : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<GameObject> mineWarriors;
    [SerializeField] List<Sprite> mineSprites;
    private List<string> allWarriorReceive;
    private List<string> allSpriteReceive;
    void Start()
    {
        allSpriteReceive = LoadStringList("SpriteUnits");
        allWarriorReceive = LoadStringList("Units");
        for(int i = 0; i < allSpriteReceive.Count; i++)
        {
            //Debug.Log(transform.Find("Unit" + i).name);
            foreach (Sprite mineSpr in mineSprites)
            {
                if (allSpriteReceive[i] == mineSpr.name)
                {
                    transform.Find("Unit" + (i+1)).transform.Find("avatar").GetComponentInChildren<Image>().sprite = mineSpr;
                }
            }
            foreach(GameObject mineWa in mineWarriors)
            {
                if (allWarriorReceive[i] == mineWa.name)
                {
                    transform.Find("Unit" + (i + 1)).GetComponentInChildren<DetailWarrior>().SetWarrior(mineWa);
                }
            }
        }
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
