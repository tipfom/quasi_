using Core.Graphics;
using System.Collections.Generic;

namespace Core.World {
    public interface IEntityRenderer {
        void QueueVertexData(int species, IEnumerable<VertexData> vertexData);

        void AddTexture(int species, Spritebatch2D entityTexture);
        bool HasTexture(int species);
    }
}
