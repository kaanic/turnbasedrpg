using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class BattleVisuals : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI levelText;

    private int currentHealth;
    private int maxHealth;
    private int level;

    private Animator anim;

    private const string LEVEL_STR = "Lvl: ";

    private const string ATTACK_PARAMETER = "IsAttack";
    private const string IS_HIT_PARAMETER = "IsHit";
    private const string IS_DEAD_PARAMETER = "IsDead";

    void Start() 
    {
        anim = gameObject.GetComponent<Animator>();
        SetStartingValues(25, 25, 99);
        PlayAttackAnimation();
    }

    public void SetStartingValues(int currentHealth, int maxHealth, int level)
    {
        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;
        this.level = level;

        levelText.text = LEVEL_STR + this.level.ToString();

        UpdateHealthBar();
    }

    public void ChangeHealth(int currentHealth)
    {
        this.currentHealth = currentHealth;

        if (currentHealth <= 0)
        {
            PlayDeathAnimation();
            Destroy(gameObject, 1f);
        }

        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    public void PlayAttackAnimation()
    {
        anim.SetTrigger(ATTACK_PARAMETER);
    }

    public void PlayHitAnimation()
    {
        anim.SetTrigger(IS_HIT_PARAMETER);
    }

    public void PlayDeathAnimation()
    {
        anim.SetTrigger(IS_DEAD_PARAMETER);
    }
}
