using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public enum Choice { None, Scissors, Rock, Paper }

    [Header("UI Buttons")]
    public Button buttonScissors;
    public Button buttonRock;
    public Button buttonPaper;
    public Button buttonRestart;

    // 플레이어가 선택한 값 저장
    private Choice playerChoice = Choice.None;

    [Header("Display Images")]
    public Image imagePlayer;
    public Image imageComputer;

    [Header("Sprites")]
    public Sprite spriteScissors;
    public Sprite spriteRock;
    public Sprite spritePaper;

    // 스프라이트 순환용 변수
    private bool isAnimating = true;
    private float animationInterval = 0.1f;
    private float animationTimer = 0f;
    private int currentSpriteIndex = 0;
    private Sprite[] sprites;

    [Header("Result Text")]
    public TextMeshProUGUI textResult;

    [Header("Scoer Board")]
    public TextMeshProUGUI textScorePlayer;
    public TextMeshProUGUI textScoreComputer;

    private int scorePlayer = 0;
    private int scoreComputer = 0;

    [Header("Panel")]

    public GameObject panelGameOver;
    public TextMeshProUGUI textGameResult;
    public Button buttonGameRestart;
    public Button buttonGameExit;

    [Header("Animators")]
    public Animator playerAnimator;
    public Animator computerAnimator;

    public int WinScore = 5; 

    void Start()
    {

        sprites = new Sprite[] { spriteScissors, spriteRock, spritePaper };

        imagePlayer.sprite = spriteRock;

        // 버튼 클릭 이벤트 연결
        buttonScissors.onClick.AddListener(() => OnPlayerChoice(Choice.Scissors));
        buttonRock.onClick.AddListener(() => OnPlayerChoice(Choice.Rock));
        buttonPaper.onClick.AddListener(() => OnPlayerChoice(Choice.Paper));
        buttonRestart.onClick.AddListener(() => OnRestart());
        panelGameOver.SetActive(false);
        buttonGameRestart.onClick.AddListener(() => OnGameRestart());
        buttonGameExit.onClick.AddListener(() => OnGameExit());
    

        textResult.text = "가위 바위 보 중 하나를 선택하세요!";
    }

    void Update()
    {
        if (isAnimating)
        {            
            animationTimer += Time.deltaTime;

            if (animationTimer >= animationInterval)
            {
                animationTimer = 0f;
                currentSpriteIndex = (currentSpriteIndex + 1) % 3; // 0, 1, 2 반복
                imageComputer.sprite = sprites[(currentSpriteIndex + 1) % 3];
            }
        }
    }

    void OnPlayerChoice(Choice choice)
    {
        // 애니메이션 중지
        isAnimating = false;

        playerChoice = choice;
        Debug.Log("플레이어 선택: " + choice.ToString());

        // 컴퓨터 선택 및 승부 판정
        Choice computerChoice = GetComputerChoice();
        Debug.Log("컴퓨터 선택: " + computerChoice.ToString());

        // 이미지에 선택 결과 표시
        imagePlayer.sprite = GetSpriteFromChoice(playerChoice);
        imageComputer.sprite = GetSpriteFromChoice(computerChoice);

        // 결과 판정 및 텍스트 표시
        string result = DetermineWinner(playerChoice, computerChoice);
        textScorePlayer.text = scorePlayer.ToString();
        textScoreComputer.text = scoreComputer.ToString();
        textResult.text = result;
        Debug.Log("결과: " + result);

        imagePlayer.transform.localScale = Vector3.one;
        imageComputer.transform.localScale = Vector3.one;

        if (result == "플레이어 승리!")
        {
            playerAnimator.SetTrigger("OnWin");
        }
        else if (result == "컴퓨터 승리!")
        {
            computerAnimator.SetTrigger("OnWin");
        }
        
    }

    void OnRestart()
    {
        isAnimating = true;
        animationTimer = 0f;
        currentSpriteIndex = 0;
        imagePlayer.sprite = spriteRock;
        imageComputer.sprite = spriteRock;
        imagePlayer.transform.localScale = Vector3.one;
        imageComputer.transform.localScale = Vector3.one;
        textResult.text = "가위 바위 보 중 하나를 선택하세요!";
    }

    Choice GetComputerChoice()
    {
        int random = Random.Range(0, 3); 
        switch (random)
        {
            case 0: return Choice.Scissors;
            case 1: return Choice.Rock;
            case 2: return Choice.Paper;
            default: return Choice.Rock;
        }
    }
    string DetermineWinner(Choice player, Choice computer)
    {
        if (player == computer)
            return "무승부!";
        
        bool playerWins = (player == Choice.Scissors && computer == Choice.Paper) ||
                          (player == Choice.Rock && computer == Choice.Scissors) ||
                          (player == Choice.Paper && computer == Choice.Rock);

        if (playerWins) 
        {
            scorePlayer++;
        }
        else
        {
            scoreComputer++;
        }

        if (scorePlayer == WinScore || scoreComputer == WinScore)
        {
            panelGameOver.SetActive(true);
            textGameResult.text = scorePlayer == WinScore ? "User Win!!! (5점 달성)" : "Computer Win!!! (5점 달성)";
            SetRpsButtonsInteractable(false);
        }

        return playerWins ? "플레이어 승리!" : "컴퓨터 승리!"; 
    }

    Sprite GetSpriteFromChoice(Choice choice)
    {
        switch (choice)
        {
            case Choice.Scissors: return spriteScissors;
            case Choice.Rock: return spriteRock;
            case Choice.Paper: return spritePaper;
            default: return spriteRock;
        }
    }

    void SetRpsButtonsInteractable(bool value)
    {
        buttonScissors.interactable = value;
        buttonRock.interactable = value;
        buttonPaper.interactable = value;
    }

    void OnGameRestart()
    {
        scorePlayer = 0;
        scoreComputer = 0;
        textScorePlayer.text = scorePlayer.ToString();
        textScoreComputer.text = scoreComputer.ToString();
        panelGameOver.SetActive(false);
        OnRestart();
        SetRpsButtonsInteractable(true);
    }

    void OnGameExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
