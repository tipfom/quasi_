using System;
using Engine;

namespace Implementation
{
    class TestManager : Manager
    {
        public override void Init()
        {
            base.Init();
            Layer testLayer = new TestLayer();
            testLayer.Load();
            Layer.Attached = testLayer;
        }
    }
}
