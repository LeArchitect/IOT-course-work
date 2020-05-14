using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class SceneChangerScript : MonoBehaviour
{
    public static string state = null;
    public bool isServer;

    private GameObject menu;
    private GameObject clientConnect;
    private GameObject client;
    private GameObject server;

    public static GameObject description;
    public static GameObject dataText0;
    public static GameObject dataText1;

    public static GameObject battery1;
    public static GameObject light1;
    public static GameObject humidity1;
    public static GameObject temperature1;
    public static GameObject gyro1;

    public static GameObject battery2;
    public static GameObject light2;
    public static GameObject humidity2;
    public static GameObject temperature2;
    public static GameObject gyro2;

    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.Find("Menu");
        clientConnect = GameObject.Find("ClientConnect");
        client = GameObject.Find("Client");
        server = GameObject.Find("Server");

        try
        {
            ClientScript.ipField = GameObject.Find("ClientConnect").transform.Find("IPField").gameObject;
            Sensors.battery = GameObject.Find("Client").transform.Find("Battery").gameObject;
            Sensors.DataTexts = GameObject.Find("Client").transform.Find("DataTexts").gameObject;
            Sensors.gyro = Sensors.DataTexts.transform.Find("Gyro").gameObject;
            Sensors.proxy = Sensors.DataTexts.transform.Find("Proxy").gameObject;
            Sensors.magnetic = Sensors.DataTexts.transform.Find("Magnetic").gameObject;
            Sensors.lumi = Sensors.DataTexts.transform.Find("Light").gameObject;
            Sensors.humidity = Sensors.DataTexts.transform.Find("Humidity").gameObject;
            Sensors.temperature = Sensors.DataTexts.transform.Find("Temperature").gameObject;
        }
        catch(Exception e){ Debug.Log("Exception in Sensor texts: " + e); }

        try
        {
            description = GameObject.Find("Server").transform.Find("Description").gameObject;
            dataText0 = description.transform.Find("DataText0").gameObject;
            dataText1 = description.transform.Find("DataText1").gameObject;

            battery1 = dataText0.transform.Find("Battery").gameObject;
            battery2 = dataText1.transform.Find("Proxy").gameObject;
            light1 = dataText0.transform.Find("Light").gameObject;
            light2 = dataText1.transform.Find("Light").gameObject;
            humidity1 = dataText0.transform.Find("Humidity").gameObject;
            humidity2 = dataText1.transform.Find("Humidity").gameObject;
            gyro1 = dataText0.transform.Find("Gyro").gameObject;
            gyro2 = dataText1.transform.Find("Gyro").gameObject;
        }
        catch (Exception e){ Debug.Log("Exception in Sensor texts Server: " + e); }
        ToMenu();
    }

    public void ToClient()
    {
        client.SetActive(true);
        clientConnect.SetActive(false);
        menu.SetActive(false);
        server.SetActive(false);
        state = "Client";
    }
    public void ToClientConnect()
    {
        clientConnect.SetActive(true);
        menu.SetActive(false);
        server.SetActive(false);
        client.SetActive(false);
        state = "ClientConnect";
    }
    public void ToServer()
    {
        server.SetActive(true);
        menu.SetActive(false);
        clientConnect.SetActive(false);
        client.SetActive(false);
        state = "Server";
    }
    public void ToMenu()
    {
        menu.SetActive(true);
        server.SetActive(false);
        clientConnect.SetActive(false);
        client.SetActive(false);
        state = "Menu";
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
