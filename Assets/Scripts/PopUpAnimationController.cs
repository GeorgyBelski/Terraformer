using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpAnimationController : MonoBehaviour
{
    public Animator popUpTextAnimator;
    public RectTransform rect;
    Vector2 defaultPosition;

    public void EndDamagePopUpAnimation() {
        popUpTextAnimator.SetBool("isDamaged", false);
        rect.anchoredPosition = defaultPosition;
    }
    public void EndHealPopUpAnimation()
    {
        popUpTextAnimator.SetBool("isHealed", false);
        rect.anchoredPosition = defaultPosition;
    }
    public void RandomHorizontalPos() {
        defaultPosition = rect.anchoredPosition;
        rect.anchoredPosition += Vector2.left * Random.RandomRange(-2f, 2f);
    }
}
