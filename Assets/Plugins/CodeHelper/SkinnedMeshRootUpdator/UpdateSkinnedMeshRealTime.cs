//Copyright(c) 2016 Tim McDaniel (TrickyHandz on forum.unity3d.com)
// Updated by Piotr Kosek 2019 (shelim on forum.unity3d.com)
// Updated by Orochii Zouveleki 2020: Added a couple extra debug info.
// Update by Nam Trần 2023: Added converting to realtime feature and removed editor logs
//
// Adapted from code provided by Alima Studios on forum.unity.com
// http://forum.unity3d.com/threads/prefab-breaks-on-mesh-update.282184/#post-2661445
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to
// the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.


using UnityEngine;

namespace CodeHelper
{
    public static class UpdateSkinnedMeshRealTime
    {
        public static void UpdateSkinnedMesh(SkinnedMeshRenderer targetSkin, Transform rootBone)
        {
            // Look for root bone
            string rootName = "";
            if (targetSkin.rootBone != null) rootName = targetSkin.rootBone.name;
            Transform newRoot = null;
            // Reassign new bones
            Transform[] newBones = new Transform[targetSkin.bones.Length];
            Transform[] existingBones = rootBone.GetComponentsInChildren<Transform>();
            int missingBones = 0;
            for (int i = 0; i < targetSkin.bones.Length; i++)
            {
                if (targetSkin.bones[i] == null)
                {
                    missingBones++;
                    continue;
                }
                string boneName = targetSkin.bones[i].name;
                bool found = false;
                foreach (var newBone in existingBones)
                {
                    if (newBone.name == rootName) newRoot = newBone;
                    if (newBone.name == boneName)
                    {
                        newBones[i] = newBone;
                        found = true;
                    }
                }
                if (!found)
                {
                    missingBones++;
                }
            }
            targetSkin.bones = newBones;
            Debug.Log("Done! Missing bones: " + missingBones);
            if (newRoot != null)
            {
                //Debug.Log("Setting " + rootName + " as root bone.");
                targetSkin.rootBone = newRoot;
            }
        }
    }
}
