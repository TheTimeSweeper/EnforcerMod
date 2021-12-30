using UnityEngine;

public class MemeRigController : MonoBehaviour
{
    [SerializeField]
    private Animator memeAnimator;
    public Animator MemeAnimator { get => memeAnimator; }

    [SerializeField]
    private Animator origAnimator;

    public bool isPlaying;

    void Awake()
    {
        memeAnimator.gameObject.SetActive(false);
    }
    
    public void playMemeAnim()
    {
        origAnimator.enabled = false;
        memeAnimator.Rebind();
        memeAnimator.gameObject.SetActive(true);
        isPlaying = true;
        //memeAnimator.Play(anim);
    }

    public void stopAnim()
    {
        memeAnimator.Play("Empty");
        memeAnimator.gameObject.SetActive(false);
        origAnimator.enabled = true;
        isPlaying = false;
    }
}
