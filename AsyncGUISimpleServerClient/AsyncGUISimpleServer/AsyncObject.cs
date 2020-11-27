﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AsyncGUISimpleServer
{

    class AsyncObject
    {
        public byte[] Buffer;
        public Socket workingSocket;
        public readonly int BufferSize;

        public AsyncObject(int bufferSize)
        {
            BufferSize = bufferSize;
            Buffer = new byte[BufferSize];
        }
        public void ClearBuffer()
        {
            Array.Clear(Buffer, 0, BufferSize);
        }

    }
}
