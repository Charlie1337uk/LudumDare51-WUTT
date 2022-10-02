using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Clock : MonoBehaviour
{
    private TMP_Text text;
    private float Timer;
    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }
    private void Update()
    {
        text.text = Timer.ToString("#.#");
        Timer += Time.deltaTime;
    }
}
