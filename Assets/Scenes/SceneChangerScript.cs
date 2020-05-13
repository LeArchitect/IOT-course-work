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
    private GameObject client;
    private GameObject server;

    public static GameObject description1;
    public static GameObject gyro1;
    public static GameObject proxy1;
    public static GameObject light1;
    public static GameObject magnetic1;

    public static GameObject description2;
    public static GameObject gyro2;
    public static GameObject proxy2;
    public static GameObject light2;
    public static GameObject magnetic2;

    public GameObject description11;
    public GameObject gyro11;
    public GameObject proxy11;
    public GameObject light11;
    public GameObject magnetic11;

    public GameObject description22;
    public GameObject gyro22;
    public GameObject proxy22;
    public GameObject light22;
    public GameObject magnetic22;

    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.Find("Menu");
        client = GameObject.Find("Client");
        server = GameObject.Find("Server");

        try
        {
            Sensors.battery = GameObject.Find("Client").transform.Find("Battery").gameObject;
            Sensors.gyro = GameObject.Find("Client").transform.Find("Gyro").gameObject;
            Sensors.proxy = GameObject.Find("Client").transform.Find("Proxy").gameObject;
            Sensors.magnetic = GameObject.Find("Client").transform.Find("Magnetic").gameObject;
            Sensors.lumi = GameObject.Find("Client").transform.Find("Light").gameObject;
        }
        catch(Exception e)
        {

        }

        try
        {
            description1 = GameObject.Find("Server").transform.Find("Description1").gameObject;
            description2 = GameObject.Find("Server").transform.Find("Description2").gameObject;
            gyro1 = description1.transform.Find("Gyro").gameObject;
            gyro2 = description2.transform.Find("Gyro").gameObject;
            proxy1 = description1.transform.Find("Proxy").gameObject;
            proxy2 = description2.transform.Find("Proxy").gameObject;
            light1 = description1.transform.Find("Light").gameObject;
            light2 = description2.transform.Find("Light").gameObject;
            magnetic1 = description1.transform.Find("Magnetic").gameObject;
            magnetic2 = description2.transform.Find("Magnetic").gameObject;

            description11 = description1;
            description22 = description2;
            gyro11 = gyro1;
            gyro22 = gyro2;
            proxy11 = proxy1;
            proxy22 = proxy2;
            light11 = light1;
            light22 = light2;
            magnetic11 = magnetic1;
            magnetic22 = magnetic2;
        }
        catch (Exception e)
        {

        }
        ToMenu();
    }

    public void ToClient()
    {
        menu.SetActive(false);
        client.SetActive(true);
        state = "Client";
    }

    public void ToServer()
    {
        menu.SetActive(false);
        server.SetActive(true);
        state = "Server";
    }

    public void ToMenu()
    {
        menu.SetActive(true);
        if(state == "Server")
            server.SetActive(false);
        else if(state == "Client")
            client.SetActive(false);
        state = "Menu";
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
