using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine.TextCore.Text;

public class CloseCombat : MonoBehaviourPunCallbacks
{
    private GameObject Enemy;
    private Animator animator;
    [SerializeField] float speedAttack;
    private bool isAttacking=false;
    private Coroutine attackCoroutine;
    private bool finalAttack= true;
    private string EnemyHouse;
    private List<GameObject> Enemys = new List<GameObject>();
    Character body;
    SelectTableUnit select;
    private bool checkEnemy = false;
    [SerializeField] Collider colliderFind;
    private void Start()
    {
        animator = GetComponent<Animator>();
        if (photonView.IsMine && PhotonNetwork.IsMasterClient)
        {
            EnemyHouse = "Team2";
        }
        else
        {
            EnemyHouse = "Team1";
        }
        body = GetComponent<Character>();
        select = transform.root.GetComponentInChildren<SelectTableUnit>();
    }
    private void Update()
    {
        if (Enemys.Count > 0 && body.StateCharacter!=1)
        {
            colliderFind.gameObject.SetActive(false);
            colliderFind.gameObject.transform.GetComponent<FindEnemy>().ResetValues();
            Enemys = Enemys.Where(c => c != null).ToList();
            DetectEnemy();
        }
        else
        {
            Enemy = null;
            colliderFind.gameObject.SetActive(true);
        }
        if (Enemy != null)
        {
            if (Enemy.GetComponentInChildren<Character>().getHeal() <= 0)
            {
                StopCoroutine(Attacking());
                animator.SetInteger("Trigger Number", 0);
                isAttacking = false;
                finalAttack = true;
                Enemy = null;
            }
        }
    }
    private IEnumerator Attacking()
    {
        attackCoroutine = null;
        isAttacking = true;
        animator.SetInteger("Trigger Number", 2);
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
        if (Enemy != null)
        {
            photonView.RPC("TakeDameClose", RpcTarget.All);
        }
    }
    [PunRPC]
    private void TakeDameClose()
    {
        if (Enemy != null)
        {
            Character enemy = Enemy.GetComponentInChildren<Character>();
            enemy.takeDame(body.getDame());
            enemy.UpdateHealthBar();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            EnemyHouse = "Team2";
        }
        else
        {
            EnemyHouse = "Team1";
        }
        if (other.CompareTag("Unit") && !other.gameObject.GetPhotonView().IsMine && photonView.IsMine && animator != null ||
            other.CompareTag(EnemyHouse) && animator != null)
        {
            Enemys.Add(other.gameObject);
            //colliderFind.gameObject.SetActive(false);
            //colliderFind.gameObject.transform.GetComponent<FindEnemy>().ResetValues();
            if (body.StateCharacter == 3)
            {
                select.StopMove(2);
            }
        }
    }
    
    private void DetectEnemy()
    {
        if (Enemys.Count < 1)
        {
            return;
        }
        Enemy = Enemys[0];
        if (Enemy != null)
        {
            if (!animator.GetBool("Moving"))
            {
                transform.parent.LookAt(Enemy.transform);
                if (!isAttacking && finalAttack)
                {
                    body.StateCharacter = 2;
                    attackCoroutine = StartCoroutine(Attacking());
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
    }
    private void OnTriggerExit(Collider other)
    {
        if (Enemy == other.gameObject)
        {
            StopCoroutine(Attacking());
            animator.SetInteger("Trigger Number", 0);
            isAttacking = false;
            finalAttack = true;
            Enemys.Remove(other.gameObject);
            Enemys = Enemys.Where(c => c != null).ToList();
            body.StateCharacter = 0;
        }
        else
        {
            checkEnemy = false;
            foreach (GameObject enemy in Enemys)
            {
                if (enemy == other.gameObject)
                {
                    checkEnemy = true;
                    break;
                }
            }
            if (checkEnemy)
            {
                Enemys.Remove(other.gameObject);
                Enemys = Enemys.Where(c => c != null).ToList();
            }
        }
    }
    public GameObject GetEnemyTransform()
    {
        return this.Enemy;
    }
}
