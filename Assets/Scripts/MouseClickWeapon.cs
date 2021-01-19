using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseClickWeapon : MonoBehaviour, IPointerClickHandler
{
    public Projectile.Type ProjectileType;
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        ProjectileCreator pc = ProjectileCreator.Instance;
        if (pointerEventData.pointerId == -1)
        {
            //Debug.Log(name + " Game Object Clicked!");
            pc.SetCurrentProjectile(ProjectileType);
        }
            
    
    }
}
