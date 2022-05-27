using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnController : MonoBehaviour
{
    public List<Transform> objects = new List<Transform>();

    [SerializeField] GameObject[] cubePrefabs;

    private void Awake()
    {

    }
    void Start()
    {
        SpawnObjects();
    }

    private void Update()
    {
        //OrganizeList();
    }
    private void SpawnObjects()
    {
        float offSetz = 0;
        float offSety = 0;

        for (int i = 0; i < cubePrefabs.Length; i++)
        {
            GameObject newObj = Instantiate(
                    cubePrefabs[i],
                    new Vector3(transform.position.x, transform.position.y + offSety, transform.position.z + offSetz),
                    Quaternion.identity);

            newObj.transform.parent = transform;

            objects.Add(newObj.transform);


            offSetz -= 0.5f;
            offSety += +0.1f;
        }

        OrganizeList();
    }
    public void OrganizeList()
    {
        float offSetz = 0;
        float offSety = 0;
        int i = 0;

        foreach (var obj in objects)
        {
            var objectController = obj.GetComponent<ObjectController>();

            obj.position = new Vector3(transform.position.x, transform.position.y + offSety, transform.position.z + offSetz);
            objectController.WhichColumn = this.transform;
            objectController.Row = i;

            offSetz -= 0.5f;
            offSety += +0.1f;
            i++;
        }

    }
    IEnumerator CheckList()
    {
        //eklenecek
        //indexe insert
        //oncraft eventi
        yield return null;
    }
}
