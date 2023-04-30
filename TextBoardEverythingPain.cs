using UnityEngine;

namespace MonkeSlicer
{
    public class TextBoardEverythingPain : MonoBehaviour
    {
        public static GameObject PlayAgainButton;
        public static GameObject PlayButton;
        public static GameObject RulesButton;

        public static GameObject StartText;
        public static GameObject RulesText;
        public static GameObject BombText;
        public static GameObject MissedFruitText;
        public static GameObject StartingGame;

        public static void TextBoardSetup()
        {
            PlayAgainButton = GameObject.Find("Board(Clone)/PlayAgainButton");
            PlayButton = GameObject.Find("Board(Clone)/StartText/PlayButton");
            RulesButton = GameObject.Find("Board(Clone)/StartText/RulesButton");
            StartText = GameObject.Find("Board(Clone)/StartText");
            RulesText = GameObject.Find("Board(Clone)/RulesText");
            BombText = GameObject.Find("Board(Clone)/BombText");
            MissedFruitText = GameObject.Find("Board(Clone)/MissedFruitText");
            StartingGame = GameObject.Find("Board(Clone)/StartingGame");
        }
        public static void GameOver(bool ByBomb)
        {
           Plugin.slicerSword.RemoveComponent<SpawnFruits>();
           if (ByBomb)
           {
                PlayAgainButton.SetActive(true);
                BombText.SetActive(true);
           }
           else
           {
                PlayAgainButton.SetActive(true);
                MissedFruitText.SetActive(true);
           }
        }
    }
}
