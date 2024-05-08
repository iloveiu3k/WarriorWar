using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static UnityEngine.ParticleSystem;
using System.Linq;

public class SorceressCombat : MonoBehaviourPunCallbacks
{
    private GameObject Enemy;
    private Animator animator;
    private GameObject attachedParticleSystem;
    private CreepData IFParticle;
    [SerializeField] private ParticleSystem particle;
    private bool isAttacking = false;
    private string EnemyHouse;
    private List<GameObject> Enemys = new List<GameObject>();
    private bool checkEnemy = false;
    private Character character;
    private SelectTableUnit select;
    private void Start()
    {
        animator = GetComponent<Animator>();
        character = GetComponent<Character>();
        select = GetComponentInParent<SelectTableUnit>();
        if (PhotonNetwork.IsMasterClient)
        {
            EnemyHouse = "Team2";
        }
        else
        {
            EnemyHouse = "Team1";
        }
    }
    private void Attacking()
    {
        if(Enemy != null)
        {
            attachedParticleSystem = PhotonNetwork.Instantiate(particle.name,new Vector3( Enemy.transform.position.x, Enemy.transform.position.y+2, Enemy.transform.position.z), Enemy.transform.rotation, 0);
            PhotonView photonView = attachedParticleSystem.GetComponent<PhotonView>();
            if (photonView != null)
            {
                int viewID = photonView.ViewID;
                IFParticle = new CreepData(viewID, attachedParticleSystem);
            }
        }
        animator.SetInteger("Trigger Number", 3);
    }
    private void ScorcerssSkill(int viewID,Transform transform)
    {
        GameObject foundPhotonView = PhotonView.Find(viewID).gameObject;
        foundPhotonView.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
    }
    private void ExitAttacking()
    {
        animator.SetInteger("Trigger Number", 0);
        //animator.SetTrigger("Disappeared");
    }
    private void haveDame()
    {
        if(Enemy!= null)
        {
            photonView.RPC("TakeDameScorcerss", RpcTarget.All);
        }
    }
    [PunRPC]
    private void TakeDameScorcerss()
    {
        if (Enemy != null)
        {
            Character enemy = Enemy.GetComponentInChildren<Character>();
            enemy.takeDame(character.getDame());
            enemy.UpdateHealthBar();
            if (Enemy.CompareTag("Unit"))
            {
                character.HealingOfPercentHealthOfEnemy(enemy.getMaxHeal() / 60f);
                character.UpdateHealthBar();
            }
        }
    }
    private void Update()
    {
        if (attachedParticleSystem != null && Enemy != null && IFParticle != null)
        {
            ScorcerssSkill(IFParticle.ViewID, Enemy.transform);
        }else if(attachedParticleSystem != null && Enemy == null)
        {
            PhotonNetwork.Destroy(attachedParticleSystem);
            attachedParticleSystem = null;
        }
        if (Enemys.Count>0)
        {
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
                isAttacking = false;
                animator.SetInteger("Trigger Number", 0);
                if (attachedParticleSystem != null)
                {
                    PhotonNetwork.Destroy(attachedParticleSystem);
                    attachedParticleSystem = null;
                }
            }
        }
    }
    private void OffParticle()
    {
        if (attachedParticleSystem != null)
        {
            PhotonNetwork.Destroy(attachedParticleSystem);
            attachedParticleSystem = null;
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
        Enemys = Enemys.Where(c => c != null).ToList();
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
                if (!isAttacking)
                {
                    animator.SetInteger("Trigger Number", 2);
                    photonView.RPC("reduceDelayAttack", RpcTarget.All);
                    isAttacking = true;
                }
            }
            else if (animator.GetBool("Moving"))
            {
                isAttacking = false;
                if (attachedParticleSystem != null)
                {
                    if (photonView.IsMine)
                    {
                        PhotonNetwork.Destroy(attachedParticleSystem);
                    }
                    attachedParticleSystem = null;
                }
            }
        }
        
    }
    [PunRPC]
    private void reduceDelayAttack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(Enemy = other.gameObject)
        {
            if (animator.GetInteger("Trigger Number") == 3)
            {
                isAttacking = false;
                animator.SetInteger("Trigger Number", 0);
                character.StateCharacter = 0;
                if (attachedParticleSystem != null)
                {
                    if (photonView.IsMine)
                    {
                        PhotonNetwork.Destroy(attachedParticleSystem);
                    }
                    attachedParticleSystem = null;
                }
            }
            else if (animator.GetInteger("Trigger Number") == 2)
            {
                isAttacking = false;
                animator.SetInteger("Trigger Number", 0);
                character.StateCharacter = 0;
                if (attachedParticleSystem != null)
                {
                    if (photonView.IsMine)
                    {
                        PhotonNetwork.Destroy(attachedParticleSystem);
                    }
                    attachedParticleSystem = null;
                }
            }
            else if (animator.GetInteger("Trigger Number") == 0)
            {
                isAttacking = false;
                character.StateCharacter = 0;
                if (attachedParticleSystem != null)
                {
                    if (photonView.IsMine)
                    {
                        PhotonNetwork.Destroy(attachedParticleSystem);
                    }
                    attachedParticleSystem = null;
                }
            }
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
}
