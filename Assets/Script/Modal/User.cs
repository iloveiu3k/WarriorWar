using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject nameUser { get; set; }
    public GameObject avatarUser { get; set; }

    public GameObject levelNumberUser { get; set; }

    public GameObject levelBarUser { get; set; }

    public User(GameObject nameUser, GameObject avatarUser, GameObject levelNumberUser, GameObject levelBarUser)
    {
        this.nameUser = nameUser;
        this.avatarUser = avatarUser;
        this.levelNumberUser = levelNumberUser;
        this.levelBarUser = levelBarUser;
    }
}
