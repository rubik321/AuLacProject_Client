using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Rubik.Common.AudioHelper
{
    public class AudioManager : Singleton<AudioManager>
    {
        public enum PlayingType
        {
            Music = 0,
            Effect,
            LoopedSound,
            Voice
        };
    
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void InitOnLoad()
        {
            Init();
        }


        [SerializeField] private List<AudioPlayer> audioPlayers = new List<AudioPlayer>();

        private AudioPlayer GetAudioPlayer(AudioManager.PlayingType type)
        {
            foreach (var player in audioPlayers.Where(x=>x.type == type))
            {
                if (type == PlayingType.Music)
                {
                    return player;
                }
                else
                {
                    if (player.state == AudioPlayer.PlayingState.Stopped)
                    {
                        return player;
                    }
                }
            }

            var audioPlayer = AudioPlayer.CreateAudioPlayer(type, transform);
            audioPlayers.Add(audioPlayer);

            return audioPlayer;
        }


        #region Play

        /// <summary>
        /// Play audio use audio clip
        /// </summary>
        public AudioPlayer PlayAudio(AudioClip clip, AudioManager.PlayingType type, float volume = 1f, float delay = 0f)
        {
            var player = GetAudioPlayer(type);

            if (type == PlayingType.Effect || type == PlayingType.LoopedSound)
            {
                player.SetVolume(volume);
            }

            if (type == PlayingType.Music)
            {
                player.SetVolume(volume);
            }

            if (clip != null)
            {
                player.Play(clip, delay);
            }
        
            return player;
        }
    
    
        /// <summary>
        /// Play audio form audio file path
        /// </summary>
        public AudioPlayer PlayAudio(string file, AudioManager.PlayingType type, float volume = 1f, float delay = 0f)
        {
            return PlayAudio(AudioExtension.GetAudioClip(file), type, volume, delay);
        }
    
    
        /// <summary>
        /// Play music from audio file path
        /// </summary>
        public AudioPlayer PlayMusic(string filePath, float volume = 0.5f, float delay=0f)
        {
            return PlayAudio(filePath, AudioManager.PlayingType.Music, volume, delay);
        }
    
        public AudioPlayer PlayMusic(AudioClip clip, float volume = 0.25f, float delay=0f)
        {
            Debug.LogWarning("PlayMusic");
            return PlayAudio(clip, AudioManager.PlayingType.Music, volume, delay);
        }

    
        /// <summary>
        /// Play loop sound from audio file path
        /// </summary>
    
        public AudioPlayer PlayLoppedSound(string filePath, float volume = .25f, float delay=0f)
        {
            return PlayAudio(filePath, AudioManager.PlayingType.LoopedSound, volume, delay);
        }
    
        public AudioPlayer PlayLoppedSound(AudioClip clip, float volume = .25f, float delay=0f)
        {
            return PlayAudio(clip, AudioManager.PlayingType.LoopedSound, volume, delay);
        }

        /// <summary>
        /// Play effect from audio file path
        /// </summary>
        public AudioPlayer PlayEffect(string filePath, float volume = .8f, float delay=0f)
        {
            return PlayAudio(filePath, AudioManager.PlayingType.Effect, volume, delay);
        }
    
        public AudioPlayer PlayEffect(AudioClip clip, float volume = .8f, float delay=0f)
        {
            return PlayAudio(clip, AudioManager.PlayingType.Effect, volume, delay);
        }

    
        /// <summary>
        /// Play voice from audio file path
        /// </summary>
        public AudioPlayer PlayVoice(string filePath, float volume = 1f, float delay=0f)
        {
            return PlayAudio(filePath, AudioManager.PlayingType.Voice,  volume, delay);
        }

        public AudioPlayer PlayVoice(AudioClip clip, float volume = 1f, float delay = 0f)
        {
            return PlayAudio(clip, AudioManager.PlayingType.Voice,  volume, delay);
        }

    
        /// <summary>
        /// Play sequence voices
        /// </summary>

        public AudioPlayer PlayVoices(params string[] vs)
        {
            return PlaySequenceVoices(vs);
        }
    
        public AudioPlayer PlayVoices(float volume, params string[] vs)
        {
            return PlaySequenceVoices(vs, volume);
        }
    
        public AudioPlayer PlayVoices(float volume, float between, params string[] vs)
        {
            return PlaySequenceVoices(vs, volume, between);
        }
    
        public AudioPlayer PlayVoices(float volume, float between, float delay, params string[] vs)
        {
            return PlaySequenceVoices(vs, volume, between, delay);
        }
    
        public AudioPlayer PlayVoices(params AudioClip[] vs)
        {
            return PlaySequenceVoices(vs);
        }
    
        public AudioPlayer PlayVoices(float volume, params AudioClip[] vs)
        {
            return PlaySequenceVoices(vs, volume);
        }
    
        public AudioPlayer PlayVoices(float volume, float between, params AudioClip[] vs)
        {
            return PlaySequenceVoices(vs, volume, between);
        }
    
        public AudioPlayer PlayVoices(float volume, float between, float delay, params AudioClip[] vs)
        {
            return PlaySequenceVoices(vs, volume, between, delay);
        }
    
        public AudioPlayer PlaySequenceVoices(AudioClip[] clips, float volume = 1f, float between = 0f, float delay = 0f)
        {
            var player = GetAudioPlayer(AudioManager.PlayingType.Voice);
            player.SetVolume(volume);
            player.PlaySequence(clips, delay, between);
            return player;
        }
    
        public AudioPlayer PlaySequenceVoices(string[] vs, float volume = 1f, float between = 0f, float delay = 0f)
        {
            var player = GetAudioPlayer(AudioManager.PlayingType.Voice);
            player.SetVolume(volume);

            var audioClips = new List<AudioClip>();
            foreach (var file in vs)
            {
                audioClips.Add(AudioExtension.GetAudioClip(file));
            }
        
            player.PlaySequence(audioClips.ToArray(), delay, between);
            return player;
        }
    
    
        public AudioPlayer PlayEffects(params string[] vs)
        {
            return PlaySequenceEffects(vs);
        }
    
        public AudioPlayer PlayEffects(float volume, params string[] vs)
        {
            return PlaySequenceEffects(vs, volume);
        }
    
        public AudioPlayer PlayEffects(float volume, float between, params string[] vs)
        {
            return PlaySequenceEffects(vs, volume, between);
        }
    
        public AudioPlayer PlayEffects(float volume, float between, float delay, params string[] vs)
        {
            return PlaySequenceEffects(vs, volume, between, delay);
        }
    
        public AudioPlayer PlaySequenceEffects(string[] vs, float volume = 1f, float between = 0f, float delay = 0f)
        {
            var player = GetAudioPlayer(AudioManager.PlayingType.Effect);
            player.SetVolume(volume);

            var audioClips = new List<AudioClip>();
            foreach (var file in vs)
            {
                audioClips.Add(AudioExtension.GetAudioClip(file));
            }
        
            player.PlaySequence(audioClips.ToArray(), delay, between);
            return player;
        }
    
        public AudioPlayer PlayEffects(params AudioClip[] vs)
        {
            return PlaySequenceEffects(vs);
        }
    
        public AudioPlayer PlayEffects(float volume, params AudioClip[] vs)
        {
            return PlaySequenceEffects(vs, volume);
        }
    
        public AudioPlayer PlayEffects(float volume, float between, params AudioClip[] vs)
        {
            return PlaySequenceEffects(vs, volume, between);
        }
    
        public AudioPlayer PlayEffects(float volume, float between, float delay, params AudioClip[] vs)
        {
            return PlaySequenceEffects(vs, volume, between, delay);
        }
    
        public AudioPlayer PlaySequenceEffects(AudioClip[] vs, float volume = 1f, float between = 0f, float delay = 0f)
        {
            var player = GetAudioPlayer(AudioManager.PlayingType.Effect);
            player.SetVolume(volume);
            player.PlaySequence(vs.ToArray(), delay, between);
            return player;
        }

        public void SetVolume(PlayingType type, float volume){
            foreach (AudioPlayer player in audioPlayers.Where(x=>x.type == type))
            {
                player.SetVolume(volume);
            }
        }

        #endregion __Play__

        #region Paused
        public void PauseAll()
        {
            foreach (AudioPlayer player in audioPlayers)
            {
                player.Pause();
            }
        }

        public void UnPauseAll()
        {
            foreach (AudioPlayer player in audioPlayers)
            {
                player.UnPause();
            }
        }

        public void Pause(AudioManager.PlayingType type)
        {
            foreach (AudioPlayer player in audioPlayers.Where(x=>x.type == type))
            {
                player.Pause();
            }
        }

        public void UnPause(AudioManager.PlayingType type)
        {
            foreach (AudioPlayer player in audioPlayers.Where(x=>x.type == type))
            {
                player.UnPause();
            }
        }

        #endregion __Paused__

        #region Stop
        public void StopAll(bool clear = false)
        {
            foreach (var player in audioPlayers)
            {
                player.Stop();
            }

            if (clear)
            {
                Clear();
            }
        }

        public void Clear()
        {
            AudioExtension.ClearWavFiles();
            foreach (var player in audioPlayers.Where(x=>x.type!=PlayingType.Music))
            {
                player.Stop();
                Destroy(player.gameObject);
            }

            var musicPlayer = audioPlayers.Where(x => x.type == PlayingType.Music).FirstOrDefault();
            audioPlayers.Clear();
            if (musicPlayer != null)
            {
                audioPlayers.Add(musicPlayer);
            }
        }

        public void Stop(AudioManager.PlayingType type)
        {
            try
            {
                foreach (AudioPlayer player in audioPlayers.Where(x=>x.type == type))
                {
                    player.Stop();
                }
            }
            catch (Exception e)
            {
                Debug.Log("no player stop: " + type);
            }
            
        }

        #endregion __Stop__

    
        private AudioPlayer musicPlayer;
        private string currentMusic = string.Empty;

        public void PlayGameMusic(AudioClip audioClip, float volume = 0.2f)
        {
            StopBackgroundMusic();
            
            currentMusic = "";
            if (musicPlayer == null)
            {
                musicPlayer = PlayMusic(audioClip, volume);
            }
            else
            {
                musicPlayer.Stop();
                musicPlayer.Play(audioClip, 0);
            }
        }

        public void PlayBackgroundMusic(string filename, bool replay = true, float volume = 0.2f)
        {
            StopBackgroundMusic();
            
            if (filename.Equals(currentMusic))
            {
                if (replay)
                {
                    musicPlayer.Replay();
                }
                else
                {
                    musicPlayer.UnPause();
                }
                return;
            }
            
            var musicClip = UnityAssetLoader.Manage.Load<AudioClip>("music/" + filename);
            if (musicClip != null)
            {
                currentMusic = filename;
                if (musicPlayer == null)
                {
                    musicPlayer = PlayMusic(musicClip, volume);
                }
                else
                {
                    musicPlayer.Stop();
                    musicPlayer.Play(musicClip, 0);
                }
            }
        }
        
        public void StopBackgroundMusic()
        {
            if (musicPlayer != null)
            {
                musicPlayer.Pause();
            }
        }
    }
}