using UnityEngine;

//handles unity components such as colliders rigidbodies and joints
//you know just 3 random examples
public class NotRetardedComponentCopier : ComponentCopier<Component> {

    public override bool hasComponent => storedComponents != null || storedComponents.Length == 0;

    public Component[] storedComponents;

    public override void StoreComponent(GameObject copiedObject) {
        copyReport = "";
        storedComponents = copiedObject.GetComponents<Component>();
        for (int i = 0; i < storedComponents.Length; i++) {
            copyReport += $"{storedComponents[i].GetType().ToString()}, ";
        }
    }

    public override void PasteComponent(GameObject selected) {
        base.PasteComponent(selected);
    }

    public override void PasteStoredComponent(GameObject selected) {

        for (int i = 0; i < storedComponents.Length; i++) {
            UnityEditorInternal.ComponentUtility.CopyComponent(storedComponents[i]);

            System.Type componentType = storedComponents[i].GetType();

            //this would move the new rig to the position and pose of the old one
            //might be fine if you want, but seems annoying
            if (componentType == typeof(Transform))
                continue;

            Component existingComponent = selected.GetComponent(componentType);
            //update values of existing component. doesn't support multiple of same component.
            if (existingComponent != null) {

                UnityEditorInternal.ComponentUtility.PasteComponentValues(existingComponent);
                pasteReport += $"already had {componentType}, ";

            } else {

                UnityEditorInternal.ComponentUtility.PasteComponentAsNew(selected);
                pasteReport += $"{componentType}, ";
            }
        }

        storedComponents = null;
    }
}

//rip
//in a good way
public class RetardedColliderCopier : ComponentCopier<Collider> {

    public RetardedColliderCopier() { }
    public RetardedColliderCopier(GameObject gameObject) {
        StoreComponent(gameObject);
    }

    public override void StoreComponent(GameObject selected) {
        base.StoreComponent(selected);
    }

    public override void PasteStoredComponent(GameObject selected) {

        if (storedComponent is BoxCollider) {
            BoxCollider col = selected.GetComponent<BoxCollider>();
            if (!col) {
                col = selected.AddComponent<BoxCollider>();
            } else {
                pasteReport += "\nselection already had a BoxCollider";
            }

            col.center = (storedComponent as BoxCollider).center;
            col.size = (storedComponent as BoxCollider).size;
            return;
        }
        if (storedComponent is SphereCollider) {

            SphereCollider col = selected.GetComponent<SphereCollider>();
            if (!col) {
                col = selected.AddComponent<SphereCollider>();
            } else {
                pasteReport += "\nselection already had a SphereCollider";
            }

            col.center = (storedComponent as SphereCollider).center;
            col.radius = (storedComponent as SphereCollider).radius;

            return;
        }
        if (storedComponent is CapsuleCollider) {
            CapsuleCollider col = selected.GetComponent<CapsuleCollider>();
            if (!col) {
                col = selected.AddComponent<CapsuleCollider>();
            } else {
                pasteReport += "\nselection already had a CapsuleCollider";
            }

            col.center = (storedComponent as CapsuleCollider).center;
            col.radius = (storedComponent as CapsuleCollider).radius;
            col.height = (storedComponent as CapsuleCollider).height;
            col.direction = (storedComponent as CapsuleCollider).direction;
            return;
        }

        pasteReport += "what even kind of collider was it what";
    }
}

