using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rubik.Combat;
using System.Linq;
using Spine.Unity;
using Spine;
using Rubik._2DGPS.UserData;
using Rubik.CharacterGear;
using System.Linq;
using NTPackage.Functions;

namespace Rubik.Combat
{
    public class AssetLoader : Pixelplacement.Singleton<AssetLoader>
    {
        public GameAssetCollection[] assets;
        Dictionary<string, GameObject> assetCollection = new Dictionary<string, GameObject>();
       
        public GearDataSO[] lsGears;
        public int TutIndex = -1;
        public bool IsTut = false;
        private void Awake()
        {
            foreach (GameAssetCollection collection in assets)
            {
                int index = 0;
                foreach (GameObject prefab in collection.prefabs)
                {
                    if (!assetCollection.TryAdd(prefab.name, prefab))
                    {
                        Debug.LogErrorFormat("Duplicate {0} at index {1}", collection.name, index);
                    }
                    index++;
                }
            }
           //for(int i = 0;i< assets[3].prefabs.Length; i++)
           // {
           //     Debug.Log(assets[3].prefabs[i].name);
           // }
        }
       

        public void MixSkin(SkeletonAnimation skeletonAnimation)
        {
            var skeleton = skeletonAnimation.Skeleton;
            var skeletonData = skeleton.Data;
            var mixAndMatchSkin = new Skin("Skin 1");
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(Rubik.UserDataPlayer.UserDataManager.Instance.SkinData.Weapon));
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(Rubik.UserDataPlayer.UserDataManager.Instance.SkinData.Eye));
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(Rubik.UserDataPlayer.UserDataManager.Instance.SkinData.Hair));
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(Rubik.UserDataPlayer.UserDataManager.Instance.SkinData.Scar));
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(Rubik.UserDataPlayer.UserDataManager.Instance.SkinData.Suit));
            skeleton.SetSkin(mixAndMatchSkin);
            skeleton.SetSlotsToSetupPose();
            skeletonAnimation.AnimationState.Apply(skeleton);
        }
        public void MixSkinWithSkinData(SkeletonAnimation skeletonAnimation,SkinData data)
        {
            var skeleton = skeletonAnimation.Skeleton;
            var skeletonData = skeleton.Data;
            var mixAndMatchSkin = new Skin("Skin 1");
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(data.Weapon));
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(data.Eye));
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(data.Hair));
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(data.Scar));
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(data.Suit));
            skeleton.SetSkin(mixAndMatchSkin);
            skeleton.SetSlotsToSetupPose();
            skeletonAnimation.AnimationState.Apply(skeleton);
        }
        public void MixSkinWithGears(SkeletonAnimation skeletonAnimation, int[] gears)
        {
            var skinData = GetSkinDataByGears(gears);
            Debug.Log("Cloth : " + JsonUtility.ToJson(skinData));
            var skeleton = skeletonAnimation.Skeleton;
            var skeletonData = skeleton.Data;

            var mixAndMatchSkin = new Skin("Skin 1");
            Debug.Log("Cloth : " + skinData.Weapon);
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(skinData.Weapon));
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(skinData.Eye));
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(skinData.Hair));
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(skinData.Scar));
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(skinData.Suit));
            skeleton.SetSkin(mixAndMatchSkin);
            skeleton.SetSlotsToSetupPose();
            skeletonAnimation.AnimationState.Apply(skeleton);
        }
        public void MixSkinWithGearsUI(SkeletonGraphic skeletonAnimation, int[] gears)
        {
           
            var skinData = GetSkinDataByGears(gears);
            Debug.Log("Cloth : " + JsonUtility.ToJson(skinData));
            var skeleton = skeletonAnimation.Skeleton;
            var skeletonData = skeleton.Data;

            var mixAndMatchSkin = new Skin("Skin 1");
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(skinData.Weapon));
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(skinData.Eye));
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(skinData.Hair));
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(skinData.Scar));
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(skinData.Suit));
            skeleton.SetSkin(mixAndMatchSkin);
            skeleton.SetSlotsToSetupPose();
            skeletonAnimation.AnimationState.Apply(skeleton);
        }
        public void MixSkinWithGears(SkeletonGraphic skeletonAnimation, string[] gearsID)
        {
            List<int> gears = new List<int>();
            foreach(string id in gearsID)
            {
                gears.Add((int)CharacterGearManager.Instance.GetCharacterGearByID(id).Index);
            }
            MixSkinWithGearsUI(skeletonAnimation, gears.ToArray());
        }
        public SkinData GetSkinDataByGears(int[] gears)
        {
            SkinData skinData = new SkinData();
            foreach(int idGear in gears)
            {
               // Rubik.CharacterGear.CharacterGear characterGear = CharacterGearManager.Instance.
                string cloth = Rubik.SpineManager.SpineController.Instance.GetCharacterGearSpineName(idGear);
                var temp = CharacterGearManager.Instance.GetGearDataByIndex((CharacterGearIndex)idGear);
                NTLog.LogMessage("Cloth id : " + cloth + "id gear : "+idGear);
                if(temp == null) continue;
                switch (temp.Type)
                {
                    case CharacterGearType.Hair:
                        skinData.Hair = cloth;
                        break;
                    case CharacterGearType.Weapon:
                        skinData.Weapon = cloth;
                        break;
                    case CharacterGearType.Armor:
                        skinData.Suit = cloth;
                        break;
                    default:
                        break;
                }
            }
           
            return skinData;
        }
        public void MixSkin(SkeletonGraphic skeletonAnimation)
        {
            var skeleton = skeletonAnimation.Skeleton;
            var skeletonData = skeleton.Data;
            var mixAndMatchSkin = new Skin("Skin 1");
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(Rubik.UserDataPlayer.UserDataManager.Instance.SkinData.Weapon));
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(Rubik.UserDataPlayer.UserDataManager.Instance.SkinData.Eye));
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(Rubik.UserDataPlayer.UserDataManager.Instance.SkinData.Hair));
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(Rubik.UserDataPlayer.UserDataManager.Instance.SkinData.Scar));
            mixAndMatchSkin.AddSkin(skeletonData.FindSkin(Rubik.UserDataPlayer.UserDataManager.Instance.SkinData.Suit));
            skeleton.SetSkin(mixAndMatchSkin);
            skeleton.SetSlotsToSetupPose();
            skeletonAnimation.AnimationState.Apply(skeleton);
        }
        public GameObject GetAsset(string id)
        {
            Debug.Log(id);
            if (assetCollection.TryGetValue(id, out GameObject obj))
            {
                return obj;
            }
            return null;
        }

        public GameObject[] GetAllMob() //Test
        {
            return assets.First(e => e.name == "Mob").prefabs;
        }

        public GameObject[] GetAllCompanion()
        {
            return assets.First(e => e.name == "Companion").prefabs;
        }

        public WeaponType GetWeaponType(string id)
        {
            GameObject asset = GetAsset(id);
            if (asset == null)
                return WeaponType.None;
            return asset.GetComponent<GearObject>().weaponType;
        }

        [Serializable]
        public class GameAssetCollection
        {
            public string name;
            public GameObject[] prefabs;
        }
       
    }


}

