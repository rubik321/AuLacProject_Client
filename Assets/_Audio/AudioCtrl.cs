using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTPackage.Functions;

namespace Rubik.Common.AudioHelper
{
    public class AudioName
    {
        public const string Test = "BGM_Splash";

        public const string BGM_Splash = "BGM_Game";
        public const string BGM_Login = "BGM_Game";
        public const string BGM_WorldMap = "BGM_Game";
        public const string BGM_Battle_1 = "BGM_Battle_1";


        public const string UI_Popup_Panel_Open_Default = "UI_Popup_Panel_Default";
        public const string UI_Popup_Panel_Close_Default = "UI_Popup_Panel_Default";
        public const string UI_Popup_Panel_Error = "UI_Popup_Panel_Error";
        public const string UI_Popup_Panel_Daily_Reward = "UI_Popup_Panel_Daily_Reward";

        public const string UI_Button_Default = "UI_Button_Default";
        public const string UI_Button_Back_Exit = "UI_Button_Back_Exit";
        public const string UI_Button_Confirm = "UI_Button_Confirm";
        public const string UI_Checkbox = "UI_Checkbox";
        public const string UI_Button_Equip = "UI_Button_Equip";
        public const string UI_Button_Unequip = "UI_Button_Unequip";
        public const string UI_Button_Worldmap = "UI_Button_Worldmap";
        public const string UI_Button_Worldmap_2 = "UI_Button_Worldmap_2";

        public const string UI_Button_Summon_1 = "UI_Button_Summon_1";
        public const string UI_Button_Summon_2 = "UI_Button_Summon_2";
        public const string UI_Button_Battle_Start = "UI_Button_Battle_Start";
        public const string UI_Button_Battle_Defeat = "UI_Button_Battle_Defeat";
        public const string UI_Button_Battle_Victory = "UI_Button_Battle_Victory";

        public const string P_Atk_Sword = "P_Atk_Sword";
        public const string P_Atk_Staff = "P_Atk_Staff";
        public const string P_Atk_Bow = "P_Atk_Bow";
        public const string P_Atk_Gun = "P_Atk_Gun";
        public const string P_Atk_Mace = "P_Atk_Mace";
        public const string P_Atk_Default = "P_Atk_Default";

        public const string Heal_Sound = "Heal_Sound";
        public const string Normal_Hit_Sound = "Normal_Hit_Sound";
        public const string Normal_Crit_Sound = "Normal_Crit_Sound";
        public const string Bleed_Sound = "Bleed_Sound";
        public const string Burn_Sound = "Burn_Sound";
        public const string Block_Sound = "Block_Sound";
        public const string Poison_Sound = "Poison_Sound";

        public const string Claim_Sound = "Claim_Sound";
        public const string Collect_Energy_Sound = "Collect_Energy_Sound";
        public const string Collect_Gem_Sound = "Collect_Gem_Sound";
        public const string Collect_Gold_Sound = "Collect_Gold_Sound";
        public const string Collect_Sound = "Collect_Sound";
        public const string Achievement_Sound = "Achievement_Sound";
    }
    public class AudioConfig

    {
        public const float VolumeMusic = 1f;
        public const float VolumeEffect = 1;

        public const string KeyMusic = "Music";
        public const string KeyEffect = "Effect";
    }

    public class AudioCtrl : NTBehaviour
    {

        public static AudioCtrl Instance;
        protected override void Awake()
        {
            base.Awake();
            if (AudioCtrl.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            AudioCtrl.Instance = this;
        }

        public List<AudioPlayer> audioPlayers;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadSound();
        }

        protected void LoadSound()
        {
            this.audioPlayers.Clear();
            AudioPlayer[] audios = transform.GetComponentsInChildren<AudioPlayer>();
            foreach (AudioPlayer item in audios)
            {
                this.audioPlayers.Add(item);
            }
        }

        [ContextMenu("Play")]
        public void Play(string name)
        {
            AudioPlayer audioPlayer = this.GetAudioPlayerByName(name);
            if (audioPlayer == null)
            {
                NTLog.LogError("AudioPlayer not found: " + name, gameObject);
                return;
            }
            try
            {
                if (audioPlayer.type == AudioManager.PlayingType.Music)
                    AudioManager.Manage.PlayAudio(audioPlayer.audioSource.clip, audioPlayer.type, this.IsMuteMusic() ? 0 : AudioConfig.VolumeMusic);
                if (audioPlayer.type == AudioManager.PlayingType.Effect && !this.IsMuteEffect())
                    AudioManager.Manage.PlayAudio(audioPlayer.audioSource.clip, audioPlayer.type, this.IsMuteEffect() ? 0 : AudioConfig.VolumeEffect);
                NTLog.LogMessage("Play: " + audioPlayer.name);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString(), gameObject);
            }
        }

        public AudioPlayer GetAudioPlayerByName(string name)
        {
            return this.audioPlayers.Find((ap) => (ap.name.Equals(name)));
        }


        public void ChangeStateMusic()
        {
            PlayerPrefs.SetInt(AudioConfig.KeyMusic, this.IsMuteMusic() ? 1 : 0);
            if (this.IsMuteMusic())
            {
                AudioManager.Manage.Pause(AudioManager.PlayingType.Music);
            }
            else
            {
                AudioManager.Manage.UnPause(AudioManager.PlayingType.Music);
                AudioManager.Manage.SetVolume(AudioManager.PlayingType.Music, AudioConfig.VolumeMusic);
            }
        }

        public void ChangeStateEffect()
        {
            PlayerPrefs.SetInt(AudioConfig.KeyEffect, this.IsMuteEffect() ? 1 : 0);
        }

        #region Function

        //Change Sceen
        public void PlaySplashSound()
        {
            this.Play(AudioName.BGM_Splash);
        }

        public void PlayLoginSound()
        {
            this.Play(AudioName.BGM_Login);
        }

        public void PlayWorldMapSound()
        {
            this.Play(AudioName.BGM_WorldMap);
        }

        public void PlayBattleSound()
        {
            this.Play(AudioName.BGM_Battle_1);
        }

        #endregion

        #region Getter 
        public bool IsMuteMusic()
        {
            return PlayerPrefs.GetInt(AudioConfig.KeyMusic, 1) == 0;
        }

        public bool IsMuteEffect()
        {
            return PlayerPrefs.GetInt(AudioConfig.KeyEffect, 1) == 0;
        }
        #endregion 
    }
}

