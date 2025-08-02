using UnityEngine;
using VContainer;

public class AxeFactory : MonoBehaviour
{
    [SerializeField] private GameObject _axePrefab;

    private AxeDirector _axeDirector;

    [Inject]
    public void Construct(AxeDirector director)
    {
        _axeDirector = director;
    }

    public ProjectileAxe CreateAxe()
    {
        GameObject axe = _axeDirector.Construct(_axePrefab);
        return axe.GetComponent<ProjectileAxe>();
    }
}
