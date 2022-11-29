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
         // image �� ��µǸ� stop cocoutine �� �ߵ��˴ϴ�.
        if (fadeAwayImage != null)
            StopCoroutine(fadeAwayImage);

         // activate the image
         // //damage �� ������ image�� Ȱ��ȭ�մϴ�.
        damageImage.enabled = true;
         // put the image alpha to white(See the image)
         // image alpha ����(��) white �� �ֽ��ϴ�.(�̹��� ����)
        damageImage.color = Color.white;
         // call couroutine function
         // �ڷ�ƾ �Լ��� ȣ���մϴ�.
        fadeAwayImage = StartCoroutine(FadeAwayImage());
    }

    IEnumerator FadeAwayImage()
    {
        float imageAlpha = 1.0f;

         // loop through image alpha and if its 0 then
         // image alpha �� loop �ϰ� 0�� ���
        while (imageAlpha > 0.0f)
        {
             // change the alpha back slowly
             // alpha ����(��)  õõ�� �ٲߴϴ�.
            imageAlpha -= (1.0f / flashSpeed) * Time.deltaTime;
            damageImage.color = new Color(1.0f, 1.0f, 1.0f, imageAlpha);
            yield return null;
        }

        damageImage.enabled = false;
    }
}
