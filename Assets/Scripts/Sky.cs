using UnityEngine;

public class Sky : MonoBehaviour
{
    public GameObject vrPerson;

    public GameObject starPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 350; i++)
        {
            var instance = Instantiate(starPrefab, transform, true);
            instance.transform.localPosition = Random.onUnitSphere / 2.05f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = vrPerson.transform.position;
    }
}
