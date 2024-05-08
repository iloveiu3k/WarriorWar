using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
public class CrafterCombat : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField] public GameObject UIResource;
    [SerializeField] private GameObject PickAxe;
    [SerializeField] private GameObject Hatchet;
    private GameObject Resource;
    private Animator animator;
    [SerializeField] float speedAttack;
    private bool isAttacking = false;
    private Coroutine attackCoroutine;
    private bool finalAttack = true;
    private int numberOfStone = 0;
    private int numberOfTree = 0;
    private int numberOfGold = 0;
    private int maxResource = 25;
    int typeResource=0;
    void Start()
    {
        animator = GetComponent<Animator>();
        UIResource = GameObject.FindGameObjectWithTag("Resource");
    }
    private IEnumerator Attacking(int typeResource)
    {
        attackCoroutine = null;
        isAttacking = true;
        animator.SetInteger("Trigger Number", typeResource);
        photonView.RPC("reduceDelayAttack", RpcTarget.All);
        if (finalAttack)
        {
            yield return new WaitForSeconds(speedAttack);
        }
        isAttacking = false;
    }
    [PunRPC]
    private void reduceDelayAttack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }
    private void FinalAttack()
    {
        finalAttack = true;
        animator.SetInteger("Trigger Number", 0);
    }
    private void StartAttack()
    {
        finalAttack = false;
    }
    private void haveDame()
    {
        if (photonView.IsMine)
        {
            if (Resource != null)
            {
                Enviroment resource = Resource.GetComponentInChildren<Enviroment>();
                Character body = GetComponent<Character>();
                if (resource.tag == "Tree")
                {
                    Debug.Log(numberOfTree);
                    if (numberOfTree >= maxResource)
                    {
                        int number = int.Parse(UIResource.transform.Find("Tree").GetComponentInChildren<TMP_Text>().text) + 25;
                        UIResource.transform.Find("Tree").Find("Number").GetComponentInChildren<TMP_Text>().text = number.ToString();
                        numberOfTree = 0;
                        return;
                    }
                    numberOfTree += 2;
                }
                else if (resource.tag == "Stone")
                {
                    if (numberOfStone >= maxResource)
                    {
                        int number = int.Parse(UIResource.transform.Find("Stone").GetComponentInChildren<TMP_Text>().text) + 25;
                        UIResource.transform.Find("Stone").Find("Number").GetComponentInChildren<TMP_Text>().text = number.ToString();
                        numberOfStone = 0;
                        return;
                    }
                    numberOfStone += 2;
                }
                else if (resource.tag == "Gold")
                {
                    if (numberOfGold >= maxResource)
                    {
                        int number = int.Parse(UIResource.transform.Find("Gold").GetComponentInChildren<TMP_Text>().text) + 25;
                        UIResource.transform.Find("Gold").Find("Number").GetComponentInChildren<TMP_Text>().text = number.ToString();
                        numberOfGold = 0;
                        return;
                    }
                    numberOfGold += 2;
                }
                photonView.RPC("takeDameResource", RpcTarget.All, body.getDame());
            }
        }
    }
    [PunRPC]
    private void takeDameResource(float dame)
    {
        Enviroment resource = Resource.GetComponentInChildren<Enviroment>();
        resource.takeDame(dame);

    }
    private void BackHome()
    {
        //if()
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Tree"))
        {
            Hatchet.SetActive(true);
            PickAxe.SetActive(false);
            if (Resource == null)
            {
                Resource = other.gameObject;
            }
            if (!animator.GetBool("Moving"))
            {
                transform.parent.LookAt(other.transform);
                if (!isAttacking && finalAttack)
                {
                    typeResource = 2;
                    attackCoroutine = StartCoroutine(Attacking(typeResource));
                }
            }
            else if (animator.GetBool("Moving"))
            {
                if (attackCoroutine != null)
                {
                    StopCoroutine(attackCoroutine);
                }
                //animator.SetInteger("Trigger Number", 0);
                isAttacking = false;
                finalAttack = true;
            }
        }
        else if (other.CompareTag("Gold") || other.CompareTag("Stone"))
        {
            Hatchet.SetActive(false);
            PickAxe.SetActive(true);
            if (Resource == null)
            {
                Resource = other.gameObject;
            }
            if (!animator.GetBool("Moving"))
            {
                transform.parent.LookAt(other.transform);
                if (!isAttacking && finalAttack)
                {
                    typeResource = 3;
                    attackCoroutine = StartCoroutine(Attacking(typeResource));
                }
            }
            else if (animator.GetBool("Moving"))
            {
                if (attackCoroutine != null)
                {
                    StopCoroutine(attackCoroutine);
                }
                isAttacking = false;
                finalAttack = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Resource)
        {
            StopCoroutine(Attacking(typeResource));
            animator.SetInteger("Trigger Number", 0);
            isAttacking = false;
            finalAttack = true;
            Resource = null;
        }
    }
}
