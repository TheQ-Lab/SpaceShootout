using System.Collections;
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
        public Projectile.Type ProjectileType;

        ProjectileIconSet(Image ImageGameObject, Sprite DeselectedIcon, Sprite SelectedIcon, Projectile.Type ProjectileType)
        {
            this.ImageGameObject = ImageGameObject;
            this.DeselectedIcon = DeselectedIcon;
            this.SelectedIcon = SelectedIcon;
            this.ProjectileType = ProjectileType;
        }

        public ProjectileIconSet zero()
        {
            return new ProjectileIconSet(null, null, null, Projectile.Type.Bomb);
        }
    }

    public List<ProjectileIconSet> Icons;
    private Image explanation;

    void Start()
    {
        explanation = transform.Find("ExplanationWeaponChange").GetComponent<Image>();
    }

    public void SelectIcon(Projectile.Type newProjectileType)
    {
        foreach(ProjectileIconSet I in Icons)
        {
            if (I.ProjectileType == newProjectileType)
                I.ImageGameObject.sprite = I.SelectedIcon;
            else
                I.ImageGameObject.sprite = I.DeselectedIcon;
        }
    }

    public void ShowExplanation(float time)
    {
        explanation.enabled = true;
        StartCoroutine(DisableExplanation(time));

    }

    IEnumerator DisableExplanation(float time)
    {
        yield return new WaitForSeconds(time);
        explanation.enabled = false;
    }
}
