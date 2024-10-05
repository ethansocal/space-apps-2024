using UnityEngine;
using UnityEngine.UIElements;

public class ExoplanetSelection : MonoBehaviour
{
    public UIDocument uiDocument;
    public VisualTreeAsset exoplanetItem;
    public Exoplanet[] exoplanets;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var exoplanetList = uiDocument.rootVisualElement.Q<ListView>("exoplanet-list");
        exoplanetList.makeItem = () => new ExoplanetItem(exoplanetItem);
        exoplanetList.bindItem = (element, i) => (element as ExoplanetItem).Bind(exoplanets[i]);
        exoplanetList.itemsSource = exoplanets;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

class ExoplanetItem : VisualElement
{
    public ExoplanetItem(VisualTreeAsset exoplanetItem)
    {
        exoplanetItem.CloneTree(this);
    }
    
    public void Bind(Exoplanet exoplanet)
    {
        var nameLabel = this.Q<Label>("name");
        nameLabel.text = exoplanet.planetName;
        
        var distanceLabel = this.Q<Label>("distance");
        distanceLabel.text = exoplanet.distanceFromEarth.ToString();
        
        var massLabel = this.Q<Label>("mass");
        massLabel.text = exoplanet.mass.ToString();
        
        var radiusLabel = this.Q<Label>("radius");
        radiusLabel.text = exoplanet.radius.ToString();
    }
}