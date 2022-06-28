using System.Collections.Concurrent;
using SteveClient.Engine.Networking.Connections;
using SteveClient.Engine.Networking.Packets;
using SteveClient.Engine.Networking.Packets.ClientBound;

namespace SteveClient.Engine.Engines.PacketProcessing;

public abstract class PacketProcessingEngine<TPacket> : BaseEngine
    where TPacket : ClientBoundPacket
{
    private readonly ConcurrentQueue<ConsumablePacket<TPacket>> _packetQueue = new();
    protected ConnectionState TargetState = ConnectionState.Play;

    public override void Ready()
    {
        PacketRegistry.RegisterClientBoundPacketListener<TPacket>(TargetState, PacketReceived);
    }

    public override void Execute(float delta)
    {
        if (_packetQueue.IsEmpty)
            return;

        if (!_packetQueue.TryPeek(out var consumablePacket))
            return;
        
        Execute(delta, consumablePacket);

        if (consumablePacket.Consumed)
            _packetQueue.TryDequeue(out _);
    }

    protected abstract void Execute(float delta, ConsumablePacket<TPacket> consumablePacket);

    private void PacketReceived(ClientBoundPacket packet)
    {
        _packetQueue.Enqueue(new ConsumablePacket<TPacket>(packet));
    }

    protected class ConsumablePacket<TSubPacket>
        where TSubPacket : ClientBoundPacket
    {
        private readonly TSubPacket _packet;
        public bool Consumed { get; private set; }

        public ConsumablePacket(ClientBoundPacket packet)
        {
            _packet = (TSubPacket)packet;
        }

        public void MarkConsumed() => Consumed = true;

        public TSubPacket Get() => _packet;

        public static implicit operator TSubPacket(ConsumablePacket<TSubPacket> right) => right.Get();
    }
}