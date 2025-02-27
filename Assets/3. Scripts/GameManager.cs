using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public TurnManager turnManager {get; private set;}
    
    public BoardManager boardManager;
    public PlayerCtrl playerCtrl;
    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        turnManager = new TurnManager();
        boardManager.Init();
        
        // BoardManage 스크립트가 붙은 모드 1, 1에 플레이어 생성
        playerCtrl.Spawn(boardManager, new Vector2Int(1, 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
