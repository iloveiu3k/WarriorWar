using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChangeAvatar : MonoBehaviour
{
    private Image imageToChange;
    [SerializeField] List<Sprite> Images;
    void Start()
    {
        imageToChange = GetComponent<Image>();
        foreach(Sprite sprite in Images)
        {
            if (sprite == DataManager.Instance.avatar)
            {
                imageToChange.sprite = sprite;
            }
        }
    }
    public Image GetImageTochange()
    {
        return this.imageToChange;
    }
    public List<Sprite> GetAllAvatar()
    {
        return Images;
    }
}
