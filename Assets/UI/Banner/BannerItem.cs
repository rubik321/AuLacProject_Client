using TMPro;
using UnityEngine;

namespace Rubik.Banner
{
    public class BannerItem : MonoBehaviour
    {
        public TextMeshProUGUI Message;
        public BannerTopData Data;

        public void SetData(BannerTopData data){
            this.Data = data;
            this.Message.text = data.Message;
        }
    }
}