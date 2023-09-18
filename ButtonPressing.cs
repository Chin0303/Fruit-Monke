using UnityEngine;

namespace MonkeSlicer
{
    public class ButtonPressing : MonoBehaviour
    {
        public static void Play()
        {
            TextBoardEverythingPain.PlayAgainButton.SetActive(false);
            TextBoardEverythingPain.PlayButton.SetActive(false);
            TextBoardEverythingPain.RulesButton.SetActive(false);
            TextBoardEverythingPain.StartText.SetActive(false);
            TextBoardEverythingPain.RulesText.SetActive(false);
            TextBoardEverythingPain.BombText.SetActive(false);
            TextBoardEverythingPain.MissedFruitText.SetActive(false); // Pls ignore
            Plugin.slicerSword.AddComponent<SpawnFruits>();

            ScoreCounter.firstX.SetActive(false);
            ScoreCounter.secondX.SetActive(false);
            ScoreCounter.lastX.SetActive(false);
            ScoreCounter.Fails = 0;
        }
        public static void Rules()
        {
            TextBoardEverythingPain.StartText.SetActive(false);
            TextBoardEverythingPain.RulesText.SetActive(true);
            TextBoardEverythingPain.PlayAgainButton.SetActive(true);
        }
    }
}
