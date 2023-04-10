using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class Settings : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentFireText;
    [SerializeField] private TextMeshProUGUI registFireText;
    private KeyCode pushedKey;
    // Start is called before the first frame update
    void Start()
    {
        Refresh();
    }

    private void OnEnable() {
        Refresh();
    }

    void Refresh() {
        currentFireText.text = $"{InputManager.Instance.Fire.ToString()}";
        registFireText.text = $"";
        pushedKey = KeyCode.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2)) {
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {
                if (Input.GetKeyDown(code)) {
                    //èàóùÇèëÇ≠
                    registFireText.text = $"{code}";
                    if (pushedKey == code) {
                        InputManager.Instance.Fire = code;
                        gameObject.SetActive(false);
                    }
                    else {
                        pushedKey = code;
                    }
                    break;
                }
            }
        }
    }
}
