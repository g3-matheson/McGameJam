using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public bool Active = false;
    public float Tick = 1f;
    private float Timer = 0f;

    public GameObject Footstep;

    void Update()
    {
        if (!Active || HunterAI.Instance.Paused) return;

        Timer += Time.deltaTime;
        if (Timer > Tick)
        {
            Vector3 hunterDir = HunterAI.Instance.HunterAgent.velocity.normalized;
            float angle = Mathf.Atan2(hunterDir.y, hunterDir.x) * Mathf.Rad2Deg - 90;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Instantiate(Footstep, new Vector3(transform.position.x, transform.position.y - 2f, 0), rotation);
            // TODO Play footstep sound
            Timer = 0f;
        }
                
    }
}
