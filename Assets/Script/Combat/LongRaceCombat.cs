using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class LongRaceCombat : MonoBehaviourPunCallbacks
{
    [Header("State")]
    public float attackRange;
    public float aggroRange;
    [SerializeField] GameObject Bullet;
    [SerializeField] float speedAttack;
    private GameObject Enemy;
    private bool isAttacking;
    [SerializeField] private Transform Bow;
    private Animator animator;
    private Coroutine attackCoroutine;
    private bool haveEnemy;
    Bullet bulletScript;
    GameObject bulletInstance;
    private string EnemyHouse;
    private List<GameObject> Enemys = new List<GameObject>();
    bool checkEnemy = false;
    private Character character;
    private SelectTableUnit select;
    private void Start()
    {
        animator = GetComponent<Animator>();
        character = GetComponentInChildren<Character>();
        select = transform.GetComponentInParent<SelectTableUnit>();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
    private void Attack()
    {
        if (photonView.IsMine)
        {
            bulletInstance = PhotonNetwork.Instantiate(Bullet.name, Bow.position, Bullet.transform.rotation);
            if (bulletInstance != null)
            {
                bulletScript = bulletInstance.GetComponent<Bullet>();
            }
        }
        photonView.RPC("SetDameLongRace", RpcTarget.All,GetDame());
    }
    [PunRPC]
    private void SetDameLongRace(float dame)
    {
        if (bulletScript != null)
        {
            bulletScript.SetDame(dame);
        }
    }
    private void Update()
    {
        if (Enemys.Count>0)
        {
            Enemys = Enemys.Where(c => c != null).ToList();
            DetectEnemy();
        }
        else
        {
            Enemy = null;
        }
        if (Enemy != null)
        {
            if (Enemy.GetComponentInChildren<Character>().getHeal() <= 0)
            {
                animator.SetInteger("Trigger Number", 0);
                haveEnemy = false;
                isAttacking = false;
                StopCoroutine(attackCoroutine);
            }
        }
    }
    private IEnumerator Attacking()
    {
        attackCoroutine = null;
        isAttacking = true;
        animator.SetInteger("Trigger Number", 2);
        photonView.RPC("reduceDelayAttack", RpcTarget.All);
        yield return new WaitForSeconds(speedAttack);
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
    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine && PhotonNetwork.IsMasterClient)
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
            if (character.StateCharacter == 3)
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
        if (Enemy == null)
        {
            return;
        }
        haveEnemy = true;
        if (!animator.GetBool("Moving"))
        {
            transform.parent.LookAt(Enemy.transform);
            if (!isAttacking)
            {
                character.StateCharacter = 2;
                attackCoroutine = StartCoroutine(Attacking());
            }
        }
        else if (animator.GetBool("Moving"))
        {
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }
            isAttacking = false;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if(Enemy == other.gameObject)
        {
            if (animator.GetInteger("Trigger Number") != 5)
            {
                animator.SetInteger("Trigger Number", 0);
            }
            haveEnemy = false;
            isAttacking = false;
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }
            character.StateCharacter = 0;
            Enemys.Remove(other.gameObject);
            Enemys = Enemys.Where(c => c != null).ToList();
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
    public bool HaveEnemy()
    {
        return this.haveEnemy;
    }
    public float GetDame()
    {
        Character body = GetComponent<Character>();
        return body.getDame();
    }
}
