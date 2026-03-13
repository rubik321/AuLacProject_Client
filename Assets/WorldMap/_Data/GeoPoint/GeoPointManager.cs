using System;
using System.Collections;
using System.Collections.Generic;
using GOA.LandEvent;
using GOA.WorldMap;
using GOA.WorldMap.PortalGlobal;
using GoShared;
using NTFunctions_old;
using Rubik.Chat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GOA.WorldMap_old
{

    public class GeoPointManager : LoadBehaviour
    {
        public Coordinates GPSLocation;
        [SerializeField]
        public Coordinates Location;
        public bool IsTeleport = false;
        public GlobalPortalData GlobalPortalData = null;
        public bool IsInitGlobalPortal = false;

        public bool IsOwnLand = false;
        public Coordinates CdTele;

        public GOA.LandEvent.EventType EventType = LandEvent.EventType.None;

        public static GeoPointManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (GeoPointManager.Instance != null)
            {
                Debug.LogWarning("Only 1 instance allow");
                return;
            }
            GeoPointManager.Instance = this;
        }

        protected override void Start()
        {
            base.Start();
            var pos = this.GetDefaulLocation();
            this.GPSLocation = new Coordinates(pos.Item1 - UnityEngine.Random.Range(-0.01f, 0.01f) , pos.Item2 - UnityEngine.Random.Range(-0.01f, 0.01f));
        }

        public int count = 0;
        [Button]
        public void Test(){
            Coordinates coordinates = new Coordinates(DefLocation[count%DefLocation.Length].Item1,DefLocation[count%DefLocation.Length].Item2);
            TeleportSystem.instance.Teleport(coordinates);
            count++;
        }

        public (double, double) GetDefaulLocation()
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Afrikaans:
                    return DefLocation[0];
                case SystemLanguage.Arabic:
                    return DefLocation[1];
                case SystemLanguage.Basque:
                    return DefLocation[2];
                case SystemLanguage.Belarusian:
                    return DefLocation[3];
                case SystemLanguage.Bulgarian:
                    return DefLocation[4];
                case SystemLanguage.Catalan:
                    return DefLocation[5];
                case SystemLanguage.Chinese:
                    return DefLocation[6];
                case SystemLanguage.Czech:
                    return DefLocation[7];
                case SystemLanguage.Danish:
                    return DefLocation[8];
                case SystemLanguage.Dutch:
                    return DefLocation[9];
                case SystemLanguage.English:
                    return DefLocation[10];
                case SystemLanguage.Estonian:
                    return DefLocation[11];
                case SystemLanguage.Faroese:
                    return DefLocation[12];
                case SystemLanguage.Finnish:
                    return DefLocation[13];
                case SystemLanguage.French:
                    return DefLocation[14];
                case SystemLanguage.German:
                    return DefLocation[15];
                case SystemLanguage.Greek:
                    return DefLocation[16];
                case SystemLanguage.Hebrew:
                    return DefLocation[17];
                case SystemLanguage.Icelandic:
                    return DefLocation[18];
                case SystemLanguage.Indonesian:
                    return DefLocation[19];
                case SystemLanguage.Italian:
                    return DefLocation[20];
                case SystemLanguage.Japanese:
                    return DefLocation[21];
                case SystemLanguage.Korean:
                    return DefLocation[22];
                case SystemLanguage.Latvian:
                    return DefLocation[23];
                case SystemLanguage.Lithuanian:
                    return DefLocation[24];
                case SystemLanguage.Norwegian:
                    return DefLocation[25];
                case SystemLanguage.Polish:
                    return DefLocation[26];
                case SystemLanguage.Portuguese:
                    return DefLocation[27];
                case SystemLanguage.Romanian:
                    return DefLocation[28];
                case SystemLanguage.Russian:
                    return DefLocation[29];
                case SystemLanguage.SerboCroatian:
                    return DefLocation[30];
                case SystemLanguage.Slovak:
                    return DefLocation[31];
                case SystemLanguage.Slovenian:
                    return DefLocation[32];
                case SystemLanguage.Spanish:
                    return DefLocation[33];
                case SystemLanguage.Swedish:
                    return DefLocation[34];
                case SystemLanguage.Thai:
                    return DefLocation[35];
                case SystemLanguage.Turkish:
                    return DefLocation[36];
                case SystemLanguage.Ukrainian:
                    return DefLocation[37];
                case SystemLanguage.Vietnamese:
                    return DefLocation[38];
                case SystemLanguage.ChineseSimplified:
                    return DefLocation[39];
                case SystemLanguage.ChineseTraditional:
                    return DefLocation[40];
                case SystemLanguage.Hungarian:
                    return DefLocation[41];
                default:
                    return DefLocation[UnityEngine.Random.Range(0, DefLocation.Length - 1)];
            }

        }

        public static (double, double)[] DefLocation = {
            (-28.736556654350593, 26.540986495274204),
            (25.648817319616192, 46.87856789160385),
            (40.47467414962355, -3.527076181164802),
            (54.119199288883145, 27.42658685788585),
            (42.81607506516459, 23.277732386831715),
            (25.28451510372681, 51.519246002958134),
            (40.69044122762942, 116.46105020244245),
            (50.084979468470316, 14.418542951300967),
            (55.361826618145344, 10.431830366072315),
            (48.52444503939871, 7.566097654760675),
            (1.281000156085257, 103.84878685787254),
            (59.43457543593684, 24.75152769622648),
            (57.86155906821195, 19.054398023029187),
            (60.23397432649071, 24.868910342254328),
            (49.03094553767432, 2.2312709491049465),
            (52.671363430562884, 13.26346440260576),
            (38.067839872511364, 23.782263926429806),
            (31.533226549697282, 35.100255104953085),
            (64.14755743643552, -21.925065816359655),
            (-5.928853309700463, 106.94077308941793),
            (43.603876037524564, 11.319035514274525),
            (35.27638181886964, 139.905401683335),
            (37.532600, 127.024612),
            (57.014218444263854, 24.01567119321808),
            (55.08307067931329, 23.944068139933574),
            (60.16872161734626, 10.91433425214467),
            (59.5192163008094, 17.37195780367309),
            (10.602241263265297, -66.78319433076227),
            (44.58163695407153, 25.870815323500647),
            (56.209589802757904, 37.37395270079909),
            (45.903630218069715, 16.07435263138128),
            (48.12429680335354, 17.88498505408838),
            (46.09022174190002, 14.464508401406135),
            (40.56518296817606, -3.470270326361324),
            (59.53793761731861, 17.869523300574503),
            (14.086001599706197, 100.48533095582336),
            (40.236869637765665, 33.0287166477628),
            (51.2693578925298, 30.871566682518363),
            (21.09536896180531, 105.89600190712919),
            (40.7204673998659, 116.87053621311769),
            (21.757588612194933, 115.31849115629299),
            (28.364126975598555, 76.5944140703595),
            (47.560402478166225, 18.900949993166744),
        };
    }


}

