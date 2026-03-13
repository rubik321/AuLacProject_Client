using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class InventoryUI : Singleton<InventoryUI>
{
    // Start is called before the first frame update
    private StateMachine stateMachine;
    [SerializeField] GameObject inventoryUI;
    [SerializeField] Image[] lsbuttonTabs;
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
    }

    public void ShowInventory()
    {
        stateMachine.ChangeState("Inventory");
    }
    public void ShowItem()
    {

        stateMachine.ChangeState("Items_Detail");
        inventoryUI.SetActive(true);
    }
    public void ShowDropItem()
    {
        stateMachine.ChangeState("DropItem");
    
    }
    public void ShowPopup()
    {
        stateMachine.ChangeState("Popup");
    }
    public void OnTabsButton(int index)
    {
        for(int i = 0; i < lsbuttonTabs.Length; i++)
        {
            lsbuttonTabs[i].enabled = false;
        }
        lsbuttonTabs[index].enabled = true;
    }
    public void CloseButton()
    {
            stateMachine.Exit();
            CharacterUIController.Instance.CameraUI.SetActive(false);
        // stateMachine.ChangeState("Popup");
    }
    public void OnButtonClose(Animator animPopup)
    {
        if (animPopup != null)
        {
            animPopup.Play("HidePopup");
            Invoke("ShowInventory", .3f);
        }

    }
   
    public void LoadAlls()
    {

    }
}
