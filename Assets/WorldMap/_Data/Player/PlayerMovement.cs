using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOA.WorldMap;
using GoShared;
namespace GOA.WorldMap
{
    public class PlayerMovement : MoveAvatar
    {
        public Animator animator;
        public Coordinates OldPos;

        public bool IsWalk = false;

        private void FixedUpdate()
        {
            this.UpdateAnim();
        }

        protected void UpdateAnim()
        {
            if (animator == null) return;
            if (this.animationState == AvatarAnimationState.Walk)
            {
                if (!this.IsWalk)
                {
                    this.animator.Play("walk_worldmap");
                    this.IsWalk = true;
                }
                MobManager.instance.UpdateData();
                if (OldPos == null)
                {
                    this.OldPos = GameMaster.instance.locationManager.currentLocation;
                }
                if (this.OldPos.DistanceFromPoint(GameMaster.instance.locationManager.currentLocation) > 400)
                {
                    this.OldPos = GameMaster.instance.locationManager.currentLocation;
                    WorldMap.WorldMapMaster.instance.UpdateData();
                }
            }

            if (this.animationState == AvatarAnimationState.Run)
            {
                if (!this.IsWalk)
                {
                    this.animator.Play("walk_worldmap");
                    this.IsWalk = true;
                }
                MobManager.instance.UpdateData();
                if (OldPos == null)
                {
                    this.OldPos = GameMaster.instance.locationManager.currentLocation;
                }
                if (this.OldPos.DistanceFromPoint(GameMaster.instance.locationManager.currentLocation) > 400)
                {
                    this.OldPos = GameMaster.instance.locationManager.currentLocation;
                    WorldMap.WorldMapMaster.instance.UpdateData();
                }
            }

            if (this.animationState == AvatarAnimationState.Idle)
            {
                if (this.IsWalk)
                {
                    this.animator.Play("idle");
                    this.IsWalk = false;
                }
                if (OldPos == null)
                {
                    this.OldPos = GameMaster.instance.locationManager.currentLocation;
                }
                if (this.OldPos.DistanceFromPoint(GameMaster.instance.locationManager.currentLocation) > 400)
                {
                    this.OldPos = GameMaster.instance.locationManager.currentLocation;
                    WorldMap.WorldMapMaster.instance.UpdateData();
                }
            }
        }

        public override void moveAvatar(Vector3 lastPosition, Vector3 currentPosition)
        {
            if (GameMaster.instance.viewModeCode == ViewModeCode.god) return;
            base.moveAvatar(lastPosition, currentPosition);
        }
    }
}
