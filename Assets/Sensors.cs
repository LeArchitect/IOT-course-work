using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
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

    public string btrStr;
    public string ownIp;
 
    void Start()
    {
        IPAddress localAddr = IPAddress.Parse(ClientScript.GetLocalIPAddress());
        ownIp = localAddr.ToString();

        try
        {
            InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
            Debug.Log("Done");
            InputSystem.EnableDevice(MagneticFieldSensor.current);
            Debug.Log("done");
            InputSystem.EnableDevice(ProximitySensor.current);
            Debug.Log("done");
            InputSystem.EnableDevice(LightSensor.current);
            Debug.Log("Done");
            //InputSystem.EnableDevice(HumiditySensor.current);
            //Debug.Log("done");
            //InputSystem.EnableDevice(AmbientTemperatureSensor.current);
            //Debug.Log("Done");

            UnityEngine.InputSystem.Gyroscope.current.samplingFrequency = 16;
            MagneticFieldSensor.current.samplingFrequency = 16;
            ProximitySensor.current.samplingFrequency = 16;
            LightSensor.current.samplingFrequency = 16;
            //HumiditySensor.current.samplingFrequency = 16;
            //AmbientTemperatureSensor.current.samplingFrequency = 16;
        }
        catch (Exception e){ Debug.Log("Exception in Sensors: " + e); }
    }

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

            System.Random rnd = new System.Random();
            string tempStr = rnd.Next(22, 25).ToString();
            string humStr = rnd.Next(50, 55).ToString();
            //string tempStr = (AmbientTemperatureSensor.current.ambientTemperature.ReadValue()).ToString("0.##");
            //string humStr = (HumiditySensor.current.relativeHumidity.ReadValue()).ToString("0.##");

            if(counter == 30)
            {
                battery.GetComponent<Text>().text = btrStr;
                ClientScript.toSendQueue.Add(ownIp + ":B:" + btrStr);

                gyro.GetComponent<Text>().text = gyroStr;
                ClientScript.toSendQueue.Add(ownIp + ":G:" + gyroStr);

                proxy.GetComponent<Text>().text = proxyStr;
                ClientScript.toSendQueue.Add(ownIp + ":P:" + proxyStr);

                lumi.GetComponent<Text>().text = lightStr;
                ClientScript.toSendQueue.Add(ownIp + ":L:" + lightStr);

                magnetic.GetComponent<Text>().text = magneticStr;
                ClientScript.toSendQueue.Add(ownIp + ":M:" + magneticStr);

                humidity.GetComponent<Text>().text = humStr;
                ClientScript.toSendQueue.Add(ownIp + ":H:" + humStr);

                temperature.GetComponent<Text>().text = tempStr;
                ClientScript.toSendQueue.Add(ownIp + ":T:" + tempStr);
                counter = 0;
            }
            counter++;
        }
    }
}
