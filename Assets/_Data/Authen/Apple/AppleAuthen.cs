using UnityEngine;
#if UNITY_IOS
using Apple.GameKit;
#endif
using System;
using NTPackage.Functions;

namespace Rubik.AppleAuthen
{
    public class AppleAuthen : NTBehaviour
    {

        public static AppleAuthen Instance;
        protected override void Awake()
        {
            base.Awake();
            if (AppleAuthen.Instance != null)
            {
                NTLog.LogWarning("Only 1 instance allow");
                return;
            }
            AppleAuthen.Instance = this;
        }

        public void LoginGameCenter(Action<string, string, bool> done = null)
        {
#if UNITY_IOS
            try
            {
                GKLocalPlayer.Authenticate().ContinueWith((task) =>
                {
                    if (task.IsCanceled)
                    {
                        NTLog.LogError("Game Center login canceled");
                        done?.Invoke(null, null, false);
                        done = null;
                        return;
                    }
                    if (task.Result == null)
                    {
                        NTLog.LogError("Game Center login failed: " + task.Exception.Message);
                        done?.Invoke(null, null, false);
                        done = null;
                        return;
                    }
                    // Grab the display name.
                    var localPlayer = GKLocalPlayer.Local;
                    NTLog.LogMessage("Local Player: " + localPlayer.DisplayName);
                    var fetchItemsResponse = GKLocalPlayer.Local.FetchItems().ContinueWith((task) =>
                    {
                        if (task.IsCanceled)
                        {
                            NTLog.LogError("Game Center fetch items canceled");
                            done?.Invoke(null, null, false);
                            done = null;
                            return;
                        }
                        if (task.Result == null)
                        {
                            NTLog.LogError("Game Center fetch items failed: " + task.Exception.Message);
                            done?.Invoke(null, null, false);
                            done = null;
                            return;
                        }
                        NTLog.LogMessage("Game Center fetch items successful");
                        NTLog.LogMessage("Game Center fetch items response: " + task.Result.ToString());
                    });
                    done?.Invoke(null, null, false);
                    done = null;
                });
                done?.Invoke(null, null, false);
                done = null;
            }
            catch (System.Exception e)
            {
                NTLog.LogError("Game Center login failed: " + e.Message);
                done?.Invoke(null, null, false);
                return;
            }
#else
            done?.Invoke(null, null, false);
#endif
        }
    }
}