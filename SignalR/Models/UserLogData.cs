using System;

namespace SignalR.Models
{
    public class UserLogData : IEquatable<UserLogData>
    {
        public Guid ConnectionId { get; }
        public string Group { get; }

        public UserLogData(string connectionId, string group = null)
        {
            this.ConnectionId = Guid.Parse(connectionId);
            this.Group = group;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as UserLogData);
        }

        public bool Equals(UserLogData other)
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
                return other.ConnectionId == this.ConnectionId;
            }
        }

        public override int GetHashCode()
        {
            return this.ConnectionId.GetHashCode();
        }
    }
}