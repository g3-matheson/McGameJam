using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BloodSlider : MonoBehaviour
{
    private Slider bloodSlider;

    [SerializeField] private float totalTime = 30f;
    public float currentTime{get; private set;}

    public float MaxValue => bloodSlider.maxValue;

    void Awake()
    {
        bloodSlider = GetComponent<Slider>();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = totalTime;
        bloodSlider.maxValue = totalTime;
        bloodSlider.value = currentTime;

        StartCoroutine(StartTimer());
    }

    public void AddBlood(float amount)
    {
        currentTime = Mathf.Clamp(currentTime + amount, 0, bloodSlider.maxValue);
        bloodSlider.value = currentTime;
    }

    void OnTimerEnd()
    {
        Debug.Log("You have starved to death!");
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            player.Death();
        }
    }

    IEnumerator StartTimer()
    {

        while (currentTime > 0)
        {
            currentTime -= 0.01f;
            bloodSlider.value = currentTime;
            yield return new WaitForSeconds(0.01f);
        }

        OnTimerEnd();
    }
}
