using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using AudioManager;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI killValue;
    [SerializeField] private TextMeshProUGUI headShotValue;
    [SerializeField] private TextMeshProUGUI clearTime;
    [SerializeField] private TextMeshProUGUI scoreValue;
    [SerializeField] private GameObject okButton;

    [SerializeField] private float startShowDuration = 2.0f;
    [SerializeField] private float duration = 0.8f;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        okButton.SetActive(false);
        killValue.text = "";
        headShotValue.text = "";
        clearTime.text = "";
        scoreValue.text = "";

        BGMManager.Instance.Play(BGMPath.DEVASTATES_STAGESELECT);

        StartCoroutine(ShowScore());
    }

    private IEnumerator ShowScore() {
        yield return new WaitForSeconds(startShowDuration);

        killValue.text = ScoreManager.getKillCound.ToString();
        //Sound

        yield return new WaitForSeconds(duration);

        headShotValue.text = ScoreManager.getHeadShotCount.ToString();
        //Sound

        yield return new WaitForSeconds(duration);

        clearTime.text = ScoreManager.getClearTime.ToString();
        //Sound

        yield return new WaitForSeconds(duration);

        scoreValue.text = ScoreManager.Score().ToString();
        //Sound

        okButton.SetActive(true);

    }

    public void BackToTitle() {
        SEManager.Instance.Play(SEPath.DECIDE);
        SceneManager.LoadScene("Title");
    }
}
