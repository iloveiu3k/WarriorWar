using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class MageCombat : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField] private GameObject ObjCreep;
    private int numberCreep = 0;
    private List<CreepData> Creeps = new List<CreepData>();
    private GameObject Creep;
    private bool isAttacking = false;
    private void Update()
    {
        if (photonView.IsMine)
        {
            List<CreepData> creepsToRemove = new List<CreepData>();
            if (numberCreep < 5 && isAttacking == false)
            {
                StartCoroutine(Skill());
            }
            if (Creeps.Count > 0)
            {
                foreach (CreepData creep in Creeps)
                {
                    if (IsCreepDestroyed(creep.ViewID))
                    {
                        creepsToRemove.Add(creep);
                        numberCreep--;
                    }
                }
            }
            foreach (var creepToRemove in creepsToRemove)
            {
                Creeps.Remove(creepToRemove);
            }
        }
    }
    bool IsCreepDestroyed(int viewID)
    {
        PhotonView foundPhotonView = PhotonView.Find(viewID);
        return foundPhotonView == null || foundPhotonView.IsMine == false;
    }
    private IEnumerator Skill()
    {
        isAttacking = true;
        Creep = PhotonNetwork.Instantiate(ObjCreep.name, transform.position + new Vector3(Random.Range(2, 4), 0, Random.Range(2, 4)), ObjCreep.transform.rotation);
        PhotonView photonView = Creep.GetComponent<PhotonView>();
        if (photonView != null)
        {
            int viewID = photonView.ViewID;
            Creeps.Add(new CreepData(viewID, Creep));
        }
        numberCreep++;
        yield return new WaitForSeconds(20);
        isAttacking = false;
    }
}
public class CreepData
{
    public int ViewID { get; private set; }
    public GameObject CreepObject { get; private set; }

    public CreepData(int viewID, GameObject creepObject)
    {
        ViewID = viewID;
        CreepObject = creepObject;
    }
}
