using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EditorCopyAndPasteComponents {

    public static List<ComponentCopierBase> ComponentCopiers = new List<ComponentCopierBase> {
        new NotRetardedComponentCopier(),
        new ChildLocatorCopier(),
        new RagdollControllerCopier()
    };

    private static string bigLog;

    [MenuItem("Edit/copy certain components #c")]
    public static void copyComponents() {
        string copyReport = "";// $"copying from {Selection.activeGameObject}: ";

        for (int i = 0; i < ComponentCopiers.Count; i++) {
            ComponentCopiers[i].StoreComponent(Selection.activeGameObject);
            copyReport += ComponentCopiers[i].copyReport;
        }

        if (string.IsNullOrEmpty(copyReport)) {
            Debug.Log("did not copy any components");
        } else {
            Debug.Log($"copying from {Selection.activeGameObject}: {copyReport}");
        }

    }

    [MenuItem("Edit/paste copied components #v")]
    public static void pasteComponents() {

        bigLog = "";

        GameObject selected = Selection.activeGameObject;

        Undo.RecordObject(selected, "paste copied components");

        for (int i = 0; i < ComponentCopiers.Count; i++) {
            ComponentCopiers[i].PasteComponent(Selection.activeGameObject);
            bigLog += ComponentCopiers[i].pasteReport;
        }

        if (string.IsNullOrEmpty(bigLog)) {
            Debug.Log("but nobody came");
        } else {
            Debug.Log($"Paste Comopnents Report: {bigLog}");
        }
    }

    #region rip but in a good way
    /*
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
    */
    #endregion
}
