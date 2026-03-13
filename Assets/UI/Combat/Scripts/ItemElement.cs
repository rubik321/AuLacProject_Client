using CodeHelper;
using NTPackage_old.EventDispatcher;
using Rubik.Combat;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Rubik.Battle;
//using static Sirenix.OdinInspector.Editor.Examples.SearchablePerksExample;

namespace Rubik.UI
{
    public class ItemElement : MonoBehaviour, IMessageHandle
    {
        [SerializeField] GameObject highlight;
        [SerializeField] Image icon;
        [SerializeField] TMP_Text textQuantity;
        public string ID { get; private set; }
        Action<string> onClick;

        private void OnEnable()
        {
            MessageManager.AddSubcriber<CodeHelper.MessageCollection.OnClickItem>(this);
            highlight.SetActive(false);
        }
        private void OnDisable()
        {
            MessageManager.RemoveSubcriber<CodeHelper.MessageCollection.OnClickItem>(this);
        }

        public void SetupData(string itemId, int quantity, Action<string> onClick)
        {
            BaseActionSO item = ActionPool.Instance.GetActionSO(itemId);
            this.ID = itemId;
            icon.sprite = SpriteHelper.Instance.GetSprite(itemId);
            //icon.SetNativeSize();
            textQuantity.text = quantity.ToString();
            this.onClick = onClick;
        }

        public void UpdateQuantity(int count)
        {
            textQuantity.text = count.ToString();
            // Todo: Pool this
            if (count <= 0)
            {
                Destroy(gameObject);
            }
        }

        public void OnClick(bool isMana = false)
        {
            onClick?.Invoke(ID);
            //MessageManager.SendMessage(new Message(nameof(CodeHelper.MessageCollection.OnClickItem)));
            highlight.SetActive(true);
            UIGamePlayBattle.Instance.ShowPanelButton();
            Vector2 newPos = new Vector2(GameController.Instance.listCardCharacter[0].transform.position.x, GameController.Instance.listCardCharacter[0].transform.position.y + 1);
            if (!isMana)
            {
                GameController.Instance.listCardCharacter[0].UpdateHP(2000);
                UIGamePlayBattle.Instance.ShowStateTxt("<color=blue>Player</color> heal HP");
                ZfxGameplayController.Instance.HealingSpawn(newPos, 2000);
            }

            else
            {
                GameController.Instance.listCardCharacter[0].AddAP(100);
                UIGamePlayBattle.Instance.ShowStateTxt("<color=blue>Player</color> heal AP");
                ZfxGameplayController.Instance.HealingSpawn(newPos, 100);
            }
            
           // GameController.Instance.ChangeState();
            //GameController.Instance.ChangeTurn();
        }

        public void Handle(Message message)
        {
            QuestManager.Instance.UpdateQuest(QuestType.Use_Potion, 1);
            highlight.SetActive(false);
        }
    }
}
