using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Used to control the game. */
public class GameController : MonoBehaviour
{

    /* Variables */


    //Unity Objects
    public GameObject[] turnIcons; //display whose turn it is
    public Sprite[] playerIcons;  // 0 = X icon, 1 = O icon
    public Button[] tictactoeSpaces; // playable space for game
    public GameObject[] winningLines; // all win condition lines
    public Text winningText; // text displayed when game is over
    public GameObject winningPannel; // panel used to prevent board to be interactable
    public Button xButton, oButton; // used to switch turns
    public Text xScoreText, oScoreText; // scores for each player displayed

    // Controller Variables
    public int whoseTurn; // 0 = X, 1 = O
    public int turnCount; // num of turns
    public int[] board;// place piece for each space
    public int xScore, oScore; // scores for each player
    public Boolean isAI; // 0 = none, 1 = easy, 2 = medium, 3 = hard



    /* Setup Functions */


    // Start is called before the first frame update
    void Start()
    {
        GameSetup();
        if (PlayerPrefs.GetInt("ai") != 0)
            InvokeRepeating("CheckAI", 0, 1);
    }

    // Sets the board
    void GameSetup()
    {
        whoseTurn = PlayerPrefs.GetInt("whoStarts", 0);
        SetCurrent(whoseTurn);
        turnCount = 0;
        for (int i = 0; i < tictactoeSpaces.Length; i++)
        {
            tictactoeSpaces[i].interactable = true;
            tictactoeSpaces[i].GetComponent<Image>().sprite = null;
        }
        for (int i = 0; i < board.Length; i++)
        {
            board[i] = 0;
        }
    }



    /* Buttons */


    // Play the game, used to place icon on a specific square
    public void TicTacToeButton(int spaceNumber)
    {
        xButton.interactable = false;
        oButton.interactable = false;
        tictactoeSpaces[spaceNumber].interactable = false;

        switch (whoseTurn)
        {
            case 0:
                tictactoeSpaces[spaceNumber].image.sprite = playerIcons[0];
                board[spaceNumber] = -1;
                if (GameOver())
                {
                    GameOverScreen();
                    return;
                }
                SetCurrent(1);
                break;
            case 1:
                tictactoeSpaces[spaceNumber].image.sprite = playerIcons[1];
                board[spaceNumber] = 1;
                if (GameOver())
                {
                    GameOverScreen();
                    return;
                }
                SetCurrent(0);
                break;
            default:
                Console.WriteLine("Default case");
                break;

        }

        turnCount++;
        if (GameOver())
        {
            GameOverScreen();
            return;
        }
    }

    // Used to set whose turn goes first
    public void SetFirst(int player)
    {
        SetCurrent(player);
        if (player == 0)
        {
            PlayerPrefs.SetInt("whoStarts", 0);
        }
        else
        {
            PlayerPrefs.SetInt("whoStarts", 1);
        }

    }

    // Restarts the board
    public void Rematch()
    {
        GameSetup();
        winningPannel.SetActive(false);
        for (int i = 0; i < winningLines.Length; i++)
        {
            winningLines[i].SetActive(false);
        }
        for (int i = 0; i < winningLines.Length; i++)
        {
            winningLines[i].SetActive(false);
        }
    }

    // Restarts the board and scores
    public void Restart()
    {
        Rematch();
        xScore = 0;
        xScoreText.text = xScore.ToString();
        oScore = 0;
        oScoreText.text = oScore.ToString();
    }

    // Goes to menu
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }



    /* Helper functions */


    // Checks if there is an AI, if so, AI makes a move
    void CheckAI()
    {
        if (!GameOver() && isAI && whoseTurn == 1)
        {
            int aiMove;
            if (PlayerPrefs.GetInt("ai") == 1)
            {
                aiMove = AI.Easy(board);
            }
            if (PlayerPrefs.GetInt("ai") == 2)
            {
                aiMove = AI.Medium(board, whoseTurn);
            }
            else
            {
                aiMove = AI.Hard(board, whoseTurn);
            }
            TicTacToeButton(aiMove);
        }
    }

    // Checks if someone has won or the game is tied
    Boolean GameOver()
    {
        SolutionLine[] solutions = SolutionLines(board);
        for (int i = 0; i < solutions.Length; i++)
        {
            if (solutions[i].HasWinner())
            {
                return true;
            }
        }
        if (turnCount > 8)
            return true;
        return false;
    }

    // If gameOver true, display a game over screen
    void GameOverScreen()
    {
        int solutionNumber = -1;

        SolutionLine[] solutions = SolutionLines(board);
        for (int i = 0; i < solutions.Length; i++)
        {
            if (solutions[i].HasWinner())
            {
                solutionNumber = i;
            }
        }
        winningPannel.SetActive(true);
        if (solutionNumber == -1)
            winningText.text = "It's A Tie!";
        else
        {
            winningLines[solutionNumber].SetActive(true);
            if (whoseTurn == 0)
            {
                xScore++;
                xScoreText.text = xScore.ToString();
                winningText.text = "Player X Wins!";
            }
            else
            {
                oScore++;
                oScoreText.text = oScore.ToString();
                winningText.text = "Player O Wins!";
            }
        }
        xButton.interactable = true;
        oButton.interactable = true;
    }


    // Used to set whose turn it is
    public void SetCurrent(int player)
    {
        if (player == 0)
        {
            whoseTurn = 0;
            turnIcons[0].SetActive(true);
            turnIcons[1].SetActive(false);
        }
        else
        {
            whoseTurn = 1;
            turnIcons[1].SetActive(true);
            turnIcons[0].SetActive(false);
        }

    }

    // Helper function, used to represent each possible
    // line that can be a solution for the tictactoe board
    public static SolutionLine[] SolutionLines(int[] board)
    {
        SolutionLine[] solutions = new SolutionLine[8];
        for (int i = 1; i < 9; i++)
        {
            solutions[i - 1] = new SolutionLine(board, i);
        }
        return solutions;
    }
}