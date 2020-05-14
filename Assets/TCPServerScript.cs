using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using System.Security.Cryptography;

public class TCPServerScript : MonoBehaviour
{
    [SerializeField]
    public int port = 5005;
    public string ownIp;

    public static bool isSending = false;

    public TcpListener server;
    public TcpClient newClient;

    public Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

    public List<TcpClient> clientList = new List<TcpClient>();

    public List<string> messageQueue = new List<string>();

    //ID Gyro Proxy Light Magnetic//
    public static List<Tuple<string, string, string, string, string, string>> valueList = new List<Tuple<string, string, string, string, string, string>>();

    // Update is called once per frame
    void Update()
    {
        if (messageQueue.Count > 0)
        {
            Debug.Log("MessageQueue size:  " + messageQueue.Count);
            ReadQueue();
        }
    }

    public void InitMasterThread()
    {
        Thread masterThread = new Thread(() => MasterThread());
        masterThread.Priority = System.Threading.ThreadPriority.Highest;
        Debug.Log("Program Started...!");
        masterThread.Start();
        Debug.Log("Master Thread Started...!");
    }

    // One Thread to rule them all //
    public void MasterThread()
    {
        while (true)
        {
            if(SceneChangerScript.state == "Server")
            {
                IPAddress localAddr = IPAddress.Parse(GetLocalIPAddress());
                server = new TcpListener(localAddr, port);
                server.Start();
                while (true)
                {
                    try
                    {
                        Debug.Log("Waiting Connection...");
                        TcpClient client = server.AcceptTcpClient();
                        newClient = client;
                        Debug.Log("Connection Established!");

                        clientList.Add(newClient);

                        Thread connThread = new Thread(() => ConnectionThread(client));
                        connThread.Start();
                    }
                    catch (SocketException e)
                    {
                        Debug.Log("SocketException: " + e);
                        server.Stop();
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Exception: " + e);
                        server.Stop();
                    }
                    Thread.Sleep(1);
                }
            }
            Thread.Sleep(1);
        }

    }

    // Thread for every connection
    public void ConnectionThread(System.Object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();
        client.NoDelay = false;

        string data = null;
        Byte[] bytes = new Byte[32];
        int i;

        //Thread sendThread = new Thread(() => SendThread(client, stream));
        //sendThread.Start();

        string clientIP = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
        Debug.Log(clientIP);

        Tuple<string, string, string, string, string, string> value = new Tuple<string, string, string, string, string, string>(clientIP, "N/A", "N/A", "N/A", "N/A", "N/A");
        valueList.Add(value);
        Debug.Log("Empty To ValueList: " + valueList[0].Item1 + " " + valueList[0].Item2 + " " + valueList[0].Item3 + " " + valueList[0].Item4 + " " + valueList[0].Item5 + " " + valueList[0].Item6);
        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
        {
            try
            {
                bytes = new Byte[32];
                string hex = BitConverter.ToString(bytes);
                data = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                messageQueue.Add(data);
            }
            catch (SocketException e)
            {
                Debug.Log("SocketException: " + e);
                Thread.CurrentThread.Abort();
            }
            catch (Exception e)
            {
                Debug.Log("Exception: " + e);
            }
            Thread.Sleep(1);
        }
    }
    /*
    // Thread to handle sending messages to Clients //
    public void SendThread(System.Object obj, NetworkStream stream)
    {
        TcpClient client = (TcpClient)obj;

        while (true)
        {
            try
            {
                if (client.Connected == false)
                {
                    Thread.CurrentThread.Abort();
                }
                if (isSending == true)
                {
                    SendConfirmed(stream);
                    isSending = false;
                }
                Thread.Sleep(100);
            }
            catch (SocketException e)
            {
                Debug.Log("SocketException: " + e);
            }
        }
    }

    // Sends the confirmed selection to the client//
    public void SendConfirmed(NetworkStream stream)
    {
        string[] splitter = new string[] { " " };
        string msg = "TEST";

        Byte[] message = Encoding.ASCII.GetBytes(msg);
        stream.Write(message, 0, message.Length);
        Debug.Log(msg);
    }
    */

    // Handles the data parsing and extraction //
    public void DataHandler(string data, TcpClient client)
    {
        Debug.Log(data);
        //ID Sensor data//
        //string[] splitter = new string[] { ":" };
        //string[] strings = data.Split(splitter, StringSplitOptions.RemoveEmptyEntries);

        //Tuple<string, string, string> newMessage = new Tuple<string, string, string>(strings[0], strings[1], strings[2]);
        //Debug.Log("Newmessage: " + strings[0] + " " + strings[1] + " " + strings[2]);
        messageQueue.Add(data);
    }

    // Gets the local ip address
    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            Debug.Log(ip);
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }

    public void ReadQueue()
    {
        try
        {
            string[] splitter = new string[] { ":" };
            string[] message = messageQueue[0].Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            messageQueue.RemoveAt(0);
            Debug.Log("ValueList: " + valueList[0].Item1 + " " + valueList[0].Item2 + " " + valueList[0].Item3 + " " + valueList[0].Item4 + " " + valueList[0].Item5 + " " + valueList[0].Item6);
            int i;
            for (i = 0; i <= valueList.Count - 1; i++)
            {
                if (valueList[i].Item1 == message[0])
                {
                    if (message[1] == "B")
                    {
                        Tuple<string, string, string, string, string, string> newValue = new Tuple<string, string, string, string, string, string>(valueList[i].Item1, message[2], valueList[i].Item3, valueList[i].Item4, valueList[i].Item5, valueList[0].Item6);
                        //valueList[valueList.FindIndex(ind => ind.Item1.Equals(newValue.Item1))] = newValue;
                        valueList[i] = newValue;
                        ShowChanges(i, valueList[i]);
                        Debug.Log("NewValue: " + valueList[i].Item1 + " " + valueList[i].Item2 + " " + valueList[i].Item3 + " " + valueList[i].Item4 + " " + valueList[i].Item5 + " " + valueList[0].Item6);
                    }
                    else if (message[1] == "L")
                    {
                        Tuple<string, string, string, string, string, string> newValue = new Tuple<string, string, string, string, string, string>(valueList[i].Item1, valueList[i].Item2, message[2], valueList[i].Item4, valueList[i].Item5, valueList[0].Item6);
                        valueList[i] = newValue;
                        ShowChanges(i, valueList[i]);
                        Debug.Log("NewValue: " + valueList[i].Item1 + " " + valueList[i].Item2 + " " + valueList[i].Item3 + " " + valueList[i].Item4 + " " + valueList[i].Item5 + " " + valueList[0].Item6);
                    }
                    else if (message[1] == "H")
                    {
                        Tuple<string, string, string, string, string, string> newValue = new Tuple<string, string, string, string, string, string>(valueList[i].Item1, valueList[i].Item2, valueList[i].Item3, message[2], valueList[i].Item5, valueList[0].Item6);
                        valueList[i] = newValue;
                        ShowChanges(i, valueList[i]);
                        Debug.Log("NewValue: " + valueList[i].Item1 + " " + valueList[i].Item2 + " " + valueList[i].Item3 + " " + valueList[i].Item4 + " " + valueList[i].Item5 + " " + valueList[0].Item6);
                    }
                    else if (message[1] == "T")
                    {
                        Tuple<string, string, string, string, string, string> newValue = new Tuple<string, string, string, string, string, string>(valueList[i].Item1, valueList[i].Item2, valueList[i].Item3, valueList[i].Item4, message[2], valueList[0].Item6);
                        valueList[i] = newValue;
                        ShowChanges(i, valueList[i]);
                        Debug.Log("NewValue: " + valueList[i].Item1 + " " + valueList[i].Item2 + " " + valueList[i].Item3 + " " + valueList[i].Item4 + " " + valueList[i].Item5 + " " + valueList[0].Item6);
                    }
                    else if (message[1] == "G")
                    {
                        Tuple<string, string, string, string, string, string> newValue = new Tuple<string, string, string, string, string, string>(valueList[i].Item1, valueList[i].Item2, valueList[i].Item3, valueList[i].Item4, valueList[i].Item5, message[2]);
                        valueList[i] = newValue;
                        ShowChanges(i, valueList[i]);
                        Debug.Log("NewValue: " + valueList[i].Item1 + " " + valueList[i].Item2 + " " + valueList[i].Item3 + " " + valueList[i].Item4 + " " + valueList[i].Item5 + " " + valueList[0].Item6);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Exception from show: " + e);
        }
    }

    public void ShowChanges(int i, Tuple<string, string, string, string, string, string> value)
    {
        if (i == 0)
        {
            SceneChangerScript.battery1.GetComponent<Text>().text = value.Item2;
            SceneChangerScript.light1.GetComponent<Text>().text = value.Item3;
            SceneChangerScript.humidity1.GetComponent<Text>().text = value.Item4;
            SceneChangerScript.temperature1.GetComponent<Text>().text = value.Item5;
            SceneChangerScript.gyro1.GetComponent<Text>().text = value.Item6;
        }
        else if (i == 1)
        {
            SceneChangerScript.battery2.GetComponent<Text>().text = value.Item2;
            SceneChangerScript.light2.GetComponent<Text>().text = value.Item3;
            SceneChangerScript.humidity2.GetComponent<Text>().text = value.Item4;
            SceneChangerScript.temperature2.GetComponent<Text>().text = value.Item5;
            SceneChangerScript.gyro2.GetComponent<Text>().text = value.Item6;
        }
    }
}

