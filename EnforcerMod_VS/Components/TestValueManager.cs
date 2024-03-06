using UnityEngine;

public class TestValueManager : MonoBehaviour {

    //how do doing attributes
    //[debugfloat("valuename", KeyCode.U, KeyCode.J, 5)]
    //would be 
    public static float testValue = 0.25f;

    private float tim;
    private float holdTime = 0.5f;

    //compiler flags when
    public static bool testingEnabled = false;
    
    internal static Vector3 testVector = new Vector3(-2.2f, -0.5f, -9f);

    void Update() {
        if (!testingEnabled)
            return;

        if (!Input.GetKey(KeyCode.LeftAlt))
            return;

        manageTestValue(ref testValue, "devl", KeyCode.Keypad7, KeyCode.Keypad4, 0.01f);

        //manageTestVector(ref testVector, "cam", 0.05f);
    }

    private void manageTestVector(ref Vector3 vector, string vectorName, float incrementAmount = 0.1f,
                                  KeyCode xUp = KeyCode.Keypad7, KeyCode xDown = KeyCode.Keypad4,
                                  KeyCode yUp = KeyCode.Keypad8, KeyCode yDown = KeyCode.Keypad5,
                                  KeyCode zUp = KeyCode.Keypad9, KeyCode zDown = KeyCode.Keypad6) {

        manageTestValue(ref vector.x, vectorName + "x", xUp, xDown, incrementAmount);
        manageTestValue(ref vector.y, vectorName + "y", yUp, yDown, incrementAmount);
        manageTestValue(ref vector.z, vectorName + "z", zUp, zDown, incrementAmount);

    }

    private void manageTestValue(ref float value, string valueName, KeyCode upKey, KeyCode downKey, float incrementAmount) {

        if (Input.GetKeyDown(upKey)) {

            value = setTestValue(value + incrementAmount, valueName);
        }

        if (Input.GetKeyDown(downKey)) {

            value = setTestValue(value - incrementAmount, valueName);
        }


        if (Input.GetKey(upKey) || Input.GetKey(downKey)) {

            float amount = incrementAmount * (Input.GetKey(upKey) ? 1 : -1);

            tim += Time.deltaTime;

            if (tim > holdTime) {

                value = setTestValue(value + amount, valueName);
                tim = holdTime - 0.05f;
            }
        }


        if (Input.GetKeyUp(upKey) || Input.GetKeyUp(downKey)) {
            tim = 0;
        }

    }

    private float setTestValue(float value, string print) {
        Debug.LogWarning($"{print}: {value}");
        return value;
    }
}