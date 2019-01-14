using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Graphics.Buffer {
    public interface IBuffer : IDisposable {
        int Length { get; }
        int Bytes { get; }
    }
}
