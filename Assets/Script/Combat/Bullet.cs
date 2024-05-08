using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Photon.Pun;
public class Bullet : MonoBehaviourPunCallbacks
{
    private GameObject targetEnemy;
    [SerializeField] float velocity;
    private float dame;

    public void SetDame(float dame)
    {
        this.dame = dame;
    }
    private void Start()
    {
        targetEnemy = FindObjectOfType<LongRaceCombat>().GetEnemyTransform();
    }
    private void Update()
    {
        AttackEnemy();
    }
    private void AttackEnemy()
    {
        if(targetEnemy == null&& photonView.IsMine && gameObject!=null)
        {
            PhotonNetwork.Destroy(gameObject);
            return;
        }
        if (targetEnemy != null)
        {
            Vector3 targetPosition = new Vector3(targetEnemy.transform.position.x, transform.position.y, targetEnemy.transform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, velocity * Time.deltaTime);
            Vector3 lookAtPosition = new Vector3(targetEnemy.transform.position.x, transform.position.y, targetEnemy.transform.position.z);

            transform.LookAt(lookAtPosition);
            if (transform.position.x == targetEnemy.transform.position.x && transform.position.z == targetEnemy.transform.position.z && gameObject != null)
            {
                photonView.RPC("TakeDameLongRace", RpcTarget.All, dame);
                if (photonView.IsMine)
                {
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }

    }
    [PunRPC]
    private void TakeDameLongRace(float currDame)
    {
        if (targetEnemy != null)
        {
            Character enemy = targetEnemy.GetComponentInChildren<Character>();
            enemy.takeDame(currDame);
            enemy.UpdateHealthBar();
        }
    }
}
