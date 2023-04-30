using UnityEngine;

namespace MonkeSlicer
{
    public class ScoreCounter : MonoBehaviour
    {
        public static GameObject firstX;
        public static GameObject secondX;
        public static GameObject lastX;

        public static int Fails;

        void Start()
        {
            firstX = GameObject.Find("X Counter(Clone)/First X");
            secondX = GameObject.Find("X Counter(Clone)/Second X");
            lastX = GameObject.Find("X Counter(Clone)/Last X");

            firstX.SetActive(false);
            secondX.SetActive(false);
            lastX.SetActive(false);
        }

        public static void GetScore()
        {
            if(Fails == 1)
            {
                firstX.SetActive(true);
            }
            else if (Fails == 2)
            {
                secondX.SetActive(true);
            }
            else if (Fails == 3)
            {
                lastX.SetActive(true);
                TextBoardEverythingPain.GameOver(false);
                Plugin.slicerSword.RemoveComponent<SpawnFruits>();
            }
        }
    }
}
