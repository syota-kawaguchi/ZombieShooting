using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private Image ClearPanel;
    [SerializeField] private TextMeshProUGUI clearText;
    [SerializeField] private Image GameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private float fadeinSpeed = 0.01f;
    [SerializeField] private float alphaOffset = 0.5f;
    [SerializeField] private float nextPageDuration = 3.0f;

    public Transform player;
    [SerializeField] private GameObject boss;
    private bool clearFlag = false;

    new private void Awake() {
        if (!Application.isEditor) {
            Debug.unityLogger.logEnabled = false;
        }
    }

    void Start()
    {
        GameStart();
        ClearPanel.gameObject.SetActive(false);
        GameOverPanel.gameObject.SetActive(false);

        if (!boss) {
            Debug.LogError("Boss is null");
        }
        clearFlag = false;
    }

    void Update() {
        if (!boss && !clearFlag) {
            clearFlag = true;
            Clear();
        }
    }

    private void GameStart() {
        ScoreManager.Init();
    }

    public void Clear() {
        StartCoroutine(EndGameProcess(true));
    }

    public void GameOver() {
        StartCoroutine(EndGameProcess(false));
    }

    private IEnumerator EndGameProcess(bool isClear) {
        ScoreManager.End(isClear);

        Cursor.lockState = CursorLockMode.None;

        var panel = isClear ? ClearPanel : GameOverPanel;
        var text = isClear ? clearText : gameOverText;
        panel.gameObject.SetActive(true);
        panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 0);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        //Panelのフェードイン
        while (panel.color.a < alphaOffset) {
            var color = panel.color;
            color.a += fadeinSpeed;
            panel.color = color;
            color = text.color;
            color.a += fadeinSpeed;
            text.color = color;
            Debug.Log($"alpha : {panel.color.a}");
            yield return new WaitForSeconds(0.01f);
        }
        //画面遷移
        yield return new WaitForSeconds(nextPageDuration);
        SceneManager.LoadScene("Result");
    } 
}
