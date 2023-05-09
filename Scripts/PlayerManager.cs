using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    //for gameover
    public static bool gameOver;
    public GameObject gameOverPanel;

    //for tap to start
    public static bool isGameStarted;
    public GameObject startingText;

    //for coins
    public static int numberOfCoins;
    public Text coinsText;

    //for Highscore
    public Text highScore;


    

    // Start is called before the first frame update
    void Start()
    { 
        gameOver = false;
        Time.timeScale = 1;
        isGameStarted = false;
        numberOfCoins = 0;

        
    }

    // Update is called once per frame
    void Update()
    {
        highScore.text = "HIGHSCORE: " + PlayerPrefs.GetInt("highscore");
        if (gameOver) 
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
            if (numberOfCoins > PlayerPrefs.GetInt("highscore"))
            {
                PlayerPrefs.SetInt("highscore", numberOfCoins);
            }
        }
        
        coinsText.text = "Coins:: " + numberOfCoins;
        if (SwipeManager.tap) 
        {
            isGameStarted = true;
            Destroy(startingText);
        }
    }
    public void ResetHighscore()
    {
        PlayerPrefs.DeleteKey("highscore");
        //highScore.text = "HIGHSCORE: 0";
    }
}
