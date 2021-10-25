using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ChildLocatorCopier : ComponentCopier<ChildLocator> {

    public override void PasteStoredComponent(GameObject selected) {

        ChildLocator newLocator = selected.GetComponent<ChildLocator>();
        if (newLocator == null) {
            newLocator = selected.AddComponent<ChildLocator>();
        } else {
            pasteReport += "\nselection already had a ChildLocator";
        }

        List<Transform> newChildren = selected.GetComponentsInChildren<Transform>(true).ToList();

        newLocator.TransformPairs = new ChildLocator.NameTransformPair[storedComponent.TransformPairs.Length];

        for (int i = 0; i < newLocator.TransformPairs.Length; i++) {

            ChildLocator.NameTransformPair newPair = newLocator.TransformPairs[i];
            ChildLocator.NameTransformPair storedPair = storedComponent.TransformPairs[i];

            newPair.name = storedPair.name;

            if (storedPair.transform == null) {
                continue;
            };

            newPair.transform = newChildren.Find(tran => {
                return tran.name == storedPair.transform.name;
            });

            if (newPair.transform == null) {
                pasteReport += $"\ncould not find transform for {newPair.name}";
            }

            newLocator.TransformPairs[i] = newPair;
        }
    }
}

