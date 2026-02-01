using TMPro;
using UnityEngine;


public class UILockScript : MonoBehaviour
{
    public GameObject currentButton;
    public int currentNumber;
    public int[] combination = new int[4]; 
    private int[] awnser = {1, 2, 3, 4};


    public void CheckCombination()
    {
        if (combination == awnser)
        {
            Debug.Log("Correct.");
        }
        UIManager.instance.lockPad.gameObject.SetActive(false);
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


    }

}
