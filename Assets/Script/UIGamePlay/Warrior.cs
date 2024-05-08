using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Warrior : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField] GameObject warrior;
    [SerializeField] List<Sprite> imageWarriors;
    [SerializeField] List<GameObject> warriors;
    public List<GameObject> GetWarriors()
    {
        return this.warriors;
    }
    public List<Sprite> ImageWarriors()
    {
        return this.imageWarriors;
    }
    public GameObject GetWarrior(int i)
    {
        //Debug.Log(warriors[i].name);
        return warriors[i];
    }
}
