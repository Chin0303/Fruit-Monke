using UnityEngine;

namespace MonkeSlicer
{
    public class KatanaSwing : MonoBehaviour
    {
        Vector3 lastPosition;
        AudioSource swingSound;

        float Cooldown = 1f;
        float lastSwing;

        void Start()
        {
            swingSound = Plugin.slicerSword.GetComponent<AudioSource>();
            lastPosition = transform.position;
        }

        void Update()
        {
            Vector3 delta = transform.position - lastPosition;
            if (delta.magnitude > 0.3f && Time.time - lastSwing >= Cooldown)
            {
                swingSound.Play();
                GorillaTagger.Instance.StartVibration(false, 1, 0.3f);
                lastSwing = Time.time;
            }
            lastPosition = transform.position;
        }
    }
}
