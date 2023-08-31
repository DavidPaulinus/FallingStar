using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpUImanager : MonoBehaviour
{
    [SerializeField] private Image HpFill;

    private EnemyManager manager;
    private Canvas canvas;

    private void Awake()
    {
        manager = GetComponent<EnemyManager>();

        canvas = GetComponentInChildren<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.hp == manager.MaxHp)
        {
            canvas.enabled = false;
        } else canvas.enabled = true;

        HpFill.fillAmount = manager.hp/ manager.MaxHp;
    }
}
