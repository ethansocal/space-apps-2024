using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Sky : MonoBehaviour
{
    public GameObject vrPerson;

    public GameObject starPrefab;

    public float radius = 1.1f;
    
    public XRInteractionManager interactionManager;

    public float lookSpeed = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 350; i++)
        {
            var instance = Instantiate(starPrefab, transform, true);
            instance.transform.localPosition = Random.onUnitSphere / radius / 2f;
            var component = instance.AddComponent<XRSimpleInteractable>();
            component.interactionManager = interactionManager;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = vrPerson.transform.position;
        transform.Rotate(new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * lookSpeed, 0, Input.GetAxis("Vertical") * Time.deltaTime * lookSpeed));
    }
}
