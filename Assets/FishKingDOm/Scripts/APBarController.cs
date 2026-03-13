using System.Collections;
using System.Collections.Generic;
using Minimalist.Bar.UI;
using UnityEngine;

namespace Rubik.Battle
{
    public class APBarController : MonoBehaviour
    {
        [SerializeField] BarBhv barBhv;
        float curAP;
        float maxAP;
        // Start is called before the first frame update
        void Start()
        {

        }
        public void Begin(float _curAP, float _maxAP = 100)
        {
            UpdateAPBar(_curAP, _maxAP);
        }
        public void UpdateAPBar(float _curAP, float _maxAP = 100)
        {
            curAP = _curAP;
            maxAP = _maxAP;
            if (curAP > maxAP) curAP = maxAP;
            barBhv.Quantity.Amount = curAP;
            barBhv.Quantity.MaximumAmount = maxAP;
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}