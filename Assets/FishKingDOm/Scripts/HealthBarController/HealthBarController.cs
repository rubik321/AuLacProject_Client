using Minimalist.Bar.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Rubik.Battle
{
    public class HealthBarController : MonoBehaviour
    {
        [SerializeField] BarBhv barBhv;
        float curHP;
        float maxHP;
        // Start is called before the first frame update
        void Start()
        {
            
        }
        public void Begin(float _curHP, float _maxHP)
        {
            UpdateHealthBar(_curHP, _maxHP);
        }
        public void UpdateHealthBar(float _curHP, float _maxHP)
        {
            curHP = _curHP;
            maxHP = _maxHP;
            if (curHP > maxHP) curHP = maxHP;
            barBhv.Quantity.Amount = curHP;
            barBhv.Quantity.MaximumAmount = maxHP;

        }
        

        // Update is called once per frame
        void Update()
        {

        }
    }
}

