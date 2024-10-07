using UnityEngine;
using TMPro;

public class SaveSettings : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadSettings();
        dropdown.onValueChanged.AddListener(delegate {  Save(); });
    }

    public void Save()
    {
        PlayerPrefs.SetInt("SelectedDropdownValue", dropdown.value);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("SelectedDropdownValue"))
        {
            dropdown.value = PlayerPrefs.GetInt("SelectedDropdownValue");
        }
    }
}