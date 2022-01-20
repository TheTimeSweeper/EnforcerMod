using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MemeRigController : MonoBehaviour
{
    [SerializeField]
    private Animator memeAnimator;

    [SerializeField]
    private GameObject memeScaler;

    [SerializeField]
    private Animator origAnimator;

    void Awake()
    {
        memeAnimator.gameObject.SetActive(false);
    }

    public void playMemeAnim(string anim, bool scale = false)
    {
        origAnimator.enabled = false;
        memeAnimator.Rebind();
        memeAnimator.gameObject.SetActive(true);
        memeScaler.SetActive(scale);
        memeAnimator.Play(anim);
    }

    public void stopAnim()
    {
        memeAnimator.Play("Empty");
        memeAnimator.gameObject.SetActive(false);
        memeScaler.SetActive(false);
        origAnimator.enabled = true;
    }

#if UNITY_EDITOR

    //blasphemy declaring a variable oustide of the top?
    [SerializeField, Header("editor")]
    private Transform origArmature;
    [SerializeField]
    private GameObject shield;
    [SerializeField]
    private GameObject gun;


    [ContextMenu("testStopAnim")]
    public void testStop()
    {
        shield.SetActive(true);
        gun.SetActive(true);
        stopAnim();
    }

    [ContextMenu("testPlayAnim Dance")]
    public void test()
    {
        shield.SetActive(false);
        gun.SetActive(false);
        playMemeAnim("DefaultDance");
    }
    [ContextMenu("testPlayAnim Earl")]
    public void test2()
    {
        shield.SetActive(false);
        gun.SetActive(false);
        playMemeAnim("FLINT LOCK WOOD");
    }
    [ContextMenu("testPlayAnim Earl2")]
    public void test3()
    {
        shield.SetActive(false);
        gun.SetActive(false);
        playMemeAnim("FLINT LOCK WOOD", true);
    }

    [ContextMenu("setBones")]
    public void SetBones()
    {
        if (origAnimator == null)
            return;

        List<MemeBoneController> oldMemeBones = memeAnimator.GetComponentsInChildren<MemeBoneController>(true).ToList();

        for (int i = 0; i < oldMemeBones.Count; i++)
        {
            UnityEditor.Undo.RecordObject(oldMemeBones[i], "adding meme bone");
            DestroyImmediate(oldMemeBones[i]);
        }

        List<Transform> memeAnimBones = memeAnimator.GetComponentsInChildren<Transform>(true).ToList();
        List<Transform> origBones = origArmature.GetComponentsInChildren<Transform>(true).ToList();

        int matchCount = 0;
        for (int i = 0; i < memeAnimBones.Count; i++)
        {
            Transform memeBone = memeAnimBones[i];
            Transform match = origBones.Find(tran => tran.name == memeBone.name);

            if (match)
            {
                matchCount++;
                UnityEditor.Undo.RecordObject(memeBone, "adding meme bone");
                MemeBoneController boneController = memeBone.gameObject.AddComponent<MemeBoneController>();
                boneController.OrigBone = match;
            }
        }
        Debug.Log($"{matchCount} meme bones added!");
    }
#endif
}
