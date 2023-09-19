using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerkleTree;

internal static class NodeExtensions
{
    public static NodeInformation Convert(this Node node) => NodeInformation.Of(node.Id, node.Value, node.Level);

}