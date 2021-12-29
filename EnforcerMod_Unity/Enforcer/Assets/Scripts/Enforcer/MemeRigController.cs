using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MemeRigController : MonoBehaviour
{
    [SerializeField]
    private Animator memeAnimator;

    [SerializeField]
    private Animator origAnimator;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void playMemeAnim(string anim)
    {
        origAnimator.enabled = false;
        gameObject.SetActive(true);
        memeAnimator.Play(anim);
    }

    [ContextMenu("testStopAnim")]
    public void stopAnim()
    {
        memeAnimator.Play("Empty");
        gameObject.SetActive(false);
        origAnimator.enabled = true;
    }

    [ContextMenu("testPlayAnim")]
    public void test()
    {
        playMemeAnim("def");
    }
    [ContextMenu("testPlayAnim2")]
    public void test2()
    {
        playMemeAnim("EnforcerOfficerEarlRun");
    }

#if UNITY_EDITOR

    //blasphemy declaring a variable oustide of the top?
    [SerializeField]
    private Transform origArmature;

    [ContextMenu("setBones")]
    public void SetBones()
    {
        if (origAnimator == null)
            return;

        List<MemeBoneController> oldMemeBones = GetComponentsInChildren<MemeBoneController>(true).ToList();

        for (int i = 0; i < oldMemeBones.Count; i++)
        {
            UnityEditor.Undo.RecordObject(oldMemeBones[i], "adding meme bone");
            DestroyImmediate(oldMemeBones[i]);
        }

        List<Transform> memeAnimBones = GetComponentsInChildren<Transform>(true).ToList();
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
