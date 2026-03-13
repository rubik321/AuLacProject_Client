using NTPackage.Functions;
using TMPro;
using UnityEngine;  
using System.Collections.Generic;
using UnityEngine.TextCore;

public class TMP_SpriteAsset_Editor : NTBehaviour
{
    public List<TMP_SpriteAsset> SpriteAssets;

    [NTButton]
    public void EditSpriteAsset(){
        foreach (TMP_SpriteAsset spriteAsset in this.SpriteAssets)
        {
            spriteAsset.spriteGlyphTable[0].scale = 1.5f;
            GlyphMetrics metrics = spriteAsset.spriteGlyphTable[0].metrics;
            metrics.horizontalBearingY = 180f;
            spriteAsset.spriteGlyphTable[0].metrics = metrics;
            //Update to local
        }
    }
}
