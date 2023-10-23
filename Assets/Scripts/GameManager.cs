using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject textWin;
    [SerializeField] private GameObject textLoss;
    [SerializeField] private TMP_Text textPoints;
    [SerializeField] private float points = 0;

    public void ShowWin()
    {
        textWin.SetActive(true);
        StartCoroutine(RestartCourutine());
    }

    public void ShowLoss()
    {
        textLoss.SetActive(true);
        StartCoroutine(RestartCourutine());
    }

    IEnumerator RestartCourutine()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(3);
        Restart();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddPoint() {
        points += 1;
        textPoints.text = $"{points} Points";
    }
}
