using UnityEngine;
using UnityEngine.InputSystem;
public class GameMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject menu;
    public InputActionProperty showButton;
    public Transform head;
    public float spawnDistance = 2;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(showButton.action.WasPressedThisFrame()) {
            menu.SetActive(!menu.activeSelf);

            menu.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistance;
        }    
        menu.transform.LookAt(new Vector3(head.position.x, menu.transform.position.y, head.position.z));
        menu.transform.forward *= -1;
    }
}
