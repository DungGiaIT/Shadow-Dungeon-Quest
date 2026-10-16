using NUnit.Framework;
using Unity.BossRoom.ConnectionManagement;
using UnityEngine;

namespace Unity.BossRoom.Tests.Runtime
{
    public class DisconnectReasonTests
    {
        [Test]
        public void ServerStatusIsPreserved()
        {
            var reason = JsonUtility.ToJson(ConnectStatus.ServerFull);
            Assert.AreEqual(ConnectStatus.ServerFull,
                ConnectionManager.ParseDisconnectReason(reason, ConnectStatus.GenericDisconnect));
        }

        [Test]
        public void TransportTextUsesFallback()
        {
            Assert.AreEqual(ConnectStatus.StartClientFailed,
                ConnectionManager.ParseDisconnectReason("Failed to connect to server.", ConnectStatus.StartClientFailed));
        }
    }
}
