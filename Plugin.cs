using System;
using BepInEx;
using UnityEngine;
using Utilla;
using EzySlice;
using System.IO;
using System.Reflection;

namespace MonkeSlicer
{
	[ModdedGamemode]
	[BepInDependency("org.legoandmars.gorillatag.utilla")]
	[BepInDependency("com.ahauntedarmy.gorillatag.tmploader")]
	[ModdedGamemode("fruitmonke", "FRUIT MONKE", Utilla.Models.BaseGamemode.Casual)]
	[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]

	public class Plugin : BaseUnityPlugin
	{
		VelocityEstimator velocityEstimator;

		GameObject startSlicePoint;
	    GameObject endSlicePoint;

		public static GameObject slicerSword;
		public static GameObject spawnPoints;

		public static GameObject Melon;
		public static GameObject Banana;
		public static GameObject Pineapple;
		public static GameObject Bomb;

		public static GameObject Board;

		public static GameObject MelonTexture;
		public static GameObject BananaTexture;
		public static GameObject PineappleTexture;
		public static Material crossMaterial;

		GameObject Destroyer;
		GameObject FailCounter;

		MeshRenderer meshSword;

		LayerMask sliceLayer;

		public static bool inRoom;
		float cutForce = 2000;

		GameObject slicedParent;

		public static int canceller = 0;

		public static bool Enabled;
		public static bool Disabled;

		float destroyTimer = 15;
		float timer;

		AudioSource bombSound;

		void Start()
		{
			Utilla.Events.GameInitialized += OnGameInitialized;
		}

		void OnEnable()
		{
			HarmonyPatches.ApplyHarmonyPatches();
		}

		void OnDisable()
		{
			HarmonyPatches.RemoveHarmonyPatches();
		}

		[ModdedGamemodeJoin]
		public void OnJoin(string gamemode)
		{
			if (gamemode.Contains("fruitmonke"))
			{
				inRoom = true;
				Setup();
			}
		}

		[ModdedGamemodeLeave]
		public void OnLeave(string gamemode)
		{
			inRoom = false;
			UnSetup();
		}

		void OnGameInitialized(object sender, EventArgs e)
		{
			Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MonkeSlicer.Resources.slicebundle");
			AssetBundle assetBundle = AssetBundle.LoadFromStream(manifestResourceStream);

			spawnPoints = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("SpawnPoints"));
			slicerSword = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("Sword"));

			Melon = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("Melon"));
			Banana = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("Banana"));
			Pineapple = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("Pineapple"));
			Bomb = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("Bomb"));

			Board = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("Board"));

			FailCounter = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("X Counter"));
			Destroyer = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("Destroyer"));

			MelonTexture = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("waterMelonTexture"));
			BananaTexture = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("bananaTexture"));
			PineappleTexture = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("pineappleTexture"));
			assetBundle.Unload(false);

			slicerSword.transform.SetParent(GameObject.Find("Global/Local VRRig/Local Gorilla Player/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R").transform, false);

			startSlicePoint = GameObject.Find("startSlicePoint");
			endSlicePoint = GameObject.Find("endSlicePoint");

			sliceLayer = LayerMask.NameToLayer("Water");

			endSlicePoint.AddComponent<VelocityEstimator>();
			velocityEstimator = endSlicePoint.GetComponent<VelocityEstimator>();
			FailCounter.AddComponent<ScoreCounter>();

			slicedParent = new GameObject("slicedParent");

			Bomb.tag = "Respawn";
			bombSound = Bomb.GetComponent<AudioSource>();


			meshSword = GameObject.Find("Global/Local VRRig/Local Gorilla Player/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R/Sword(Clone)/Katana").GetComponent<MeshRenderer>();
			meshSword.enabled = false;
			FailCounter.SetActive(false);
			Board.SetActive(false);
		}

		void FixedUpdate()
		{
			if (!inRoom)
				return;
			timer += Time.deltaTime;

			if(timer > destroyTimer)
            {
				DestroySlices();
			}

			bool hasHit = Physics.Linecast(startSlicePoint.transform.position, endSlicePoint.transform.position, out RaycastHit hit, sliceLayer);
			if (hasHit)
			{
				if (hit.transform.gameObject.name == "PlayAgainButton")
				{
					ButtonPressing.Play();
				}
				else if (hit.transform.gameObject.name == "PlayButton")
				{
					ButtonPressing.Play();
				}
				else if (hit.transform.gameObject.name == "RulesButton")
				{
					ButtonPressing.Rules();
				}
				else if (hit.transform.gameObject.tag == "Respawn")
				{
					ScoreCounter.GetScore();
					TextBoardEverythingPain.GameOver(true);
					hit.transform.gameObject.layer = LayerMask.NameToLayer("Gorilla Body Collider");
				}
				else
				{ 
					GameObject target = hit.transform.gameObject;
					Slice(target);
					GorillaTagger.Instance.StartVibration(false, 15, 0.3f);
				}
			}
		}

		void Slice(GameObject target)
		{
			Vector3 velocity = velocityEstimator.GetVelocityEstimate();
			Vector3 planeNormal = Vector3.Cross(endSlicePoint.transform.position - startSlicePoint.transform.position, velocity);
			planeNormal.Normalize();

			SlicedHull hull = target.Slice(endSlicePoint.transform.position, planeNormal);

			if (hull != null && canceller == 0)
			{
				canceller++;

				FruitInfo.GetFruitInfo(target);

				GameObject upperHull = hull.CreateUpperHull(target, crossMaterial);
				SetupSlicedComponent(upperHull);

				GameObject lowerHull = hull.CreateLowerHull(target, crossMaterial);
				SetupSlicedComponent(lowerHull);

				Destroy(target);
			}
		}

		public void SetupSlicedComponent(GameObject slicedObject)
		{
			slicedObject.tag = "Finish";
			Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
			MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
			collider.convex = true;
			rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
			slicedObject.transform.SetParent(GameObject.Find("slicedParent").transform, false);
			slicedObject.layer = LayerMask.NameToLayer("Gorilla Body Collider");
		}

		public void DestroySlices()
        {
			timer -= destroyTimer;
			if (slicedParent != null)
			{
				foreach (Transform child in slicedParent.transform)
				{
					Destroy(child.gameObject);
				}
			}
		}

		void Setup()
        {
			Board.SetActive(true);

			TextBoardEverythingPain.TextBoardSetup();

			meshSword.enabled = true;
			Destroyer.SetActive(true);
			FailCounter.SetActive(true);

			TextBoardEverythingPain.PlayAgainButton.SetActive(false);
			TextBoardEverythingPain.PlayButton.SetActive(true);
			TextBoardEverythingPain.RulesButton.SetActive(true);
			TextBoardEverythingPain.StartText.SetActive(true);
			TextBoardEverythingPain.RulesText.SetActive(false);
			TextBoardEverythingPain.BombText.SetActive(false);
			TextBoardEverythingPain.MissedFruitText.SetActive(false); // Pls ignore
			TextBoardEverythingPain.StartingGame.SetActive(false);

			Destroyer.AddComponent<DestroyFruit>();
			slicerSword.AddComponent<KatanaSwing>();
		}

		void UnSetup()
        {
			Board.SetActive(false);

			meshSword.enabled = false;
			Destroyer.SetActive(false);
			FailCounter.SetActive(false);

			Destroyer.RemoveComponent<DestroyFruit>();
			slicerSword.RemoveComponent<SpawnFruits>();
			slicerSword.RemoveComponent<KatanaSwing>();
		}
	}
}
