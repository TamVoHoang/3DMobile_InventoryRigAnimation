using UnityEngine;
using UnityEngine.UI;
public class AiUIHealthBar : MonoBehaviour
{
    // game object = enemy dat tren nguoi enemy de show health bar
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Image backGroundImage;
    [SerializeField] private Image foreGroundImage;

    private void LateUpdate() {
        NormalizedUIHealthBar();
        transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
    }
    public void SetHealthBarEnemyPercent(float percent) {
        float parentWidth = GetComponent<RectTransform>().rect.width;
        float width = parentWidth * percent;
        foreGroundImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }


    private void NormalizedUIHealthBar() {
        Vector3 direction = (target.position - Camera.main.transform.position).normalized;
        bool isBehind = Vector3.Dot(direction, Camera.main.transform.forward) <= 0.0f;
        foreGroundImage.enabled = !isBehind;
        backGroundImage.enabled = !isBehind;
    }
}
