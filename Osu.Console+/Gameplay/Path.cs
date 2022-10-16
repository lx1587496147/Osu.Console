// osu.Framework.Utils.PathApproximator
// osuTK.Vector2
using OpenTK.Mathematics;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

public struct Vector2 : IEquatable<Vector2>
{
    public float X;

    public float Y;

    public static readonly Vector2 UnitX = new Vector2(1f, 0f);

    public static readonly Vector2 UnitY = new Vector2(0f, 1f);

    public static readonly Vector2 Zero = new Vector2(0f, 0f);

    public static readonly Vector2 One = new Vector2(1f, 1f);

    public static readonly int SizeInBytes = Marshal.SizeOf(default(Vector2));

    private static string listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;

    public float this[int index]
    {
        get
        {
            return index switch
            {
                0 => X,
                1 => Y,
                _ => throw new IndexOutOfRangeException("You tried to access this vector at index: " + index),
            };
        }
        set
        {
            switch (index)
            {
                case 0:
                    X = value;
                    break;
                case 1:
                    Y = value;
                    break;
                default:
                    throw new IndexOutOfRangeException("You tried to set this vector at index: " + index);
            }
        }
    }

    public float Length => (float)Math.Sqrt(X * X + Y * Y);

    public float LengthFast => 1f / MathHelper.InverseSqrtFast(X * X + Y * Y);

    public float LengthSquared => X * X + Y * Y;

    public Vector2 PerpendicularRight => new Vector2(Y, 0f - X);

    public Vector2 PerpendicularLeft => new Vector2(0f - Y, X);

    [XmlIgnore]
    public Vector2 Yx
    {
        get
        {
            return new Vector2(Y, X);
        }
        set
        {
            Y = value.X;
            X = value.Y;
        }
    }

    public Vector2(float value)
    {
        X = value;
        Y = value;
    }

    public Vector2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public Vector2 Normalized()
    {
        Vector2 result = this;
        result.Normalize();
        return result;
    }

    public void Normalize()
    {
        float num = 1f / Length;
        X *= num;
        Y *= num;
    }

    public void NormalizeFast()
    {
        float num = MathHelper.InverseSqrtFast(X * X + Y * Y);
        X *= num;
        Y *= num;
    }

    public static Vector2 Add(Vector2 a, Vector2 b)
    {
        Add(ref a, ref b, out a);
        return a;
    }

    public static void Add(ref Vector2 a, ref Vector2 b, out Vector2 result)
    {
        result.X = a.X + b.X;
        result.Y = a.Y + b.Y;
    }

    public static Vector2 Subtract(Vector2 a, Vector2 b)
    {
        Subtract(ref a, ref b, out a);
        return a;
    }

    public static void Subtract(ref Vector2 a, ref Vector2 b, out Vector2 result)
    {
        result.X = a.X - b.X;
        result.Y = a.Y - b.Y;
    }

    public static Vector2 Multiply(Vector2 vector, float scale)
    {
        Multiply(ref vector, scale, out vector);
        return vector;
    }

    public static void Multiply(ref Vector2 vector, float scale, out Vector2 result)
    {
        result.X = vector.X * scale;
        result.Y = vector.Y * scale;
    }

    public static Vector2 Multiply(Vector2 vector, Vector2 scale)
    {
        Multiply(ref vector, ref scale, out vector);
        return vector;
    }

    public static void Multiply(ref Vector2 vector, ref Vector2 scale, out Vector2 result)
    {
        result.X = vector.X * scale.X;
        result.Y = vector.Y * scale.Y;
    }

    public static Vector2 Divide(Vector2 vector, float scale)
    {
        Divide(ref vector, scale, out vector);
        return vector;
    }

    public static void Divide(ref Vector2 vector, float scale, out Vector2 result)
    {
        result.X = vector.X / scale;
        result.Y = vector.Y / scale;
    }

    public static Vector2 Divide(Vector2 vector, Vector2 scale)
    {
        Divide(ref vector, ref scale, out vector);
        return vector;
    }

    public static void Divide(ref Vector2 vector, ref Vector2 scale, out Vector2 result)
    {
        result.X = vector.X / scale.X;
        result.Y = vector.Y / scale.Y;
    }

    public static Vector2 ComponentMin(Vector2 a, Vector2 b)
    {
        a.X = ((a.X < b.X) ? a.X : b.X);
        a.Y = ((a.Y < b.Y) ? a.Y : b.Y);
        return a;
    }

    public static void ComponentMin(ref Vector2 a, ref Vector2 b, out Vector2 result)
    {
        result.X = ((a.X < b.X) ? a.X : b.X);
        result.Y = ((a.Y < b.Y) ? a.Y : b.Y);
    }

    public static Vector2 ComponentMax(Vector2 a, Vector2 b)
    {
        a.X = ((a.X > b.X) ? a.X : b.X);
        a.Y = ((a.Y > b.Y) ? a.Y : b.Y);
        return a;
    }

    public static void ComponentMax(ref Vector2 a, ref Vector2 b, out Vector2 result)
    {
        result.X = ((a.X > b.X) ? a.X : b.X);
        result.Y = ((a.Y > b.Y) ? a.Y : b.Y);
    }

    public static Vector2 MagnitudeMin(Vector2 left, Vector2 right)
    {
        if (!(left.LengthSquared < right.LengthSquared))
        {
            return right;
        }
        return left;
    }

    public static void MagnitudeMin(ref Vector2 left, ref Vector2 right, out Vector2 result)
    {
        result = ((left.LengthSquared < right.LengthSquared) ? left : right);
    }

    public static Vector2 MagnitudeMax(Vector2 left, Vector2 right)
    {
        if (!(left.LengthSquared >= right.LengthSquared))
        {
            return right;
        }
        return left;
    }

    public static void MagnitudeMax(ref Vector2 left, ref Vector2 right, out Vector2 result)
    {
        result = ((left.LengthSquared >= right.LengthSquared) ? left : right);
    }

    [Obsolete("Use MagnitudeMin() instead.")]
    public static Vector2 Min(Vector2 left, Vector2 right)
    {
        if (!(left.LengthSquared < right.LengthSquared))
        {
            return right;
        }
        return left;
    }

    [Obsolete("Use MagnitudeMax() instead.")]
    public static Vector2 Max(Vector2 left, Vector2 right)
    {
        if (!(left.LengthSquared >= right.LengthSquared))
        {
            return right;
        }
        return left;
    }

    public static Vector2 Clamp(Vector2 vec, Vector2 min, Vector2 max)
    {
        vec.X = ((vec.X < min.X) ? min.X : ((vec.X > max.X) ? max.X : vec.X));
        vec.Y = ((vec.Y < min.Y) ? min.Y : ((vec.Y > max.Y) ? max.Y : vec.Y));
        return vec;
    }

    public static void Clamp(ref Vector2 vec, ref Vector2 min, ref Vector2 max, out Vector2 result)
    {
        result.X = ((vec.X < min.X) ? min.X : ((vec.X > max.X) ? max.X : vec.X));
        result.Y = ((vec.Y < min.Y) ? min.Y : ((vec.Y > max.Y) ? max.Y : vec.Y));
    }

    public static float Distance(Vector2 vec1, Vector2 vec2)
    {
        Distance(ref vec1, ref vec2, out var result);
        return result;
    }

    public static void Distance(ref Vector2 vec1, ref Vector2 vec2, out float result)
    {
        result = (float)Math.Sqrt((vec2.X - vec1.X) * (vec2.X - vec1.X) + (vec2.Y - vec1.Y) * (vec2.Y - vec1.Y));
    }

    public static float DistanceSquared(Vector2 vec1, Vector2 vec2)
    {
        DistanceSquared(ref vec1, ref vec2, out var result);
        return result;
    }

    public static void DistanceSquared(ref Vector2 vec1, ref Vector2 vec2, out float result)
    {
        result = (vec2.X - vec1.X) * (vec2.X - vec1.X) + (vec2.Y - vec1.Y) * (vec2.Y - vec1.Y);
    }

    public static Vector2 Normalize(Vector2 vec)
    {
        float num = 1f / vec.Length;
        vec.X *= num;
        vec.Y *= num;
        return vec;
    }

    public static void Normalize(ref Vector2 vec, out Vector2 result)
    {
        float num = 1f / vec.Length;
        result.X = vec.X * num;
        result.Y = vec.Y * num;
    }

    public static Vector2 NormalizeFast(Vector2 vec)
    {
        float num = MathHelper.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y);
        vec.X *= num;
        vec.Y *= num;
        return vec;
    }

    public static void NormalizeFast(ref Vector2 vec, out Vector2 result)
    {
        float num = MathHelper.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y);
        result.X = vec.X * num;
        result.Y = vec.Y * num;
    }

    public static float Dot(Vector2 left, Vector2 right)
    {
        return left.X * right.X + left.Y * right.Y;
    }

    public static void Dot(ref Vector2 left, ref Vector2 right, out float result)
    {
        result = left.X * right.X + left.Y * right.Y;
    }

    public static float PerpDot(Vector2 left, Vector2 right)
    {
        return left.X * right.Y - left.Y * right.X;
    }

    public static void PerpDot(ref Vector2 left, ref Vector2 right, out float result)
    {
        result = left.X * right.Y - left.Y * right.X;
    }

    public static Vector2 Lerp(Vector2 a, Vector2 b, float blend)
    {
        a.X = blend * (b.X - a.X) + a.X;
        a.Y = blend * (b.Y - a.Y) + a.Y;
        return a;
    }

    public static void Lerp(ref Vector2 a, ref Vector2 b, float blend, out Vector2 result)
    {
        result.X = blend * (b.X - a.X) + a.X;
        result.Y = blend * (b.Y - a.Y) + a.Y;
    }

    public static Vector2 BaryCentric(Vector2 a, Vector2 b, Vector2 c, float u, float v)
    {
        return a + u * (b - a) + v * (c - a);
    }

    public static void BaryCentric(ref Vector2 a, ref Vector2 b, ref Vector2 c, float u, float v, out Vector2 result)
    {
        result = a;
        Vector2 a2 = b;
        Subtract(ref a2, ref a, out a2);
        Multiply(ref a2, u, out a2);
        Add(ref result, ref a2, out result);
        a2 = c;
        Subtract(ref a2, ref a, out a2);
        Multiply(ref a2, v, out a2);
        Add(ref result, ref a2, out result);
    }

    public static Vector2 Transform(Vector2 vec, Quaternion quat)
    {
        Transform(ref vec, ref quat, out var result);
        return result;
    }

    public static void Transform(ref Vector2 vec, ref Quaternion quat, out Vector2 result)
    {
        Quaternion right = new Quaternion(vec.X, vec.Y, 0f, 0f);
        Quaternion.Invert(quat, out var result2);
        Quaternion.Multiply(quat, right, out var result3);
        Quaternion.Multiply(result3, result2, out right);
        result.X = right.X;
        result.Y = right.Y;
    }

    public static Vector2 operator +(Vector2 left, Vector2 right)
    {
        left.X += right.X;
        left.Y += right.Y;
        return left;
    }

    public static Vector2 operator -(Vector2 left, Vector2 right)
    {
        left.X -= right.X;
        left.Y -= right.Y;
        return left;
    }

    public static Vector2 operator -(Vector2 vec)
    {
        vec.X = 0f - vec.X;
        vec.Y = 0f - vec.Y;
        return vec;
    }

    public static Vector2 operator *(Vector2 vec, float scale)
    {
        vec.X *= scale;
        vec.Y *= scale;
        return vec;
    }

    public static Vector2 operator *(float scale, Vector2 vec)
    {
        vec.X *= scale;
        vec.Y *= scale;
        return vec;
    }

    public static Vector2 operator *(Vector2 vec, Vector2 scale)
    {
        vec.X *= scale.X;
        vec.Y *= scale.Y;
        return vec;
    }

    public static Vector2 operator /(Vector2 vec, float scale)
    {
        vec.X /= scale;
        vec.Y /= scale;
        return vec;
    }

    public static bool operator ==(Vector2 left, Vector2 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vector2 left, Vector2 right)
    {
        return !left.Equals(right);
    }

    public override string ToString()
    {
        return string.Format("({0}{2} {1})", X, Y, listSeparator);
    }

    public override int GetHashCode()
    {
        return (X.GetHashCode() * 397) ^ Y.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Vector2))
        {
            return false;
        }
        return Equals((Vector2)obj);
    }

    public bool Equals(Vector2 other)
    {
        if (X == other.X)
        {
            return Y == other.Y;
        }
        return false;
    }
}

public static class PathApproximator
{
    private readonly struct CircularArcProperties
    {
        public readonly bool IsValid;

        public readonly double ThetaStart;

        public readonly double ThetaRange;

        public readonly double Direction;

        public readonly float Radius;

        public readonly Vector2 Centre;

        public double ThetaEnd => ThetaStart + ThetaRange * Direction;

        public CircularArcProperties(double thetaStart, double thetaRange, double direction, float radius, Vector2 centre)
        {
            IsValid = true;
            ThetaStart = thetaStart;
            ThetaRange = thetaRange;
            Direction = direction;
            Radius = radius;
            Centre = centre;
        }
    }

    private const float bezier_tolerance = 0.25f;

    private const int catmull_detail = 50;

    private const float circular_arc_tolerance = 0.1f;

    public static List<Vector2> ApproximateBezier(ReadOnlySpan<Vector2> controlPoints)
    {
        return ApproximateBSpline(controlPoints);
    }

    public static List<Vector2> ApproximateBSpline(ReadOnlySpan<Vector2> controlPoints, int p = 0)
    {
        List<Vector2> list = new List<Vector2>();
        int num = controlPoints.Length - 1;
        if (num < 0)
        {
            return list;
        }
        Stack<Vector2[]> stack = new Stack<Vector2[]>();
        Stack<Vector2[]> stack2 = new Stack<Vector2[]>();
        Vector2[] array = controlPoints.ToArray();
        if (p > 0 && p < num)
        {
            for (int i = 0; i < num - p; i++)
            {
                Vector2[] array2 = new Vector2[p + 1];
                array2[0] = array[i];
                for (int j = 0; j < p - 1; j++)
                {
                    array2[j + 1] = array[i + 1];
                    for (int k = 1; k < p - j; k++)
                    {
                        int num2 = Math.Min(k, num - p - i);
                        array[i + k] = (num2 * array[i + k] + array[i + k + 1]) / (num2 + 1);
                    }
                }
                array2[p] = array[i + 1];
                stack.Push(array2);
            }
            stack.Push(array[(num - p)..]);
            stack = new Stack<Vector2[]>(stack);
        }
        else
        {
            p = num;
            stack.Push(array);
        }
        Vector2[] array3 = new Vector2[p + 1];
        Vector2[] array4 = new Vector2[p * 2 + 1];
        Vector2[] array5 = array4;
        while (stack.Count > 0)
        {
            Vector2[] array6 = stack.Pop();
            if (bezierIsFlatEnough(array6))
            {
                bezierApproximate(array6, list, array3, array4, p + 1);
                stack2.Push(array6);
                continue;
            }
            Vector2[] array7 = ((stack2.Count > 0) ? stack2.Pop() : new Vector2[p + 1]);
            bezierSubdivide(array6, array5, array7, array3, p + 1);
            for (int l = 0; l < p + 1; l++)
            {
                array6[l] = array5[l];
            }
            stack.Push(array7);
            stack.Push(array6);
        }
        list.Add(controlPoints[num]);
        return list;
    }

    public static List<Vector2> ApproximateCatmull(ReadOnlySpan<Vector2> controlPoints)
    {
        List<Vector2> list = new List<Vector2>((controlPoints.Length - 1) * 50 * 2);
        for (int i = 0; i < controlPoints.Length - 1; i++)
        {
            Vector2 vec = ((i > 0) ? controlPoints[i - 1] : controlPoints[i]);
            Vector2 vec2 = controlPoints[i];
            Vector2 vec3 = ((i < controlPoints.Length - 1) ? controlPoints[i + 1] : (vec2 + vec2 - vec));
            Vector2 vec4 = ((i < controlPoints.Length - 2) ? controlPoints[i + 2] : (vec3 + vec3 - vec2));
            for (int j = 0; j < 50; j++)
            {
                list.Add(catmullFindPoint(ref vec, ref vec2, ref vec3, ref vec4, (float)j / 50f));
                list.Add(catmullFindPoint(ref vec, ref vec2, ref vec3, ref vec4, (float)(j + 1) / 50f));
            }
        }
        return list;
    }

    public static List<Vector2> ApproximateCircularArc(ReadOnlySpan<Vector2> controlPoints)
    {
        CircularArcProperties circularArcProperties = PathApproximator.circularArcProperties(controlPoints);
        if (!circularArcProperties.IsValid)
        {
            return ApproximateBezier(controlPoints);
        }
        int num = ((2f * circularArcProperties.Radius <= 0.1f) ? 2 : Math.Max(2, (int)Math.Ceiling(circularArcProperties.ThetaRange / (2.0 * Math.Acos(1f - 0.1f / circularArcProperties.Radius)))));
        List<Vector2> list = new List<Vector2>(num);
        for (int i = 0; i < num; i++)
        {
            double num2 = (double)i / (double)(num - 1);
            double num3 = circularArcProperties.ThetaStart + circularArcProperties.Direction * num2 * circularArcProperties.ThetaRange;
            Vector2 vector = new Vector2((float)Math.Cos(num3), (float)Math.Sin(num3)) * circularArcProperties.Radius;
            list.Add(circularArcProperties.Centre + vector);
        }
        return list;
    }

    public static RectangleF CircularArcBoundingBox(ReadOnlySpan<Vector2> controlPoints)
    {
        CircularArcProperties circularArcProperties = PathApproximator.circularArcProperties(controlPoints);
        if (!circularArcProperties.IsValid)
        {
            return RectangleF.Empty;
        }
        List<Vector2> list = new List<Vector2>
        {
            controlPoints[0],
            controlPoints[2]
        };
        double num = Math.PI / 2.0 * circularArcProperties.Direction;
        double num2 = circularArcProperties.ThetaStart / (Math.PI / 2.0);
        double num3 = Math.PI / 2.0 * ((circularArcProperties.Direction > 0.0) ? Math.Ceiling(num2) : Math.Floor(num2));
        for (int i = 0; i < 4; i++)
        {
            double num4 = num3 + num * (double)i;
            //if (Precision.DefinitelyBigger((num4 - circularArcProperties.ThetaEnd) * circularArcProperties.Direction, 0.0))
            //{
            //    break;
            //}
            Vector2 vector = new Vector2((float)Math.Cos(num4), (float)Math.Sin(num4)) * circularArcProperties.Radius;
            list.Add(circularArcProperties.Centre + vector);
        }
        float num5 = list.Min((Vector2 p) => p.X);
        float num6 = list.Min((Vector2 p) => p.Y);
        float num7 = list.Max((Vector2 p) => p.X);
        float num8 = list.Max((Vector2 p) => p.Y);
        return new RectangleF(num5, num6, num7 - num5, num8 - num6);
    }

    public static List<Vector2> ApproximateLinear(ReadOnlySpan<Vector2> controlPoints)
    {
        List<Vector2> list = new List<Vector2>(controlPoints.Length);
        ReadOnlySpan<Vector2> readOnlySpan = controlPoints;
        for (int i = 0; i < readOnlySpan.Length; i++)
        {
            Vector2 item = readOnlySpan[i];
            list.Add(item);
        }
        return list;
    }

    //public static List<Vector2> ApproximateLagrangePolynomial(ReadOnlySpan<Vector2> controlPoints)
    //{
    //    List<Vector2> list = new List<Vector2>(51);
    //    double[] weights = Interpolation.BarycentricWeights(controlPoints);
    //    float num = controlPoints[0].X;
    //    float num2 = controlPoints[0].X;
    //    for (int i = 1; i < controlPoints.Length; i++)
    //    {
    //        num = Math.Min(num, controlPoints[i].X);
    //        num2 = Math.Max(num2, controlPoints[i].X);
    //    }
    //    float num3 = num2 - num;
    //    for (int j = 0; j < 51; j++)
    //    {
    //        float num4 = num + num3 / 50f * (float)j;
    //        float y = (float)Interpolation.BarycentricLagrange(controlPoints, weights, num4);
    //        list.Add(new Vector2(num4, y));
    //    }
    //    return list;
    //}

    private static CircularArcProperties circularArcProperties(ReadOnlySpan<Vector2> controlPoints)
    {
        Vector2 vector = controlPoints[0];
        Vector2 vector2 = controlPoints[1];
        Vector2 vector3 = controlPoints[2];
        //if (Precision.AlmostEquals(0f, (vector2.Y - vector.Y) * (vector3.X - vector.X) - (vector2.X - vector.X) * (vector3.Y - vector.Y)))
        //{
        //    return default(CircularArcProperties);
        //}
        float num = 2f * (vector.X * (vector2 - vector3).Y + vector2.X * (vector3 - vector).Y + vector3.X * (vector - vector2).Y);
        float lengthSquared = vector.LengthSquared;
        float lengthSquared2 = vector2.LengthSquared;
        float lengthSquared3 = vector3.LengthSquared;
        Vector2 vector4 = new Vector2(lengthSquared * (vector2 - vector3).Y + lengthSquared2 * (vector3 - vector).Y + lengthSquared3 * (vector - vector2).Y, lengthSquared * (vector3 - vector2).X + lengthSquared2 * (vector - vector3).X + lengthSquared3 * (vector2 - vector).X) / num;
        Vector2 vector5 = vector - vector4;
        Vector2 vector6 = vector3 - vector4;
        float length = vector5.Length;
        double num2 = Math.Atan2(vector5.Y, vector5.X);
        double num3;
        for (num3 = Math.Atan2(vector6.Y, vector6.X); num3 < num2; num3 += Math.PI * 2.0)
        {
        }
        double num4 = 1.0;
        double num5 = num3 - num2;
        Vector2 vector7 = vector3 - vector;
        vector7 = new Vector2(vector7.Y, 0f - vector7.X);
        if (Vector2.Dot(vector7, vector2 - vector) < 0f)
        {
            num4 = 0.0 - num4;
            num5 = Math.PI * 2.0 - num5;
        }
        return new CircularArcProperties(num2, num5, num4, length, vector4);
    }

    private static bool bezierIsFlatEnough(Vector2[] controlPoints)
    {
        for (int i = 1; i < controlPoints.Length - 1; i++)
        {
            if ((controlPoints[i - 1] - 2f * controlPoints[i] + controlPoints[i + 1]).LengthSquared > 0.25f)
            {
                return false;
            }
        }
        return true;
    }

    private static void bezierSubdivide(Vector2[] controlPoints, Vector2[] l, Vector2[] r, Vector2[] subdivisionBuffer, int count)
    {
        for (int i = 0; i < count; i++)
        {
            subdivisionBuffer[i] = controlPoints[i];
        }
        for (int j = 0; j < count; j++)
        {
            l[j] = subdivisionBuffer[0];
            r[count - j - 1] = subdivisionBuffer[count - j - 1];
            for (int k = 0; k < count - j - 1; k++)
            {
                subdivisionBuffer[k] = (subdivisionBuffer[k] + subdivisionBuffer[k + 1]) / 2f;
            }
        }
    }

    private static void bezierApproximate(Vector2[] controlPoints, List<Vector2> output, Vector2[] subdivisionBuffer1, Vector2[] subdivisionBuffer2, int count)
    {
        bezierSubdivide(controlPoints, subdivisionBuffer2, subdivisionBuffer1, subdivisionBuffer1, count);
        for (int i = 0; i < count - 1; i++)
        {
            subdivisionBuffer2[count + i] = subdivisionBuffer1[i + 1];
        }
        output.Add(controlPoints[0]);
        for (int j = 1; j < count - 1; j++)
        {
            int num = 2 * j;
            Vector2 item = 0.25f * (subdivisionBuffer2[num - 1] + 2f * subdivisionBuffer2[num] + subdivisionBuffer2[num + 1]);
            output.Add(item);
        }
    }

    private static Vector2 catmullFindPoint(ref Vector2 vec1, ref Vector2 vec2, ref Vector2 vec3, ref Vector2 vec4, float t)
    {
        float num = t * t;
        float num2 = t * num;
        Vector2 result = default(Vector2);
        result.X = 0.5f * (2f * vec2.X + (0f - vec1.X + vec3.X) * t + (2f * vec1.X - 5f * vec2.X + 4f * vec3.X - vec4.X) * num + (0f - vec1.X + 3f * vec2.X - 3f * vec3.X + vec4.X) * num2);
        result.Y = 0.5f * (2f * vec2.Y + (0f - vec1.Y + vec3.Y) * t + (2f * vec1.Y - 5f * vec2.Y + 4f * vec3.Y - vec4.Y) * num + (0f - vec1.Y + 3f * vec2.Y - 3f * vec3.Y + vec4.Y) * num2);
        return result;
    }
}
