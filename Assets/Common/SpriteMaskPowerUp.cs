using UnityEngine;

namespace Rubik.Common
{
    [RequireComponent(typeof(SpriteMask))]
    public class SpriteMaskPowerUp : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sp;

        private SpriteMask spriteMask;
        
        // Start is called before the first frame update
        void Start()
        {
            spriteMask = GetComponent<SpriteMask>();
            spriteMask.isCustomRangeActive = true;
        }

        // Update is called once per frame
        void Update()
        {
            Refresh();
        }

        void Refresh()
        {
            spriteMask.backSortingOrder = sp.sortingOrder - 1;
            spriteMask.backSortingLayerID = sp.sortingLayerID;
            
            spriteMask.frontSortingOrder = sp.sortingOrder + 1;
            spriteMask.frontSortingLayerID = sp.sortingLayerID;
        }
    }
}
