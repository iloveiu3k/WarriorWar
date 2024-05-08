using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GetSourceImage : MonoBehaviour
{
    // Start is called before the first frame update
    private Sprite SourceImage;
    private Image image;
    ChangeAvatar change ;

    private void Start()
    {
        change = FindObjectOfType<ChangeAvatar>();
        //image = transform.FindChild("Avatar").GetComponentInChildren<Image>();
    }
    public void ChangeImage()
    {
        SourceImage = transform.Find("Avatar").GetComponentInChildren<Image>().sprite;
        //SourceImage = image.sprite;
        foreach(Sprite sprite in change.GetAllAvatar())
        {
            if(sprite.name == SourceImage.name)
            {
                change.GetImageTochange().sprite = sprite;
                DataManager.Instance.SetAvatar(sprite);
                return;
            }
        }
    }
}
