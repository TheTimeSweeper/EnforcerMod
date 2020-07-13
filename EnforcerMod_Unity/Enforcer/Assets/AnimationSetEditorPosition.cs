using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSetEditorPosition : MonoBehaviour {

    private Transform[] _transforms;

    private Vector3[] _storedPositions;

    [ContextMenu("Record Transforms")]
    private void getAllTransformPositions() {

        //NEVER do this at runtime, unless you don't care about high FPS in which case you're not a true gamer -ts
        _transforms = GetComponentsInChildren<Transform>();

        int aye = 0;
        for (int i = 0; i < _transforms.Length; i++) {
            _storedPositions[i] = _transforms[i].position;
            aye = i;
        }

        Debug.Log($"{aye} transforms have been recorded. Turn off animation Preview and try Set ALL Recorded Transforms");
    }

    [ContextMenu("Set All Recorded Transforms")]
    private void setAllTransformPositions() {

        if(_transforms == null) {
            Debug.LogError("no transforms are recorded. use Record Transforms first");
            return;
        }

        for (int i = 0; i < _transforms.Length; i++) {
            if (_transforms[i] != null) {
                _transforms[i].position = _storedPositions[i];
            }
        }
    }
}
