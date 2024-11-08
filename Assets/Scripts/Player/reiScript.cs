using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool modoFantasma = false;

    public Collider2D jogadorCollider;

    private bool aterrado = false;

    private Collider2D paredeAtual;
    

    void Start()
    {
        estaminaAtual = estaminaMax;
    }

    void Update()
    {
        // Verifica se a barra de espaço está pressionada
        modoFantasma = Input.GetKey(KeyCode.Space) && estaminaAtual > 0;
        
        if(dentroParede && !modoFantasma)
        {
            expulsarParede();
        }

        // Movimentação Horizontal
        if (Input.GetKey(KeyCode.A))
        {
            velX -= acel * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            velX += acel * Time.deltaTime;
        }
        else
        {
            velX = Mathf.Lerp(velX, 0, 5 * Time.deltaTime);
        }

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

        // Consumo de estamina no modo fantasma
        if (modoFantasma)
        {
            estaminaAtual -= drenoEstamina * Time.deltaTime;

            if (estaminaAtual <= 0 && dentroParede)
            {
                expulsarParede();
                modoFantasma = false;
            }
        }
        else
        {
            // Regenera a estamina quando fora do modo fantasma
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
        if (collision.CompareTag("objeto") && modoFantasma)
        {
            dentroParede = true;
            paredeAtual = collision;
            Physics2D.IgnoreCollision(collision, jogadorCollider, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("objeto"))
        {
            dentroParede = false;
            paredeAtual = null;
            Physics2D.IgnoreCollision(collision, jogadorCollider, false);
        }
    }

    private void expulsarParede()
    {
        if (paredeAtual != null)
    {
        // Calcula a direção horizontal para fora da parede
        Vector3 direcao = (transform.position.x - paredeAtual.bounds.center.x) >= 0 ? Vector3.right : Vector3.left;

        // Calcula a distância na horizontal com base na largura do colisor da parede e do jogador
        float distanciaX = paredeAtual.bounds.extents.x + jogadorCollider.bounds.extents.x;

        // Define a nova posição do jogador fora da parede, apenas na direção horizontal
        transform.position = new Vector3(
            paredeAtual.bounds.center.x + direcao.x * (distanciaX + 0.5f),
            transform.position.y,
            transform.position.z
        );
    }
        dentroParede = false;
    }
}