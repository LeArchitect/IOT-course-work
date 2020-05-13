using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Sensors : MonoBehaviour
{
    //public GameObject battery;
    public static GameObject gyro;
    public static GameObject battery;
    public static GameObject proxy;
    public static GameObject lumi;
    public static GameObject magnetic;

    public string prevGyroStr = "0";
    public string prevProxyStr = "0";
    public string prevLightStr = "0";
    public string prevMagneticStr = "0";

    public int counter = 0;

    public float btrLvl;

    void Start()
    {
        InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
        InputSystem.EnableDevice(MagneticFieldSensor.current);
        InputSystem.EnableDevice(ProximitySensor.current);
        InputSystem.EnableDevice(LightSensor.current);

        UnityEngine.InputSystem.Gyroscope.current.samplingFrequency = 16;
        MagneticFieldSensor.current.samplingFrequency = 16;
        ProximitySensor.current.samplingFrequency = 16;
        LightSensor.current.samplingFrequency = 16;
    }

    // Update is called once per frame
 
    void Update()
    {
        if (SceneChangerScript.state == "Client")
        {
            btrLvl = SystemInfo.batteryLevel;

            string magneticStr = (MagneticFieldSensor.current.magneticField.x.ReadValue()).ToString("0.#####") + " ";
            magneticStr = magneticStr + (MagneticFieldSensor.current.magneticField.y.ReadValue()).ToString("0.#####") + " ";
            magneticStr = magneticStr + (MagneticFieldSensor.current.magneticField.z.ReadValue()).ToString("0.#####") + " ";

            string gyroStr = (UnityEngine.InputSystem.Gyroscope.current.angularVelocity.x.ReadValue()).ToString("0.#####") + " ";
            gyroStr = gyroStr + (UnityEngine.InputSystem.Gyroscope.current.angularVelocity.y.ReadValue()).ToString("0.#####") + " ";
            gyroStr = gyroStr + (UnityEngine.InputSystem.Gyroscope.current.angularVelocity.z.ReadValue()).ToString("0.#####") + " ";

            string proxyStr = (ProximitySensor.current.distance.ReadValue()).ToString("0.#####");
            string lightStr = (LightSensor.current.lightLevel.ReadValue()).ToString("0.#####");

            battery.GetComponent<Text>().text = btrLvl.ToString();

            gyro.GetComponent<Text>().text = gyroStr;
            ClientScript.toSendQueue.Add("G:" + gyroStr);

            proxy.GetComponent<Text>().text = proxyStr;
            ClientScript.toSendQueue.Add("P:" + proxyStr);

            lumi.GetComponent<Text>().text = lightStr;
            ClientScript.toSendQueue.Add("L:" + lightStr);

            magnetic.GetComponent<Text>().text = magneticStr;
            ClientScript.toSendQueue.Add("M:" + magneticStr);
        }
    }
}
