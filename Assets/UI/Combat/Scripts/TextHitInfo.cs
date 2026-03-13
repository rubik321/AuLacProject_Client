using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextHitInfo : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    public void SetText(int dmg)
    {
        if (dmg > 0)
        {
            text.color = Color.red;
        }
        else if (dmg == 0)
        {
            text.color = Color.gray;
        }
        else
        {
            text.color = Color.green;
        }
        text.text = Mathf.Abs(dmg).ToString();
    }

    // Assign to animation event 
    public void Deactivate()
    {
        this.Recycle();
    }
}
