using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
public class DetailWarrior : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public GameObject warrior;
    public Transform PositionOfTeam1;
    public Transform PositionOfTeam2;
    [SerializeField] private TMP_Text numberTree;
    [SerializeField] private TMP_Text numberStone;
    [SerializeField] private TMP_Text numberGold;
    [SerializeField] Image imageLoad;
    [SerializeField] private TMP_Text numberLoads;
    private bool state= true;
    private int numberUnits = 0;
    List<GameObject> Units = new List<GameObject>();
    private float timeWait;
    private float currTime=0;
    private NumberResource numberResource;
    private NumberResource currResource;
    private void Start()
    {
        timeWait = warrior.GetComponentInChildren<Character>().GetTimeLoad();
        numberResource = warrior.GetComponentInChildren<Character>().GetNumberResource();
    }
    public void SummonWarrior()
    {
        if (currResource.tree >= numberResource.tree && currResource.stone >= numberResource.stone && currResource.gold >= numberResource.gold)
        {
            numberTree.text = (currResource.tree - numberResource.tree).ToString();
            numberStone.text = (currResource.stone - numberResource.stone).ToString();
            numberGold.text = (currResource.gold - numberResource.gold).ToString();

            numberUnits++;
        }
    }
    public void Update()
    {
        currResource = new NumberResource(Convert(numberTree), Convert(numberStone), Convert(numberGold));
        if (numberUnits > 0)
        {
            imageLoad.gameObject.SetActive(true);
            numberLoads.gameObject.SetActive(true);
            numberLoads.text = numberUnits.ToString();
            if (state == true)
            {
                imageLoad.fillAmount = 1;
                currTime = 0;
                state = false;
            }
            else
            {
                if (currTime < timeWait)
                {
                    float fillValue = 1 - currTime / timeWait;
                    fillValue = Mathf.Max(fillValue, 0);
                    imageLoad.fillAmount = fillValue;
                    currTime += Time.deltaTime;
                }
                else
                {
                    waitSummonWarrior();
                    state = true;
                }
            }
        }
        else
        {
            numberLoads.gameObject.SetActive(false);
            imageLoad.gameObject.SetActive(false);
        }
    }
    private void waitSummonWarrior()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject currWarrior;
            currWarrior = PhotonNetwork.Instantiate(warrior.name, GetRandomPosition(PositionOfTeam1.position), Quaternion.identity);
            numberUnits--;
        }
        else
        {
            GameObject currWarrior;
            currWarrior = PhotonNetwork.Instantiate(warrior.name, GetRandomPosition(PositionOfTeam2.position), Quaternion.identity);
            numberUnits--;
        }
    }
    private Vector3 GetRandomPosition(Vector3 center)
    {
        float randomAngle = Random.Range(0f, 360f);
        float randomDistance = Random.Range(4f, 10f);
        float angleInRadians = randomAngle * Mathf.Deg2Rad;
        float x = center.x + randomDistance * Mathf.Cos(angleInRadians);
        float z = center.z + randomDistance * Mathf.Sin(angleInRadians);
        float y = center.y;
        return new Vector3(x, y, z);
    }
    public void SetWarrior(GameObject mine)
    {
        this.warrior = mine;
    }
    private int Convert(TMP_Text textConvert)
    {
        // Lấy giá trị từ TMP_Text
        string textValue = textConvert.text;

        // Chuyển đổi chuỗi thành int
        if (int.TryParse(textValue, out int result))
        {
            return result;
        }
        else
        {
            return 0;
        }
    }
}
