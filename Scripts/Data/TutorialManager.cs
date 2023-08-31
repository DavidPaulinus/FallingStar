using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private float andarTimer;
    [SerializeField] private InputManager input;
    private float andarCounterSegundos;

    private void Awake()
    {
        andarCounterSegundos = andarTimer;    
    }

    void Update()
    {
        var _input = input.RetriveXValue();

        if (_input == 0)
        {
            if (andarCounterSegundos <= 0)
            {
                //TO - DO
            }
            andarCounterSegundos -= Time.deltaTime;
        }else andarCounterSegundos = andarTimer;
    }
}
