using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowController : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    private const int GWL_STYLE = -16;
    private const uint WS_BORDER = 0x00800000;
    private const uint WS_POPUP = 0x80000000;
    private const uint SWP_SHOWWINDOW = 0x0040;

    void Start()
    {
        HideTitleBar();
        SetWindowPosition();
    }

    void HideTitleBar()
    {
        IntPtr hWnd = GetActiveWindow();
        int style = GetWindowLong(hWnd, GWL_STYLE);
        SetWindowLong(hWnd, GWL_STYLE, (uint)(style & ~WS_BORDER & ~WS_POPUP));
        SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, SWP_SHOWWINDOW);
    }

    void SetWindowPosition()
    {
        IntPtr hWnd = GetActiveWindow();
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;
        int windowWidth = 1920; // ���ô��ڿ��
        int windowHeight = 224; // ���ô��ڸ߶�

        // �����ڷ�������Ļ�ײ�
        int posX = (screenWidth - windowWidth) / 2;
        int posY = screenHeight - windowHeight;

        SetWindowPos(hWnd, IntPtr.Zero, posX, posY, windowWidth, windowHeight, SWP_SHOWWINDOW);
    }
}