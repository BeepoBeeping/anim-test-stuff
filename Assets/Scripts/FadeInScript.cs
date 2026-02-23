using UnityEngine;

public class FadeInScript : MonoBehaviour
{
    public Animator anim;

    // Update is called once per frame

    public void FadeIn()
    {
        if (anim.GetBool("fadeIn"))
        {
            this.gameObject.SetActive(true);
        }

        if (anim.GetBool("fadeOut"))
        {
            this.gameObject.SetActive(false);
        }
    }
}
