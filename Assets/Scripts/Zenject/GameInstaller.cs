using UnityEngine;
using Zenject;

// Sets up the fireball and laser systems
public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // Fireball system
        Container.Bind<IFireballBuilder>().To<FireballBuilder>().AsSingle();
        Container.Bind<FireballDirector>().AsSingle();
        Container.Bind<FireballPoolManager>().FromComponentInHierarchy().AsSingle();

        // Laser system
        Container.Bind<ILaserBuilder>().To<LaserBuilder>().AsSingle();
        Container.Bind<LaserDirector>().AsSingle();
        Container.Bind<LaserPoolManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<LaserFactory>().FromComponentInHierarchy().AsSingle();
    }
}
