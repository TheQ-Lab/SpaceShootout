﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProjectileSelection : MonoBehaviour
{
    [System.Serializable]
    public class ProjectileIconSet
    {
        public Image ImageGameObject;
        public Sprite DeselectedIcon;
        public Sprite SelectedIcon;
        public Projectile.ProjectileTypeEnum ProjectileType;

        ProjectileIconSet(Image ImageGameObject, Sprite DeselectedIcon, Sprite SelectedIcon, Projectile.ProjectileTypeEnum ProjectileType)
        {
            this.ImageGameObject = ImageGameObject;
            this.DeselectedIcon = DeselectedIcon;
            this.SelectedIcon = SelectedIcon;
            this.ProjectileType = ProjectileType;
        }

        public ProjectileIconSet zero()
        {
            return new ProjectileIconSet(null, null, null, Projectile.ProjectileTypeEnum.Bomb);
        }
    }

    public List<ProjectileIconSet> Icons;


    public void SelectIcon(Projectile.ProjectileTypeEnum newProjectileType)
    {
        foreach(ProjectileIconSet I in Icons)
        {
            if (I.ProjectileType == newProjectileType)
                I.ImageGameObject.sprite = I.SelectedIcon;
            else
                I.ImageGameObject.sprite = I.DeselectedIcon;
        }
    }
}
