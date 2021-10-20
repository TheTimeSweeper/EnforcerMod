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

            //NEVER do GetComponentsInChildren at runtime, unless you don't care about high FPS in which case you're not a true gamer -ts
            children = Selection.transforms[select].GetComponentsInChildren<Transform>();

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
            copied += "\nCharacterJoint, ";
        if (storedChildLocator != null)
            copied += "\nChildLocator, ";
        if (storedRagdollController != null)
            copied += "\nRagdollController, ";

        Debug.Log(copied);

    }

    [MenuItem("Edit/paste copied components #v")]
    public static void pasteComponents() {

        GameObject selected = Selection.activeGameObject;

        Undo.RecordObject(selected, "paste copied components");

        if (storedCollider != null) {
            setStoredCollider(selected);
        }

        if (storedRigidbody != null) {
            setStoredRigidBody(selected);
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
    }

    private static void setStoredCollider(GameObject selected) {
        if (storedCollider is SphereCollider) {

            SphereCollider col = selected.GetComponent<SphereCollider>();
            if (!col) {
                col = selected.AddComponent<SphereCollider>();
            }

            col.center = (storedCollider as SphereCollider).center;
            col.radius = (storedCollider as SphereCollider).radius;

        } else {
            CapsuleCollider col = selected.GetComponent<CapsuleCollider>();
            if (!col) {
                col = selected.AddComponent<CapsuleCollider>();
            }

            col.center = (storedCollider as CapsuleCollider).center;
            col.radius = (storedCollider as CapsuleCollider).radius;
            col.height = (storedCollider as CapsuleCollider).height;
            col.direction = (storedCollider as CapsuleCollider).direction;
        }
    }

    private static void setStoredRigidBody(GameObject selected) {
        Rigidbody rig = selected.GetComponent<Rigidbody>();
        if (!rig) {
            rig = selected.AddComponent<Rigidbody>();
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
        }

        List<Transform> children = selected.GetComponentsInChildren<Transform>().ToList();

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

            locator.TransformPairs[i] = pair;
        }
    }

    private static void setStoredRagdollController(GameObject selected) {

        RagdollController controller = selected.GetComponent<RagdollController>();
        if (controller == null) {
            controller = selected.AddComponent<RagdollController>();
        }

        List<Transform> children = selected.GetComponentsInChildren<Transform>().ToList();

        controller.bones = new Transform[storedRagdollController.bones.Length];

        for (int i = 0; i < controller.bones.Length; i++) {

            Transform bone = controller.bones[i];
            Transform storedBone = storedRagdollController.bones[i];

            if (storedBone == null) 
                continue;

            bone = children.Find(tran => { return tran.name == storedBone.name; });
        }
    }


}
