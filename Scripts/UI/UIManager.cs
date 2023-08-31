using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image HpFill;
    [SerializeField] private TextMeshProUGUI HpText;

    [SerializeField] private Image MpFill;
    [SerializeField] private TextMeshProUGUI MpText;

    [SerializeField] private PlayerStatus status;


    void Update()
    {
        HpFill.fillAmount = (float) status.HP / (float)status.MaxHP;
        HpText.text = status.HP + "/" + status.MaxHP;

        MpFill.fillAmount = status.MP / (float)status.MaxMP;
        MpText.text = status.MP + "/" + status.MaxMP;
    }
}
