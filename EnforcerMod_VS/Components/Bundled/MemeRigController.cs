using UnityEngine;

public class MemeRigController : MonoBehaviour
{
    [SerializeField]
    private Animator memeAnimator;
    public Animator MemeAnimator { get => memeAnimator; }

    [SerializeField]
    private GameObject memeScaler;

    [SerializeField]
    private Animator origAnimator;

    public bool isPlaying;

    void Awake()
    {
        memeAnimator.gameObject.SetActive(false);
    }
    
    public void playMemeAnim(bool scale = false)
    {
        origAnimator.enabled = false;
        memeAnimator.Rebind();
        memeAnimator.gameObject.SetActive(true);
        //memeScaler.gameObject.SetActive(scale);
        isPlaying = true;
    }

    public void stopAnim()
    {
        memeAnimator.Play("Empty");
        memeAnimator.gameObject.SetActive(false);
        //memeScaler.gameObject.SetActive(false);
        origAnimator.enabled = true;
        isPlaying = false;
    }
}
