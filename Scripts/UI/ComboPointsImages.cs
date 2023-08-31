using UnityEngine;
using UnityEngine.UI;

public class ComboPointsImages : MonoBehaviour
{
    public static ComboPointsImages instance;

    [SerializeField] private Image numberImage;

    private void Awake()
    {
        instance = this;
        numberImage.enabled = false;
    }

    public void SetImage(int number)
    {
        numberImage.enabled = true;

        var _sprite = Resources.Load<Sprite>("UI/Combo/" + number + "combo");

        numberImage.sprite = _sprite;
    }

    public void RemoveImage()
    {
        numberImage.enabled = false;
    }
}
