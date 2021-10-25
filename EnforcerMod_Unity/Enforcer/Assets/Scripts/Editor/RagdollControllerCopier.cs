using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RoR2;

public class RagdollControllerCopier : ComponentCopier<RagdollController> {

    public override void PasteStoredComponent(GameObject selected) {

        RagdollController newController = selected.GetComponent<RagdollController>();
        if (newController == null) {
            newController = selected.AddComponent<RagdollController>();
        } else {
            pasteReport += "\nselection already had a RagdollController";
        }

        List<Transform> children = selected.GetComponentsInChildren<Transform>(true).ToList();

        newController.bones = new Transform[storedComponent.bones.Length];

        for (int i = 0; i < newController.bones.Length; i++) {

            Transform newBone = newController.bones[i];
            Transform storedBone = storedComponent.bones[i];

            //if (storedBone == null) 
            //    continue;

            newBone = children.Find(tran => { return tran.name == storedBone.name; });

            newController.bones[i] = newBone;

            if (newBone == null) {
                pasteReport += $"\ncould not get bone for {storedBone.name}";
            }

            if (newBone != null && !newBone.GetComponent<Rigidbody>() && storedBone.GetComponent<Rigidbody>()) {

                NotRetardedComponentCopier componentCopier = new NotRetardedComponentCopier();
                componentCopier.TransferComponents(storedBone.gameObject, newBone.gameObject);

                pasteReport += $"\n found collider for {newBone.name}. Adding: {componentCopier.pasteReport}";
            }
        }
    }
}

