using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Rubik.Common
{
    public class UnityAssetLoader : Singleton<UnityAssetLoader>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void InitOnLoad()
        {
            Init();
        }
        
        public T Load<T>(string path) where T : Object
        {
            return Resources.Load<T>(path) as T;
        }
        
        public void LoadAsync<T>(string path, UnityAction<T> callback) where T : Object
        {
            StartCoroutine(LoadAssetAsync<T>(path, callback));
        }

        IEnumerator LoadAssetAsync<T>(string path, UnityAction<T> callback)  where T : Object
        {
            var resourceRequest = Resources.LoadAsync<T>(path);
            while (!resourceRequest.isDone)
            {
                yield return 0;
            }
            
            callback?.Invoke(resourceRequest.asset as T);
        }
        

    }
}
