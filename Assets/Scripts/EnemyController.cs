using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private List<GameObject> barrelCats;
    [SerializeField] private List<GameObject> windowEnemies;
    [SerializeField] private List<GameObject> rats;
    [SerializeField] private GameObject dogPrefab;
    [SerializeField] private GameObject bottlePrefab;
    [SerializeField] private GameObject player;

    private GameObject dog = null;
    private Dog dogScript = null;

    private GameObject bottle = null;
    private Bottle bottleScript = null;

    [SerializeField] private List<Rat> ratsScripts;
    [SerializeField] private List<BarrelCat> barrelCatsScripts;
    
    private List<bool> activeBarrelCats;

    private void Start() {
        for (int i = 0; i < rats.Count; ++i)
        {
            ratsScripts.Add(rats[i].GetComponent<Rat>());
        }
        for (int i = 0; i < barrelCats.Count; ++i)
        {
            barrelCatsScripts.Add(barrelCats[i].GetComponent<BarrelCat>());
        }
        activeBarrelCats = new List<bool>(new bool[barrelCats.Count]);

        StartCoroutine(BlackCatActivity());
        StartCoroutine(BlackCatActivity());
    
        StartCoroutine(DogActivity());

        StartCoroutine(WindowActivity());
    }

    private void FixedUpdate()
    {
        float deltaTime = Time.deltaTime;
        for (int i = 0; i < rats.Count; ++i)
        {
            ratsScripts[i].Activity(deltaTime);
        }

        if (dog is not null) {
            bool runOut = dogScript.Activity(deltaTime);
            if (runOut) {
                Destroy(dog);
                dogScript = null;
                dog = null;
            }
        }

        if (bottle is not null) {
            bool runOut = bottleScript.Activity(deltaTime);
            if (runOut) {
                Destroy(bottle);
                bottleScript = null;
                bottle = null;
            }
        }
    }

    IEnumerator WindowActivity()
    {
        while (true) {
            if (bottle is null) {
                yield return new WaitForSeconds(Random.Range(2, 5));

                int windowNumber = Random.Range(0, windowEnemies.Count);
                windowEnemies[windowNumber].SetActive(true);

                bottle = Instantiate(bottlePrefab);
                bottle.transform.position = windowEnemies[windowNumber].transform.position + new Vector3(0, -0.4f, 0);

                bottleScript = bottle.GetComponent<Bottle>();
                Vector3 direction = player.transform.position - windowEnemies[windowNumber].transform.position;
                float length = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);
                bottleScript.SetDirection(direction.x / length, direction.y / length);

                yield return new WaitForSeconds(1);

                windowEnemies[windowNumber].SetActive(false);
            } else {
                yield return new WaitForSeconds(1);
            }
        }        
    }

    IEnumerator DogActivity()
    {
        while (true) {
            if (dog is null) {
                yield return new WaitForSeconds(Random.Range(0, 6));
                bool isRunningLeft = Random.Range(0, 2) == 1;
                dog = Instantiate(dogPrefab);
                dog.transform.position = new Vector3(isRunningLeft ? 10 : -10, -3.3f, 0);
                dogScript = dog.GetComponent<Dog>();
                dogScript.Turn(isRunningLeft);
            } else {
                yield return new WaitForSeconds(1);
            }
        }        
    }

    IEnumerator BlackCatActivity()
    {
        while (true) {
            int catNumber = -1;
            while (true) {
                catNumber = Random.Range(0, barrelCats.Count);
                if (!activeBarrelCats[catNumber]) {
                    activeBarrelCats[catNumber] = true;
                    barrelCats[catNumber].SetActive(true);
                    break;
                }
            }
            StartCoroutine(barrelCatsScripts[catNumber].Up());
            yield return new WaitForSeconds(2);
            StartCoroutine(barrelCatsScripts[catNumber].Down());
            yield return new WaitForSeconds(3);
            activeBarrelCats[catNumber] = false;
        }        
    }
}
