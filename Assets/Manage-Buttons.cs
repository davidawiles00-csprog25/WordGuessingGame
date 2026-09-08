using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManageButtons : MonoBehaviour
{
    public GameObject PlayerName;

    public GameObject NumChars;
    public int NumCharsVal;

    public GameObject TimeToGuess;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.SetInt("score", 0);
        if (SceneManager.GetActiveScene().name == "wordGameStart") PlayerPrefs.SetInt("score", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartWordGame()
    {
        //Debug.Log("Player Name: "+PlayerName.GetComponent<InputField>().text);
        //Debug.Log("Number of characters: " + PlayerName.GetComponent<Dropdown>().value);
        //SceneManager.LoadScene("WordGame");
    }
}
