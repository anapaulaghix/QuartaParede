using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class reiScript : MonoBehaviour
{
    // Configurações de Movimento e Estamina
    public float acel = 10f;
    public float velMax = 5f;
    public float pulo = 15f;
    public float gravidade = -9.8f;
    private float velX = 0f;
    private float velY = 0f;

    public float estaminaMax = 100f;
    public float estaminaAtual;
    public float drenoEstamina = 20f;
    public bool dentroParede = false;

    public Collider2D jogadorCollider;

    private bool aterrado = false;
    private Vector3 sentido = Vector3.left;

    void Start()
    {
        estaminaAtual = estaminaMax;
    }

    void Update()
    {
        // Movimentação Horizontal
        if (Input.GetKey(KeyCode.A))
            velX -= acel * Time.deltaTime;
        else if (Input.GetKey(KeyCode.D))
            velX += acel * Time.deltaTime;
        else
            velX = Mathf.Lerp(velX, 0, 5 * Time.deltaTime);

        velX = Mathf.Clamp(velX, -velMax, velMax);

        // Pulo
        if (Input.GetKeyDown(KeyCode.W) && aterrado)
        {
            velY = pulo;
            aterrado = false;
        }

        if (!aterrado)
            velY += gravidade * Time.deltaTime;

        // Atualiza a posição do jogador
        transform.position += new Vector3(velX, velY, 0) * Time.deltaTime;

        // Consumo de estamina dentro da parede
        if (dentroParede)
        {
            estaminaAtual -= drenoEstamina * Time.deltaTime;


            if (estaminaAtual <= 0)
                expulsarParede();
        }
        else
        {
            estaminaAtual = Mathf.Min(estaminaAtual + drenoEstamina * Time.deltaTime, estaminaMax);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("chao"))
        {
            aterrado = true;
            velY = 0f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("chao"))
        {
            aterrado = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("objeto"))
        {
            dentroParede = true;
            Physics2D.IgnoreCollision(collision, jogadorCollider, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("objeto"))
        {
            dentroParede = false;
            Physics2D.IgnoreCollision(collision, jogadorCollider, false);
        }
    }

    private void expulsarParede()
    {
        transform.position += sentido * 0.5f;
        estaminaAtual = 0;
        dentroParede = false;
    }
}