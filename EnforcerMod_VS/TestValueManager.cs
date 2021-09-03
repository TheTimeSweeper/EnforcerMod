using UnityEngine;

namespace EnforcerPlugin {
    public class TestValueManager : MonoBehaviour {

        //how do doing attributes
        //[debugfloat("forceShield: ", KeyCode.U, KeyCode.J, 5)]
        //would be neat
        public static float forceShield = 96.9f;
        public static float forceUnshield = 150;

        private float tim;
        private float holdTime = 0.5f;

        void Update() {
            return;

            manageTestValue(ref forceShield, "force Shielded: ", KeyCode.U, KeyCode.J, 5);
            manageTestValue(ref forceUnshield, "force Unshielded: ", KeyCode.Y, KeyCode.H, 5);
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