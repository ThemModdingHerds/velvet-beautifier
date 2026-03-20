#ifndef __TMH_IO_VECTORS_HPP
#define __TMH_IO_VECTORS_HPP

#include <istream>
#include <ostream>

namespace TMH
{
    namespace IO
    {
        template<typename T>
        union TVector2
        {
            struct {
                T x;
                T y;
            };
            T data[2];
        };
        template<typename T>
        union TVector3
        {
            struct {
                T x;
                T y;
                T z;
            };
            T data[3];
        };
        template<typename T>
        union TVector4
        {
            struct {
                T x;
                T y;
                T z;
                T w;
            };
            T data[4];
        };
        using Vector2f = TVector2<float>;
        using Vector3f = TVector3<float>;
        using Vector4f = TVector4<float>;
        template<typename T>
        static void writeVector2LE(::std::ostream &stream,const TVector2<T> &vector)
        {
            writeLittleEndian(stream,vector.x);
            writeLittleEndian(stream,vector.y);
        }
        template<typename T>
        static void writeVector2BE(::std::ostream &stream,TVector2<T> &vector)
        {
            writeBigEndian(stream,vector.x);
            writeBigEndian(stream,vector.y);
        }
        template<typename T>
        static TVector2<T> readVector2LE(::std::istream &stream)
        {
            TVector2<T> vector;
            vector.x = readLittleEndian(stream);
            vector.y = readLittleEndian(stream);
            return vector;
        }
        template<typename T>
        static TVector2<T> readVector2BE(::std::istream &stream)
        {
            TVector2<T> vector;
            vector.x = readBigEndian(stream);
            vector.y = readBigEndian(stream);
            return vector;
        }
        template<typename T>
        static void writeVector3LE(::std::ostream &stream,const TVector3<T> &vector)
        {
            writeLittleEndian(stream,vector.x);
            writeLittleEndian(stream,vector.y);
            writeLittleEndian(stream,vector.z);
        }
        template<typename T>
        static void writeVector3BE(::std::ostream &stream,TVector3<T> &vector)
        {
            writeBigEndian(stream,vector.x);
            writeBigEndian(stream,vector.y);
            writeBigEndian(stream,vector.z);
        }
        template<typename T>
        static TVector3<T> readVector3LE(::std::istream &stream)
        {
            TVector3<T> vector;
            vector.x = readLittleEndian(stream);
            vector.y = readLittleEndian(stream);
            vector.z = readLittleEndian(stream);
            return vector;
        }
        template<typename T>
        static TVector3<T> readVector3BE(::std::istream &stream)
        {
            TVector3<T> vector;
            vector.x = readBigEndian(stream);
            vector.y = readBigEndian(stream);
            vector.z = readBigEndian(stream);
            return vector;
        }
        template<typename T>
        static void writeVector4LE(::std::ostream &stream,const TVector4<T> &vector)
        {
            writeLittleEndian(stream,vector.x);
            writeLittleEndian(stream,vector.y);
            writeLittleEndian(stream,vector.z);
            writeLittleEndian(stream,vector.w);
        }
        template<typename T>
        static void writeVector4BE(::std::ostream &stream,TVector4<T> &vector)
        {
            writeBigEndian(stream,vector.x);
            writeBigEndian(stream,vector.y);
            writeBigEndian(stream,vector.z);
            writeBigEndian(stream,vector.w);
        }
        template<typename T>
        static TVector4<T> readVector4LE(::std::istream &stream)
        {
            TVector4<T> vector;
            vector.x = readLittleEndian(stream);
            vector.y = readLittleEndian(stream);
            vector.z = readLittleEndian(stream);
            vector.w = readLittleEndian(stream);
            return vector;
        }
        template<typename T>
        static TVector4<T> readVector4BE(::std::istream &stream)
        {
            TVector4<T> vector;
            vector.x = readBigEndian(stream);
            vector.y = readBigEndian(stream);
            vector.z = readBigEndian(stream);
            vector.w = readBigEndian(stream);
            return vector;
        }
    }
}

#endif