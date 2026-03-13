using System.Collections;
using System.Collections.Generic;
using TMPro;
using NTPackage.EventDispatcher;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.UI;
using UnityEngine;
using Rubik.UserDataPlayer;
using UnityEngine.UI;
using Rubik.Myrk.Arena;

namespace Rubik.ItemPlayer
{
    public class UserItemDataBarUI : NTBehaviour
    {
        public string _id = "";
        public ItemType Type = ItemType.Coin;
        public ItemDataBarUI ItemDataBarUI;
        public Transform MoreInfor;
        public TextMeshProUGUI MoreInforText;
        public Transform BtnAdd;

        public Coroutine CorUpdateData;

        protected override void Start()
        {
            base.Start();
            this.SetData(this.Type);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.SetData(this.Type);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            this.Unset();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            this.Unset();
        }

        public void SetData(ItemType type)
        {
            if (this._id == "")
            {
                this._id = NTFunction.GenerateId();
            }
            this.Unset();
            this.Type = type;
            EventListenerManager.instance.Register(EventCode.UpdateItemData, this._id, (object data) =>
            {
                this.UpdateData();
            });
            this.UpdateData();
            ItemDataInfo itemDataInfo = ItemDataManager.Instance.GetItemDataInfo(this.Type);
            if (ItemDataManager.Instance.IsAdd(this.Type))
            {
                this.BtnAdd.gameObject.SetActive(true);
            }
            else
            {
                this.BtnAdd.gameObject.SetActive(false);
            }

        }

        public void Unset()
        {
            EventListenerManager.instance.RemoveListener(EventCode.UpdateItemData, this._id);
        }

        protected virtual void UpdateData()
        {
            this.MoreInfor.gameObject.SetActive(false);
            this.ItemDataBarUI.SetData(ItemDataManager.Instance.GetItem(this.Type), true, true);
            if (this.Type == ItemType.Energy)
            {
                if (this.CorUpdateData != null)
                {
                    StopCoroutine(this.CorUpdateData);
                }
                this.CorUpdateData = StartCoroutine(this.IEUpdateEnergy());
            }
            if (this.Type == ItemType.ArenaTicket)
            {
                if (this.CorUpdateData != null)
                {
                    StopCoroutine(this.CorUpdateData);
                }
                this.CorUpdateData = StartCoroutine(this.IEUpdateArenaTicket());
            }
            if (ItemDataManager.Instance.IsAdd(this.Type))
            {
                this.BtnAdd.gameObject.SetActive(true);
            }
            else
            {
                this.BtnAdd.gameObject.SetActive(false);
            }
        }

        public IEnumerator IEUpdateEnergy()
        {
            while (true)
            {
                this.MoreInfor.gameObject.SetActive(true);
                (int energy, int energyMax, int energyRecover) = UserDataManager.Instance.GetUserEnergyItem();

                this.ItemDataBarUI.Amount.text = energy + "/" + energyMax;
                if (energyRecover > 0)
                {
                    this.MoreInforText.text = NTFunction.FormatTimeMinus(energyRecover);
                }
                else
                {
                    this.MoreInforText.text = Lean.Localization.LeanLocalization.GetTranslationText("max", "Max");
                }
                if (ItemDataManager.Instance.IsAdd(this.Type))
                {
                    this.BtnAdd.gameObject.SetActive(true);
                }
                else
                {
                    this.BtnAdd.gameObject.SetActive(false);
                }
                yield return new WaitForSeconds(1);
            }
        }

        public IEnumerator IEUpdateArenaTicket(){
            while (true)
            {
                this.MoreInfor.gameObject.SetActive(true);
                (int ticket, int maxTicket, int recover) = ArenaManager.Instance.GetUserArenaTicketItem();

                this.ItemDataBarUI.Amount.text = ticket + "/" + maxTicket;
                if (recover > 0)
                {
                    this.MoreInforText.text = NTFunction.FormatTimeMinus(recover);
                }
                else
                {
                    this.MoreInforText.text = Lean.Localization.LeanLocalization.GetTranslationText("max", "Max");
                }
                if (ItemDataManager.Instance.IsAdd(this.Type))
                {
                    this.BtnAdd.gameObject.SetActive(true);
                }
                else
                {
                    this.BtnAdd.gameObject.SetActive(false);
                }
                yield return new WaitForSeconds(1);
            }
        }

        public void OnClickAdd()
        {
            ItemDataManager.Instance.OnClickAdd(this.Type);
        }
    }
}