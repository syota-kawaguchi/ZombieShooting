using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using AudioManager;
using TMPro;

using Google.XR.Cardboard;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject titlePage;
    [SerializeField] private GameObject stageSelectPage;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private TextMeshProUGUI debugText;

    private bool isTitlePage = true;

    // Start is called before the first frame update
    void Start()
    {
        titlePage.SetActive(true);
        stageSelectPage.SetActive(false);
        settingsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        isTitlePage = titlePage.activeSelf;
        if (Input.GetKeyDown(InputManager.Instance.Fire)) {
            debugText.text = $"MouseButton(0) is Pressed";
            if (!settingsPanel.activeSelf) {
                if (isTitlePage) {
                    OnClickTitlePage();
                }
                else {
                    OnClickStartButton();
                }
            }
        }
        else if (Input.GetMouseButton(1)) {
            debugText.text = $"MouseButton(1) is Pressed";
        }
        else if (Input.GetMouseButton(2)) {
            debugText.text = $"MouseButton(2) is Pressed";
        }
        else if (Input.anyKey && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2)) {
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {
                if (Input.GetKeyDown(code)) {
                    //èàóùÇèëÇ≠
                    debugText.text = $"{code} is Pressed";
                    break;
                }
            }
        }

        if (Api.IsGearButtonPressed) {
            settingsPanel.SetActive(true);
        }
    }

    public void OnClickTitlePage() {
        SEManager.Instance.Play(SEPath.DECIDE);

        titlePage.SetActive(false);
        stageSelectPage.SetActive(true);
        BGMManager.Instance.Play(BGMPath.DEVASTATES_STAGESELECT);
    }

    public void OnClickStartButton() {
        SEManager.Instance.Play(SEPath.DECIDE);
        BGMManager.Instance.Stop();
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene() {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Stage0");
        while(!asyncLoad.isDone) {
            yield return null;
        }
    }
}
