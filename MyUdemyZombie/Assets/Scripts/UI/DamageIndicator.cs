using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Image damageImage;
    public float flashSpeed;
    private Coroutine fadeAwayImage;

    public void Flashing()
    {
         // if image exist then stop cocoutine
         // image 가 출력되면 stop cocoutine 가 발동됩니다.
        if (fadeAwayImage != null)
            StopCoroutine(fadeAwayImage);

         // activate the image
         // //damage 를 입으면 image를 활성화합니다.
        damageImage.enabled = true;
         // put the image alpha to white(See the image)
         // image alpha 값을(를) white 에 넣습니다.(이미지 참조)
        damageImage.color = Color.white;
         // call couroutine function
         // 코루틴 함수를 호출합니다.
        fadeAwayImage = StartCoroutine(FadeAwayImage());
    }

    IEnumerator FadeAwayImage()
    {
        float imageAlpha = 1.0f;

         // loop through image alpha and if its 0 then
         // image alpha 를 loop 하고 0인 경우
        while (imageAlpha > 0.0f)
        {
             // change the alpha back slowly
             // alpha 값을(를)  천천히 바꿉니다.
            imageAlpha -= (1.0f / flashSpeed) * Time.deltaTime;
            damageImage.color = new Color(1.0f, 1.0f, 1.0f, imageAlpha);
            yield return null;
        }

        damageImage.enabled = false;
    }
}
