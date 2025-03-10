using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public AudioClip walkClip;

    private BoardManager board;

    private Vector2Int cellPos;
    public Vector2Int readCellPos {get {return cellPos;}}

    private bool gameOver;
    private bool isMoving;
    private Vector3 moveTarget; 
    private SpriteRenderer spriteRenderer;
    private Animator playerAnim;
    private int moveHash = Animator.StringToHash("Moving");
    private int BreakWallHash = Animator.StringToHash("BreakWall");
    private Vector2Int newCellTarget;
    private bool hasmoved;

    // Start is called before the first frame update
    void Awake()
    {
        playerAnim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init()
    {
        gameOver = false;
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOver == true)
        {
            if(Keyboard.current.enterKey.wasPressedThisFrame)
            {
                GameManager.Instance.StartNewGame();
            }
            return;
        }

        // 플레이어가 움직이는 중이면 위치를 적당한 속도로 조정
        if(isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);

            // 이동이 완료되면 isMving false 후 오브젝트 처리
            if(transform.position == moveTarget)
            {
                isMoving = false;
                playerAnim.SetBool(moveHash, false);
                var cellData = board.GetCellData(cellPos);
                if(cellData.ContainedObject != null)
                {
                    cellData.ContainedObject.PlayerEntered();
                }
            }
            return;
        }

        // 키보드 입력에 따라 이동
        newCellTarget = cellPos;
        hasmoved = false;
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            MoveSkip();
        }
        else if(Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            MoveUp();
        }
        else if(Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            MoveDown();
        }
        else if(Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            spriteRenderer.flipX = false;
            MoveRight();
        }
        else if(Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            spriteRenderer.flipX = true;
            Moveleft();
        }
    }

    private void UpdatePlayer()
    {
            if(hasmoved)
        {
            // 이동 가능하면 이동 = cellData.Passable하면 이동
            BoardManager.CellData cellData = board.GetCellData(newCellTarget);
            if(cellData != null && cellData.Passable)
            {
                GameManager.Instance.turnManager.Tick();
                if(cellData.ContainedObject == null)
                {
                    GameManager.Instance.PlaySound(walkClip);
                    MoveTo(newCellTarget, false);
                }
                else
                {
                    if(cellData.ContainedObject.PlayerWantsToEnter())
                    {
                        MoveTo(newCellTarget, false);
                    }
                    else
                    {
                        playerAnim.SetTrigger(BreakWallHash);
                    }
                }
            }
        }
    }

    public void MoveTo(Vector2Int cell, bool immediate)
    {
        cellPos = cell;
        // immediate면 즉시 이동, 일반적으로는 점진적 이동
        if(immediate)
        {
            isMoving = false;
            transform.position = board.CellToWorld(cellPos);
        }
        else
        {
            isMoving = true;
            moveTarget = board.CellToWorld(cellPos);
        }
        playerAnim.SetBool(moveHash, true);
    }

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {  
        // 플레이어가 위치할 보드와 셀
        board = boardManager;
        MoveTo(cell, true);

        // CellToWorld로 보드 위 cell 상에 Player 위치 지정
        // CellToWorld는 Cell 좌표로 World 좌표로 변환
        // transform.position = board.CellToWorld(cellPos);
    }

    public void OnGameOver()
    {
        gameOver = true;
    }

    public void Attack()
    {
        playerAnim.SetTrigger(BreakWallHash);
    }

    public void MoveUp()
    {
        if(isMoving)
        {
            return;
        }
        newCellTarget.y += 1;
        hasmoved = true;
        UpdatePlayer();
    }
    public void MoveDown()
    {
        if(isMoving)
        {
            return;
        }
        newCellTarget.y -= 1;
        hasmoved = true;
        UpdatePlayer();
    }
    public void MoveRight()
    {
        if(isMoving)
        {
            return;
        }
        newCellTarget.x += 1;
        hasmoved = true;
        UpdatePlayer();
    }
    public void Moveleft()
    {
        if(isMoving)
        {
            return;
        }
        newCellTarget.x -= 1;
        hasmoved = true;
        UpdatePlayer();
    }
    public void MoveSkip()
    {
        if(gameOver)
        {
            GameManager.Instance.StartNewGame();
        }
        if(isMoving)
        {
            return;
        }
        hasmoved = true;
        UpdatePlayer();
    }
}
