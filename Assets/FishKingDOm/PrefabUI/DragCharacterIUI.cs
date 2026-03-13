using System.Collections;
using System.Collections.Generic;
using Rubik._2DGPS.Card;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rubik.CardPlayer;

//[RequireComponent(typeof(BoxCollider2D))]
public class DragCharacterIUI : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    // Start is called before the first frame update
    public Vector2 oriPos, shopPos;
     Camera cam;
    private bool dragging = false, isStartDragging = false;
    public SkeletonGraphic charAnim;
    public SkeletonAnimation charAnimation;
    public int indexOfCharacter;
    public string idChar, _ID;
    public List<GameObject> lsStars = new List<GameObject>();
    [SerializeField] Text nameTxt, lvTxt;
    [SerializeField] Image originImg;
    HeroPanel heroControl;
    private bool isOver;

    void Start()
    {
        oriPos = transform.position;

        heroControl = GetComponentInParent<HeroPanel>();

        //charAnim = GetComponent<SkeletonAnimation>();
    }
    public void SetUp(CardPlayer card,Camera camTemp)
    {
        var hero = CardPlayerManager.Instance.GetCardName(card.Index);
        nameTxt.text = hero; 
        cam = camTemp;
        _ID = card._id;
        //lvTxt.text = "Lv." + card.Lv;
        //int classIndex = (int)hero.baseData.Origin;
        //originImg.sprite = AssetLoader.Instance.lsOriginSprs[classIndex - 1];
        //for (int i = 0; i < lsStars.Count; i++)
        //{
        //    if (i < card.LvEnhance)
        //    {
        //        lsStars[i].SetActive(true);
        //    }
        //    else
        //    {
        //        lsStars[i].SetActive(false);
        //    }

        //}
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (charAnimation != null|| !isOver)
            return;
        Debug.Log("I dag");
        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(mousePosition.x, mousePosition.y);
       
        GetComponentInParent<HeroPanel>().CheckNearPos(mousePosition);
       
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isOver&& charAnimation==null)
        {
            isOver = true;
            Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            
            gameObject.SetActive(false);
            heroControl.SetLayerCharacter(this.transform, indexOfCharacter, true);
            transform.position = new Vector2(mousePosition.x, mousePosition.y);
            gameObject.SetActive(true);

        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (charAnimation != null)
            return;
        isOver = false;
        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        int temp = GetComponentInParent<HeroPanel>().CheckNearPos(mousePosition);
        if (temp == -1)
        {
            gameObject.transform.position = oriPos;
            heroControl.SetLayerCharacter(this.transform, indexOfCharacter, false);
        }
        else
        {
            GetComponentInParent<HeroPanel>().ChangePosition(indexOfCharacter, temp, gameObject);
        }
    }

    //public void OnMouseDown()
    //{
    //    Debug.Log("Draw");
    //    if (!isOver)
    //    {
    //        isOver = true;
    //        heroControl.SetLayerCharacter(this.transform, indexOfCharacter, true);
    //    }
    //}

    //public void OnMouseDrag()
    //{
    //    dragging = true;
    //    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    gameObject.transform.position = new Vector2(mousePosition.x, mousePosition.y);

    //}
    //bool isOver = false;

    //public void OnMouseUp()
    //{
    //    isOver = false;
    //    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    int temp = GetComponentInParent<HeroPanel>().CheckNearPos(mousePosition);
    //    if (temp == -1)
    //    {
    //        gameObject.transform.position = oriPos;
    //        heroControl.SetLayerCharacter(this.transform, indexOfCharacter, false);
    //    }
    //    else
    //    {
    //        GetComponentInParent<HeroPanel>().ChangePosition(indexOfCharacter, temp,gameObject);
    //    }

    //}
}
