using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextMeshFlush : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float flushSpeed = 5.0f;

    private float value = 0f;

    private void Start() {
        if (!text) text = GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        var color = text.color;

        value += flushSpeed * Time.deltaTime;
        color.a = Mathf.Abs(Mathf.Sin(value));
        text.color = color;
    }
}
