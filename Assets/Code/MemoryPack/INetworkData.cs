using MemoryPack;

namespace NetworkData
{
    [MemoryPackable]
    [MemoryPackUnion(0, typeof(InputData))]
    public partial interface INetworkData
    {

    }
}