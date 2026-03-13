using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine;
using NTPackage.Functions;
namespace Rubik.UnityServicesAuthen
{
    public class UnityServicesAuthen : MonoBehaviour
    {
        async void Awake()
        {
            await UnityServices.InitializeAsync();
            NTLog.LogMessage("Unity Services initialized: " + UnityServices.State);

            // Optional: anonymous sign-in for fallback
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            NTLog.LogMessage("PlayerID: " + AuthenticationService.Instance.PlayerId);
        }
    }
}