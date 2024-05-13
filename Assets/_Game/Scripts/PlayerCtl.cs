using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Direction { Right, Left, Down, Up, None }

public class PlayerCtl : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;
    [SerializeField]
    private GameObject PrefabBrick;

    private Vector3 mousePosDown;
    private Vector3 mousePosUp;
    private Vector3 directionVector;
    private Vector3 directionRaycast;


    private Stack<GameObject> stackBrick = new Stack<GameObject>();
    private List<GameObject> quettedUnbricks = new List<GameObject>(); // Danh sách các khối UnBrick đã được quét


    private Vector3 curPosBrick;
    [SerializeField]
    private Transform Body;
    private Rigidbody rb;

    private bool isMove;
    private bool isCheckMoving;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        isMove = false;
        isCheckMoving = true;
    }
    private void Start()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        curPosBrick = Vector3.zero;
    }
    private void OnInit()
    {
        ClearBrick();
        this.isMove = false;
        this.isCheckMoving = true;
        rb.velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        Debug.Log(stackBrick.Count);
        //check đường vuốt màn hình
        if (isCheckMoving)
        {
            CheckMoving();
        }

        CheckWall(directionRaycast);

        if (isMove)
        {
            Move();
        }
        if (isMove)
        {
            CheckBrick();
        }

    }
    private void CheckBrick()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 1f, 0), Vector3.down, out hit, 1f))
        {
            // Kiểm tra xem đối tượng được hit có tag là "brick" hay không
            if (hit.collider.CompareTag("Brick"))
            {
                hit.collider.gameObject.SetActive(false);
                AddBrick();
            }
            else if (hit.collider.CompareTag("UnBrick"))
            {
                GameObject unbrick = hit.collider.gameObject;
                // Kiểm tra xem khối UnBrick đã được quét hay chưa
                if (!quettedUnbricks.Contains(unbrick))
                {
                    // Bật MeshRenderer lên
                    unbrick.GetComponent<MeshRenderer>().enabled = true;

                    // Thêm khối UnBrick vào danh sách đã quét
                    quettedUnbricks.Add(unbrick);

                    // Xử lý logic cho việc gỡ bỏ brick
                    RemoveBrick();
                }

            }
            else if (hit.collider.CompareTag("Finish"))
            {
                Debug.Log("Finish");
                this.speed = 20f;
            }


        }
    }
    private Direction GetDirection(float angle)
    {
        if (angle >= 45 && angle < 135)
        {
            return Direction.Up;
        }
        else if (angle >= -45 && angle < 45 && angle != 0)
        {
            return Direction.Right;
        }
        else if (angle >= 135 || angle < -135)
        {
            return Direction.Left;
        }
        else if (angle >= -135 && angle < -45)
        {
            return Direction.Down;
        }
        else
        {
            return Direction.None;
        }
    }

    private void CheckMoving()
    {
        float angle = Angle();
        Direction direction = GetDirection(angle);

        switch (direction)
        {
            case Direction.Up:
                SetMovementParameters(0, 180, new Vector3(0, 0, 1f));
                break;
            case Direction.Right:
                SetMovementParameters(0, -90, new Vector3(1f, 0, 0));
                break;
            case Direction.Left:
                SetMovementParameters(0, 90, new Vector3(-1f, 0, 0));
                break;
            case Direction.Down:
                SetMovementParameters(0, 0, new Vector3(0, 0, -1f));
                break;
            case Direction.None:
                // Handle case where direction is not determined
                break;
            default:
                break;
        }
    }
    private void SetMovementParameters(float xRotation, float yRotation, Vector3 direction)
    {
        transform.rotation = Quaternion.Euler(0, yRotation, xRotation);
        this.directionVector = direction;
        this.directionRaycast = direction;
        this.isMove = true;
    }
    private float Angle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePosDown = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            mousePosUp = Input.mousePosition;
            Vector3 directionVector = mousePosUp - mousePosDown;
            float angle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
            return angle;
        }

        return 0;
    }
    //di chuyển
    //-----------------------------------
    private void Move()
    {
        isCheckMoving = false;
        rb.velocity = (this.directionVector * speed);
    }

    private void CheckWall(Vector3 directionRaycast)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 1f, 0), directionRaycast, out hit, .52f))
        {
            // Kiểm tra xem đối tượng được hit có tag là "brick" hay không
            if (hit.collider.CompareTag("Wall"))
            {
                Debug.Log("WALL");
                this.isMove = false;
                this.isCheckMoving = true;
                rb.velocity = Vector3.zero;
            }else if(hit.collider.CompareTag("WallFinish"))
            {
                Debug.Log("WALLFinish");
                this.isMove = false;
                this.isCheckMoving = true;
                rb.velocity = Vector3.zero;
            }
        }
        Debug.DrawRay(transform.position + new Vector3(0, 1f, 0), directionRaycast, Color.red);
    }
    //-----------------------------------

    //Brick
    //-----------------------------------
    //thêm 1 khối Brick dưới chân
    private void AddBrick()
    {
        GameObject SpawnBrick = Instantiate(PrefabBrick, transform); // prefab sẽ là con của transform hiện tại
        SpawnBrick.transform.localPosition = curPosBrick; // set vị trí local của prefab là (0, 0, 0)
        SpawnBrick.transform.localRotation = Quaternion.Euler(-90f, 0f, 90f);
        curPosBrick.y += .15f;
        Body.transform.position += new Vector3(0, .15f, 0);
        stackBrick.Push(SpawnBrick);
    }
    //xóa 1 khối Brick dưới chân
    private void RemoveBrick()
    {
        if (stackBrick.Count == 0) return;
        Destroy(stackBrick.Pop());
        Body.transform.position -= new Vector3(0, .15f, 0);
        curPosBrick.y -= .15f;
    }
    //xóa tất cả khối Brick dưới chân
    private void ClearBrick()
    {
        if (stackBrick.Count == 0) return;
        for(int i = 0; i < stackBrick.Count; i++)
        {
            Destroy(stackBrick.Pop());
            Body.transform.position -= new Vector3(0, .15f, 0);
            curPosBrick.y -= .15f;
        }
    }
    //-----------------------------------


}
