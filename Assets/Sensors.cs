using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading;
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

    public bool firstTime = true;

    public int counter = 0;

    public string ownIp;

    public string btrStr;
    public string lightStr;
    public string humStr;
    public string tempStr;
    public string gyroStr;
    public string magneticStr;
    public string proxyStr;

    void Start()
    {
        IPAddress localAddr = IPAddress.Parse(ClientScript.GetLocalIPAddress());
        ownIp = localAddr.ToString();
    }

    void Update()
    {
        if (ClientScript.isConnected == true && firstTime == true)
        {
            InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
            Debug.Log("Done");
            //InputSystem.EnableDevice(MagneticFieldSensor.current);
            //Debug.Log("done");
            //InputSystem.EnableDevice(ProximitySensor.current);
            //Debug.Log("done");
            InputSystem.EnableDevice(LightSensor.current);
            Debug.Log("Done");
            //InputSystem.EnableDevice(HumiditySensor.current);
            //Debug.Log("done");
            //InputSystem.EnableDevice(AmbientTemperatureSensor.current);
            //Debug.Log("Done");

            UnityEngine.InputSystem.Gyroscope.current.samplingFrequency = 64;
            //MagneticFieldSensor.current.samplingFrequency = 16;
            //ProximitySensor.current.samplingFrequency = 16;
            LightSensor.current.samplingFrequency = 64;
            //HumiditySensor.current.samplingFrequency = 16;
            //AmbientTemperatureSensor.current.samplingFrequency = 16;
            firstTime = false;
        }

        if (SceneChangerScript.state == "Client" && firstTime == false && ClientScript.isConnected == true)
        {
            btrStr = (SystemInfo.batteryLevel * 100).ToString();
            battery.GetComponent<Text>().text = btrStr;
            ClientScript.toSendQueue.Add(ownIp + ":B:" + btrStr);

            //magneticStr = Math.Pow(Math.Pow(MagneticFieldSensor.current.magneticField.x.ReadValue(), 2) + Math.Pow(MagneticFieldSensor.current.magneticField.y.ReadValue(), 2) + Math.Pow(MagneticFieldSensor.current.magneticField.z.ReadValue(), 2), 0.5f).ToString("0.#") + " ";
            //magneticStr = magneticStr + (MagneticFieldSensor.current.magneticField.y.ReadValue()).ToString("0.#####") + " ";
            //magneticStr = magneticStr + (MagneticFieldSensor.current.magneticField.z.ReadValue()).ToString("0.#####") + " ";

            gyroStr = Math.Pow(Math.Pow(UnityEngine.InputSystem.Gyroscope.current.angularVelocity.x.ReadValue(), 2) + Math.Pow(UnityEngine.InputSystem.Gyroscope.current.angularVelocity.y.ReadValue(), 2) + Math.Pow(UnityEngine.InputSystem.Gyroscope.current.angularVelocity.z.ReadValue(), 2), 0.5f).ToString("#") + " ";
            //gyroStr = gyroStr + (UnityEngine.InputSystem.Gyroscope.current.angularVelocity.y.ReadValue()).ToString("0.#####") + " ";
            //gyroStr = gyroStr + (UnityEngine.InputSystem.Gyroscope.current.angularVelocity.z.ReadValue()).ToString("0.#####") + " ";
            gyro.GetComponent<Text>().text = gyroStr;
            ClientScript.toSendQueue.Add(ownIp + ":G:" + gyroStr);

            //proxyStr = (ProximitySensor.current.distance.ReadValue()).ToString("0.#####");
            lightStr = (LightSensor.current.lightLevel.ReadValue()).ToString("0.#####");
            lumi.GetComponent<Text>().text = lightStr;
            ClientScript.toSendQueue.Add(ownIp + ":L:" + lightStr);

            System.Random rnd = new System.Random();
            tempStr = rnd.Next(22, 25).ToString();
            temperature.GetComponent<Text>().text = tempStr;
            ClientScript.toSendQueue.Add(ownIp + ":T:" + tempStr);

            humStr = rnd.Next(50, 55).ToString();
            humidity.GetComponent<Text>().text = humStr;
            ClientScript.toSendQueue.Add(ownIp + ":H:" + humStr);
            //string tempStr = (AmbientTemperatureSensor.current.ambientTemperature.ReadValue()).ToString("0.##");
            //string humStr = (HumiditySensor.current.relativeHumidity.ReadValue()).ToString("0.##");

            //proxy.GetComponent<Text>().text = proxyStr;
            //ClientScript.toSendQueue.Add(ownIp + ":P:" + proxyStr);

            //magnetic.GetComponent<Text>().text = magneticStr;
            //ClientScript.toSendQueue.Add(ownIp + ":M:" + magneticStr);
        }
    }
}
