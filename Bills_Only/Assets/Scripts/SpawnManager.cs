using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] cubePrefabs;
    [SerializeField] ColumnController[] columnControllers;

    int oldRandomCube;
    void Start()
    {
        float offSetz = 0;
        float offSety = 0;

        int randomCube = Random.Range(0, cubePrefabs.Length);
        int randomNum = Random.Range(1, 5);

        for (int j = 0; j <= columnControllers.Length -1; j++)
        {
            for (int i = 0; i <= randomNum; i++)
            {
                if(oldRandomCube == randomCube)
                {
                    if(oldRandomCube == 0)
                    {
                        randomCube = Random.Range(1, cubePrefabs.Length);
                    }
                    else
                    {
                        randomCube = Random.Range(0, randomCube);
                    }
                }

                GameObject newObj = Instantiate(
                        cubePrefabs[randomCube],
                        new Vector3(columnControllers[j].transform.position.x, columnControllers[j].transform.position.y + offSety, columnControllers[j].transform.position.z + offSetz),
                        Quaternion.identity);

                newObj.transform.parent = this.transform;

                columnControllers[j].objects.Add(newObj.transform);

                offSetz -= 0.5f;
                offSety += +0.25f;

                oldRandomCube = randomCube;
                randomCube = Random.Range(0, cubePrefabs.Length);

            }

            randomNum = Random.Range(1, 5);
            columnControllers[j].OrganizeList();
        }

    }
}
