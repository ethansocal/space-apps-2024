using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactables.Visuals;

public class Sky : MonoBehaviour
{
    public GameObject vrPerson;

    public GameObject starPrefab;
    public GameObject linePrefab;

    public float radius = 1.1f;

    public XRInteractionManager interactionManager;

    public float lookSpeed = 1.0f;

    private readonly List<Star> selectedStars = new();

    private LineRenderer currentLine;

    private Star[] stars;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        transform.position = vrPerson.transform.position;
    }

    private void Start()
    {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        stars = new Star[350];
        for (var i = 0; i < stars.Length; i++)
        {
            stars[i].position = Random.onUnitSphere / radius / 2f;
            stars[i].radius = Random.Range(0.1f, 1.0f); // Example radius range
            stars[i].color = new Color(Random.value, Random.value, Random.value); // Random color
        }

        for (var i = 0; i < stars.Length; i++)
        {
            var star = stars[i];
            var instance = Instantiate(starPrefab, transform, true);
            instance.transform.localPosition = star.position;
            var component = instance.AddComponent<XRSimpleInteractable>();
            component.interactionManager = interactionManager;
            component.selectEntered.AddListener(arg0 => StarSelected(i));
            instance.AddComponent<XRTintInteractableVisual>();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = vrPerson.transform.position;
        transform.Rotate(new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * lookSpeed, 0,
            Input.GetAxis("Vertical") * Time.deltaTime * lookSpeed));
    }

    private void StarSelected(int starIndex)
    {
        if (currentLine == null) currentLine = Instantiate(linePrefab, transform).GetComponent<LineRenderer>();
        selectedStars.Add(stars[starIndex]);
        currentLine.positionCount = selectedStars.Count;
        currentLine.SetPosition(selectedStars.Count - 1, stars[starIndex].position);

        Debug.Log("Star " + starIndex + " selected");
    }
}

public struct Star
{
    public Vector3 position;
    public float radius;
    public Color color;
}