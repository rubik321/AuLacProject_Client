using System;
using System.IO;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
using NAudio.Wave;
using NLayer.NAudioSupport;
#endif

namespace Rubik.Common.AudioHelper
{
    public static class AudioExtension 
    {
        public static float GetDuration(this AudioClip audioClip, float defaultValue = 0f)
		{
			try
			{
				return audioClip.length;
			}
			catch (Exception e)
			{
				return defaultValue;
			}
		}

		public static TweenerCore<float, float, FloatOptions> DOVolume(this AudioPlayer target, float endValue,
			float duration)
		{
			if (endValue < 0) endValue = 0;
			else if (endValue > 1) endValue = 1;
			TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.volume, x => target.volume = x,
				endValue, duration);
			t.SetTarget(target);
			return t;
		}

		public static TweenerCore<float, float, FloatOptions> DOFadeIn(this AudioPlayer target,
			float duration)
		{
			return target.DOVolume(1f, duration);
		}
		
		public static TweenerCore<float, float, FloatOptions> DOFadeOut(this AudioPlayer target,
			float duration)
		{
			return target.DOVolume(0f, duration);
		}


		
		public static AudioClip GetAudioClip(string fileName)
		{
			return GetClip(fileName, false);
		}

		private static AudioClip GetClip(string fileName, bool retry)
		{
			FileInfo fileInfo = new FileInfo(fileName);
			string clipName = fileInfo.FullName.Substring(Application.persistentDataPath.Length);
			if (fileInfo.Exists)
			{
				string _inPath_ = fileInfo.FullName;
				if (fileInfo.Extension == ".wav")
				{
					if (!_inPath_.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
						_inPath_ = "file://" + _inPath_;

					WWW audioLoaderWav = new WWW(_inPath_);
					while (!audioLoaderWav.isDone)
					{
						//DinoDebugger.LogGameBase("uploading wav");
					}

					AudioClip audioClipWav = audioLoaderWav.GetAudioClip(false, false, AudioType.WAV);
					audioClipWav.name = clipName;
					return audioClipWav;
				}

#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
			string wavPath = Application.persistentDataPath + "/wav/";
			if (!Directory.Exists(wavPath))
			{
				Directory.CreateDirectory(wavPath);
			}

			string _outPath_ = wavPath + fileInfo.Name + ".wav";
			if (!File.Exists(_outPath_))
			{
				if (retry) //(WIN ONLY)
				{
					//#1: Using Mp3FileReader
					using (Mp3FileReader mp3 = new Mp3FileReader(_inPath_))
					{
						using (WaveStream ws = WaveFormatConversionStream.CreatePcmStream(mp3))
						{
							try
							{
								WaveFileWriter.CreateWaveFile(_outPath_, ws);
							}
							catch (Exception e)
							{
								Debug.Log("MP3 - Retry convert to WAV using MP3FileReader file failed");
								return null;
							}
						}
					}

					//#2: Using the MediaFoundationReader class
					// using (MediaFoundationReader reader = new MediaFoundationReader(_inPath_))
					// {
					// 	WaveFileWriter.CreateWaveFile(_outPath_, reader);
					// }
				}
				else
				{
					using (Mp3FileReader reader = new Mp3FileReader(_inPath_, wf => new Mp3FrameDecompressor(wf)))
					{
						try
						{
							WaveFileWriter.CreateWaveFile(_outPath_, reader);
						}
						catch (Exception e)
						{
							File.Delete(_outPath_);
							Debug.Log("MP3 - Convert to WAV using NLayer failed");

							if (!retry)
							{
#if UNITY_STANDALONE_WIN
								return GetClip(fileName, true);
#endif
							}

							return null;
						}
					}
				}

			}

			if (!_outPath_.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
				_outPath_ = "file://" + _outPath_;

			WWW audioLoader = new WWW(_outPath_);
			while (!audioLoader.isDone)
			{
				//Debug.Log("Uploading wav");
			}

			AudioClip audioClip = audioLoader.GetAudioClip(false, false, AudioType.WAV);
			audioClip.name = clipName;
			return audioClip;
#else
				if (!_inPath_.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
					_inPath_ = "file://" + _inPath_;

				WWW audioLoader = new WWW(_inPath_);
				while (!audioLoader.isDone)
				{
					//DinoDebugger.LogGameBase("uploading mp3");
				}

				AudioClip audioClip = audioLoader.GetAudioClip(false, false, AudioType.MPEG);
				audioClip.name = clipName;
				return audioClip;
#endif
			}

			return null;
		}

		public static void ClearWavFiles()
		{
			string wavPath = Application.persistentDataPath + "/wav/";
			if (Directory.Exists(wavPath))
			{
				Directory.Delete(wavPath, true);
			}
		}
        
    }
}
