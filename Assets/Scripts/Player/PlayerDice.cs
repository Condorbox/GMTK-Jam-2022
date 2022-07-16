using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;


public class PlayerDice : MonoBehaviour
{
    [SerializeField] private GameObject dice;
    [SerializeField] private TextMeshProUGUI diceText;

    [SerializeField] private int minValue = 1;
    [SerializeField] private int maxValue = 100;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) ThrowDice();

        //if (Input.GetKeyUp(KeyCode.Q)) dice.SetActive(false);
    }

    public async void ThrowDice()
    {
        dice.SetActive(true);

        string realValue = Mathf.RoundToInt(UnityEngine.Random.Range(minValue, maxValue)).ToString();

        await ChangeDiceNumber();

        diceText.text = realValue;
        Debug.Log(realValue);

        //Debug.Log(Mathf.RoundToInt(UnityEngine.Random.Range(minValue, maxValue)));
    }

    private async Task ChangeDiceNumber()
    {
        for (int i = 0; i < 100; i++)
        {
            diceText.text = Mathf.RoundToInt(UnityEngine.Random.Range(minValue, maxValue)).ToString();
            await Task.Yield();
        }
    }
}
