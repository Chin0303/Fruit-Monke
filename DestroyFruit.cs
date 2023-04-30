using UnityEngine;

namespace MonkeSlicer
{
    public class DestroyFruit : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Finish")
            {
                ScoreCounter.Fails++;
                ScoreCounter.GetScore();
                Destroy(collision.gameObject);
            }
            else if(collision.gameObject.tag == "Respawn")
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
