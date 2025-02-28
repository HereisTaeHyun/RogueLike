using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public TurnManager turnManager {get; private set;}

    public BoardManager boardManager;
    public PlayerCtrl playerCtrl;
    public UIDocument UIdoc;

    private Label foodLabel;
    private int foodAmount = 100;
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
        turnManager.OnTick += OnTurnHappen;
        
        // Board 생성 후 BoardManage 스크립트가 붙은 모드 1, 1에 플레이어 생성
        boardManager.Init();
        playerCtrl.Spawn(boardManager, new Vector2Int(1, 1));

        foodLabel = UIdoc.rootVisualElement.Q<Label>("FoodLabel");
        foodLabel.text = $"Food : {foodAmount}";
    }

    public void OnTurnHappen()
    {
        ChangeFood(-1);
    }

    public void ChangeFood(int amount)
    {
        foodAmount += amount;
        foodLabel.text = $"Food : {foodAmount}";
    }
}
