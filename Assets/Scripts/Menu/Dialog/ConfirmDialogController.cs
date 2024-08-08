using UnityEngine;
using UnityEngine.UI;

public abstract class ConfirmDialogController : MonoBehaviour
{
    [SerializeField] private TextLocalized caption;
    [SerializeField] private TextLocalized body;
    [SerializeField] private Button Yes;
    [SerializeField] private Button No;

    public void Init(ConfirmDialogParam param)
    {
        if (caption != null)
        {
            caption.SetText(param.Caption);
        }

        if (body != null)
        {
            body.SetText(param.Body);
        }

        Yes.onClick.AddListener(() =>
        {
            param.Yes.Invoke();
            Destroy(gameObject);
        });
        No.onClick.AddListener(() =>
        {
            param.No.Invoke();
            Destroy(gameObject);
        });
        if (param.YesText != null)
        {
            Yes.GetComponentInChildren<TextLocalized>().SetText(param.YesText);
        }

        if (param.NoText != null)
        {
            No.GetComponentInChildren<TextLocalized>().SetText(param.NoText);
        }
    }
}