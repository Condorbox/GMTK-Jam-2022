using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class PlayerDice : MonoBehaviour
{
    [SerializeField] private GameObject dice;
    [SerializeField] private TextMeshProUGUI diceText;
    [SerializeField] private int[] diceMaxValues;
    private int diceMinValue = 1;
    private int index = 0;
    [SerializeField] private float diceTime = 1f;
    [SerializeField] private int diceNumberTarget = 1;
    private int diceNumber = 0;

    private bool roll = false;

    public static EventHandler OnDiceActivated;
    public static EventHandler OnDiceDeactivated;

    private void Awake()
    {
        roll = false;
    }

    public void OnEnable()
    {
        HealthSystem.OnAnyDead += HealthSystem_OnAnyDead;
    }

    public void OnDisable()
    {
        HealthSystem.OnAnyDead -= HealthSystem_OnAnyDead;
    }

    private void HealthSystem_OnAnyDead(object sender, Death e)
    {
        ThrowDice();
        StartCoroutine(CheckDice(e));
    }

    private IEnumerator CheckDice(Death e) //TODO Change Index?
    {
        yield return new WaitUntil(() => roll);
        if (e.murdered.CompareTag("Player"))
        {
            if (diceNumber == diceNumberTarget)
            {
                Destroy(e.killer);
                index = IndexUp(index);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            if (diceNumber == diceNumberTarget)
            {
                SceneManager.LoadScene(0); //BadLuck
            }
            else
            {
                Destroy(e.murdered);
                index = IndexDown(index);
            }
        }

        roll = false;
    }

    private int IndexUp(int index)
    {
        index--;
        if (index < 0)
        {
            index = 0;
        }

        return index;
    }

    private int IndexDown(int index)
    {
        index++;
        if (index >= diceMaxValues.Length)
        {
            index = diceMaxValues.Length - 1;
        }

        return index;
    }

    public async void ThrowDice()
    {
        dice.SetActive(true);
        OnDiceActivated?.Invoke(this, EventArgs.Empty);

        int realValue = Mathf.RoundToInt(UnityEngine.Random.Range(diceMinValue, diceMaxValues[index]));

        await ChangeDiceNumber();

        diceText.text = realValue.ToString();
        diceNumber = realValue;
        roll = true;
        Invoke("DeactivateDice", diceTime);

        //Debug.Log(Mathf.RoundToInt(UnityEngine.Random.Range(minValue, maxValue)));
    }

    private async Task ChangeDiceNumber()
    {
        for (int i = 0; i < 100; i++)
        {
            diceText.text = Mathf.RoundToInt(UnityEngine.Random.Range(diceMinValue, diceMaxValues[index])).ToString();
            await Task.Yield();
        }
    }

    private void DeactivateDice()
    {
        dice.SetActive(false);
        OnDiceDeactivated?.Invoke(this, EventArgs.Empty);
    }
}
