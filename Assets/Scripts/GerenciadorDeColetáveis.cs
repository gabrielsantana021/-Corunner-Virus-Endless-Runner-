using UnityEngine;

public class GerenciadorDeColetáveis : MonoBehaviour
{
	public GameObject[] coin;
	public Transform jogador;
	public float spawnZ = 20f, tamanhoCoins;

	// Use this for initialization
	void Start()
	{
		jogador = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void SpawnCoins()
	{

		int position = Random.Range(0, 2);
		GameObject go;
		go = Instantiate(coin[Random.Range(0, 5)]) as GameObject;
		go.transform.SetParent(transform);
		if (position == 0)
		{
			go.transform.position = new Vector3(-2, 0, spawnZ);
		}
		else if (position == 1)
		{
			go.transform.position = new Vector3(0, 0, spawnZ);
		}
		else if (position == 2)
		{
			go.transform.position = new Vector3(2, 0, spawnZ);
		}
		spawnZ += tamanhoCoins;
	}

	void Update()
	{
		if (jogador.position.z < spawnZ )
		{
			SpawnCoins();
		}
	}
}