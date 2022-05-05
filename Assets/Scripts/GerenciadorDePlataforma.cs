using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorDePlataforma : MonoBehaviour
{
    public GameObject[] plataformas; //Guarda informações dos modelos da plataforma
    public Transform jogador;

    private float spawnZ = 0; //Onde instanciar a próxima plataforma
    private float tamanhoPlat = 406f; //Tamanho da plataforma (Sujeito a Mudanças)
    private int maxNaTela = 2; //Máximo de plataformas aparecendo na tela


    // Start is called before the first frame update
    void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < maxNaTela; i++)
        {
            SpawnPlat(); //Cria a plataforma inicial
        }
    }

    void SpawnPlat(int prefabIndex = -1)
    {
        GameObject go;
        go = Instantiate(plataformas[0]) as GameObject; //Instancia a plataforma
        go.transform.SetParent(transform); //Torna a plataforma filha do gerenciador
        go.transform.position = new Vector3(-6, 0, spawnZ); // Move a plataforma para frente
        spawnZ += tamanhoPlat;
    }

    // Update is called once per frame
    void Update()
    {
        if (jogador.position.z > (spawnZ - maxNaTela * tamanhoPlat))
        {
            SpawnPlat(); //Cria uma nova plat. assim que o player termina o percurso da plat. inicial
        }
    }
}
