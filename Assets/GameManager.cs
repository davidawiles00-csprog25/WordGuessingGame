using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int score = 0;
    int nbAttempts, maxNbAttempts;
    string[] wordsToGuess = { "car", "elephant", "autocar" };
    public GameObject letter;
    public GameObject cen;
    public float letterSpacing = 100f; // Distance between letters in Canvas pixels

    string wordToGuess = "";
    int lengthOfWordToGuess;
    char[] lettersToGuess;
    bool[] lettersGuessed;

    void Start()
    {
        cen = GameObject.Find("centerOfScreen");
        InitGame();
        InitLetters();
        nbAttempts = 0;
        maxNbAttempts = 10;
        UpdateNbAttempts();
        UpdateScore();
        letter.GetComponent<TextMeshProUGUI>().text = "";
    }

    void OnEnable()
    {
        if (Keyboard.current != null)
        {
            Keyboard.current.onTextInput += ProcessInput;
        }
    }

    void OnDisable()
    {
        if (Keyboard.current != null)
        {
            Keyboard.current.onTextInput -= ProcessInput;
        }
    }

    void InitLetters()
    {
        Transform canvasTransform = GameObject.Find("Canvas").transform;
        int nbLetters = lengthOfWordToGuess;

        for (int i = 0; i < nbLetters; i++)
        {
            // 1. Instantiate as child of Canvas directly
            GameObject gameObject1 = Instantiate(letter, canvasTransform);
            gameObject1.name = "letter" + (i + 1);

            // 2. Adjust RectTransform relative position
            RectTransform rect = gameObject1.GetComponent<RectTransform>();
            if (rect != null)
            {
                // Calculate horizontal offset centered around middle
                float xOffset = (i - (nbLetters - 1) / 2.0f) * letterSpacing;

                // If 'cen' has a RectTransform, offset from its position; otherwise align on Canvas center
                Vector2 centerPos = Vector2.zero;
                if (cen != null && cen.TryGetComponent<RectTransform>(out RectTransform cenRect))
                {
                    centerPos = cenRect.anchoredPosition;
                }

                rect.anchoredPosition = new Vector2(centerPos.x + xOffset, centerPos.y);
            }
        }
    }

    void InitGame()
    {
        //wordToGuess = "Elephant";
        //wordToGuess = "Elephant";
        int randomNumber = Random.Range(0, wordsToGuess.Length);
        wordToGuess = wordsToGuess[randomNumber];
        lengthOfWordToGuess = wordToGuess.Length;
        wordToGuess = wordToGuess.ToUpper();
        lettersToGuess = wordToGuess.ToCharArray();
        lettersGuessed = new bool[lengthOfWordToGuess];
        maxNbAttempts = wordToGuess.Length * 2;
    }

    void ProcessInput(char character)
    {
        char letterPressed = System.Char.ToUpper(character);
        int letterPressedAsInt = System.Convert.ToInt32(letterPressed);

        if (letterPressedAsInt >= 65 && letterPressedAsInt <= 90)
        {
            for (int i = 0; i < lengthOfWordToGuess; i++)
            {
                if (!lettersGuessed[i] && lettersToGuess[i] == letterPressed)
                {
                    lettersGuessed[i] = true;
                    GameObject letterObj = GameObject.Find("letter" + (i + 1));
                    if (letterObj != null)
                    {
                        letterObj.GetComponent<TextMeshProUGUI>().text = letterPressed.ToString();
                    }
                }
            }
        }
    }

    void CheckKeyboard2()
    {
        // Fixed hyphen syntax error in GetMouseButtonDown
        if (Input.anyKeyDown && !Input.GetMouseButtonDown(0))
        {
            // Fixed IndexOutOfRangeException by checking inputString length first
            if (!string.IsNullOrEmpty(Input.inputString))
            {
                char letterPressed = Input.inputString[0];
                int letterPressedAsInt = System.Convert.ToInt32(letterPressed);

                if (letterPressedAsInt >= 97 && letterPressedAsInt <= 122)
                {
                    nbAttempts ++;
                    UpdateNbAttempts(); 
                    if (nbAttempts > maxNbAttempts)
                    {
                        SceneManager.LoadScene("wordGameEnd");
                    }
                    for (int i = 0; i < lengthOfWordToGuess; i++)
                    {
                        if (lettersGuessed[i])
                        {
                            letterPressed = System.Char.ToUpper(letterPressed);
                            if (lettersToGuess[i] == letterPressed)
                            {
                                lettersGuessed[i] = true;
                                GameObject letterObj = GameObject.Find("letter" + (i + 1));
                                if (letterObj != null)
                                {
                                    letterObj.GetComponent<TextMeshProUGUI>().text = letterPressed.ToString();
                                }
                                score = PlayerPrefs.GetInt("score");
                                score++;
                                PlayerPrefs.SetInt("score", score);
                                UpdateScore();
                                score++;
                                PlayerPrefs.SetInt("score", score);
                                UpdateScore();
                                CheckIfWordWasFound();
                            }
                        }
                    }
                }
            }
        }
    }
    void UpdateNbAttempts()
    {
        GameObject.Find("nbAttempts").GetComponent<TextMeshProUGUI>().text = nbAttempts + "/" + maxNbAttempts;
    }
    void UpdateScore()
    {
        GameObject.Find("scoreUI").GetComponent<TextMeshProUGUI>().text = "Score:" + score;
    }
    void CheckIfWordWasFound()
    {
        bool condition = true;
        for (int i = 0; i < lengthOfWordToGuess;i++)
        {
            condition = condition && lettersGuessed[i];
        }
        if (condition)
        {
            PlayerPrefs.SetString("lastWordGuessed", wordToGuess);
            SceneManager.LoadScene("wordGameWin");
        }
    }

    private void Update()
    {
        CheckKeyboard2();
    }
}