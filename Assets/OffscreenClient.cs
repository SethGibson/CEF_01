using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using Xilium.CefGlue;
using System;
using HTC;

public class OffscreenClient : CefClient
{
    private readonly OffscreenLoadHandler   mOnLoad;
    private readonly OffscreenRenderHandler mOnRender;

    private static System.Object    sPixelLock;
    private static byte[]           sPixelBuffer;

    private static CefBrowserHost   sHost;

    public OffscreenClient(int pWidth, int pHeight)
    {
        mOnLoad = new OffscreenLoadHandler();
        mOnRender = new OffscreenRenderHandler(pWidth, pHeight);

        sPixelLock = new object();
        sPixelBuffer = new byte[pWidth * pHeight * 4];

        LogUtilility.LogI("Constructed Offscreen Client");
    }

    public void UpdateTexture(Texture2D pTexture)
    {
        if (sHost != null)
        {
            lock (sPixelLock)
            {
                pTexture.LoadRawTextureData(sPixelBuffer);
                pTexture.Apply();
            }
        }
    }

    public void Shutdown()
    {
        if(sHost!=null) {
            LogUtilility.LogI("Host Cleanup");
            sHost.Dispose();
        }
    }
    #region Interface 
    protected override CefRenderHandler GetRenderHandler()
    {
        return mOnRender;
    }

    protected override CefLoadHandler GetLoadHandler()
    {
        return mOnLoad;
    }
    #endregion

    #region Handlers
    internal class OffscreenLoadHandler : CefLoadHandler
    {
        public OffscreenLoadHandler()
        {
            LogUtilility.LogI("Constructed Load Handler.");
        }
        protected override void OnLoadStart(CefBrowser browser, CefFrame frame)
        {
            LogUtilility.LogI("load handler load start...");
            if (browser != null) {
                sHost = browser.GetHost();
            }
            if (frame.IsMain)
                LogUtilility.LogI("START: {0}", browser.GetMainFrame().Url);
        }

        protected override void OnLoadEnd(CefBrowser browser, CefFrame frame, int httpStatusCode)
        {
            if (frame.IsMain)
                LogUtilility.LogI("END: {0}, {1}", browser.GetMainFrame().Url, httpStatusCode.ToString());
        }
    }

    internal class OffscreenRenderHandler : CefRenderHandler
    {
        private readonly int _width;
        private readonly int _height;

        public OffscreenRenderHandler(int pWidth, int pHeight)
        {
            _width = pWidth;
            _height = pHeight;
            LogUtilility.LogI("Constructed Render Handler.");
        }

        protected override bool GetRootScreenRect(CefBrowser browser, ref CefRectangle rect)
        {
            return GetViewRect(browser, ref rect);
        }

        protected override bool GetScreenPoint(CefBrowser browser, int viewX, int viewY, ref int screenX, ref int screenY)
        {
            screenX = viewX;
            screenY = viewY;
            return true;
        }

        protected override bool GetViewRect(CefBrowser browser, ref CefRectangle rect)
        {
            rect.X = 0;
            rect.Y = 0;
            rect.Width = _width;
            rect.Height = _height;
            return true;
        }

        protected override void OnPaint(CefBrowser browser, CefPaintElementType type, CefRectangle[] dirtyRects, IntPtr buffer, int width, int height)
        {
            if (browser != null)
            {
                lock (sPixelLock)
                {
                    Marshal.Copy(buffer, sPixelBuffer, 0, sPixelBuffer.Length);
                }
            }
        }

        protected override bool GetScreenInfo(CefBrowser browser, CefScreenInfo screenInfo)
        {
            return false;
        }

        protected override void OnCursorChange(CefBrowser browser, IntPtr cursorHandle, CefCursorType type, CefCursorInfo customCursorInfo)
        {
        }

        protected override void OnPopupSize(CefBrowser browser, CefRectangle rect)
        {
        }

        protected override void OnScrollOffsetChanged(CefBrowser browser, double x, double y)
        {
        }
    }
    #endregion

    public class OffscreenCEFApp : CefApp
    {
    }
}
