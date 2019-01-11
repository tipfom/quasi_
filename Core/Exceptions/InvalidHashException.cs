using System;

namespace Core.Exceptions {
    public class InvalidHashException : Exception {
        public InvalidHashException (Exception innerException) : base ("while loading data, an invalid hash value was detected", innerException) {

        }
    }
}
