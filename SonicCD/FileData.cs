// Decompiled with JetBrains decompiler
// Type: Retro_Engine.FileData
// Assembly: Sonic CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D35AF46A-1892-4F52-B201-E664C9200079
// Assembly location: E:\wamwo\Downloads\Sonic CD Mod Via Rebel\Sonic CD Mod Via Rebel\Sonic CD.dll

using System.IO;

namespace RetroEngine
{
    public class FileData
    {
        public char[] fileName = new char[64];
        public uint fileSize;
        public uint position;
        public uint bufferPos;
        public uint virtualFileOffset;
        public byte eStringPosA;
        public byte eStringPosB;
        public byte eStringNo;
        public bool eNybbleSwap;
        internal Stream fileStream;
        internal string filePath;
    }
}
