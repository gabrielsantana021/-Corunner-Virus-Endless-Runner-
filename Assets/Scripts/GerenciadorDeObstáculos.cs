using UnityEngine;

public class GerenciadorDeObstáculos : MonoBehaviour
{
	public GameObject[] obstacles;
	public Transform jogador;
	public float spawnZ = 20f, tamanhoObstaculo;

	// Use this for initialization
	void Start()
	{
		jogador = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void SpawnObstacles(int prefabIndex = -1)
	{
		int position = Random.Range(0, 2);
		GameObject go;
		go = Instantiate(obstacles[Random.Range(0, 5)]) as GameObject;
		go.transform.SetParent(transform);
		if(position == 0)
        {
			go.transform.position = new Vector3(-2, 0, spawnZ);
        }
		else if (position == 1)
        {
			go.transform.position = new Vector3(0, 0, spawnZ);
		}
		else if (position ==2)
        {
			go.transform.position = new Vector3(2, 0, spawnZ);
		}
		spawnZ += tamanhoObstaculo;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			other.GetComponent<Player>().IncreaseSpeed();
			transform.position = new Vector3(0, 0, 0);
		}
	}

	void Update()
	{
		if (jogador.position.z < spawnZ + 10)
		{
			SpawnObstacles();
		}
	}
}