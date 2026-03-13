using UnityEngine;
using System; // Cần thiết để sử dụng Action
using System.Collections;

public class HeroJumpController : MonoBehaviour
{
   
    private Coroutine _currentJumpCoroutine;

    /// <summary>
    /// Khiến nhân vật nhảy theo đường vòng cung từ điểm A đến điểm B.
    /// </summary>
    /// <param name="startPoint">Vị trí bắt đầu (Điểm A).</param>
    /// <param name="endPoint">Vị trí kết thúc (Điểm B).</param>
    /// <param name="arcHeight">Độ cao của vòng cung so với đường thẳng nối A và B.</param>
    /// <param name="onReachedTarget">Sự kiện được gọi khi nhân vật đến Điểm B.</param>
    /// <param name="jumpDuration">Thời gian ước tính để hoàn thành cú nhảy (ảnh hưởng đến tốc độ).</param>
    public void JumpToTarget(Vector2 startPoint, Vector2 endPoint, float arcHeight, Action onReachedTarget, float jumpDuration = 1.0f)
    {
        if (_currentJumpCoroutine != null)
        {
            StopCoroutine(_currentJumpCoroutine);
        }
        _currentJumpCoroutine = StartCoroutine(ArcMovementCoroutine(startPoint, endPoint, arcHeight, onReachedTarget, jumpDuration));
    }

    private IEnumerator ArcMovementCoroutine(Vector2 startPoint, Vector2 endPoint, float arcHeight, Action onReachedTarget, float duration)
    {
        float elapsedTime = 0f;
        transform.position = startPoint; // Đảm bảo nhân vật bắt đầu đúng vị trí

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration; // Tiến trình từ 0 đến 1

            // Nội suy tuyến tính cho vị trí X và Z (hoặc X và Y trong 2D)
            // Vector2 currentPos = Vector2.Lerp(startPoint, endPoint, t);

            // Tính toán vị trí trên đường vòng cung (Parabolic arc)
            // Công thức cơ bản cho quỹ đạo parabol: y = a*x^2 + b*x + c
            // Tuy nhiên, ở đây chúng ta sẽ sử dụng phương pháp đơn giản hơn bằng cách nội suy giữa 3 điểm:
            // P0 = startPoint
            // P2 = endPoint
            // P1 = điểm giữa của start và end, được nâng lên theo arcHeight

            Vector2 p0 = startPoint;
            Vector2 p2 = endPoint;

            // Tính toán điểm giữa theo chiều ngang
            Vector2 midPointHorizontal = Vector2.Lerp(p0, p2, 0.5f);

            // Tính toán điểm đỉnh của vòng cung
            // Hướng vuông góc với đường thẳng từ p0 tới p2 (để nâng điểm giữa lên)
            // Trong 2D, nếu vector (dx, dy) thì vector vuông góc là (-dy, dx) hoặc (dy, -dx)
            // Vector hướng từ p0 sang p2
            Vector2 directionP0toP2 = (p2 - p0).normalized;
            // Vector vuông góc hướng lên (giả sử trục Y là trục "lên" trong không gian 2D của bạn)
            // Nếu bạn muốn vòng cung luôn hướng lên bất kể vị trí A và B, bạn có thể dùng Vector2.up
            // Tuy nhiên, để vòng cung "tự nhiên" hơn theo hướng nhảy, ta sẽ tính toán điểm đỉnh phức tạp hơn một chút
            // hoặc đơn giản là cộng arcHeight vào thành phần y của điểm giữa.

            Vector2 peakPoint = midPointHorizontal + Vector2.up * arcHeight;
            // Nếu startPoint và endPoint có cùng y, thì peakPoint.y sẽ là startPoint.y + arcHeight.
            // Nếu y khác nhau, chúng ta muốn arcHeight là độ cao thêm vào từ đường thẳng nối A và B tại điểm giữa.
            float baseHeightAtMid = Mathf.Lerp(p0.y, p2.y, 0.5f);
            peakPoint.y = baseHeightAtMid + arcHeight;


            // Sử dụng công thức Bezier bậc hai: B(t) = (1-t)^2 * P0 + 2 * (1-t) * t * P1 + t^2 * P2
            Vector2 positionOnArc = (1 - t) * (1 - t) * p0 +
                                    2 * (1 - t) * t * peakPoint +
                                    t * t * p2;

            transform.position = positionOnArc;

            yield return null; // Chờ đến frame tiếp theo
        }

        transform.position = endPoint; // Đảm bảo nhân vật đến chính xác điểm cuối
        onReachedTarget?.Invoke(); // Kích hoạt sự kiện khi đã đến đích

        _currentJumpCoroutine = null;
    }

    // --- Ví dụ sử dụng (có thể đặt trong một script khác) ---
    /*
    public Transform targetMarker; // Kéo một đối tượng vào đây để làm điểm B
    public float jumpArcHeight = 3.0f;
    public float heroJumpDuration = 1.5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (targetMarker != null)
            {
                Vector2 startPos = transform.position;
                Vector2 endPos = targetMarker.position;
                JumpToTarget(startPos, endPos, jumpArcHeight, HandleHeroLanded, heroJumpDuration);
            }
            else
            {
                Debug.LogWarning("Target marker chưa được gán!");
            }
        }
    }

    void HandleHeroLanded()
    {
        Debug.Log("Hero đã đáp xuống điểm B!");
        // Thêm các hành động bạn muốn thực hiện khi hero đáp xuống
        // Ví dụ: Phát âm thanh, thay đổi animation, v.v.
    }
    */
}