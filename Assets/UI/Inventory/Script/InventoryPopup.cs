using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPopup : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (anim != null)
        {
            anim.Play("ShowPopup");
        }
    }
    private void OnDisable()
    {
        if (anim != null)
        {
            anim.Play("HidePopup");
        }
    }
    public void OnButtonClose()
    {
        if (anim != null)
        {
            GetComponentInParent<InventoryUI>().OnButtonClose(anim);
        }

    }
}
