using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class PlayerInput : MonoBehaviourPunCallbacks
{
    private Camera Camera;
    [SerializeField]
    private RectTransform SelectionBox;
    [SerializeField]
    public RectTransform SelectionBoxEnemy;
    [SerializeField]
    private LayerMask UnitLayers;
    [SerializeField]
    private LayerMask FloorLayers;
    [SerializeField]
    private LayerMask EnemyLayer;
    [SerializeField]
    private float DragDelay = 0.1f;
    private float MouseDownTime;
    [SerializeField]
    private GameObject LineZone;
    [SerializeField]
    private GameObject Listener;
    public float speed = 2f;
    private Vector2 StarMousePosition;
    private GameObject targetEnemy;

    private void Start()
    {
        Camera = GetComponent<Camera>();
        if (photonView.IsMine)
        {
            Listener.SetActive(true);
            LineZone.SetActive(true);
        }
    }
    private void Update()
    {
        HandleSelectionInputs();
        HandleMovementInputs();
    }
    private void FixedUpdate()
    {
        HandleMovementInputs();
    }
    private Transform RandomPositionEnemy(HashSet<SelectTableUnit> Enemys)
    {
        Enemys = Enemys.Where(c => c != null).ToList().ToHashSet();

        if (Enemys.Count > 0)
        {
            List<SelectTableUnit> enemyList = new List<SelectTableUnit>(Enemys);
            int randomIndex = Random.Range(0, enemyList.Count);
            Transform randomSpawnPoint = enemyList[randomIndex].transform;
            if (randomSpawnPoint != null)
            {
                return randomSpawnPoint;
            }
        }

        return null;
    }
    private void HandleMovementInputs()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                if (Input.GetKey(KeyCode.Mouse1) && SelectionManager.Instance.EnemySelectedUnits.Count > 0)
                {
                    //RaycastHit hit;
                    foreach (SelectTableUnit unit in SelectionManager.Instance.SelectedUnits)
                    {
                        Transform enemy = RandomPositionEnemy(SelectionManager.Instance.EnemySelectedUnits);
                        if (enemy != null)
                        {
                            if (unit.gameObject.GetComponentInChildren<Character>().StateCharacter != 2)
                            {
                                unit.transform.LookAt(enemy);
                                unit.Moveto(enemy.position, 3);
                            }
                        }
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Mouse1) && SelectionManager.Instance.SelectedUnits.Count > 0)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out hit, 500f, FloorLayers))
                    {
                        foreach (SelectTableUnit unit in SelectionManager.Instance.SelectedUnits)
                        {
                            unit.transform.LookAt(hit.point);
                            unit.Moveto(hit.point,1);

                            //var targetRotation = Quaternion.LookRotation(hit.point - unit.transform.position);
                            //unit.transform.rotation = Quaternion.Slerp(unit.transform.rotation, targetRotation, speed );
                            //unit.AnimatonStopMove();
                        }
                    }
                    else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 500f, EnemyLayer))
                    {
                        foreach (SelectTableUnit unit in SelectionManager.Instance.SelectedUnits)
                        {
                            unit.transform.LookAt(hit.point);
                            unit.Moveto(hit.point, 3);
                        }
                    }
                }
            }
        }
    }
    private void HandleSelectionInputs()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)&& !SelectionBox.gameObject.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    SelectionBoxEnemy.sizeDelta = Vector2.zero;
                    SelectionBoxEnemy.gameObject.SetActive(true);
                    StarMousePosition = Input.mousePosition;
                    MouseDownTime = Time.time;
                }
                else if (Input.GetKey(KeyCode.Mouse0) && MouseDownTime + DragDelay < Time.time)
                {
                    ResizeSelectionBox();
                }
                else if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    SelectionBoxEnemy.sizeDelta = Vector2.zero;
                    SelectionBoxEnemy.gameObject.SetActive(false);
                    //Debug.Log("ngu2");
                    if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 500f, EnemyLayer)
                        && hit.collider.TryGetComponent<SelectTableUnit>(out SelectTableUnit unit))
                    {
                        if (unit != null)
                        {
                            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                            {
                                if (SelectionManager.Instance.IsSelectedEnemy(unit))
                                {
                                    SelectionManager.Instance.DeSelectEnemy(unit);
                                }
                                else
                                {
                                    SelectionManager.Instance.SelectEnemy(unit);
                                    //Debug.Log("ngu");
                                }
                            }
                            else
                            {
                                SelectionManager.Instance.DeSelectAllEnemy();
                                SelectionManager.Instance.SelectEnemy(unit);
                                //Debug.Log("ngu");
                            }
                        }
                    }
                    else if (MouseDownTime + DragDelay > Time.time)
                    {
                        SelectionManager.Instance.DeSelectAllEnemy();
                    }
                    MouseDownTime = 0f;
                }
            }
            else
            {
                SelectionBoxEnemy.gameObject.SetActive(false);
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    SelectionBox.sizeDelta = Vector2.zero;
                    SelectionBox.gameObject.SetActive(true);
                    StarMousePosition = Input.mousePosition;
                    MouseDownTime = Time.time;
                }
                else if (Input.GetKey(KeyCode.Mouse0) && MouseDownTime + DragDelay < Time.time)
                {
                    ResizeSelectionBox();
                }
                else if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    SelectionBox.sizeDelta = Vector2.zero;
                    SelectionBox.gameObject.SetActive(false);
                    //Debug.Log("ngu2");
                    if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 500f, UnitLayers)
                        && hit.collider.TryGetComponent<SelectTableUnit>(out SelectTableUnit unit))
                    {
                        //Debug.Log("ngu1");
                        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                        {
                            if (SelectionManager.Instance.IsSelected(unit))
                            {
                                SelectionManager.Instance.DeSelect(unit);
                            }
                            else
                            {
                                SelectionManager.Instance.Select(unit);
                                //Debug.Log("ngu");
                            }
                        }
                        else
                        {
                            SelectionManager.Instance.DeSelectAll();
                            SelectionManager.Instance.Select(unit);
                            //Debug.Log("ngu");
                        }
                    }
                    else if (MouseDownTime + DragDelay > Time.time)
                    {
                        SelectionManager.Instance.DeSelectAll();
                    }
                    MouseDownTime = 0f;
                }
            }
            
        }
    }
    private void ResizeSelectionBox()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {

                float width = Input.mousePosition.x - StarMousePosition.x;
                float height = Input.mousePosition.y - StarMousePosition.y;

                SelectionBoxEnemy.anchoredPosition = StarMousePosition + new Vector2(width / 2, height / 2);
                SelectionBoxEnemy.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

                Bounds bounds = new Bounds(SelectionBoxEnemy.anchoredPosition, SelectionBoxEnemy.sizeDelta);
                for (int i = 0; i < SelectionManager.Instance.EnemyAvailableUnits.Count; i++)
                {
                    if (SelectionManager.Instance.EnemyAvailableUnits[i] != null)
                    {
                        if (UnitIsSelectionBox(Camera.WorldToScreenPoint(SelectionManager.Instance.EnemyAvailableUnits[i].transform.position), bounds))
                        {
                            SelectionManager.Instance.SelectEnemy(SelectionManager.Instance.EnemyAvailableUnits[i]);
                        }
                        else
                        {
                            SelectionManager.Instance.DeSelectEnemy(SelectionManager.Instance.EnemyAvailableUnits[i]);
                        }
                    }
                }
            }
            else
            {
                float width = Input.mousePosition.x - StarMousePosition.x;
                float height = Input.mousePosition.y - StarMousePosition.y;

                SelectionBox.anchoredPosition = StarMousePosition + new Vector2(width / 2, height / 2);
                SelectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

                Bounds bounds = new Bounds(SelectionBox.anchoredPosition, SelectionBox.sizeDelta);

                for (int i = 0; i < SelectionManager.Instance.AvailableUnits.Count; i++)
                {
                    if (UnitIsSelectionBox(Camera.WorldToScreenPoint(SelectionManager.Instance.AvailableUnits[i].transform.position), bounds))
                    {
                        SelectionManager.Instance.Select(SelectionManager.Instance.AvailableUnits[i]);
                    }
                    else
                    {
                        SelectionManager.Instance.DeSelect(SelectionManager.Instance.AvailableUnits[i]);
                    }
                }
            }
        }
    }
    private bool UnitIsSelectionBox(Vector2 Position,Bounds Bounds)
    {
        return Position.x > Bounds.min.x && Position.x < Bounds.max.x
               && Position.y > Bounds.min.y && Position.y < Bounds.max.y;
    }
}
