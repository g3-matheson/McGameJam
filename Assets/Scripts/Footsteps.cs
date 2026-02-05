using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public bool Active = false;
    public float Tick = 1f;
    private float Timer = 0f;

    public GameObject Footstep;
    private bool parity = false;
    public float GaitOffset = 0.2f;

    void Update()
    {
        if (!Active || HunterAI.Instance.Paused) return;

        Timer += Time.deltaTime;
        if (Timer > Tick)
        {
            parity = !parity;

            Vector3 hunterDir = HunterAI.Instance.HunterAgent.velocity.normalized;
            float angle = Mathf.Atan2(hunterDir.y, hunterDir.x) * Mathf.Rad2Deg - 90;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            Vector3 perpendicular = new Vector3(-hunterDir.y, hunterDir.x, 0);
            Vector3 offset = perpendicular * (parity ? -GaitOffset : GaitOffset);

            Vector3 pos = new Vector3(transform.position.x, transform.position.y - 0.5f, 0) + offset;

            Instantiate(Footstep, pos, rotation);
            // TODO Play footstep sound
            Timer = 0f;
        }
                
    }
}
