﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    private enum TipoAtaque
    {
        rapido,
        fuerte
    }

    [SerializeField] private float radiusAttack;
    [SerializeField] private int comboAttack;
    [SerializeField] private float comboTimeLimit;
    [SerializeField] private LayerMask enemigoLayer;

    [SerializeField] private Transform attackChecker; //Transform Player
                                                      //[SerializeField] private Vector2 weaponSize; 
    //[SerializeField] private Rect weaponSize; //tamaño arma
    private Vector2 origen; //Origen del ataque

    private int damage;
    private TipoAtaque ataque;
    private float time;  
    private bool completado;
    private float duracionAtaqueRapido;
    private float realizarAtaqueRapido;

    public static bool attack;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {       
        animator = GetComponent<Animator>();

        attack = false;
        completado = true;
        duracionAtaqueRapido = 1f;
        realizarAtaqueRapido = 0.7f;

        radiusAttack = 0.5f;

        //weaponSize.width = 0.5f;
        //weaponSize.height = 0.1f;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && completado)
        {   //Left mouse
            completado = false;
            AtaqueRapido();
        }


        else if (Input.GetButtonDown("Fire2") && completado)
        {   //Right mouse
            completado = false;
            AtaqueFuerte();
        }

        if (time > comboTimeLimit && comboAttack != 0)
        {
            comboAttack = 0;
            time = 0;
        }
        else
        {
            time += Time.fixedDeltaTime;
        }
       
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        origen = new Vector2(
            transform.position.x - 0.1f,
            transform.position.y);

        Gizmos.DrawWireSphere(origen, radiusAttack);
    }

    private void CheckAttack()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y);

        origen = new Vector2(
            transform.position.x + 0.1f,
            transform.position.y);

        var rayo = Physics2D.RaycastAll(origen, direction, radiusAttack, 1 << LayerMask.NameToLayer("Enemy"));
        
        foreach (var x in rayo) {
            if (x.collider.gameObject.tag.Equals("Enemy")) {
                Debug.Log(x.collider.gameObject.name);
                Damaged.golpeado = true; //Enemigo hace la animacion del golpe
            }
        }
            

        /*Collider2D[] enemigos = Physics2D.OverlapCircleAll(
            attackChecker.position, radiusAttackCheck, enemigoLayer);

        if (enemigos.Length != 0)
        {
            Damaged.golpeado = true; //Enemigo hace la animacion del golpe
        }*/
    }

    private void AtaqueRapido()
    {
        ataque = TipoAtaque.rapido;

        //Animacion ataque
        attack = true;
        animator.SetBool("Attack", attack);
        StartCoroutine(PararAtaque());

        StartCoroutine(RealizarAtaque());
    }

    private void AtaqueFuerte()
    {
        ataque = TipoAtaque.fuerte;

        //Animacion ataque
        attack = true;
        animator.SetBool("Attack", attack);
        StartCoroutine(PararAtaque());

        StartCoroutine(RealizarAtaque());
    }

    IEnumerator PararAtaque()
    {

        yield return new WaitForSeconds(duracionAtaqueRapido);

        attack = false;
        completado = true;
        animator.SetBool("Attack", attack);
    }

    IEnumerator RealizarAtaque()
    {

        yield return new WaitForSeconds(realizarAtaqueRapido);

        CheckAttack();
    }

}
