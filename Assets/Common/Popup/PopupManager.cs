using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Rubik.Common.Popup
{
    public class PopupManager : Singleton<PopupManager>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void InitOnLoad()
        {
            Init();
        }

        public class PopupItem
        {
            public string prefabName;
            public BasePopup instance;
        }

        public List<PopupItem> popupItems = new List<PopupItem>();

        private PopupItem GetPopup(string prefabName, bool oneTime)
        {
            var item = popupItems.Where(x => x.prefabName == prefabName).FirstOrDefault();
            if (item == null)
            {
                item = new PopupItem();
                item.prefabName = prefabName;
                item.instance = Instantiate(UnityAssetLoader.Manage.Load<GameObject>(prefabName), transform).GetComponent<BasePopup>();
                if (oneTime)
                {
                    item.instance.onCompleted = OnDestroyPopup;
                }
                else
                {
                    item.instance.onCompleted = OnHidePopup;
                    popupItems.Add(item);
                }
            }
            else
            {
                item.instance.onAccept.RemoveAllListeners();
                item.instance.onDecline.RemoveAllListeners();
            }

            item.instance.gameObject.SetActive(false);
            return item;
        }

        public BasePopup Show(string prefabName, bool oneTime = true)
        {
            var item = GetPopup(prefabName, oneTime);
            item.instance.gameObject.SetActive(true);
            return item.instance;
        }

        public BasePopup Show(string prefabName, UnityAction acceptAction, bool oneTime = true)
        {
            var item = GetPopup(prefabName, oneTime);
            item.instance.onAccept.AddListener(acceptAction);
            item.instance.gameObject.SetActive(true);
            return item.instance;
        }

        public BasePopup Show(string prefabName, UnityAction acceptAction, UnityAction declineAction,
            bool oneTime = true)
        {
            var item = GetPopup(prefabName, oneTime);
            item.instance.onAccept.AddListener(acceptAction);
            item.instance.onDecline.AddListener(declineAction);
            item.instance.gameObject.SetActive(true);
            return item.instance;
        }

        public BasePopup ShowWithEvent(string prefabName, UnityEvent acceptAction, UnityEvent declineAction,
            bool oneTime = true)
        {
            var item = GetPopup(prefabName, oneTime);
            item.instance.onAccept = acceptAction;
            item.instance.onDecline = declineAction;
            item.instance.gameObject.SetActive(true);
            return item.instance;
        }

        private void OnHidePopup(BasePopup popup)
        {
            popup.gameObject.SetActive(false);
        }

        private void OnDestroyPopup(BasePopup popup)
        {
            Destroy(popup.gameObject);
        }
    }
}
