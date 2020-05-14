using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ClientScript : MonoBehaviour
{
    [SerializeField]
    public int port = 5005;
    public string ownIp;
    public string serverIp;

    public bool isConnected = false;

    public static GameObject ipField = null;

    public TcpClient client;

    public static List<string> toSendQueue = new List<string>();


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (SceneChangerScript.state == "Client")
        {
            try
            {
                serverIp = ipField.transform.Find("Text").gameObject.GetComponent<Text>().text;
                Debug.Log(serverIp);
            }
            catch (Exception e){ Debug.Log(serverIp); }

            if (isConnected == false)
            {
                try
                {
                    Debug.Log("Waiting Connection To the Server...");
                    client = new TcpClient(serverIp, port);
                    NetworkStream stream = client.GetStream();
                    Thread clientThread = new Thread(() => ClientSend(client));
                    clientThread.Start();
                    isConnected = true;
                }
                catch (Exception e) {Debug.Log("SocketException: " + e);}
            }
        }
        */
    }
    public void ConnectToHost()
    {
        while (isConnected == false)
        {
            try
            {
                serverIp = ipField.transform.Find("Text").gameObject.GetComponent<Text>().text;
                Debug.Log("Waiting Connection To the Server...");
                client = new TcpClient(serverIp, port);
                NetworkStream stream = client.GetStream();
                Thread clientThread = new Thread(() => ClientSend(client));
                clientThread.Start();
                isConnected = true;
            }
            catch (SocketException e){Debug.Log("SocketException: " + e);}
            Thread.Sleep(1);
        }
        Debug.Log("Connection to The server Established!");
        Thread.CurrentThread.Abort();
    }

    public void ClientSend(System.Object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();
        client.NoDelay = false;
        Debug.Log("Client Send thread started!");
        while (true)
        {
            try
            {
                if (toSendQueue.Count > 0)
                {
                    Debug.Log("TosendObject: " + toSendQueue[0]);
                    string toSendString = toSendQueue[0];
                    string message = ownIp + ":" + toSendString;
                    Byte[] data = Encoding.ASCII.GetBytes(message);
                    // Send the message to the connected TcpServer. 
                    stream.Write(data, 0, data.Length);
                    Debug.Log("Sent: " + message);
                    toSendQueue.RemoveAt(0);
                }
            }
            catch (SocketException e)
            {
                Debug.Log("SocketException: " + e);
                stream.Close();
                client.Close();
            }
            catch (Exception e)
            {
                Debug.Log("Exception: " + e);
            }
            Thread.Sleep(10);
        }
    }

    // Gets the local ip address
    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            //Debug.Log(ip);
            if (ip.AddressFamily == AddressFamily.InterNetwork)
                return ip.ToString();
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }

    public void OpenConnection()
    {
        IPAddress localAddr = IPAddress.Parse(GetLocalIPAddress());
        ownIp = localAddr.ToString();

        Thread clientThread = new Thread(() => ConnectToHost());
        clientThread.Start();
        Debug.Log("Program Started...!");
    }
}
