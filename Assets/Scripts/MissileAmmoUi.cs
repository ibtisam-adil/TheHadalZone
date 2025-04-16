using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissileAmmoUI : MonoBehaviour
{
    public Image missileIcon;
    public TextMeshProUGUI ammoCountText;
    public Image cooldownOverlay;

    public void UpdateAmmo(int current, int max)
    {
        ammoCountText.text = $"x{current}";
        missileIcon.enabled = current > 0;
    }

    public void SetCooldown(float fillAmount)
    {
        cooldownOverlay.fillAmount = fillAmount;

        Color baseColor = Color.white;
        Color cooldownColor = Color.grey;
        missileIcon.color = Color.Lerp(baseColor, cooldownColor, fillAmount);
    }

}
