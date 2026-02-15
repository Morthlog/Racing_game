using UnityEngine;
using UnityEngine.SceneManagement;

public class CarManager : MonoBehaviour
{

    [SerializeField]
    GameObject[] carPrefabs;
    GameObject[] carPool;

    int carIntex = 0;
    private CarManager instance;
    GameObject chosenCar;
    IntroSceneManager sceneManager;

    [SerializeField]
    PlayerData playerData;
    void Start()
    {
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<IntroSceneManager>();
        carPool = new GameObject[carPrefabs.Length];
        
        for (int i = 0; i < carPrefabs.Length; i++)
        {
            carPool[i]= Instantiate(carPrefabs[i]);
            carPool[i].SetActive(false);
        }

        //enable first car
        carPool[carIntex].SetActive(true);
        playerData.chosenCar = carPrefabs[carIntex];
        sceneManager.SetButtons(carIntex, carPrefabs.Length);
        sceneManager.SetCarNameTxt(playerData.chosenCar.name);
    }

    public void SetNextCar()
    {
        if (carIntex + 1 == carPool.Length)
            return;
            
        carPool[carIntex].SetActive(false);

        carIntex++;
        carPool[carIntex].SetActive(true);
        playerData.chosenCar = carPrefabs[carIntex];
        sceneManager.SetButtons(carIntex, carPrefabs.Length);
        sceneManager.SetCarNameTxt(playerData.chosenCar.name);
    }

    public void SetPreviousCar()
    {
        if (carIntex - 1 < 0)
            return;
       
 
        carPool[carIntex].SetActive(false);

        carIntex--;
        carPool[carIntex].SetActive(true);
        playerData.chosenCar = carPrefabs[carIntex];
        sceneManager.SetButtons(carIntex, carPrefabs.Length);
        sceneManager.SetCarNameTxt(playerData.chosenCar.name);
    }



}
