using System.Collections;
using System.Collections.Generic;
using GOA.Item;
using UnityEngine;

namespace GOA.Mat
{
    public class MatListUI : MonoBehaviour
    {
        public MatItemUI MatItemUISample;

        public Transform Content;

        public List<MatItemUI> MatItemUIs;

        protected void LoadMatItemUIs(){
            this.MatItemUIs.Clear();
            foreach (Transform item in this.Content)
            {
                item.gameObject.SetActive(false);
                if(item.TryGetComponent<MatItemUI>(out MatItemUI matItemUI)){
                    this.MatItemUIs.Add(matItemUI);
                }
            }
        }

        public void SetData(List<(Sprite, string, Color)> values){
            this.LoadMatItemUIs();
            for (int i = 0; i < values.Count; i++)
            {
                try
                {
                    this.MatItemUIs[i].gameObject.SetActive(true);
                }
                catch (System.Exception)
                {
                    MatItemUI matItemUI = Instantiate(this.MatItemUISample);
                    matItemUI.transform.SetParent(this.Content);
                    this.MatItemUIs.Add(matItemUI);
                    this.MatItemUIs[i].ResetScale();
                    this.MatItemUIs[i].gameObject.SetActive(true);
                }
                this.MatItemUIs[i].SetData(values[i].Item1, values[i].Item2, values[i].Item3);
            }
        }
    }
}

