using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using UnityEngine.Networking;
using System.Net.NetworkInformation;
using System.Net.Sockets;

public class HostMessageSender : MonoBehaviour
{

    int port = 50001;
    private UdpClient client;
    IPEndPoint mIPEndPoint;
    List<string> mTargetIps;
    byte[] dgram = new byte[1];

    void Start()
    {
        client = new UdpClient();
        mTargetIps = new List<string>();
        //mTargetIP = NetworkManager.singleton.networkAddress;
        //string hostName = System.Net.Dns.GetHostName();
        //string localIP = System.Net.Dns.GetHostEntry(hostName).AddressList[0].ToString();
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            {
                foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        //do what you want with the IP here... add it to a list, just get the first and break out. Whatever.
                        Debug.Log(ip.Address.ToString());
                        mTargetIps.Add(ip.Address.ToString());
                    }
                }
            }
        }
        //mTargetIP = IPManager.GetIP(ADDRESSFAM.IPv6);
        //Debug.Log(IPManager.GetIP(ADDRESSFAM.IPv6));
        for (int i = 0; i < mTargetIps.Count; i++)
        {
            mTargetIps[i] = mTargetIps[i].Substring(0, mTargetIps[i].LastIndexOf("."));
        }
        StartCoroutine(_Sender());
    }

    IEnumerator _Sender()
    {
        while (true)
        {
            yield return null;
            for (int j = 0; j < mTargetIps.Count; j++)
            {
                for (int i = 0; i < 255; i++)
                {
                    //建立255个线程扫描IP
                    string ip = mTargetIps[j] + "." + i.ToString();
                    IPAddress address = IPAddress.Parse(ip);
                    mIPEndPoint = new IPEndPoint(address, port);
                    try
                    {
                        client.Send(dgram, dgram.Length, mIPEndPoint);
                    }
                    catch (Exception ex)
                    {
                        //                  Debug.LogError (ex.Message);
                    }
                    yield return null;
                }
            }
        }
    }

}

public class IPManager
{
    public static string GetIP(ADDRESSFAM Addfam)
    {
        //Return null if ADDRESSFAM is Ipv6 but Os does not support it
        if (Addfam == ADDRESSFAM.IPv6 && !Socket.OSSupportsIPv6)
        {
            return null;
        }

        string output = "";

        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            NetworkInterfaceType _type1 = NetworkInterfaceType.Wireless80211;
            NetworkInterfaceType _type2 = NetworkInterfaceType.Ethernet;

            if ((item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2) && item.OperationalStatus == OperationalStatus.Up)
#endif 
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    //IPv4
                    if (Addfam == ADDRESSFAM.IPv4)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }

                    //IPv6
                    else if (Addfam == ADDRESSFAM.IPv6)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
        }
        return output;
    }
}

public enum ADDRESSFAM
{
    IPv4, IPv6
}
