using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberResource 
{
    // Start is called before the first frame update
    public int tree { get; set; }
    public int stone { get; set; }
    public int gold { get; set; }
    public NumberResource() { }
    public NumberResource(int tree, int stone, int gold)
    {
        this.tree = tree;
        this.stone = stone;
        this.gold = gold;
    }
}
