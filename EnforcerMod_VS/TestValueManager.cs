using UnityEngine;

namespace EnforcerPlugin {
    public class TestValueManager : MonoBehaviour {

        //how do doing attributes
        //[debugfloat("forceShield: ", KeyCode.U, KeyCode.J, 5)]
        //would be neat
        public static float cameraPivot = 0;
        public static float aimorigin = 3;

        private float tim;
        private float holdTime = 0.5f;

        private bool testingEnabled;

        void Update() {
            if (!testingEnabled)
                return; 

            manageTestValue(ref cameraPivot, "cameraPivot: ", KeyCode.U, KeyCode.J, 0.05f);
            manageTestValue(ref aimorigin, "aimorigin: ", KeyCode.Y, KeyCode.H, 0.05f);
        }

        //forgive my insolence declaring valuables here. i'll probably move this to utils class
        private void manageTestValue(ref float value, string printString, KeyCode upKey, KeyCode downKey, float incrementAmount) {

            if (Input.GetKeyDown(upKey)) {

                value = setTestValue(value + incrementAmount, printString);
            }

            if (Input.GetKeyDown(downKey)) {

                value = setTestValue(value - incrementAmount, printString);
            }


            if (Input.GetKey(upKey) || Input.GetKey(downKey)) {

                float amount = incrementAmount * (Input.GetKey(upKey) ? 1 : -1);

                tim += Time.deltaTime;

                if(tim > holdTime) {

                    value = setTestValue(value + amount, "valueName");
                }
            }


            if (Input.GetKeyUp(upKey) || Input.GetKeyUp(downKey)) {
                tim = 0;
            }

        }

        private float setTestValue(float value, string print) {
            Debug.LogWarning($"{print}{value}");
            return value;
        }
    }

}