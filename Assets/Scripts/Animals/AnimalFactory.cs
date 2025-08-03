using UnityEngine;

public class AnimalFactory : MonoBehaviour
{
    [SerializeField] private AnimalBase bluePrefab;
    [SerializeField] private AnimalBase redPrefab;
    [SerializeField] private AnimalBase greenPrefab;

    public AnimalBase CreateAnimal(AnimalType type)
    {
        switch (type)
        {
            case AnimalType.Blue: return Instantiate(bluePrefab);
            case AnimalType.Red: return Instantiate(redPrefab);
            case AnimalType.Green: return Instantiate(greenPrefab);
            default:
                Debug.LogError("[AnimalFactory] Unknown animal type.");
                return null;
        }
    }
}
