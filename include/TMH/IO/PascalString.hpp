#ifndef __TMH_IO_PASCALSTRING_HPP
#define __TMH_IO_PASCALSTRING_HPP

#include "Endianness.hpp"

#include <string>
#include <istream>
#include <ostream>

namespace TMH
{
    namespace IO
    {
        template<typename T,typename Char = char>
        void writePascalStringLE(::std::ostream &stream,const ::std::basic_string<Char> &string)
        {
            stream.write(&string.size(),sizeof(T));
            for(T i = 0;i < string.size();i++)
                stream.write(string.at(i),sizeof(Char));
        }
        template<typename T,typename Char = char>
        void writePascalStringBE(::std::ostream &stream,const ::std::basic_string<Char> &string)
        {
            writeBigEndian(stream,string.size());
            for(T i = 0;i < string.size();i++)
                writeBigEndian(stream,string.at(i));
        }
        template<typename T,typename Char = char>
        ::std::basic_string<Char> readPascalStringLE(::std::istream &stream)
        {
            T length;
            stream.read(&length,sizeof(length));
            ::std::basic_string<Char> string(length,static_cast<Char>(' '));
            for(T i = 0;i < length;i++)
                stream.read(&string[i],sizeof(Char));
            return string;
        }
        template<typename T,typename Char = char>
        ::std::basic_string<Char> readPascalStringBE(::std::istream &stream)
        {
            T length = readBigEndian<T>(stream);
            ::std::basic_string<Char> string(length,static_cast<Char>(' '));
            for(T i = 0;i < length;i++)
                string[i] = readBigEndian<Char>(stream);
            return string;
        }
    }
}

#endif