using UnityEngine;

namespace MonkeSlicer
{
    public class SpawnFruits : MonoBehaviour
    {
        public static int waitThing = 0;

        static float spawnTimer = 2;
        static float timer;
        static float throwSpeed = 8;

        GameObject Melon;
        GameObject Banana;
        GameObject Pineapple;
        GameObject Bomb;

        static GameObject SpawnPoint1;
        static GameObject SpawnPoint2;
        static GameObject SpawnPoint3;
        static GameObject SpawnPoint4;
        static GameObject SpawnPoint5;
        static GameObject SpawnPoint6;
        static GameObject SpawnPoint7;
        static GameObject SpawnPoint8;
        static GameObject SpawnPoint9;
        static GameObject SpawnPoint10;

        static GameObject[] spawns;
        static GameObject[] fruits;

        static GameObject FruitsParent;

        void Start()
        {

            FruitsParent = new GameObject("FruitsParent");

            spawns = new GameObject[]
            {
                SpawnPoint1 = GameObject.Find("SpawnPoint1"),
                SpawnPoint2 = GameObject.Find("SpawnPoint2"),
                SpawnPoint3 = GameObject.Find("SpawnPoint3"),
                SpawnPoint4 = GameObject.Find("SpawnPoint4"),
                SpawnPoint5 = GameObject.Find("SpawnPoint5"),
                SpawnPoint6 = GameObject.Find("SpawnPoint6"),
                SpawnPoint7 = GameObject.Find("SpawnPoint7"),
                SpawnPoint8 = GameObject.Find("SpawnPoint8"),
                SpawnPoint9 = GameObject.Find("SpawnPoint9"),
                SpawnPoint10 = GameObject.Find("SpawnPoint10")
            };
            fruits = new GameObject[]
            {
                Melon = GameObject.Find("Melon(Clone)"),
                Banana = GameObject.Find("Banana(Clone)"),
                Pineapple = GameObject.Find("Pineapple(Clone)"),
                Bomb = GameObject.Find("Bomb(Clone)"),
            };
            print("Fruit Setup DOne");
        }

        void Update()
        {
            if (!Plugin.inRoom)
                return;

            timer += Time.deltaTime;
            if (timer > spawnTimer)
            {
                Plugin.canceller = 0;

                GameObject randomPoint = spawns[Random.Range(0, spawns.Length)];
                GameObject randomFruit = fruits[Random.Range(0, fruits.Length)]; 

                GameObject spawnedFruit = Instantiate(randomFruit, randomPoint.transform.position, randomPoint.transform.rotation);

                if (spawnedFruit.name == "Bomb(Clone)(Clone)")
                {
                    spawnedFruit.tag = "Respawn";
                    spawnedFruit.transform.SetParent(GameObject.Find("FruitsParent").transform, false);
                    spawnedFruit.layer = LayerMask.NameToLayer("Ignore Raycast");
                    spawnedFruit.AddComponent<Rigidbody>();
                    Rigidbody rb = spawnedFruit.GetComponent<Rigidbody>();
                    rb.velocity = randomPoint.transform.forward * throwSpeed;
                    timer -= spawnTimer;
                }
                else
                {
                    spawnedFruit.tag = "Finish";
                    timer -= spawnTimer;

                    spawnedFruit.transform.SetParent(GameObject.Find("FruitsParent").transform, false);
                    spawnedFruit.layer = LayerMask.NameToLayer("Ignore Raycast");

                    spawnedFruit.AddComponent<Rigidbody>();
                    Rigidbody rb = spawnedFruit.GetComponent<Rigidbody>();
                    rb.velocity = randomPoint.transform.forward * throwSpeed;
                }
            }
        }
    }
}
