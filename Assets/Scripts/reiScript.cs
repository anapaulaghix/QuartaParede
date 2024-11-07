using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reiScript : MonoBehaviour
{

    public float acel = 10f; //aceleração  
    public float velMax = 5f;   //velocidade maxima     
    public float pulo = 15f;      // força do pulo
    public float gravidade = -9.8f;      //gravidade
    private float velX = 0f;   //velocidade do eixo vertical
    private float velY = 0f;   //velocidade do eixo horizontal

    private bool aterrado = true;  //verifica se ele esta no chao  

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Verifica se esta apertando A
        if (Input.GetKey(KeyCode.A))
        {
            velX -= acel * Time.deltaTime;
        }
        //Verifica se esta apertando D
        else if (Input.GetKey(KeyCode.D))
        {
            velX += acel * Time.deltaTime;
        }
        //Se não esta apertando nada ele desacelera
        else
        {
            velX = Mathf.Lerp(velX, 0, 5 * Time.deltaTime);
        }

        //Limite de velocidade com o velMax
        velX = Mathf.Clamp(velX, -velMax, velMax);

        //Verifica se esta apertando W enquanto esta no chao
        if (Input.GetKeyDown(KeyCode.W) && aterrado)
        {
            velY = pulo; 
            aterrado = false;
        }

        //Verifica se o jogador esta no ar
        if (!aterrado)
        {
            velY += gravidade * Time.deltaTime;
        }

        //Atualização de posição
        transform.position += new Vector3(velX, velY, 0) * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("chao"))
        {
            aterrado = true;
            velY = 0f;
        }
    }
}
