using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class FindEnemy : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private LongRaceCombat lr;
    private CloseCombat cl;
    private SorceressCombat so;
    private GameObject Enemy;
    private NavMeshAgent agent;
    private Animator animator;
    private AnimationClass aniClass;
    private bool hasFindEnemy;
    private GameObject EnemyToFind;
    private SelectTableUnit sl;
    List<GameObject> Enemys = new List<GameObject>();
    private Character character;
    bool checkEnemy = false;
    void Start()
    {
        agent = transform.root.GetComponent<NavMeshAgent>();
        animator = transform.root.GetComponentInChildren<Animator>();
        aniClass = gameObject.AddComponent<AnimationClass>();
        sl = transform.root.GetComponentInChildren<SelectTableUnit>();
        character = transform.root.GetComponentInChildren<Character>();
    }
    void Update()
    {
        lr = transform.root.GetComponentInChildren<LongRaceCombat>();
        so = transform.root.GetComponentInChildren<SorceressCombat>();
        cl = transform.root.GetComponentInChildren<CloseCombat>();
        if (lr != null)
        {
            Enemy = lr.GetEnemyTransform();
        }
        else if (so != null)
        {
            Enemy = so.GetEnemyTransform();
        }
        else if (cl != null)
        {
            Enemy = cl.GetEnemyTransform();
        }
        if (Enemy == null)
        {
            //if (EnemyToFind != null)
            //{
            //    if (Enemy.GetComponentInChildren<Character>().getHeal() <= 0)
            //    {
            //        aniClass.AnimationStopMove(animator);
            //        if (agent.enabled)
            //        {
            //            agent.SetDestination(transform.parent.position);
            //        }
            //    }
            //}
            if (Enemys.Count > 0)
            {
                FindEnemyCollider();
            }
            else
            {
                EnemyToFind = null;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        PhotonView photonViewOfEnemy = other.gameObject.GetPhotonView();
        if (other.CompareTag("Unit") && !photonViewOfEnemy.IsMine && photonView.IsMine && animator != null)
        {
            Debug.Log(Enemys.Count);
            Enemys.Add(other.gameObject);
        }
    }
    private void FindEnemyCollider()
    {
        if(character.StateCharacter==0|| character.StateCharacter == 3)
        {
            Enemys = Enemys.Where(c => c != null).ToList();
            if (Enemys.Count < 1)
            {
                return;
            }
            EnemyToFind = Enemys[0];
            transform.parent.LookAt(EnemyToFind.transform.position);
            sl.Moveto(EnemyToFind.transform.position, 3);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == EnemyToFind)
        {
            if (character.StateCharacter == 3|| character.StateCharacter == 2)
            {
                sl.StopMove(0);
                Enemys.Remove(other.gameObject);
                Enemys = Enemys.Where(c => c != null).ToList();
            }

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
    public void ResetValues()
    {
        Enemys = new List<GameObject>();
        Enemy = null;
        EnemyToFind = null;
    }
}
