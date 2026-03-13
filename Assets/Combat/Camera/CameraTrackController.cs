
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

namespace Rubik.Combat
{
    public class CameraTrackController : Pixelplacement.Singleton<CameraTrackController>
    {
        [SerializeField] CinemachineCamera cam;
        [SerializeField] CinemachineCamera focusCam;
        [SerializeField] CinemachineCamera strategyCam;
        [SerializeField] CinemachineCamera playerCam;
        [SerializeField] CinemachineCamera botCam;
        [SerializeField] CinemachineCamera camCart;
        [SerializeField] CinemachineCamera trackIdle;
        [SerializeField] Track[] trackAction;
        StageObject stage;

        public void Start()
        {
            //CombatManager.Instance.SubscribeOnActivateTurnState<StateStartTurn>(ResetCamera);
            //CombatManager.Instance.SubscribeOnActivateTurnState<StateChooseAction>(SetIdleCamera);
            //CombatManager.Instance.SubscribeOnActivateTurnState<StateAction>(SetActionCamera);
        }
        private void ResetCamera(string character)
        {
            //if (stage == null)
            //    stage = FindObjectOfType<StageObject>();
            //focusCam.Priority = 1;
            //strategyCam.Priority = 0;

            //camCart.m_Path = null; // trackAction.First(e => e.actionType == ActionCameraType.Attack).track;
            //cam.LookAt = stage.center; // CharacterManager.Instance.GetCharacterObject(character).CameraFocusTarget;
            //cam.transform.DOMove(stage.focusCamPos.position, 0.5f).SetEase(Ease.OutCirc);
            //cam.Follow = stage.focusCamPos;
        }

        public IEnumerator SetActionReadyCam(BaseCharacter character)
        {
            focusCam.Priority = 1;
            strategyCam.Priority = 0;
            yield return new WaitForSeconds(1);
        }

        public IEnumerator SetActionCam(BaseCharacter character)
        {
            focusCam.Priority = 0;
            strategyCam.Priority = 1;
            yield return new WaitForSeconds(1);
        }

        private void SetIdleCamera(string character)
        {
            // Move to default pos

            StartCoroutine(IEDelayIdleCamera());
        }

        private IEnumerator IEDelayIdleCamera()
        {
            // If player have more
            float timer = 2;
            while (CombatManager.Instance.CurrentGameState.GetTurnType() == typeof(StateChooseAction))
            {
                yield return null;
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    // Set Idle Camera
                    yield break;
                }
            }
        }

        public void SetPlayerCam()
        {
            if (playerCam.Priority == 1)
                return;
            playerCam.Priority = 1;
            botCam.Priority = 0;
        }

        public void SetBotCam()
        {
            if (playerCam.Priority == 0)
                return;
            playerCam.Priority = 0;
            botCam.Priority = 1;
        }

        [Serializable]
        private class Track
        {
            public ActionCameraType actionType;
            public CinemachineSmoothPath track;
        }
    }

    public enum ActionCameraType
    {
        Attack,

    }
}
