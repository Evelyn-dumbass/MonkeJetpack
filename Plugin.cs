using BepInEx;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using Utilla;

namespace MonkeJetpack
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla")]
    [BepInPlugin("com.octoburr.gorillatag.monkejetpack", "MonkeJetpack", "1.0.2")]
    public class Plugin : BaseUnityPlugin
    {
        private GameObject jetpack;
        private float speedConfig;
        private bool inModded;
        private bool ModEnabled;

        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
            JetpackConfig.Initialize();
        }

        void OnEnable()
        {
            if (!ModEnabled)
                ModEnabled = true;
        }

        void OnDisable()
        {
            if (ModEnabled)
                ModEnabled = false;
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            AssetBundle bundle = LoadAssetBundle("MonkeJetpack.Resources.jetpack");
            GameObject asset = bundle.LoadAsset<GameObject>("jetpack");
            jetpack = Instantiate(asset);
            jetpack.SetActive(false);
            jetpack.transform.SetParent(GorillaTagger.Instance.bodyCollider.transform);
            jetpack.transform.localPosition = new Vector3(0f, 0.05f, -0.25f);
            jetpack.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            jetpack.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            speedConfig = JetpackConfig.Speed.Value;
        }

        void FixedUpdate()
        {
            // no need for == true or == false, use !inModded for false and inModded for true
            if (inModded)
            {
                if (ModEnabled)
                {
                    if (ControllerInputPoller.instance.rightControllerPrimaryButton)
                        GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * (speedConfig * 50), ForceMode.Acceleration);
                }
            }
        }

        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            jetpack.SetActive(true);
            inModded = true;
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            jetpack.SetActive(false);
            inModded = false;
        }

        public AssetBundle LoadAssetBundle(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;
        }
    }
}
