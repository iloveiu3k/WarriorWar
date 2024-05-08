using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public class CameraController : MonoBehaviourPunCallbacks
{
    public float panSpeed = 20f;
    [SerializeField] private Transform starPosition;
    [SerializeField] private float boder;
    [SerializeField] private Vector2 limit;
    [SerializeField] private Vector2 limit2;

    [SerializeField] private float scrollSpeed;
    [SerializeField] private float minY=40f;
    [SerializeField] private float maxY=80f;
    private bool isEnable = true;
    private PhotonView minePlayer;
    public void Start()
    {
        //transform.position = new Vector3(starPosition.position.x, 90f, starPosition.position.z);

        transform.rotation = Quaternion.Euler(60f, 0, 0);
        minePlayer = GetComponent<PhotonView>();
        if (!minePlayer.IsMine)
        {
            gameObject.transform.GetComponent<Camera>().enabled=false;
        }
    }
    void Update()
    {
        if (minePlayer.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isEnable = !isEnable;
            }
            if (isEnable == true)
            {
                Vector3 pos = transform.position;
                if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - boder)
                {
                    pos.z += panSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= boder)
                {
                    pos.z -= panSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - boder)
                {
                    pos.x += panSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= boder)
                {
                    pos.x -= panSpeed * Time.deltaTime;
                }

                float scroll = Input.GetAxis("Mouse ScrollWheel");
                //Debug.Log(scroll);
                pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

                pos.x = Mathf.Clamp(pos.x, limit2.x, limit.x);
                pos.y = Mathf.Clamp(pos.y, minY, maxY);
                pos.z = Mathf.Clamp(pos.z, limit2.y, limit.y);
                transform.position = pos;
            }

        }

    }
}
