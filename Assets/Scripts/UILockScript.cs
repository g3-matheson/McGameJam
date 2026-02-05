using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;

public class UILockScript : MonoBehaviour
{
    public GameObject currentButton;
    private int currentButtonNumber;
    public int currentNumber;
    public bool isUnlocked = false;
    public int[] combination = { 1, 1, 1, 1};
    public int[] Answer = {1, 2, 4, 3};

    public void CheckCombination()
    {
        bool result = true;
        for (int i = 0; i < 4; i++)
        {
            if (combination[i] != Answer[i]) result = false;
        }

        if (result) isUnlocked = true;
    }

    public void ButtonPress(GameObject self)
    {   
        currentButton = self;
        currentButtonNumber = int.Parse(currentButton.transform.name.Replace("Button",""));
        
        combination[currentButtonNumber] = combination[currentButtonNumber] + 1;
        if (combination[currentButtonNumber] == 5) combination[currentButtonNumber] = 1; 

        currentButton.GetComponentInChildren<TextMeshProUGUI>().text = combination[currentButtonNumber].ToString();

        CheckCombination();
    }

}
