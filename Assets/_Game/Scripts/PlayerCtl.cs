using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Direction { Right, Left, Down, Up, None }

public class PlayerCtl : MonoBehaviour
{
    [SerializeField]
    public Animator animator;

    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject PrefabBrick;

    private Vector3 mousePosDown;
    private Vector3 mousePosUp;

    private Vector3 directionVector;
    private Vector3 directionRaycast;

    private bool isAddFirstDistance;

    private Stack<GameObject> stackBrick = new Stack<GameObject>();
    private List<GameObject> quettedUnbricks = new List<GameObject>();


    private Vector3 curPosBrick;

    [SerializeField]
    private Transform Body;

    private Rigidbody rb;

    private bool isMove;
    private bool isCheckMoving;
    private bool isWallFinish;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        isAddFirstDistance = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        curPosBrick = Vector3.zero;
        OnInit();
    }
    public void OnInit()
    {
        ClearBrick();

        this.speed = Constant.SPEED;
        this.Body.localPosition = Vector3.zero;

        isAddFirstDistance = false;
        isWallFinish = false;
        this.isMove = false;
        this.isCheckMoving = true;

        rb.velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        //check đường vuốt màn hình
        if (isCheckMoving && GameManager.Instance.currentGameState == GameState.Playing)
        {
            CheckMoving();
        }

        CheckWall(directionRaycast);

        if (isMove)
        {
            Move();
            CheckBrick();
        }

    }
    private void CheckBrick()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, Constant.DISTANCE_RAYCAST_BRICK))
        {
            // Kiểm tra xem đối tượng được hit có tag là "brick" hay không
            if (hit.collider.CompareTag("Brick"))
            {
                hit.collider.gameObject.SetActive(false);

                AddBrick();

                //Update Score
                GameManager.Instance.UpdateScore(1);
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
                this.speed = Constant.SPEED_FINISH;
                LevelManager.Instance.curLevel.PlayParticleSystem();
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
                SetMovementParameters(0, 180, Vector3.forward);
                break;
            case Direction.Right:
                SetMovementParameters(0, -90, Vector3.right);
                break;
            case Direction.Left:
                SetMovementParameters(0, 90, Vector3.left);
                break;
            case Direction.Down:
                SetMovementParameters(0, 0, Vector3.back);
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
        if (Physics.Raycast(transform.position + Vector3.up, directionRaycast, out hit, Constant.DISTANCE_RAYCAST_WALL))
        {
            // Kiểm tra xem đối tượng được hit có tag là "brick" hay không
            if (hit.collider.CompareTag("Wall"))
            {
                this.isMove = false;
                this.isCheckMoving = true;

                rb.velocity = Vector3.zero;
            }
            else if (hit.collider.CompareTag("WallFinish"))
            {
                if (isWallFinish == false)
                {
                    isWallFinish = true;
                    this.isMove = false;
                    this.isCheckMoving = true;

                    LevelManager.Instance.curLevel.AcitveChest();

                    rb.velocity = Vector3.zero;

                    animator.SetInteger("renwu",2);

                    // show victory
                    StartCoroutine(VictoryCoroutine(Constant.DELAY_TIME_VICTORY));
                }

            }
        }
    }

    IEnumerator VictoryCoroutine(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        GameManager.Instance.UpdateBestScore();
        GameManager.Instance.ResetScore();
        UIManager.Instance.ShowUIVictory();
        GameManager.Instance.ChangeGameState(GameState.Victory);
        animator.SetInteger("renwu", 0);
    }

    //-----------------------------------

    //Brick
    //-----------------------------------
    //thêm 1 khối Brick dưới chân
    private void AddBrick()
    {
        if(isAddFirstDistance == true)
        {
            Body.transform.position += Vector3.up * (Constant.DISTANCE_BRICK_POS_Y);
        }
        isAddFirstDistance = true;
        GameObject SpawnBrick = Instantiate(PrefabBrick, transform); 
        SpawnBrick.transform.localPosition = curPosBrick; 
        SpawnBrick.transform.localRotation = Quaternion.Euler(-90f, 0f, 90f);

       
        curPosBrick.y += Constant.DISTANCE_BRICK_POS_Y;

        stackBrick.Push(SpawnBrick);
    }
    //xóa 1 khối Brick dưới chân
    private void RemoveBrick()
    {
        if (stackBrick.Count == 0) return;

        Destroy(stackBrick.Pop());

        Body.transform.position -= Vector3.up * (Constant.DISTANCE_BRICK_POS_Y);
        curPosBrick.y -= Constant.DISTANCE_BRICK_POS_Y;
    }
    //xóa tất cả khối Brick dưới chân
    private void ClearBrick()
    {
        if (stackBrick.Count == 0) return;

        while (stackBrick.Count > 0)
        {
            Destroy(stackBrick.Pop());

            Body.transform.position -= Vector3.up * (Constant.DISTANCE_BRICK_POS_Y);
            curPosBrick.y -= Constant.DISTANCE_BRICK_POS_Y;
        }
    }
    //-----------------------------------


}
