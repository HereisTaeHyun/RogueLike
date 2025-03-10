using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
#region public
    public static GameManager Instance {get; private set;}
    public TurnManager turnManager {get; private set;}

    public BoardManager boardManager;
    public PlayerCtrl playerCtrl;
    public UIDocument UIdoc;
    public GameObject AndroidPanel;
#endregion

#region private
    private const int START_FOOD_AMOUNT = 100;

    private int currentLevel = 1;
    public int readCurrentLevel {get {return currentLevel;}}
    private Label foodLabel;
    private int foodAmount;
    private AudioSource audioSource;

    private VisualElement gameOverPanel;
    private Label gameOverMessage;
#endregion

#region singleton
    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
#endregion

    void Start()
    {
#if UNITY_ANDROID
    Camera camera = Camera.main;
    camera.orthographicSize = 12;
    camera.transform.position = new Vector3(6, 4, -10);
    AndroidPanel.SetActive(true);
#else
    AndroidPanel.SetActive(false);
#endif
        turnManager = new TurnManager();
        turnManager.OnTick += OnTurnHappen;

        gameOverPanel = UIdoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
        gameOverMessage = gameOverPanel.Q<Label>("GameOverMessage");

        foodLabel = UIdoc.rootVisualElement.Q<Label>("FoodLabel");

        audioSource = GetComponent<AudioSource>();

        StartNewGame();
    }

    // Turn이 발생할때바다 음식을 감소, OnTurnHappen은 turnManager event에 제어됨
    public void OnTurnHappen()
    {
        ChangeFood(-1);
    }
    public void ChangeFood(int amount)
    {
        foodAmount += amount;
        foodLabel.text = $"Food : {foodAmount}";

        if (foodAmount <= 0)
        {
            gameOverPanel.style.visibility = Visibility.Visible;
            gameOverMessage.text = $"Game Over ! \n\n You Survived {currentLevel} Days ! \n\n Press Enter To Start New Game ! ";
            playerCtrl.OnGameOver();
        }
    }

    // 다음 레벨로 이동
    public void NewLevel()
    {
        boardManager.ResetField();
        boardManager.Init();

        playerCtrl.Spawn(boardManager, new Vector2Int(1, 1));
        currentLevel += 1;
    }

    // 변수 초기화 후 새 게임 시작
    public void StartNewGame()
    {
        gameOverPanel.style.visibility = Visibility.Hidden;
        foodLabel.text = $"Food : {foodAmount}";

        currentLevel = 0;
        foodAmount = START_FOOD_AMOUNT;
        playerCtrl.Init();

        NewLevel();
    }

    public void PlaySound(AudioClip clip)
    {
        if(clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
