using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Enviroment : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private float heal;

    private float maxHeal;
    private string TypeResource;
    void Start()
    {
        TypeResource = transform.root.tag;
    }

    // Update is called once per frame
    void Update()
    {
        if (heal <= 0 )
        {
            Destroy(gameObject);
        }
    }
    public void takeDame(float dameIn)
    {
        this.heal -= dameIn;
    }
    public float getHeal()
    {
        return this.heal;
    }
    public float getMaxHeal()
    {
        return this.maxHeal;
    }
    public void SetHeal(float heal)
    {
        this.heal = heal;
    }
    public string getTag()
    {
        return this.TypeResource;
    }
}
