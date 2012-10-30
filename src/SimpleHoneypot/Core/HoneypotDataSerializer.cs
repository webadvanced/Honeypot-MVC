// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HoneypotDataSerializer.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SimpleHoneypot.Core {
    using System;
    using System.IO;
    using System.Text;
    using System.Web.Security;

    using SimpleHoneypot.Core.Common;

    public class HoneypotDataSerializer {
        // Testing hooks

        #region Constants and Fields

        public Func<string, byte[]> Decoder =
            (value) => MachineKey.Decode(Base64ToHex(value), MachineKeyProtection.All);

        public Func<byte[], string> Encoder =
            (bytes) => HexToBase64(MachineKey.Encode(bytes, MachineKeyProtection.All).ToUpperInvariant());

        #endregion

        #region Public Methods and Operators

        public virtual HoneypotData Deserialize(string serializedToken) {
            Check.Argument.IsNotNullOrEmpty(serializedToken, "serializedToken");

            try {
                using (var stream = new MemoryStream(this.Decoder(serializedToken)))
                using (var reader = new BinaryReader(stream)) {
                    return new HoneypotData {
                        Key = Guid.Parse(reader.ReadString()),
                        InputNameValue = reader.ReadString(),
                        CreationDate = new DateTime(reader.ReadInt64())
                    };
                }
            }
            catch(Exception e)  {
                throw new InvalidOperationException(e.Message);
            }
        }

        public virtual string Serialize(HoneypotData token) {
            if (token == null) {
                throw new ArgumentNullException("token");
            }

            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream)) {
                writer.Write(token.Key.ToString());
                writer.Write(token.InputNameValue);
                writer.Write(token.CreationDate.Ticks);

                return this.Encoder(stream.ToArray());
            }
        }

        #endregion

        // String transformation helpers

        #region Methods

        private static string Base64ToHex(string base64) {
            var builder = new StringBuilder(base64.Length * 4);
            foreach (byte b in Convert.FromBase64String(base64)) {
                builder.Append(HexDigit(b >> 4));
                builder.Append(HexDigit(b & 0x0F));
            }
            string result = builder.ToString();
            return result;
        }

        private static char HexDigit(int value) {
            return (char)(value > 9 ? value + '7' : value + '0');
        }

        private static string HexToBase64(string hex) {
            int size = hex.Length / 2;
            var bytes = new byte[size];
            for (int idx = 0; idx < size; idx++) {
                bytes[idx] = (byte)((HexValue(hex[idx * 2]) << 4) + HexValue(hex[idx * 2 + 1]));
            }
            string result = Convert.ToBase64String(bytes);
            return result;
        }

        private static int HexValue(char digit) {
            return digit > '9' ? digit - '7' : digit - '0';
        }

        #endregion
    }
}