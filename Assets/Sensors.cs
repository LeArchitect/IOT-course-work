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
    public static GameObject DataTexts;
    public static GameObject battery;
    public static GameObject lumi;
    public static GameObject humidity;
    public static GameObject temperature;
    public static GameObject gyro;

    public static GameObject magnetic;
    public static GameObject proxy;

    public string prevGyroStr = "0";
    public string prevProxyStr = "0";
    public string prevLightStr = "0";
    public string prevMagneticStr = "0";
    public string prevHumidityStr = "0";
    public string prevTemperatureStr = "0";

    public int counter = 0;

    public string btrStr;
 
    void Update()
    {
        if (SceneChangerScript.state == "Client")
        {
            btrStr = (SystemInfo.batteryLevel * 100).ToString();

            string magneticStr = (MagneticFieldSensor.current.magneticField.x.ReadValue()).ToString("0.#####") + " ";
            magneticStr = magneticStr + (MagneticFieldSensor.current.magneticField.y.ReadValue()).ToString("0.#####") + " ";
            magneticStr = magneticStr + (MagneticFieldSensor.current.magneticField.z.ReadValue()).ToString("0.#####") + " ";

            string gyroStr = (UnityEngine.InputSystem.Gyroscope.current.angularVelocity.x.ReadValue()).ToString("0.#####") + " ";
            gyroStr = gyroStr + (UnityEngine.InputSystem.Gyroscope.current.angularVelocity.y.ReadValue()).ToString("0.#####") + " ";
            gyroStr = gyroStr + (UnityEngine.InputSystem.Gyroscope.current.angularVelocity.z.ReadValue()).ToString("0.#####") + " ";

            string proxyStr = (ProximitySensor.current.distance.ReadValue()).ToString("0.#####");
            string lightStr = (LightSensor.current.lightLevel.ReadValue()).ToString("0.#####");
            string tempStr = (AmbientTemperatureSensor.current.ambientTemperature.ReadValue()).ToString("0.##");
            string humStr = (HumiditySensor.current.relativeHumidity.ReadValue()).ToString("0.##");

            battery.GetComponent<Text>().text = btrStr;
            ClientScript.toSendQueue.Add("B:" + btrStr);

            gyro.GetComponent<Text>().text = gyroStr;
            ClientScript.toSendQueue.Add("G:" + gyroStr);

            proxy.GetComponent<Text>().text = proxyStr;
            ClientScript.toSendQueue.Add("P:" + proxyStr);

            lumi.GetComponent<Text>().text = lightStr;
            ClientScript.toSendQueue.Add("L:" + lightStr);

            magnetic.GetComponent<Text>().text = magneticStr;
            ClientScript.toSendQueue.Add("M:" + magneticStr);

            humidity.GetComponent<Text>().text = humStr;
            ClientScript.toSendQueue.Add("H:" + humStr);

            temperature.GetComponent<Text>().text = tempStr;
            ClientScript.toSendQueue.Add("T:" + tempStr);
        }
    }

    public void InitSensors()
    {
        InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
        InputSystem.EnableDevice(MagneticFieldSensor.current);
        InputSystem.EnableDevice(ProximitySensor.current);
        InputSystem.EnableDevice(LightSensor.current);
        InputSystem.EnableDevice(HumiditySensor.current);
        InputSystem.EnableDevice(AmbientTemperatureSensor.current);

        UnityEngine.InputSystem.Gyroscope.current.samplingFrequency = 16;
        MagneticFieldSensor.current.samplingFrequency = 16;
        ProximitySensor.current.samplingFrequency = 16;
        LightSensor.current.samplingFrequency = 16;
        HumiditySensor.current.samplingFrequency = 16;
        AmbientTemperatureSensor.current.samplingFrequency = 16;
    }
}
