﻿using System;
using System.Security.Cryptography;

namespace Impl.Test.Random
{
    interface IRandomProvider
    {
        int Next(int min, int max);
    }

    class SimpleRandomProvider : IRandomProvider
    {
        public int Next(int min, int max)
        {
            return new System.Random().Next(min, max);
        }
    }

    class RNGRandomProvider : IRandomProvider
    {
        private readonly RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();

        public int Next(int min, int max)
        {
            uint scale = uint.MaxValue;
            while (scale == uint.MaxValue)
            {
                // Get four random bytes.
                byte[] four_bytes = new byte[4];
                provider.GetBytes(four_bytes);

                // Convert that into an uint.
                scale = BitConverter.ToUInt32(four_bytes, 0);
            }

            // Add min to the scaled difference between max and min.
            return (int)(min + (max - min) *
                (scale / (double)uint.MaxValue));
        }
    }
}
