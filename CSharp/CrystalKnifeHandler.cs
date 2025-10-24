using ArchitectsLibrary.API;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Violet.AV
{
    public class CrystalKnifeHandler
    {
        public class CrystalKnifeTool : Knife
        {

            // All this code was possible thanks to Lee23
            // Lee23 Thank you so much for allowing modders to use code from your projects!

            // Most/Basically all Credits gose to Lee23!

            public override string animToolName => "knife";

            public float crystaldamage;


            public override void OnToolUseAnim(GUIHand hand)
            {
                Vector3 position = default(Vector3);
                GameObject closestObj = null;
                UWE.Utils.TraceFPSTargetPosition(Player.main.gameObject, attackDist, ref closestObj, ref position);
                if (position == default)
                    position = MainCamera.camera.transform.position + MainCamera.camera.transform.forward * attackDist;
                if (closestObj == null)
                {
                    InteractionVolumeUser component = Player.main.gameObject.GetComponent<InteractionVolumeUser>();
                    if (component != null && component.GetMostRecent() != null)
                    {
                        closestObj = component.GetMostRecent().gameObject;
                    }
                }

                if ((bool)closestObj)
                {
                    LiveMixin liveMixin = closestObj.FindAncestor<LiveMixin>();
                    if (IsValidTarget(liveMixin))
                    {
                        if ((bool)liveMixin)
                        {
                            bool wasAlive = liveMixin.IsAlive();
                            liveMixin.TakeDamage(crystaldamage, position, DamageType.Electrical);
                            liveMixin.TakeDamage(damage, position, damageType);
                            GiveResourceOnDamage(closestObj, liveMixin.IsAlive(), wasAlive);
                        }

                        Utils.PlayFMODAsset(attackSound, base.transform);
                        VFXSurface component2 = closestObj.GetComponent<VFXSurface>();
                        Vector3 euler = MainCameraControl.main.transform.eulerAngles + new Vector3(300f, 90f, 0f);
                        VFXSurfaceTypeManager.main.Play(component2, vfxEventType, position, Quaternion.Euler(euler),
                            Player.main.transform);
                    }
                    else
                    {
                        closestObj = null;
                    }
                }
            }
        }
    }
}
