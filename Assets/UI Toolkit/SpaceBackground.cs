using UnityEngine;
using UnityEngine.UIElements;

public partial class SpaceBackground : VisualElement
{
    // StisualElementart is called once before the first execution of Update after the MonoBehaviour is created
    SpaceBackground()
    {
        generateVisualContent += GenerateVisualContent;
    }

    public void GenerateVisualContent(MeshGenerationContext context)
    {
        
    }
}
