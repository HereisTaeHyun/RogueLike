using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    private BoardManager board;
    private Vector2Int cellPos;
    private bool gameOver;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init()
    {
        gameOver = false;
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
        Vector2Int newCellTarget = cellPos;
        bool hasmoved = false;

        if(Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y += 1;
            hasmoved = true;
        }
        else if(Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y -= 1;
            hasmoved = true;
        }
        else if(Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x += 1;
            hasmoved = true;
        }
        else if(Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x -= 1;
            hasmoved = true;
        }

        if(hasmoved)
        {
            // 이동 가능하면 이동
            BoardManager.CellData cellData = board.GetCellData(newCellTarget);
            if(cellData != null && cellData.Passable)
            {
                GameManager.Instance.turnManager.Tick();

                if(cellData.ContainedObject == null)
                {
                    MoveTo(newCellTarget);
                }
                else if(cellData.ContainedObject.PlayerWantsToEnter())
                {
                    MoveTo(newCellTarget);
                    cellData.ContainedObject.PlayerEntered();
                }
            }
        }
    }

    public void MoveTo(Vector2Int cell)
    {
        cellPos = cell;
        transform.position = board.CellToWorld(cellPos);
    }

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {  
        // 플레이어가 위치할 보드와 셀
        board = boardManager;
        cellPos = cell;

        // CellToWorld로 보드 위 cell 상에 Player 위치 지정
        // CellToWorld는 Cell 좌표로 World 좌표로 변환
        transform.position = board.CellToWorld(cellPos);
    }

    public void OnGameOver()
    {
        gameOver = true;
    }
}
