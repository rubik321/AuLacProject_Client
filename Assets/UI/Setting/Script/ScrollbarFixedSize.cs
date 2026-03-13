 using UnityEngine;
 using UnityEngine.UI;
 
 public class ScrollbarFixedSize : MonoBehaviour
 {
     public ScrollRect Rect;
     public Slider ScrollSlider;
 
     private void Update()
     {
         Rect.verticalNormalizedPosition = ScrollSlider.value;
     }
 }