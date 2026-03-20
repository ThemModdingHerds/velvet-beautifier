#ifndef __TMH_IO_MATRICES_HPP
#define __TMH_IO_MATRICES_HPP

#include "Vectors.hpp"

namespace TMH
{
    namespace IO
    {
        template<typename  T>
        union TMatrix3x3
        {
            struct {
                T m11;
                T m12;
                T m13;

                T m21;
                T m22;
                T m23;

                T m31;
                T m32;
                T m33;
            };
            struct {
                TVector3<T> row1;
                TVector3<T> row2;
                TVector3<T> row3;
            };
            T data[9];
        };
        template<typename  T>
        union TMatrix4x4
        {
            struct {
                T m11;
                T m12;
                T m13;
                T m14;

                T m21;
                T m22;
                T m23;
                T m24;

                T m31;
                T m32;
                T m33;
                T m34;

                T m41;
                T m42;
                T m43;
                T m44;
            };
            struct {
                TVector4<T> row1;
                TVector4<T> row2;
                TVector4<T> row3;
                TVector4<T> row4;
            };
            T data[16];
        };
        using Matrix3x3f = TMatrix3x3<float>;
        using Matrix4x4f = TMatrix4x4<float>;
        template<typename T>
        static inline void writeMatrix3x3LE(::std::ostream &stream,const TMatrix3x3<T> &matrix)
        {
            for(size_t i = 0;i < 9;i++)
                writeLittleEndian(stream,matrix.data[i]);
        }
        template<typename T>
        static inline TMatrix3x3<T> readMatrix3x3LE(::std::istream &stream)
        {
            TMatrix3x3<T> mat;
            for(size_t i = 0;i < 9;i++)
                mat.data[i] = readLittleEndian<T>(stream);
            return mat;
        }
        template<typename T>
        static inline void writeMatrix3x3BE(::std::ostream &stream,const TMatrix3x3<T> &matrix)
        {
            for(size_t i = 0;i < 9;i++)
                writeBigEndian(stream,matrix.data[i]);
        }
        template<typename T>
        static inline TMatrix3x3<T> readMatrix3x3BE(::std::istream &stream)
        {
            TMatrix3x3<T> mat;
            for(size_t i = 0;i < 9;i++)
                mat.data[i] = readBigEndian<T>(stream);
            return mat;
        }
        template<typename T>
        static inline void writeMatrix4x4LE(::std::ostream &stream,const TMatrix4x4<T> &matrix)
        {
            for(size_t i = 0;i < 16;i++)
                writeLittleEndian(stream,matrix.data[i]);
        }
        template<typename T>
        static inline TMatrix4x4<T> readMatrix4x4LE(::std::istream &stream)
        {
            TMatrix3x3<T> mat;
            for(size_t i = 0;i < 16;i++)
                mat.data[i] = readLittleEndian<T>(stream);
            return mat;
        }
        template<typename T>
        static inline void writeMatrix4x4BE(::std::ostream &stream,const TMatrix4x4<T> &matrix)
        {
            for(size_t i = 0;i < 16;i++)
                writeBigEndian(stream,matrix.data[i]);
        }
        template<typename T>
        static inline TMatrix4x4<T> readMatrix4x4BE(::std::istream &stream)
        {
            TMatrix4x4<T> mat;
            for(size_t i = 0;i < 16;i++)
                mat.data[i] = readBigEndian<T>(stream);
            return mat;
        }
    }
}

#endif