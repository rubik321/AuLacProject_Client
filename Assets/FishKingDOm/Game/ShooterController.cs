using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Pixelplacement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum BulletName
{
    Bullet_1,
    Bullet_2,
    Bullet_3
}
public class ShooterController : Singleton<ShooterController>
{
    //Coroutine coAttack;
    //public BulletName bulletName;
    //public float bulletSpeed;
    //public float bulletSpeedRate;

    //public float Dmg = 10;

    //private void Start()
    //{
    //    this.Dmg = FinlordManager.instance.Finlord.DmgTap();
    //}
    //public void Shoot(Vector2 pos)
    //{
    //    UserDataManager.instance.TapTime++;
    //    var target = GameController.Instance.PlayerAIAttackTarget();
    //    if (target != null)
    //    {
    //        GameObject bullet = ObjectPool.Spawn(GetBullet());
    //        Vector2 posTarGet = target.positionStart.positionHitRange();
    //        bullet.transform.eulerAngles = new Vector3(0, 0, Angle(posTarGet, pos));

    //        //bullet.transform.LookAt(target.transform);
    //        //bullet.transform.localScale = new Vector2(1f, 1f);
    //        bullet.transform.position = pos;

    //        Common.MoveObjectToPosition(bullet, posTarGet, bulletSpeed, () =>
    //        {
    //            bullet.transform.position = new Vector3(-10, 10);
    //            bullet.Recycle();
    //            target.HitDameInShoot(this.Dmg);
    //            GameObject hit = ObjectPool.Spawn(GetHitBullet());
    //            //hit.transform.localScale = new Vector2(1, 1);
    //            hit.transform.position = posTarGet;
    //        });
    //    }

    //}
    //float Angle(Vector2 pointA, Vector2 pointB)
    //{
    //    Vector2 direction = pointA - pointB;
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    return angle;
    //}
    //public void AutoAttack(Transform pos, int isAtk = -1)
    //{
    //    if (isAtk == -1) isAttack = !isAttack;
    //    if (isAtk == 1) isAttack = true;
    //    if (isAtk == 0) isAttack = false;

    //    if (!isAttack)
    //    {
    //        StopCoroutine(coAttack);
    //    }
    //    else
    //    {
    //        coAttack = StartCoroutine(CoAutoAttack(pos));
    //    }

    //}
    //public bool isAttack = false;

    //public float MaxDistance = 2000;

    //IEnumerator CoAutoAttack(Transform pos)
    //{
    //    while (isAttack)
    //    {
    //        yield return new WaitForSeconds(bulletSpeedRate);
    //        //Debug.Log("Shoot");
    //        Shoot(pos.position);
    //    }
    //}

    //public float delay = 0;

    //public void Update()
    //{
    //    if(delay > 0){
    //        this.delay -= Time.deltaTime;
    //        return;
    //    }
    //    if (Input.GetMouseButtonDown(0) && !StaticData.IsAuto)
    //    {
    //        if (EventSystem.current.IsPointerOverGameObject())
    //        {
    //            //Debug.Log("Clicked on the UI");
    //            return;
    //        }
    //        delay = 0.05f;
    //        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        Shoot(mousePosition);
    //    }
    //}
    //string GetBullet()
    //{
    //    switch (bulletName)
    //    {
    //        case BulletName.Bullet_1:
    //            return "Bullet 1";
    //        case BulletName.Bullet_2:
    //            return "Bullet 2";
    //        case BulletName.Bullet_3:
    //            return "Bullet 3";

    //    }
    //    return null;
    //}
    //string GetHitBullet()
    //{
    //    switch (bulletName)
    //    {
    //        case BulletName.Bullet_1:
    //            return "Hit 1";
    //        case BulletName.Bullet_2:
    //            return "Hit 2";
    //        case BulletName.Bullet_3:
    //            return "Hit 3";
    //    }
    //    return null;
    //}
}
