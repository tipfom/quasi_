using System;
using OpenTK.Graphics.ES20;

#if __ANDROID__
using Android.Util;
#endif

namespace Engine
{
#if DEBUG
    public static class Debug
    {
        public static void Print(object sender, string message, params object[] args)
        {
            Print(sender.GetType(), message, args);
        }

        public static void Print(object sender, string message)
        {
            Print(sender.GetType(), message);
        }

        public static void Print(Type sender, string message, params object[] args)
        {
            Print(sender, String.Format(message, args));
        }

        public static void Print(Type sender, string message)
        {
#if __ANDROID__
            Log.Debug(sender.Name, message);
#elif __WINDOWS__
            Console.WriteLine($"[{sender.Name}] : {message}");
#else
            System.Diagnostics.Debug.WriteLine(sender.Name + ": " + message);
#endif
        }

        public static void Print(object sender, object message)
        {
            Print(sender.GetType(), message.ToString());
        }

        public static void Print(Type sender, object message)
        {
            Print(sender, message.ToString());
        }

        public static void CheckGL(object sender)
        {
            CheckGL(sender.GetType());
        }

        public static bool CheckGL(Type sender)
        {
            ErrorCode error =
#if __ANDROID__
                GL.GetErrorCode();
#elif __WINDOWS__
                GL.GetError();
#else
                ErrorCode.NoError;
#endif
            if (error != ErrorCode.NoError) {
                Print(sender, $"OPENGLERROR {error}");
            }
            return error != ErrorCode.NoError;
        }
    }
#endif
}
