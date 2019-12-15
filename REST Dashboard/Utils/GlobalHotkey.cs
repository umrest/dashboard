using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace REST_Dashboard.Utils
{
    public class GlobalHotkey
    {
        [DllImport("User32.dll")]
        private static extern bool RegisterHotKey(
    [In] IntPtr hWnd,
    [In] int id,
    [In] uint fsModifiers,
    [In] uint vk);

        [DllImport("User32.dll")]
        private static extern bool UnregisterHotKey(
            [In] IntPtr hWnd,
            [In] int id);

        private HwndSource _source;
        private const int HOTKEY_ID = 9000;

        WindowInteropHelper helper;

        Action keypress_callback; 

        public GlobalHotkey(Window w, Action keypress_callback_in)
        {
            helper = new WindowInteropHelper(w);
            _source = HwndSource.FromHwnd(helper.Handle);
            _source.AddHook(HwndHook);
            RegisterHotKey();

            keypress_callback = keypress_callback_in;
        }
        public void disable()
        {
            _source.RemoveHook(HwndHook);
            _source = null;
            UnregisterHotKey();
        }
        ~GlobalHotkey()
        {
            disable();
        }

        private void RegisterHotKey()
        {
            const uint VK_SPACE = 0x20;
            const uint MOD_CTRL = 0x0002;
            if (!RegisterHotKey(helper.Handle, HOTKEY_ID, 0, VK_SPACE))
            {
                // handle error
            }
        }

        private void UnregisterHotKey()
        {
            UnregisterHotKey(helper.Handle, HOTKEY_ID);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:
                            OnHotKeyPressed();
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        private void OnHotKeyPressed()
        {
            // do stuff
            keypress_callback();
        }
    }
}
