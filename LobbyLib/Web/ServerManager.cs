﻿using EIV_Common;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using ModdableWebServer.Servers;
using NetCoreServer;
using System.Reflection;

namespace LobbyLib.Web;

public class ServerManager
{
    public static string IpPort = "http://127.0.0.1:7777/";
    public static string IpPort_WS = "ws://127.0.0.1:7777/";
    public static string IP = "127.0.0.1:7777";
    static WSS_Server? WSS_Server = null;
    static WS_Server? WS_Server = null;

    static bool IsSsl = true;
    static Dictionary<string, Dictionary<HTTPAttribute, MethodInfo>> HTTP_Plugins = [];
    static Dictionary<string, Dictionary<string, MethodInfo>> WS_Plugins = [];
    static Dictionary<HTTPAttribute, MethodInfo> Main_HTTP = [];
    static Dictionary<string, MethodInfo> Main_WS = [];
    public static void Start(string ip, int port, bool ssl = true, bool OnlyWS = false, bool IsCertValidate = false)
    {
        var ServerManagerAssembly = Assembly.GetAssembly(typeof(ServerManager));
        ArgumentNullException.ThrowIfNull(ServerManagerAssembly, nameof(ServerManagerAssembly));
        //JWTHandler.CreateRSA();
        IsSsl = ssl;
        if (ssl)
        {
            SslContext? context = null;

            if (IsCertValidate)
                context = CertHelper.GetContextNoValidate(System.Security.Authentication.SslProtocols.Tls12, ConfigINI.Read("config.ini", "Lobby", "PfxPath"), ConfigINI.Read("config.ini", "Lobby", "PfxPasword"));
            else
                context = CertHelper.GetContext(System.Security.Authentication.SslProtocols.Tls12, ConfigINI.Read("config.ini", "Lobby", "PfxPath"), ConfigINI.Read("config.ini", "Lobby", "PfxPasword"));
            WSS_Server = new(context, ip, port);

            Main_HTTP = AttributeMethodHelper.UrlHTTPLoader(ServerManagerAssembly);
            Main_WS = AttributeMethodHelper.UrlWSLoader(ServerManagerAssembly);
            WSS_Server.DoReturn404IfFail = false;
            WSS_Server.ReceivedFailed += Failed;
            WSS_Server.OverrideAttributes(ServerManagerAssembly);
            if (!OnlyWS)
                WSS_Server.OverrideAttributes(ServerManagerAssembly);
            WSS_Server.Start();
        }
        else
        {
            WS_Server = new(ip, port);
            WS_Server.OverrideAttributes(ServerManagerAssembly);
            if (!OnlyWS)
                WS_Server.OverrideAttributes(ServerManagerAssembly);
            WS_Server.DoReturn404IfFail = false;
            WS_Server.ReceivedFailed += Failed;
            WS_Server.Start();
        }
        IpPort = ssl ? $"https://{ip}:{port}/" : $"http://{ip}:{port}/";
        IpPort_WS = ssl ? $"wss://{ip}:{port}/" : $"ws://{ip}:{port}/";
        IP = $"{ip}:{port}";
        Console.WriteLine("Server started on " + IpPort + " | " + IpPort_WS);

    }

    public static void Failed(object? sender, HttpRequest request)
    {
        File.WriteAllText("REQUESTED.txt", request.Method + " " + request.Url + "\n" + request.Body + "\n" + request.ToString());
    }


    public static void Stop()
    {
        if (WS_Server != null)
        {
            WS_Server.Stop();
            WS_Server = null;
        }
        if (WSS_Server != null)
        {
            WSS_Server.Stop();
            WSS_Server = null;
        }

        Console.WriteLine("Server stopped.");
    }

    public static void AddRoutes(Assembly assembly)
    {
        if (IsSsl && WSS_Server != null)
        {
            var name = assembly.GetName().FullName;
            HTTP_Plugins.Add(name, AttributeMethodHelper.UrlHTTPLoader(assembly));
            WS_Plugins.Add(name, AttributeMethodHelper.UrlWSLoader(assembly));
            WSS_Server.MergeWSAttribute(assembly);
            WSS_Server.MergeAttribute(assembly);
        }
        if (!IsSsl && WS_Server != null)
        {
            WS_Server.MergeWSAttribute(assembly);
            WS_Server.MergeAttribute(assembly);
        }
    }

    public static void RemoveRoutes(Assembly assembly)
    {
        var name = assembly.GetName().FullName;
        HTTP_Plugins.Remove(name);
        WS_Plugins.Remove(name);
        if (IsSsl && WSS_Server != null)
        {
            WSS_Server.HTTP_AttributeToMethods = Main_HTTP;
            WSS_Server.WS_AttributeToMethods = Main_WS;
            foreach (var plugin in HTTP_Plugins)
            {
                if (plugin.Key == name)
                    return;

                foreach (var item in plugin.Value)
                {
                    WSS_Server.HTTP_AttributeToMethods.TryAdd(item.Key, item.Value);
                }
            }
            foreach (var plugin in WS_Plugins)
            {
                if (plugin.Key == name)
                    return;

                foreach (var item in plugin.Value)
                {
                    WSS_Server.WS_AttributeToMethods.TryAdd(item.Key, item.Value);
                }
            }
        }
        if (!IsSsl && WS_Server != null)
        {
            WS_Server.HTTP_AttributeToMethods = Main_HTTP;
            WS_Server.WS_AttributeToMethods = Main_WS;
            foreach (var plugin in HTTP_Plugins)
            {
                if (plugin.Key == name)
                    return;

                foreach (var item in plugin.Value)
                {
                    WS_Server.HTTP_AttributeToMethods.TryAdd(item.Key, item.Value);
                }
            }
            foreach (var plugin in WS_Plugins)
            {
                if (plugin.Key == name)
                    return;

                foreach (var item in plugin.Value)
                {
                    WS_Server.WS_AttributeToMethods.TryAdd(item.Key, item.Value);
                }
            }
        }
    }
}
