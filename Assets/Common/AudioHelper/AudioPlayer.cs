using System;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Rubik.Common.AudioHelper
{
	public class AudioPlayer : MonoBehaviour
	{
		public enum PlayingState
		{
			Play = 0,
			Paused,
			Stopped
		}

		public AudioManager.PlayingType type;
		public PlayingState state = PlayingState.Stopped;
        public AudioSource _audioSource;
		public AudioSource audioSource {
            get {
                if (_audioSource == null){
                    _audioSource = GetComponent<AudioSource>();
                }
                return _audioSource;
            }
        }

		public bool keepAudioClip = false;

		public float volume
		{
			get => audioSource.volume;
			set => SetVolume(value);
		}

		private float volumeScale = 1f;
		public float VolumeScale
		{
			get => volumeScale;
			set => volumeScale = value;
		}
		
		private Queue<AudioClip> queueClips = new Queue<AudioClip>();
		private float delayBetween = 0;
		private Sequence tween;


		public UnityEngine.Events.UnityAction<AudioPlayer> onAudioPlayerCompleted;
		public UnityEngine.Events.UnityAction<AudioPlayer> onAudioPlayerStepped;

		// Start is called before the first frame update
		void Start()
		{

		}

		public static AudioPlayer CreateAudioPlayer(AudioManager.PlayingType playingType, Transform parent = null)
		{
			GameObject playerObject = new GameObject(playingType.ToString(), typeof(AudioPlayer), typeof(AudioSource));
			playerObject.transform.SetParent(parent);
			var player = playerObject.GetComponent<AudioPlayer>();
			player.type = playingType;

			if (playingType == AudioManager.PlayingType.Music)
			{
				player.volumeScale = 0.6f;
			}
			else if (playingType == AudioManager.PlayingType.Effect ||
			         playingType == AudioManager.PlayingType.LoopedSound)
			{
				player.volumeScale = 0.6f;
			}
			else
			{
				player.volumeScale = 1f;
			}

			player.audioSource.loop = playingType == AudioManager.PlayingType.Music ||
			                          playingType == AudioManager.PlayingType.LoopedSound;

			return player;
		}

		public void SetVolume(float v)
		{
			audioSource.volume = v * volumeScale;
		}

		public float Play(string audioPath, float delay, bool clear = true)
		{
			return Play(AudioExtension.GetAudioClip(audioPath), delay, clear);
		}

		public float Play(AudioClip clip, float delay, bool clear = true, bool unload = true)
		{
			if (state == PlayingState.Play)
			{
				if (type == AudioManager.PlayingType.Music && audioSource.clip.Equals(clip))
				{
					return 0;
				}

				audioSource.Stop();
				tween?.Kill();
			}

			if (clear)
			{
				queueClips.Clear();
			}

			/*
			if (unload)
			{
				try
				{
					if (!audioSource.clip.UnloadAudioData())
					{
						Destroy(audioSource.clip);
					}
				}
				catch (Exception e)
				{
					Debug.Log("Fail to try unload audio clip data");
				}
			}
			*/

			audioSource.clip = clip;
			state = PlayingState.Play;

			if (delay <= 0)
			{
				audioSource.Play();
			}
			else
			{
				audioSource.PlayDelayed(delay);
			}

			if (type == AudioManager.PlayingType.Effect || type == AudioManager.PlayingType.Voice)
			{
				tween = DOTween.Sequence()
					.AppendInterval(delay + clip.GetDuration())
					.AppendCallback(OnCompleted)
					.Play();
			}

			return clip.GetDuration();
		}

		public void PlaySequence(params AudioClip[] clips)
		{
			PlaySequence(0, 0, clips);
		}

		public void PlaySequence(float delayed, float between, params AudioClip[] clips)
		{
			if (type == AudioManager.PlayingType.Music || type == AudioManager.PlayingType.LoopedSound)
			{
				return;
			}

			delayBetween = between;
			queueClips.Clear();
			foreach (var clip in clips)
			{
				queueClips.Enqueue(clip);
			}

			Play(queueClips.Dequeue(), delayed, false);
		}

		public void PlaySequence(AudioClip[] clips, float delayed, float between)
		{
			if (type == AudioManager.PlayingType.Music || type == AudioManager.PlayingType.LoopedSound)
			{
				return;
			}

			queueClips.Clear();
			foreach (var clip in clips)
			{
				queueClips.Enqueue(clip);
			}

			delayBetween = between;

			if (!queueClips.IsEmpty())
			{
				Play(queueClips.Dequeue(), delayed, false);
			}
		}

		private void OnCompleted()
		{
			if (queueClips.IsEmpty())
			{
				if (state != PlayingState.Stopped)
				{
					Stop();
					onAudioPlayerCompleted?.Invoke(this);
				}
			}
			else
			{
				Play(queueClips.Dequeue(), delayBetween / 2f, false);
				DOTween.Sequence()
					.AppendInterval(delayBetween / 2f)
					.AppendCallback(() => onAudioPlayerStepped?.Invoke(this))
					.Play();
			}
		}

		public void Pause()
		{
			if (state == PlayingState.Play)
			{
				audioSource.Pause();
				state = PlayingState.Paused;
				tween?.Pause();
			}
		}

		public void UnPause()
		{
			if (state == PlayingState.Paused)
			{
				audioSource.UnPause();
				state = PlayingState.Play;
				tween?.Play();
			}
		}

		public void Stop()
		{
			if (state == PlayingState.Stopped)
				return;

			queueClips.Clear();
			audioSource.Stop();

			/*
			if (!keepAudioClip)
			{
				try
				{
					if (!audioSource.clip.UnloadAudioData())
					{
						Destroy(audioSource.clip);
					}
				}
				catch (Exception e)
				{
					Debug.Log("Fail to try unload audio clip data");
				}
			}
			*/

			tween?.Kill();
			state = PlayingState.Stopped;
		}

		public float GetDuration(float d = 0f)
		{
			try
			{
				return audioSource.clip.GetDuration(d);
			}
			catch (Exception e)
			{
				return d;
			}
		}

		public void Replay()
		{
			if (audioSource.clip != null)
			{
				Play(audioSource.clip, 0, true, false);
			}
		}
	}
}