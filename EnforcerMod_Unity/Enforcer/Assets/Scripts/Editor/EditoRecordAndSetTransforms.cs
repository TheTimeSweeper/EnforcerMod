using RoR2;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
public class EditoRecordAndSetTransforms {

    public static List<Transform> _transforms;

    public static Vector3[] _storedPositions;
    public static Quaternion[] _storedRotations;
    public static Vector3[] _storedScales;

    public static Collider storedCollider;
    public static Rigidbody storedRigidbody;
    public static CharacterJoint storedJoint;
    public static ChildLocator storedChildLocator;
    public static RagdollController storedRagdollController;

    private static string bigLog;

    [MenuItem("CONTEXT/Transform/Record All Child Transforms")]
    public static void getAllTransformPositions() {

        FillTransformList();

        _storedPositions = new Vector3[_transforms.Count];
        _storedRotations = new Quaternion[_transforms.Count];
        _storedScales = new Vector3[_transforms.Count];

        int aye = 0;
        for (int i = 0; i < _transforms.Count; i++) {

            if (_transforms[i]) {
                _storedPositions[i] = _transforms[i].position;
                _storedRotations[i] = _transforms[i].rotation;
                _storedScales[i] = _transforms[i].localScale;

                aye++;
            }
        }

        Debug.Log($"{aye}/{_transforms.Count} transforms have been recorded. Turn off animation Preview and click 'Set ALL Recorded Transforms'");
    }

    private static void FillTransformList() {

        if (_transforms == null)
            _transforms = new List<Transform>();

        Transform[] children;
        for (int select = 0; select < Selection.transforms.Length; select++) {

            //NEVER do GetComponentsInChildren at runtime, unless you don't care about FPS in which case you're not a true gamer -ts
            children = Selection.transforms[select].GetComponentsInChildren<Transform>(true);

            //holy shit i'm doing the SRM thing kinda
            for (int i = 0; i < children.Length || i < _transforms.Count; i++) {

                if (i < children.Length) {

                    if (_transforms.Count <= i) {
                        _transforms.Add(children[i]);
                    } else {
                        _transforms[i] = children[i];
                    }
                } else {
                    _transforms[i] = null;
                }
            }
        }
    }

    [CanEditMultipleObjects]
    [MenuItem("CONTEXT/Transform/Set All Recorded Transforms")]
    public static void setAllTransformPositions() {

        if (_transforms == null) {
            Debug.LogError("no transforms are recorded. use Record Transforms first");
            return;
        }

        Undo.RecordObjects(_transforms.ToArray(), "setting transforms");

        for (int i = 0; i < _transforms.Count; i++) {

            if (_transforms[i] != null) {
                _transforms[i].position = _storedPositions[i];
                _transforms[i].rotation = _storedRotations[i];
                _transforms[i].localScale = _storedScales[i];
            }
        }
    }


    [MenuItem("Edit/copy certain components #c")]
    public static void copyComponents() {

        storedCollider = Selection.activeGameObject.GetComponent<Collider>();
        storedRigidbody = Selection.activeGameObject.GetComponent<Rigidbody>();
        storedJoint = Selection.activeGameObject.GetComponent<CharacterJoint>();
        storedChildLocator = Selection.activeGameObject.GetComponent<ChildLocator>();
        storedRagdollController = Selection.activeGameObject.GetComponent<RagdollController>();

        if (storedCollider == null &&
            storedRigidbody == null &&
            storedJoint == null &&
            storedChildLocator == null &&
            storedRagdollController == null) {

            Debug.Log("did not copy any components");
            return;
        }

        string copied = "copied ";

        if(storedCollider != null) 
            copied += "Collider, ";
        if (storedRigidbody != null)
            copied += "Rigidbody, ";
        if (storedJoint != null)
            copied += "CharacterJoint, ";
        if (storedChildLocator != null)
            copied += "ChildLocator, ";
        if (storedRagdollController != null)
            copied += "RagdollController, ";

        Debug.Log(copied);

    }

    [MenuItem("Edit/paste copied components #v")]
    public static void pasteComponents() {

        bigLog = "Paste Components Report:";

        GameObject selected = Selection.activeGameObject;

        Undo.RecordObject(selected, "paste copied components");

        if (storedCollider != null) {
            setStoredCollider(selected);
        }

        if (storedRigidbody != null) {
            setStoredRigidbody(selected);
        }

        if (storedJoint != null) {
            setStoredJoint(selected);
        }

        if(storedChildLocator != null) {
            setStoredChildLocator(selected);
        }

        if (storedChildLocator != null) {
            setStoredRagdollController(selected);
        }

        storedCollider = null;
        storedJoint = null;
        storedRigidbody = null;
        storedChildLocator = null;
        storedRagdollController = null;

        Debug.Log(bigLog);
    }

    private static void setStoredCollider(GameObject selected) {

        if (storedCollider is SphereCollider) {

            SphereCollider col = selected.GetComponent<SphereCollider>();
            if (!col) {
                col = selected.AddComponent<SphereCollider>();
            } else {
                bigLog += "\nselection already had a SphereCollider";
            }

            col.center = (storedCollider as SphereCollider).center;
            col.radius = (storedCollider as SphereCollider).radius;

            return;
        }
        if (storedCollider is CapsuleCollider) {
            CapsuleCollider col = selected.GetComponent<CapsuleCollider>();
            if (!col) {
                col = selected.AddComponent<CapsuleCollider>();
            } else {
                bigLog += "\nselection already had a CapsuleCollider";
            }

            col.center = (storedCollider as CapsuleCollider).center;
            col.radius = (storedCollider as CapsuleCollider).radius;
            col.height = (storedCollider as CapsuleCollider).height;
            col.direction = (storedCollider as CapsuleCollider).direction;
            return;
        }
        if (storedCollider is BoxCollider) {
            BoxCollider col = selected.GetComponent<BoxCollider>();
            if (!col) {
                col = selected.AddComponent<BoxCollider>();
            } else {
                bigLog += "\nselection already had a BoxCollider";
            }

            col.center = (storedCollider as BoxCollider).center;
            col.size = (storedCollider as BoxCollider).size;
            return;
        }

        bigLog += "what even kind of collider was it what";
    }

    private static void setStoredRigidbody(GameObject selected) {
        Rigidbody rig = selected.GetComponent<Rigidbody>();
        if (!rig) {
            rig = selected.AddComponent<Rigidbody>();
        } else {
            bigLog += "\nselection already had a Rigidbody";
        }

        rig.drag = storedRigidbody.drag;
        rig.angularDrag = storedRigidbody.angularDrag;
        rig.isKinematic = storedRigidbody.isKinematic;
        rig.useGravity = storedRigidbody.useGravity;
    }

    private static void setStoredJoint(GameObject selected) {
        CharacterJoint joint = selected.GetComponent<CharacterJoint>();
        if (!joint) {
            joint = selected.AddComponent<CharacterJoint>();
        } else {
            bigLog += "\nselection already had a CharacterJoint";
        }

        bool found = false;
        Transform check = selected.transform.parent;
        Transform body = null;

        while (!found) {

            if (storedJoint.connectedBody != null) {

                if (check.transform.name == storedJoint.connectedBody.transform.name) {
                    body = check;
                    found = true;
                } else {
                    check = check.transform.parent;
                }
            } else {

                found = true;
            }

            if (check == null) {
                found = true;
            }
        }

        if (body != null) {

            joint.connectedBody = body.GetComponent<Rigidbody>();
            if (joint.connectedBody == null) {
                body.gameObject.AddComponent<Rigidbody>();
            }
        }
    }

    private static void setStoredChildLocator(GameObject selected) {

        ChildLocator locator = selected.GetComponent<ChildLocator>();
        if(locator == null) {
            locator = selected.AddComponent<ChildLocator>();
        } else {
            bigLog += "\nselection already had a ChildLocator";
        }

        List<Transform> children = selected.GetComponentsInChildren<Transform>(true).ToList();

        locator.TransformPairs = new ChildLocator.NameTransformPair[storedChildLocator.TransformPairs.Length];

        for (int i = 0; i < locator.TransformPairs.Length; i++) {

            ChildLocator.NameTransformPair pair = locator.TransformPairs[i];
            ChildLocator.NameTransformPair storedPair = storedChildLocator.TransformPairs[i];

            pair.name = storedPair.name;

            if (storedPair.transform == null) {
                continue;
            };

            pair.transform = children.Find(tran => { 
                return tran.name == storedPair.transform.name; 
            });

            if(pair.transform == null) {
                bigLog += $"\ncould not find transform for {pair.name}";
            }

            locator.TransformPairs[i] = pair;
        }
    }

    private static void setStoredRagdollController(GameObject selected) {

        RagdollController controller = selected.GetComponent<RagdollController>();
        if (controller == null) {
            controller = selected.AddComponent<RagdollController>();
        } else {
            bigLog += "\nselection already had a RagdollController";
        }

        List<Transform> children = selected.GetComponentsInChildren<Transform>(true).ToList();

        controller.bones = new Transform[storedRagdollController.bones.Length];

        for (int i = 0; i < controller.bones.Length; i++) {

            Transform bone = controller.bones[i];
            Transform storedBone = storedRagdollController.bones[i];

            //if (storedBone == null) 
            //    continue;

            bone = children.Find(tran => { return tran.name == storedBone.name; });

            controller.bones[i] = bone;

            if (bone == null) {
                bigLog += $"\ncould not get bone for {storedBone.name}. let's try this";
            }

            if (bone != null && !bone.GetComponent<Collider>() && storedBone.GetComponent<Collider>()) {

                bigLog += $"\n found collider for {bone.name}. let's try this";

                storedCollider = storedBone.GetComponent<Collider>();
                if (storedCollider != null) {
                    setStoredCollider(bone.gameObject);
                }

                storedRigidbody = storedBone.GetComponent<Rigidbody>();
                if (storedRigidbody != null) {
                    setStoredRigidbody(bone.gameObject);
                }

                storedJoint = storedBone.GetComponent<CharacterJoint>();
                if (storedJoint != null) {
                    setStoredJoint(bone.gameObject);
                }
                
                bigLog += $"\n successfully added to {bone.name}: Collider {bone.GetComponent<Collider>()}, Rigidbody {bone.GetComponent<Rigidbody>()}, CharacterJoint {bone.GetComponent<CharacterJoint>()}";
            }
        }
    }
}
