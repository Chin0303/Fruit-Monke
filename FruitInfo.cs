using System.Collections;
using System.Collections.Generic;
using EzySlice;
using UnityEngine.InputSystem.XR;
using System.IO;
using System.Reflection;
using UnityEngine.XR;
using UnityEngine.UI;
using UnityEngine;
using System;
using GorillaLocomotion;

namespace MonkeSlicer
{
    public class FruitInfo : MonoBehaviour
    {
        public static AudioSource MelonSound;
        public static AudioSource BananaSound;
        public static AudioSource PineappleSound;

        public static void GetFruitInfo(GameObject Fruit)
        {
            if (Fruit.name == "Melon(Clone)(Clone)")
            {
                MelonSound = Plugin.Melon.GetComponent<AudioSource>();
                MelonSound.Play();
                Plugin.crossMaterial = Plugin.MelonTexture.GetComponent<Renderer>().material;
            }
            else if (Fruit.name == "Banana(Clone)(Clone)")
            {
                BananaSound = Plugin.Banana.GetComponent<AudioSource>();
                BananaSound.Play();
                Plugin.crossMaterial = Plugin.BananaTexture.GetComponent<Renderer>().material;
            }
            else if (Fruit.name == "Pineapple(Clone)(Clone)")
            {
                PineappleSound = Plugin.Pineapple.GetComponent<AudioSource>();
                PineappleSound.Play();
                Plugin.crossMaterial = Plugin.PineappleTexture.GetComponent<Renderer>().material;
            }
        }
    }
}
