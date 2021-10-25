using UnityEditor;
using UnityEngine;

public abstract class ComponentCopierBase { /*uh*/

    public abstract bool hasComponent { get; }

    public virtual string copyReport { get; set; }
    public virtual string pasteReport { get; set; }

    public abstract void StoreComponent(GameObject copiedObject);

    public abstract void PasteComponent(GameObject selected);

    public void TransferComponents(GameObject from, GameObject to) {
        StoreComponent(from);
        PasteComponent(to);
    }
}

public abstract class ComponentCopier<T> : ComponentCopierBase where T : Component{

    public override bool hasComponent => storedComponent != null;

    public T storedComponent;

    public override void StoreComponent(GameObject copiedObject) {

        storedComponent = copiedObject.GetComponent<T>();
        copyReport = $"{storedComponent?.GetType().ToString()}, ";
    }
    public override void PasteComponent(GameObject selected) {

        if (storedComponent == null)
            return;

        pasteReport = "";
        Undo.RecordObject(selected, "paste copied components");

        PasteStoredComponent(selected);

        storedComponent = null;
    }

    public abstract void PasteStoredComponent(GameObject selected);
}
