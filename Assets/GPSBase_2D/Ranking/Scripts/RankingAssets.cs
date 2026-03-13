using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace Rubik.Ranking
{
    public class RankingAssets : MonoBehaviour
    {
        public List<FlagData> flagDatas = new List<FlagData>();
        public List<TopRankIcon> topRankIcons = new List<TopRankIcon>();
        Dictionary<NameFlagCountry, Sprite> dicFlagDatas = new Dictionary<NameFlagCountry, Sprite>();
        Dictionary<int, Sprite> dicTopRankIcon = new Dictionary<int, Sprite>();
        public TextAsset RankingAssetDatas;
        public List<RankingData> RankingDatas = new List<RankingData>();
        void Awake()
        {
            SetDicFlagData();
            SetDicTopRankIcon();
            LoadRankingAsset();
        }
        public void LoadRankingAsset()
        {
            foreach (JSONNode item in JSON.Parse(this.RankingAssetDatas.text))
            {
                RankingData rankingData = JsonUtility.FromJson<RankingData>(item.ToString());
                RankingDatas.Add(rankingData);
            }
        }
        public List<RankingData> GetRankingDatas()
        {
            List<RankingData> rankingDatas = new List<RankingData>();
            foreach (RankingData rankingData in RankingDatas)
            {
                if (!rankingData.IsUser)
                {
                    rankingDatas.Add(rankingData);
                }
            }
            rankingDatas.Sort((data1, data2) => data1.Stt.CompareTo(data2.Stt));
            return rankingDatas;
        }
        public RankingData GetUserRankingData()
        {
            foreach (RankingData rankingData in RankingDatas)
            {
                if (rankingData.IsUser)
                {
                    return rankingData;
                }
            }
            return null;
        }
        void SetDicFlagData()
        {
            foreach (FlagData flagData in flagDatas)
            {
                dicFlagDatas.Add(flagData.nameFlagCountry, flagData.iconFlag);
            }
        }
        public Sprite GetIconFlagCountry(NameFlagCountry nameFlagCountry)
        {

            if (dicFlagDatas.TryGetValue(nameFlagCountry, out Sprite iconFlag))
            {
                return iconFlag;
            }
            return null;
        }
        void SetDicTopRankIcon()
        {
            foreach (TopRankIcon topRankIcon in topRankIcons)
            {
                dicTopRankIcon.Add(topRankIcon.index, topRankIcon.iconTopRankIcon);
            }
        }
        public Sprite GetIconTopRankIcon(int index)
        {

            if (dicTopRankIcon.TryGetValue(index, out Sprite iconTopRankIcon))
            {
                return iconTopRankIcon;
            }
            return null;
        }
    }
    [Serializable]
    public class RankingData
    {
        public int Stt;
        public string UserName;
        public int IdFlag;
        public int Score;
        public bool IsUser;
    }
    [Serializable]
    public class TopRankIcon
    {
        public int index;
        public Sprite iconTopRankIcon;
    }
    [Serializable]
    public class FlagData
    {
        public NameFlagCountry nameFlagCountry;
        public Sprite iconFlag;
    }
    public enum NameFlagCountry
    {
        None = 0,
        VietNam = 1,
    }
}
