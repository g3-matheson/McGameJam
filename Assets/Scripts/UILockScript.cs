using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class UILockScript : MonoBehaviour
{
    private TextMeshProUGUI[] nubmers;
    public GameObject[] buttons;
    private List<int> combinations = new List<int>();
    public int index;
    private int awnser = 1234;

    void Awake()
    {
        nubmers = GetComponentsInChildren<TextMeshProUGUI>();
        
    }
    void Start()
    {
        
    }

    public void SetNumber(int[] newNumbers)
    {
        combinations.Clear();
        foreach (int combination in newNumbers)
        {
            combinations.Add(combination);
        }
        gameObject.SetActive(true);
    }


    public void CheckCombination()
    {

        foreach (var number in nubmers)
        {
            number.text = combinations[index].ToString();
            index = (index + 1) % combinations.Count;
        }
        if (int.Parse(string.Join("", combinations)) == awnser)
        {
            Debug.Log("Padlock Unlocked!");
        }
        else
        {
            Debug.Log("Incorrect Combination");
            gameObject.SetActive(false);
        }
        
    }

    public void ButtonPress(GameObject numberRef)
    {
        Debug.Log($"Button Pressed {numberRef.name}");
    }

}
