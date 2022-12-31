using Modding;
using System;
using System.Collections.Generic;

namespace RandoPlus.Imports
{
    /// <summary>
    /// Class containing methods to safely broadcast messages to ItemSync
    /// </summary>
    internal static class ItemSyncUtil
    {
        private static bool ItemSyncInstalled => ModHooks.GetMod("ItemSync") is not null;

        public static void Broadast(string label, string message)
        {
            if (!ItemSyncInstalled)
            {
                return;
            }

            BroadcastInternal(label, message);
        }

        private static void BroadcastInternal(string label, string message)
        {
            ItemSyncMod.ItemSyncMod.Connection.SendDataToAll(label, message);
        }

        private static readonly Dictionary<string, Action<string>> ItemSyncHandlers = new();

        /// <summary>
        /// Invoke the handler whenever an IS message with the given label is received.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="handler">(string message) -> (bool handled)</param>
        public static void AddHandler(string label, Action<string> handler)
        {
            ItemSyncHandlers[label] += handler;
            HookItemSync();
        }

        public static void RemoveHandler(string label, Action<string> handler)
        {
            ItemSyncHandlers[label] -= handler;
            
            if (ItemSyncHandlers[label] is null)
            {
                ItemSyncHandlers.Remove(label);
            }

            if (ItemSyncHandlers.Count == 0)
            {
                UnhookItemSync();
            }
        }

        private static bool _hooked = false;
        private static void HookItemSync()
        {
            if (_hooked)
            {
                return;
            }

            if (!ItemSyncInstalled)
            {
                return;
            }

            _hooked = true;
            HookInternal();
        }
        private static void UnhookItemSync()
        {
            if (!ItemSyncInstalled)
            {
                return;
            }

            _hooked = false;
            UnhookInternal();
        }


        private static void HookInternal()
        {
            ItemSyncMod.ItemSyncMod.Connection.OnDataReceived += HandleISMessages;
        }
        private static void UnhookInternal()
        {
            ItemSyncMod.ItemSyncMod.Connection.OnDataReceived -= HandleISMessages;
        }


        private static void HandleISMessages(MultiWorldLib.DataReceivedEvent e)
        {
            if (ItemSyncHandlers.TryGetValue(e.Label, out Action<string> handler) && handler is not null)
            {
                handler.Invoke(e.Content);
                e.Handled = true;
            }
        }
    }
}
