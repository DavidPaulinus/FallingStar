using UnityEngine;

public class GhostTrail : MonoBehaviour
{
    [SerializeField] private float ghostDelay;
    [SerializeField] private GameObject ghost;
    [SerializeField] private Color[] ghostColors;

    private float ghostDelayCounter;
    private bool makeGhost = false;
    private int colorIndex = 0;

    private Color color;
    private float delay;

    private void Awake()
    {
        ghostDelayCounter = ghostDelay;
    }

    private void Update()
    {
        if (ghostDelayCounter <= 0 && makeGhost)
        {
            var _currentGhost = Instantiate(ghost, transform.position, transform.rotation);

            var _sprite = _currentGhost.GetComponent<SpriteRenderer>();
            _sprite.sprite = GetComponent<SpriteRenderer>().sprite;
            _sprite.color = color;

            ghostDelayCounter = ghostDelay - delay;

            Destroy(_currentGhost, .5f);
        }
        ghostDelayCounter -= Time.deltaTime;
    }

    public void MakeGhost(bool make)
    {
        makeGhost = make;
    }

    public void DecreaseGhostDelay(float value)
    {
        delay = value;
    }

    public void ResetColors()
    {
        colorIndex = 0;
        color = ghostColors[0];
    }
    public void NextColor()
    {
        colorIndex++;

        if (colorIndex >= ghostColors.Length) color = ghostColors[colorIndex];
        color = ghostColors[colorIndex];
    }
}
