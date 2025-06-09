using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatText : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<TextMeshPro>().DOFade(1f,3f).From();
        transform.DOScale(0, .2f).From().SetEase(Ease.OutBack);
        transform.DOMoveY(transform.position.y + 1f, 5f).OnComplete(() => Destroy(gameObject));
    }
}
