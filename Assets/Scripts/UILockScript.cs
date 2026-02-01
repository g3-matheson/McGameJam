using TMPro;
using UnityEngine;

public class UILockScript : MonoBehaviour
{
    public GameObject currentButton;
    public int currentNumber;
    public int[] combination = new int[4]; 
    private int[] Answer = {1, 2, 3, 4};


    public void CheckCombination()
    {
        bool result = true;
        for (int i = 0; i < 3; i++)
        {
            if (combination[i] != Answer[i]) result = false;
        }

        if (result)
        {
            Debug.Log($"CORRECT!");
        }
    }

    public void ButtonPress(GameObject self)
    {   
        currentButton = self;
        currentNumber = int.Parse(currentButton.GetComponentInChildren<TextMeshProUGUI>().text);
        currentButton.GetComponentInChildren<TextMeshProUGUI>().text = (currentNumber + 1).ToString();
        for (int i = 0; i > 3; i++)
        {
            if (currentButton.name == $"Button {i}")
            {
                combination[i] = currentNumber;
                break;
            }
        }
        
        if (currentNumber >= 9)
        {
            currentButton.GetComponentInChildren<TextMeshProUGUI>().text = "0";
        }

        CheckCombination();


    }

}
