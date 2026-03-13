using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Arrow : MonoBehaviour
{
	public Transform pos1, pos2;
	Vector3 posTarget;
	bool isPlay = false;
	[SerializeField] Sprite[] arraySpriteArrow;
	[SerializeField] SpriteRenderer spriteRendererArrow;
    private void Awake()
    {
		isPlay = false;
	}
    void FixedUpdate()
	{
        if (Input.GetKeyDown(KeyCode.K))
            MoveToPosition(pos1.position, pos2.position, 10);

        //if (isPlay)
        //    transform.Rotate(Vector3.back * Time.deltaTime * speedRotate);

        //if (isKnifePlayer)
        //{
        //    transform.Rotate(Vector3.back * Time.deltaTime * speedRotate);
        //}
        //else
        //{
        //    transform.Rotate(-Vector3.back * Time.deltaTime * speedRotate);
        //}

        if (isPlay)
            transform.rotation = Quaternion.Euler(0, 0, CalculateAngle(posTarget - transform.position) + 45);
    }
	private void OnEnable()
	{
		isPlay = false;
	}

	public void SetArrow(Vector3 pos1, Vector3 charHit)
	{
		MoveToPosition(pos1, charHit, 70);

	}
	
	public void MoveToPosition(Vector3 pos1, Vector3 pos2, float v,UnityEngine.Events.UnityAction callback = null)
	{
		posTarget = pos2;
		transform.DOKill();
		isPlay = true;
		transform.position = pos1;
		Vector3 tempPos = new Vector3((pos1.x + pos2.x) / 2f, (pos1.y + pos2.y) / 2f + 1f, (pos1.z + pos2.z) / 2f);

		Vector3[] path = new Vector3[]
		{
			pos1,
			tempPos,
			pos2
		};
		//float q = Vector3.Distance(pos1, tempPos) + Vector3.Distance(tempPos, pos2);
		float q = Vector3.Distance(pos1, tempPos) + Vector3.Distance(pos2, tempPos);

		//Debug.Log("qqqqqqqqqqqqqqqqqqqqq: " + q);
		float t = q / v;

		var dopath = transform.DOPath(path, t, PathType.CatmullRom, PathMode.Sidescroller2D).SetLookAt(0.01f).OnUpdate(() =>
		{
		}).SetEase(Ease.Linear).OnComplete(() => {
			isPlay = false;
            if (callback != null)
            {
				callback();
            }
			//gameObject.SetActive(false);
		});
		//transform.DOLookAt(targetRotate.position- transform.position, t);
		//dopath.SetLookAt(new Vector3(2, 1, 3), new Vector3(2, 1, 3), Vector3.up, false);
		//dopath.SetLookAt(new Vector3(2, 1, 3), new Vector3(2, 1, 3), Vector3.up, false);



	}
	public static float CalculateAngle(Vector2 velocity)
	{
		float _angleTemp = 0;
		if (velocity.x > 0 && velocity.y > 0)
		{
			float temp = Mathf.Atan(velocity.x / velocity.y);
			_angleTemp = -temp;
		}
		if (velocity.x > 0 && velocity.y == 0)
		{
			_angleTemp = 1.5f * Mathf.PI;
		}
		if (velocity.x < 0 && velocity.y > 0)
		{
			_angleTemp = Mathf.Atan(-velocity.x / velocity.y);

		}
		if (velocity.x < 0 && velocity.y == 0)
		{
			_angleTemp = 0.5f * Mathf.PI;
		}
		if (velocity.x < 0 && velocity.y < 0)
		{
			_angleTemp = Mathf.Atan(velocity.y / velocity.x);
			_angleTemp += Mathf.PI / 2;
		}
		if (velocity.x == 0 && velocity.y < 0)
		{
			_angleTemp = Mathf.PI;
		}
		if (velocity.x > 0 && velocity.y < 0)
		{
			float temp = Mathf.Atan(-velocity.y / velocity.x);
			_angleTemp = -Mathf.PI / 2 - temp;
		}
		if (velocity.x == 0 && velocity.y > 0)
		{
			_angleTemp = 0f;
		}

		return _angleTemp * 180 / Mathf.PI; ;
	}
}