using UnityEngine;

public struct DamageInfo//разнообразная информация о уроне 
{
    public int Damage;
    public Vector3 Direction;
    public float Force;
    public DamageInfo( int damage, Vector3 direction, float force)
    {
        Damage = damage;
        Direction = direction;
        Force = force;
    }
    
}