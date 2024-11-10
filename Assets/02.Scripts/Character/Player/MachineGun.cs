using UnityEngine;

public class MachineGun : Weapon
{
    public ParticleSystem FireEffect;
    public override void Using(Character target)
    {
        base.Using(target);
        FireEffect.Play();
    }
}
