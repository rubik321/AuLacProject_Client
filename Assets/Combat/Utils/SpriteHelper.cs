using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GOA.UserData;
public class SpriteHelper : Pixelplacement.Singleton<SpriteHelper>
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] public Sprite[] ListBGGear;
    [SerializeField] public Sprite[] ListOrigins;
    public Sprite[] lsGear;
    public Sprite[] color, patterm, icons,bgColor;
    private Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();
    protected override void OnRegistration()
    {
        foreach (Sprite sprite in sprites)
        {
            try
            {
                spriteDict.TryAdd(sprite.name, sprite);
            }catch(Exception e)
            {
                Debug.LogException(e);
            }
        }
        //foreach (Sprite sprite in ListIconGear)
        //{
        //    try
        //    {
        //        spriteDict.TryAdd(sprite.name, sprite);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogException(e);
        //    }
        //}
        // spriteDict.TryAdd("M040006", sprites[27]);
        // spriteDict.TryAdd("M040005", sprites[27]);
        // spriteDict.TryAdd("M040004", sprites[27]);
    }

    public Sprite GetSprite(string id)
    {
        if (spriteDict.TryGetValue(id, out Sprite sprite)) 
            return sprite;
        else 
        {
            string nameOfGear = UserData.Instance.lsGoIdToName[id];
            if (spriteDict.TryGetValue(nameOfGear, out Sprite sprites))
                return sprites;
        }
        Debug.LogError("Missing asset " + id);
        return null;
    }
    public Sprite GetIconGearSprite(string ID)
    {
        string nameOfGear = UserData.Instance.lsGoIdToName[ID];
        return GetSprite(nameOfGear);
    }
    
    [System.Serializable]
    public class SpriteData
    {
        public string name;
        public Sprite sprite;
    }
   
}
