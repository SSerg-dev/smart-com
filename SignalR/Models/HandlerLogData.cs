using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace SignalR.Models
{
    public class HandlerLogData : IEquatable<HandlerLogData>
    {
        public Guid HandlerId { get; }
        public string Status { get; set; }
        public string Log { get; }
        public Timer Timer { get; }

        public HandlerLogData(string handlerId, string log = null, Timer timer = null)
        {
            this.HandlerId = Guid.Parse(handlerId);
            this.Log = log;
            this.Timer = timer;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as HandlerLogData);
        }

        public bool Equals(HandlerLogData other)
        {
            if (other == null)
            {
                return false;
            }
            else if (other.GetType() != this.GetType())
            {
                return false;
            }
            else
            {
                return other.HandlerId == this.HandlerId;
            }
        }

        public override int GetHashCode()
        {
            return this.HandlerId.GetHashCode();
        }
    }
}
