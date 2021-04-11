using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public GameObject packagePrefab;
    public TextMeshProUGUI scoreText;
    public Transform[] boundaries;
    public float rainHeight;
    public float rainInterval;
    public int rainAmount;
    public int initialNumPackages;
    private int score = 0;
    private DataManager dataManager;


    // Start is called before the first frame update
    void Start()
    {
        dataManager = (DataManager)FindObjectOfType(typeof(DataManager));
        scoreText.text = "" + score;
        StartCoroutine(RainControl());
    }

    private IEnumerator RainControl()
    {
        // initial rain
        Rain(initialNumPackages);
        // keep raining at the specified interval
        while (true)
        {
            Rain(rainAmount);
            yield return new WaitForSeconds(rainInterval);
        }
    }

    // spawn packages
    private void Rain(int numPackages)
    {
        for (int i = 0; i < numPackages; i++)
        {
            Instantiate(packagePrefab,
                        new Vector3(
                            Random.Range(boundaries[2].position.x,
                                            boundaries[3].position.x),
                            rainHeight,
                            Random.Range(boundaries[1].position.z,
                                            boundaries[0].position.z)),
                        Quaternion.identity);                            
        }
    }

    public void IncrementScore()
    {
        score++;
        scoreText.text = "" + score;
    }

    public void EndGame()
    {
        dataManager.Score = score;
        SceneManager.LoadScene(2);
    }
}
