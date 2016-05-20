using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using Xilium.CefGlue;

public class OffscreenCEF : MonoBehaviour
{
    private bool mShouldQuit = false;
    private static OffscreenClient sCEFClient;

    private const int kWidth = 1280;
    private const int kHeight = 720;

    public Texture2D BrowserTexture;

    void Awake()
    {
        BrowserTexture = new Texture2D(kWidth, kHeight, TextureFormat.RGBA32, false);

        CefRuntime.Load();


        var mainArgs = new CefMainArgs(new string[] { });
        var mainApp = new OffscreenClient.OffscreenCEFApp();
        var settings = new CefSettings
        {
            MultiThreadedMessageLoop = false,
            SingleProcess = true,
            WindowlessRenderingEnabled = true,
            NoSandbox = true,
        };

        try {
            CefRuntime.Initialize(mainArgs, settings, mainApp, IntPtr.Zero);
        }
        catch (Exception e) {
            HTC.LogUtilility.LogE(e.Message);
        }

        string url = "http://www.google.com/";
        sCEFClient = new OffscreenClient(kWidth, kHeight);
        var browserSettings = new CefBrowserSettings();
        var windowSettings = CefWindowInfo.Create();
        windowSettings.SetAsWindowless(IntPtr.Zero, false);
        CefBrowserHost.CreateBrowser(windowSettings, sCEFClient, browserSettings, url);

        StartCoroutine("MessagePump");
        DontDestroyOnLoad(gameObject);
    }

    void OnDisable()
    {
        mShouldQuit = true;
        sCEFClient.Shutdown();
        CefRuntime.Shutdown();
    }

    IEnumerator MessagePump()
    {
        while (!mShouldQuit) {
            CefRuntime.DoMessageLoopWork();
            sCEFClient.UpdateTexture(BrowserTexture);
            yield return new WaitForEndOfFrame();
        }
    }
}

