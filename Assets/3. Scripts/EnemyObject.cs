using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : CellObject
{
    public int maxHealth = 3;
    private int healthPoint;
    private Animator enemyAnim;
    private int AttackHash = Animator.StringToHash("Attack");

    // 생성시 턴 매니저 등록
    private void Awake()
    {
        GameManager.Instance.turnManager.OnTick += TurnHappened;
        enemyAnim = GetComponent<Animator>();
    }

    // 생성시 턴 매니저 등록 해제
    private void OnDestroy()
    {
        GameManager.Instance.turnManager.OnTick -= TurnHappened;
    }

    // 생성시 부모 메서드처럼 좌표 등록 후 체력 초기화
    public override void Init(Vector2Int coord)
    {
        base.Init(coord);
        healthPoint = maxHealth;
    }
    public override bool PlayerWantsToEnter()
    {
        healthPoint -= 1;
        if(healthPoint > 0)
        {
            return false;
        }
        Destroy(gameObject);
        return true;
    }

    private bool MoveTo(Vector2Int coord)
    {
        var board = GameManager.Instance.boardManager;
        var targetCell = board.GetCellData(coord);

        // 이동 가능 여부 체크, 아래 사항이면 불가능
        if(targetCell == null || !targetCell.Passable || targetCell.ContainedObject != null)
        {
            return false;
        }

        // 셀 단위 이동 전 현재 위치의 셀은 null화
        var currentCell = board.GetCellData(cell);
        currentCell.ContainedObject = null;

        // 다음 셀로 이동
        targetCell.ContainedObject = this;
        cell = coord;
        transform.position = board.CellToWorld(coord);

        return true;
    }

    private bool TryMoveInX(int xDist)
    {
        // Player가 오른쪽이라면
        if(xDist > 0)
        {
            return MoveTo(cell + Vector2Int.right);
        }

        // Player가 오른쪽이 아닌, 즉 왼쪽인 경우
        return MoveTo(cell + Vector2Int.left);
    }

    private bool TryMoveInY(int yDist)
    {
        // Player가 위이라면
        if(yDist > 0)
        {
            return MoveTo(cell + Vector2Int.up);
        }

        // Player가 위가 아닌, 즉 아래인 경우
        return MoveTo(cell + Vector2Int.down);
    }

    private void TurnHappened()
    {
        // player cellPos 받아와 x, y 거리 계산
        var playerCell = GameManager.Instance.playerCtrl.readCellPos;
        int xDist = playerCell.x - cell.x;
        int yDist = playerCell.y - cell.y;
        int absXDist = Math.Abs(xDist);
        int absYDist = Math.Abs(yDist);

        // 플레이어가 인접하다면
        if((xDist == 0 && absYDist == 1) || (yDist == 0 && absXDist == 1))
        {
            enemyAnim.SetTrigger(AttackHash);
            GameManager.Instance.ChangeFood(-3);
        }
        else
        {
            if (absXDist > absYDist)
            {
                if (!TryMoveInX(xDist))
                {
                    TryMoveInY(yDist);
                }
            }
            else
            {
                if (!TryMoveInY(yDist))
                {
                    TryMoveInX(xDist);
                }
            }
      }
    }
}
