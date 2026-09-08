using UnityEngine;
using TMPro;

public class DisplayLastWordGuessed : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<TextMeshProUGUI>(). text = PlayerPrefs.GetString("lastWordGuessed");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
