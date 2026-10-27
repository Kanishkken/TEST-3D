using UnityEngine;

public class Rifle : Gun
{
    private void Awake()
    {
        fireMode = FireMode.Single;
        damage = 50f;
        range = 200f;
        fireRate = 3f;
    }
}
