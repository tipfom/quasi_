using System;

namespace Core.Exceptions {
    public class MapLoadException : Exception {
        public MapLoadException () : base ("could not load map") {

        }
    }
}
