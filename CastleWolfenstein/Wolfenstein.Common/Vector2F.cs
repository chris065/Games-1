using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfenstein.Common
{
    public struct Vector2F
    {
        public float X;
        public float Y;

        public Vector2F(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Vector2F operator *(Vector2F value1, int value2)
        {
            return new Vector2F(value1.X * value2, value1.Y * value2);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector2F) return this.Equals((Vector2F)obj);
            else return false;
        }

        public static bool operator ==(Vector2F value1, Vector2F value2)
        {
            return ((value1.X == value2.X) && (value1.Y == value2.Y));
        }

        public static bool operator !=(Vector2F value1, Vector2F value2)
        {
            if (value1.X == value2.X) return value1.Y != value2.Y;
            return true;
        }
        public override int GetHashCode()
        {
            return (this.X.GetHashCode() + this.Y.GetHashCode());
        }
    }
}
