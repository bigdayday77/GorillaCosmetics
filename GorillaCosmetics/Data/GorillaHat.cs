﻿using GorillaCosmetics.Utils;
using System;
using UnityEngine;

namespace GorillaCosmetics.Data
{
    public class GorillaHat : IAsset
    {
        public string FileName { get; }
        public CosmeticDescriptor Descriptor { get; }

        AssetBundle assetBundle;
        GameObject assetTemplate;

		public GorillaHat(string path)
        {
            if (path != "Default")
            {
                try
                {
                    FileName = path;
                    var bundleAndJson = PackageUtils.AssetBundleAndJSONFromPackage(FileName);
                    assetBundle = bundleAndJson.bundle;
                    PackageJSON json = bundleAndJson.json;

                    // get material object and stuff
                    assetTemplate = assetBundle.LoadAsset<GameObject>("_Hat");
                    foreach (Collider collider in assetTemplate.GetComponentsInChildren<Collider>())
                    {
                        collider.enabled = false; // Disable colliders. They can be left in accidentally and cause some really weird issues.
                    }
                    assetTemplate.SetActive(false);

                    // Make Descriptor
                    Descriptor = PackageUtils.ConvertJsonToDescriptor(json);
                }
                catch (Exception err)
                {
                    // loading failed. that's not good.
                    Debug.Log(err);
                    throw new Exception($"Loading hat at {path} failed.");
                }
            } else
			{
                Descriptor = new CosmeticDescriptor();
                Descriptor.Name = "Default";
                assetTemplate = null;
			}
        }

        public GameObject GetAsset()
		{
            var gameObject = UnityEngine.Object.Instantiate(assetTemplate);
            gameObject.SetActive(true);
            return gameObject;
		}
    }
}
