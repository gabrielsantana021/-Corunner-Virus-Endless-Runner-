using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public int coins;
    public int[] characterCost;
    public int characterIndex;

    private MissionBase[] missions;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
        else if (gm != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        missions = new MissionBase[2];

        for (int i = 0; i < missions.Length; i++)
        {
            GameObject newMission = new GameObject("Mission" + i);
            newMission.transform.SetParent(transform);
            MissionType[] missionType = { MissionType.SingleRun, MissionType.TotalMeters, MissionType.CoinsSingleRun };
            int randomType = Random.Range(0, missionType.Length);
            if (randomType == (int)MissionType.SingleRun)
            {
                missions[i] = newMission.AddComponent<SingleRun>();
            }
            else if (randomType == (int)MissionType.TotalMeters)
            {
                missions[i] = newMission.AddComponent<TotalMeters>();
            }
            else if (randomType == (int)MissionType.CoinsSingleRun)
            {
                missions[i] = newMission.AddComponent<CoinsSingleRun>();
            }

            missions[i].Created();
        }

    }
    public void StartRun(int charIndex)
    {
        characterIndex = charIndex;
        SceneManager.LoadScene("Corunner Virus");
    }

    public void EndRun()
    {
        SceneManager.LoadScene("Menu");
    }

    public MissionBase GetMission(int index)
    {
        return missions[index];
    }

    public void StartMissions()
    {
        for (int i = 0; i < 2; i++)
        {
            missions[i].RunStart();
        }
    }

    public void GenerateMission(int i)
    {
        Destroy(missions[i].gameObject);

        GameObject newMission = new GameObject("Mission" + i);
        newMission.transform.SetParent(transform);
        MissionType[] missionType = { MissionType.SingleRun, MissionType.TotalMeters, MissionType.CoinsSingleRun };
        int randomType = Random.Range(0, missionType.Length);
        if (randomType == (int)MissionType.SingleRun)
        {
            missions[i] = newMission.AddComponent<SingleRun>();

        }
        else if (randomType == (int)MissionType.TotalMeters)
        {
            missions[i] = newMission.AddComponent<TotalMeters>();

        }
        else if (randomType == (int)MissionType.CoinsSingleRun)
        {
            missions[i] = newMission.AddComponent<CoinsSingleRun>();

        }

        missions[i].Created();

        FindObjectOfType<Menu>().SetMission();
    }
}