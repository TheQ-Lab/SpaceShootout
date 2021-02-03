using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MMTutorial : MonoBehaviour
{
    public List<GameObject> TutSlides;
    public Button prevBtn, nextBtn;
    public Sprite prevBtnActive, prevBtnInactive, nextBtnActive, nextBtnInactive;

    public int CurrentSlide = 1;

    public void OnClickNext()
    {
        if(!(CurrentSlide+1 > TutSlides.Count))
        {
            TutSlides[CurrentSlide - 1].SetActive(false);
            TutSlides[CurrentSlide].SetActive(true);
            CurrentSlide++;
            if (CurrentSlide + 1 > TutSlides.Count)
                SetButtonState(nextBtn, false);
            SetButtonState(prevBtn, true);
        }
    }

    public void OnClickPrev()
    {
        if (!(CurrentSlide - 1 < 1))
        {
            TutSlides[CurrentSlide - 1].SetActive(false);
            TutSlides[CurrentSlide - 2].SetActive(true);
            CurrentSlide--;
            if (CurrentSlide - 1 < 1)
                SetButtonState(prevBtn, false);
            SetButtonState(nextBtn, true);
        }
    }

    private void SetButtonState(Button button, bool active)
    {
        button.interactable = active;
        if(button.Equals(prevBtn))
        {
            if (active)
                button.GetComponent<Image>().sprite = prevBtnActive;
            else
                button.GetComponent<Image>().sprite = prevBtnInactive;
        }
        else
        {
            if (active)
                button.GetComponent<Image>().sprite = nextBtnActive;
            else
                button.GetComponent<Image>().sprite = nextBtnInactive;
        }
    }
}
