using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Character : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    //[Header("Health")]
    [SerializeField]
    private float speed;

    [SerializeField]
    private float heal;

    [SerializeField]
    private float dame;

    [SerializeField]
    private int timeLoad;

    [SerializeField]
    private Scrollbar healBar;

    [Header("Number Resource")]
    [SerializeField]
    private int tree;
    [SerializeField]
    private int stone;
    [SerializeField]
    private int gold;

    private NumberResource numberResource;
    private float maxHeal;
    [SerializeField]
    public int StateCharacter = 0;
    //0 = Stand, 1 = Run, 2 = Attack 3 = Find Enemy
    private Animator animator;
    private NavMeshAgent agent;
    private bool checkDie=false;
    [SerializeField] Collider mainCollider;
    [SerializeField]  Collider findCollider;
    [SerializeField]  Collider attackCollider;
    void Start()
    {
        animator = GetComponent<Animator>();
        StateCharacter = 0;
        if (animator != null)
        {
            animator.SetInteger("Trigger Number", 0);
            agent = GetComponentInParent<NavMeshAgent>();
            agent.speed = speed;
            agent.acceleration = 100f;
            agent.stoppingDistance = 1;
        }
        if (!photonView.IsMine)
        {

        }
        maxHeal = heal;
        numberResource = new NumberResource(tree, stone, gold);
    }
    // Update is called once per frame
    void Update()
    {
        if (heal <= 0 && animator!=null&& !gameObject.CompareTag("Team1") && !gameObject.CompareTag("Team2")&& !checkDie)
        {
            photonView.RPC("SetTriggerDie", RpcTarget.All);
            checkDie = true;
        }
    }
    [PunRPC]
    private void SetTriggerDie()
    {
        mainCollider.enabled = false;
        findCollider.enabled = false;
        attackCollider.enabled = false;
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        Die();
        SelectTableUnit unit = GetComponentInParent<SelectTableUnit>();
        unit.enabled = false;
        SelectionManager.Instance.DeUnit(unit);
        SelectionManager.Instance.DeSelect(unit);
        NavMeshAgent navMeshAgent = GetComponentInParent<NavMeshAgent>();
        navMeshAgent.enabled = false;
        StartCoroutine(DelayedDestroy(gameObject.transform.parent.gameObject, 8f));
    }
    public void Die()
    {
        if (photonView.IsMine)
        {
            SelectionManager.Instance.DeUnit(transform.root.GetComponentInChildren<SelectTableUnit>());
        }
        else
        {
            SelectionManager.Instance.DeEnemyUnit(transform.root.GetComponentInChildren<SelectTableUnit>());
            SelectionManager.Instance.DeSelectEnemy(transform.root.GetComponentInChildren<SelectTableUnit>());
        }
    }
    public void  UpdateHealthBar()
    {
        float healPer = heal / maxHeal;
        photonView.RPC("UpdateHealthBar", RpcTarget.All, healPer);
    }
    [PunRPC]
    private void UpdateHealthBar(float healPer)
    {
        healBar.size = healPer;

    }
    private IEnumerator DelayedDestroy(GameObject obj, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (obj != null)
        {
            if (PhotonView.Get(obj).IsMine)
            {
                PhotonNetwork.Destroy(obj);
            }
        }
    }
    public void takeDame(float dameIn)
    {
        photonView.RPC("UpdateHealth", RpcTarget.All, dameIn);
    }
    [PunRPC]
    private void UpdateHealth(float dameIn)
    {
        this.heal -= dameIn;
    }
    public float getDame() {
        return this.dame;
    }
    public void HealingOfPercentHealthOfEnemy(float dame)
    {
        photonView.RPC("Healing", RpcTarget.All, dame);
    }
    [PunRPC]
    private void Healing(float curr)
    {
        if (this.heal < maxHeal)
        {
            this.heal += curr;
        }
        else
        {
            this.heal = maxHeal;
        }
    }
    public float getHeal()
    {
        return this.heal;
    }
    [PunRPC]
    public float getMaxHeal()
    {
        return this.maxHeal;
    }
    public NumberResource GetNumberResource()
    {
        return new NumberResource(tree, stone, gold);
    }
    public void SetHeal(float heal)
    {
        this.heal = heal;
    }
    public float GetTimeLoad()
    {
        return this.timeLoad;
    }
}
