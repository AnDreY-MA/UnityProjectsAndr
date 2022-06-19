using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite heartFull;
    [SerializeField] private Sprite heartHalf;
    [SerializeField] private Sprite heartEmpty;
    //[SerializeField] private int maxHearts;

    public float maxHearts;

    private void Start()
    {
        InitHearts();
    }

    private void Update()
    {
        ChangeHearts();
    }

    private void InitHearts()
    {
        for (int i = 0; i < maxHearts; i++)
        {
            hearts[i].gameObject.SetActive(true);
            hearts[i].sprite = heartFull;
        }
    }

    private void ChangeHearts()
    {
        float health = player.currentHealth;

        for (int i = 0; i < maxHearts; i++)
        {
            
            hearts[i].gameObject.SetActive(true);

            if (health > 1)
                hearts[i].sprite = heartFull;
            else if (health == 1)
                hearts[i].sprite = heartHalf;
            else
                hearts[i].sprite = heartEmpty;          

            health -= 2;
        }
    }

}
