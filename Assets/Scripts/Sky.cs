using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
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
    public TextMeshPro text;

    private readonly List<Star> selectedStars = new();

    private readonly List<Star> stars = new();

    private LineRenderer currentLine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        transform.position = vrPerson.transform.position;
    }

    private void Start()
    {
        StartCoroutine(GetStars("11 Com b"));
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
        currentLine.SetPosition(selectedStars.Count - 1, stars[starIndex].position.normalized);

        Debug.Log("Star " + starIndex + " selected");
    }

    private IEnumerator GetStars(string planetName)
    {
        var www = UnityWebRequest.Get("http://localhost:5000/get_stars/" + planetName);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
            Debug.Log(www.error);
        else
            Debug.Log(www.downloadHandler.text);
        foreach (var starData in www.downloadHandler.text.Split("\n"))
            try
            {
                float position_x, position_y, position_z, brightness, color;
                if (float.TryParse(starData.Split(",")[3], out position_x) &&
                    float.TryParse(starData.Split(",")[4], out position_y) &&
                    float.TryParse(starData.Split(",")[5], out position_z))
                    stars.Add(new Star
                    {
                        position = new Vector3(position_x, position_y, position_z),
                        gaia_id = starData.Split(",")[0],
                        color = float.TryParse(starData.Split(",")[2], out color) ? color : 0f,
                        brightness = float.TryParse(starData.Split(",")[1], out brightness) ? brightness : 0f
                    });
            }
            catch
            {
            }

        foreach (Transform child in transform) Destroy(child.gameObject);

        for (var i = 0; i < stars.Count; i++)
        {
            var star = stars[i];
            var instance = Instantiate(starPrefab, transform, true);
            instance.transform.localPosition = star.position.normalized / radius / 2f;
            instance.transform.localScale *= star.brightness;
            var component = instance.AddComponent<XRSimpleInteractable>();
            component.interactionManager = interactionManager;
            var current = i;
            component.selectEntered.AddListener(arg0 => StarSelected(current));
            component.hoverEntered.AddListener(arg1 => text.text = star.gaia_id);
            Debug.Log("Added listener");

            instance.AddComponent<XRTintInteractableVisual>();
        }
    }
}

public struct Star
{
    public Vector3 position;
    public float color;
    public float brightness;
    public string gaia_id;
}