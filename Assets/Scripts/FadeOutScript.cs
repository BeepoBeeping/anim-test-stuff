using UnityEngine;

public class FadeOutScript : MonoBehaviour
{
    public Animator anim;
    public PlayerScript script;

    // Update is called once per frame

    public void FadeOut()
    {
        if (anim.GetBool("fadeOut"))
        {
            this.gameObject.SetActive(true);
        }

        if (anim.GetBool("fadeIn"))
        {
            this.gameObject.SetActive(false);
        }
    }
}
