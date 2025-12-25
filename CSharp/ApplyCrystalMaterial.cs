using System;
using UnityEngine;
using AV;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Violet.AV.MaterialUtilitys
{
    internal class ApplyCrystalMaterial
    {
        static GameObject GameObjectToApplyMat;
        static Material Mat;
        static bool returned = false;
        static Material CrystalMaterialPurple = Plugin.ShaderBundle.LoadAsset<Material>("Crystal_Purple");
        static Material CrystalMaterialBlue = Plugin.ShaderBundle.LoadAsset<Material>("Crystal_Blue");
        static Material CrystalMaterialCyan = Plugin.ShaderBundle.LoadAsset<Material>("Crystal_Cyan");


        private static Material GetMaterialFromMaterialIndex(int MaterialIndex)
        {

            if (MaterialIndex == 0)
            {
                Mat = CrystalMaterialPurple;
                returned = true;
                return CrystalMaterialPurple;
                
            }
            else if (MaterialIndex == 1)
            {

                Mat = CrystalMaterialBlue;
                returned = true;
                return CrystalMaterialBlue;
            }
            else if (MaterialIndex == 2)
            {

                Material Mat = CrystalMaterialCyan;
                returned = true;
                return CrystalMaterialCyan;
                
                

            }
            else if (MaterialIndex < 0 || MaterialIndex > 2)
            {
                Plugin.Log.LogError($"Material Index Is less then 0 Or Greater Then 2 Value is {MaterialIndex}");
                return null;
            }
            return null;
        }

        public static IEnumerator ApplyMaterialToObject(GameObject Object, int MaterialIndexInterger)
        {
            if (Object != null)
            {
                returned = false;
                Material crystalmaterial = GetMaterialFromMaterialIndex(MaterialIndexInterger);
                yield return new WaitUntil(() => returned == true);

                Renderer rend = Object.GetComponent<Renderer>();
                rend.sharedMaterial = crystalmaterial;

                yield return new WaitUntil(() => rend.material == crystalmaterial);
            }


            yield return null;
        }

    }
}
