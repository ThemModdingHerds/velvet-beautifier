#ifndef __TMH_IO_ENDIANNESS_HPP
#define __TMH_IO_ENDIANNESS_HPP

#include <istream>
#include <ostream>
#include <bit>

namespace TMH
{
    namespace IO
    {
        template<typename T>
        static T swapEndian(const T value)
        {
            T result;
            const char* cvalue = reinterpret_cast<const char*>(&value);
            char* cresult = reinterpret_cast<char*>(&result);
            size_t length = sizeof(T);
            for(size_t i = 0;i < length;i++)
                cresult[i] = cvalue[length - i - 1];
            return result;
        }
        template<typename T>
        static inline void writeBigEndian(::std::ostream &stream,const T &value)
        {
            T v = value;
            if constexpr(::std::endian::native != ::std::endian::big)
                v = swapEndian(v);
            stream.write(reinterpret_cast<const char*>(&v),sizeof(v));
        }
        template<typename T>
        static inline T readBigEndian(::std::istream &stream)
        {
            T value;
            stream.read(reinterpret_cast<char*>(&value),sizeof(value));
            if constexpr(::std::endian::native != ::std::endian::big)
                value = swapEndian(value);
            return value;
        }
        template<typename T>
        static inline void writeLittleEndian(::std::ostream &stream,const T &value)
        {
            T v = value;
            if constexpr(::std::endian::native != ::std::endian::little)
                v = swapEndian(v);
            stream.write(reinterpret_cast<const char*>(&v),sizeof(v));
        }
        template<typename T>
        static inline T readLittleEndian(::std::istream &stream)
        {
            T value;
            stream.read(reinterpret_cast<char*>(&value),sizeof(value));
            if constexpr(::std::endian::native != ::std::endian::little)
                value = swapEndian(value);
            return value;
        }
    }
}

#endif