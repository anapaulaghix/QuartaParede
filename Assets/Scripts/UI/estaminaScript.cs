using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class estaminaScript : MonoBehaviour
{
    [SerializeField] private Image EstaminaVerde;
    [SerializeField] private Image EstaminaVermelha;  

    public GameObject estamina;

    public GameObject jogadorObj;


    public reiScript jogadorScript;
    // Start is called before the first frame update
    void Start()
    {
        if (jogadorObj == null)
        {
            jogadorObj = GameObject.FindWithTag("Player");
            jogadorScript = jogadorObj.GetComponent<reiScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        estamina.transform.position = new Vector3(jogadorObj.transform.position.x + 1f, jogadorObj.transform.position.y + 0.5f);

        if (jogadorObj != null && EstaminaVerde != null && EstaminaVermelha != null)
        {
            if(jogadorScript.modoFantasma)
            {
                EstaminaVermelha.fillAmount = (jogadorScript.estaminaAtual / jogadorScript.estaminaMax + 0.07f);
            }
        }
        else
        {
            EstaminaVermelha.fillAmount = (jogadorScript.estaminaAtual / jogadorScript.estaminaMax);
        }

        EstaminaVerde.fillAmount = (jogadorScript.estaminaAtual / jogadorScript.estaminaMax);

    }
}
