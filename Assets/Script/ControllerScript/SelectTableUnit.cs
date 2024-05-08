using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;
[RequireComponent(typeof(NavMeshAgent),(typeof(NavMeshObstacle)))]
public class SelectTableUnit : MonoBehaviourPunCallbacks
{
    private NavMeshAgent Agent;
    private NavMeshObstacle obstacle;
    [SerializeField]
    private SpriteRenderer SelectionSprite;
    [SerializeField]
    private Canvas healBar;
    [SerializeField]
    private float CarvingTime = 0.1f;
    [SerializeField]
    private float CarvingMoveThreshold = 0.1f;
    private Animator Animator;
    private bool checkMove = false;
    private new AnimationClass animation;
    //[SerializeField] Collider limitAttack;
    public string Team;
    public string Enemy;
    private Vector3 LastPosition;
    private float LastMoveTime;
    private Character character;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            SelectionManager.Instance.AvailableUnits.Add(this);
            Agent = GetComponent<NavMeshAgent>();
            obstacle = GetComponent<NavMeshObstacle>();
            SelectionSprite.gameObject.SetActive(false);
            healBar.gameObject.SetActive(false);
            photonView.RPC("ListEnemy", RpcTarget.Others);
            gameObject.layer = LayerMask.NameToLayer("Units");
            character = GetComponentInChildren<Character>();
            obstacle = GetComponent<NavMeshObstacle>();
            obstacle.enabled = false;
            obstacle.carveOnlyStationary = false;
            obstacle.carving = true;
            LastPosition = transform.position;
        }
        else
        {
            obstacle = GetComponent<NavMeshObstacle>();
            obstacle.enabled = false;
        }

        if (photonView.IsMine && PhotonNetwork.IsMasterClient)
        {
            Team = "Team1";
            Enemy = "Team2";
        }
        else
        {
            Team = "Team2";
            Enemy = "Team1";
        }
    }
    public void Start()
    {
        if (photonView.IsMine)
        {
            Animator = GetComponentInChildren<Animator>();
            if (Animator == null)
            {
                Debug.Log("Not Animation");
            }
            else
            {
                //Debug.Log(Animator.name);
                Animator.SetFloat("Animation Speed", 1f);
            }
            animation = gameObject.AddComponent<AnimationClass>();            
        }
    }
    [PunRPC]
    private void ListEnemy()
    {
        SelectionManager.Instance.EnemyAvailableUnits.Add(this);
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            if (Vector3.Distance(LastPosition, transform.position) > CarvingMoveThreshold)
            {
                LastMoveTime = Time.time;
                LastPosition = transform.position;
            }
            if (LastMoveTime + CarvingTime < Time.time)
            {
                if (checkMove)
                {
                    StopMove(0);
                }
            }
        }
    }
    public void StopMove(int State)
    {
        if (photonView.IsMine)
        {
            Agent.SetDestination(transform.position);
            checkMove = false;
            animation.AnimationStopMove(Animator);
            character.StateCharacter = State;
            Agent.enabled = false;
            obstacle.enabled = true;
        }

    }
    public void Moveto(Vector3 Position,int State)
    {
        if (photonView.IsMine)
        {
            //animation.AnimationMove(Animator);
            //if (Agent.enabled)
            //{
            //    Agent.SetDestination(Position);
            //}
            //checkMove = true;
            //animation.AnimationMove(Animator);
            obstacle.enabled = false;
            LastMoveTime = Time.time;
            LastPosition = transform.position;
            StartCoroutine(MoveAgent(Position, State));
            character.StateCharacter = State;
        }
    }
    private IEnumerator MoveAgent(Vector3 Position,int trigger)
    {
        yield return null;
        Agent.enabled = true;
        Agent.SetDestination(Position);
        checkMove = true;
        if (trigger == 1)
        {
            animation.AnimationMove(Animator);
        }
        else if (trigger == 3)
        {
            animation.AnimationMoveFind(Animator);
        }
    }

    public void OnSelected()
    {
        //SelectionManager.Instance.AvailableUnits.Add(this);
        if (photonView.IsMine)
        {
            SelectionSprite.gameObject.SetActive(true);
            healBar.gameObject.SetActive(true);
        }
        else
        {
            SelectionSprite.gameObject.SetActive(true);
            SelectionSprite.color = Color.red;
        }
    }
    public void OnDeSelected()
    {
        //SelectionManager.Instance.AvailableUnits.Remove(this);
        if (photonView == null)
        {
            return;
        }
        if (photonView.IsMine)
        {
            SelectionSprite.gameObject.SetActive(false);
            healBar.gameObject.SetActive(false);
        }
        else
        {
            SelectionSprite.gameObject.SetActive(false);
        }
    }
    public string GetEnemy()
    {
        if (photonView.IsMine)
        {
            return this.Enemy;
        }
        else
        {
            return this.Team;
        }
    }
}
