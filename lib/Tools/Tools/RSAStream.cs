﻿using System.IO;
using System.Threading;

namespace System.Security.Cryptography
{
    public static partial class Crypto
    {
        public static void Encrypt(this RSACryptoServiceProvider rsa, byte[] input, Stream output)
        {
            int blockSize = rsa.KeySize / 8 - 11;
            int offset = 0;
            while (offset < input.Length)
            {
                int tmp_size = input.Length - offset;
                if (tmp_size > blockSize)
                    tmp_size = blockSize;
                byte[] tmp = new byte[tmp_size];
                Array.Copy(input, offset, tmp, 0, tmp_size);
                byte[] tmp_enc = rsa.Encrypt(tmp, false);
                output.Write(tmp_enc, 0, tmp_enc.Length);
                offset += tmp_size;
            }
            output.Flush();
        }
        public static void Decrypt(this RSACryptoServiceProvider rsa, byte[] input, Stream stream)
        {
            int keySize = rsa.KeySize / 8;
            int offset = 0;
            while (offset < input.Length)
            {
                int tmp_size = input.Length - offset;
                if (tmp_size > keySize)
                    tmp_size = keySize;
                byte[] tmp = new byte[tmp_size];
                Array.Copy(input, offset, tmp, 0, tmp_size);
                byte[] tmp_dec = rsa.Decrypt(tmp, false);
                stream.Write(tmp_dec, 0, tmp_dec.Length);
                offset += tmp_size;
            }
            stream.Flush();
        }

        public static void Encrypt(this RSACryptoServiceProvider rsa, Stream input, Stream output)
        {
            int blockSize = rsa.KeySize / 8 - 11;
            while (input.Position < input.Length)
            {
                int tmp_size = (int)(input.Length - input.Position);
                if (tmp_size > blockSize)
                    tmp_size = blockSize;
                byte[] tmp = new byte[tmp_size];
                input.Read(tmp, 0, tmp_size);
                byte[] tmp_enc = rsa.Encrypt(tmp, false);
                output.Write(tmp_enc, 0, tmp_enc.Length);
            }
            output.Flush();
        }
        public static void Decrypt(this RSACryptoServiceProvider rsa, Stream input, Stream stream)
        {
            int keySize = rsa.KeySize / 8;
            for (; ; )
            {
                byte[] tmp = new byte[keySize];
                int n = input.Read(tmp, 0, keySize);
                if (n == 0) break;
                if (n != keySize) Array.Resize(ref tmp, n);
                byte[] tmp_dec = rsa.Decrypt(tmp, false);
                stream.Write(tmp_dec, 0, tmp_dec.Length);
            }
            stream.Flush();
            //while (input.Position < input.Length)
            //{
            //    int tmp_size = (int)(input.Length - input.Position);
            //    if (tmp_size > keySize)
            //        tmp_size = keySize;
            //    byte[] tmp = new byte[tmp_size];
            //    input.Read(tmp, 0, tmp.Length);
            //    byte[] tmp_dec = rsa.Decrypt(tmp, false);
            //    stream.Write(tmp_dec, 0, tmp_dec.Length);
            //}
            //stream.Flush();
        }
    }

    public abstract class RSAStream : Stream
    {
        protected Stream _s1;
        protected Stream s1;
        protected MemoryStream s2;
        protected RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

        public string XmlString { get; set; }
        public byte[] CspBlob { get; set; }
        public string Base64CspBlob
        {
            get { if (this.CspBlob == null) return null; return Convert.ToBase64String(this.CspBlob); }
            set { if (value == null) this.CspBlob = null; else this.CspBlob = Convert.FromBase64String(value); }
        }
        public RSAParameters? Parameter { get; set; }

        protected RSAStream(Stream stream, bool leaveOpen = false)
        {
            this.s1 = stream;
            this.s2 = new MemoryStream();
            if (!leaveOpen)
                _s1 = s1;
        }

        public override bool CanRead
        {
            get { return s1.CanRead; }
        }

        public override bool CanSeek
        {
            get { return s1.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return s1.CanWrite; }
        }

        public override long Length
        {
            get { return s1.Length; }
        }

        public override long Position
        {
            get { return s1.Position; }
            set { s1.Position = value; }
        }

        protected override void Dispose(bool disposing)
        {
            using (_s1)
            using (s2)
                base.Dispose(disposing);
        }

        public override void Close()
        {
            using (_s1)
            using (s2)
                base.Close();
        }

        public override void Flush()
        {
            s1.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return s1.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            s1.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return s1.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            s1.Write(buffer, offset, count);
        }
    }

    public class RSADecryptStream : RSAStream
    {
        public RSADecryptStream(Stream stream, bool leaveOpen = false) : base(stream, leaveOpen) { }

        public override int Read(byte[] buffer, int offset, int count)
        {
            using (RSACryptoServiceProvider rsa = Interlocked.Exchange(ref base.rsa, null))
            {
                if (rsa != null)
                {
                    if (this.XmlString != null)
                        rsa.FromXmlString(this.XmlString);
                    else if (this.CspBlob != null)
                        rsa.ImportCspBlob(this.CspBlob);
                    else if (this.Parameter.HasValue)
                        rsa.ImportParameters(this.Parameter.Value);
                    rsa.Decrypt(s1, s2);
                    s2.Position = 0;
                }
            }
            return s2.Read(buffer, offset, count);
        }
    }

    public class RSAEncryptStream : RSAStream
    {
        public RSAEncryptStream(Stream stream, bool leaveOpen = false) : base(stream, leaveOpen) { }

        public override void Write(byte[] buffer, int offset, int count)
        {
            s2.Write(buffer, offset, count);
        }

        public override void Flush()
        {
            using (RSACryptoServiceProvider rsa = Interlocked.Exchange(ref base.rsa, null))
            {
                if (this.XmlString != null)
                    rsa.FromXmlString(this.XmlString);
                else if (this.CspBlob != null)
                    rsa.ImportCspBlob(this.CspBlob);
                else if (this.Parameter.HasValue)
                    rsa.ImportParameters(this.Parameter.Value);
                s2.Flush();
                s2.Position = 0;
                rsa.Encrypt(s2, s1);
            }
        }
    }
}
