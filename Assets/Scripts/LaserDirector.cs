using UnityEngine;

// Tells the builder how to make a laser
public class LaserDirector
{
    private ILaserBuilder _builder; // The builder that knows how to make a laser

    // Give the director a builder to work with
    public LaserDirector(ILaserBuilder builder)
    {
        _builder = builder;
    }

    // Build the laser step by step using the builder
    public GameObject Construct(GameObject laserPrefab)
    {
        _builder.SetSpeed();       
        _builder.SetLifeTime();   
        return _builder.Build(laserPrefab); // Build and return the final laser
    }
}
