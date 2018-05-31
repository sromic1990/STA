//  
// Coder:			Ranpariya Ankur {GameAnax} 
// EMail:			ankur.ranpariya@indianic.com
// Copyright:		GameAnax Studio Pvt Ltd	
// Social:			http://www.gameanax.com, @GameAnax, https://www.facebook.com/@gameanax
// 
// Orignal Source :	N/A
// Last Modified: 	Ranpariya Ankur
// Contributed By:	N/A
// Curtosey By:		N/A	
// 
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the
// following conditions are met:
// 
//  *	Redistributions of source code must retain the above copyright notice, this list of conditions and the following
//  	disclaimer.
//  *	Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following 
//  	disclaimer in the documentation and/or other materials provided with the distribution.
//  *	Neither the name of the [ORGANIZATION] nor the names of its contributors may be used to endorse or promote products
//  	derived from this software without specific prior written permission.
// 
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the 
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
// 

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace GameAnax.Core.Security {
	//
	public class EncOption {
		public string secureKey;
		public string viKey;
		public string saltKey;
		public CipherMode cMode;
		public PaddingMode pMode;
		public int KEY_SIZE;
		public int BLOCK_SIZE;

		public EncOption() {
			secureKey = Encryption.SALT_KEY;
			viKey = Encryption.VI_KEY;
			saltKey = Encryption.SALT_KEY;

			cMode = Encryption.CIPHER_MODE;
			pMode = Encryption.PADDING_MODE;

			KEY_SIZE = 128;
			BLOCK_SIZE = 128;
		}
	}
	public static class Encryption {
		public const string SALT_KEY = "S@LT&KEY";
		public const string VI_KEY = "S@LT&1B2c3D4e5F6";
		public const string SECURE_KEY = "S@ssK3y&KEY";
		public const CipherMode CIPHER_MODE = CipherMode.CBC;
		public const PaddingMode PADDING_MODE = PaddingMode.Zeros;
		//
		public static string MD5Sum(string toEncrypt) {
			byte[] bytes = Encoding.UTF8.GetBytes(toEncrypt);
			// encrypt bytes
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			byte[] hashBytes = md5.ComputeHash(bytes);

			// Convert the encrypted bytes back to a string (base 16)
			string hashString = string.Empty;

			for(int i = 0; i < hashBytes.Length; i++) {
				hashString += Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
			}

			return hashString.PadLeft(32, '0');
		}
		//

		#region RFC2898 Encryption Decryption
		public static string RFC2898Encrypt(string plainText) {
			return RFC2898Encrypt(plainText, new EncOption());
		}
		public static string RFC2898Encrypt(string plainText, EncOption option) {
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			byte[] keyBytes = new Rfc2898DeriveBytes(option.secureKey, Encoding.ASCII.GetBytes(option.saltKey)).GetBytes(256 / 8);
			RijndaelManaged symmetricKey = new RijndaelManaged() {
				Mode = option.cMode,
				Padding = option.pMode
			};
			var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(option.viKey));

			byte[] cipherTextBytes;

			using(var memoryStream = new MemoryStream()) {
				using(var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)) {
					cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
					cryptoStream.FlushFinalBlock();
					cipherTextBytes = memoryStream.ToArray();
					cryptoStream.Close();
				}
				memoryStream.Close();
			}
			return Convert.ToBase64String(cipherTextBytes);
		}

		public static string RFC2898Decrypt(string encryptedText) {
			return RFC2898Decrypt(encryptedText, new EncOption());
		}
		public static string RFC2898Decrypt(string encryptedText, EncOption option) {
			byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
			byte[] keyBytes = new Rfc2898DeriveBytes(option.secureKey, Encoding.ASCII.GetBytes(option.saltKey)).GetBytes(256 / 8);
			var symmetricKey = new RijndaelManaged() {
				Mode = option.cMode,
				Padding = option.pMode
			};

			var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(option.viKey));
			var memoryStream = new MemoryStream(cipherTextBytes);
			var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
			byte[] plainTextBytes = new byte[cipherTextBytes.Length];

			int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
			memoryStream.Close();
			cryptoStream.Close();
			return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
		}
		#endregion
		//
		#region AES Encryption Decryption
		public static string AESEncrypt(string plainData) {
			return AESEncrypt(plainData, new EncOption());
		}
		public static string AESEncrypt(string plainData, EncOption option) {
			AesCryptoServiceProvider dataencrypt = new AesCryptoServiceProvider();

			byte[] bytearraytoencrypt = Encoding.UTF8.GetBytes(plainData);
			//Block size : Gets or sets the block size, in bits, of the cryptographic operation.  
			dataencrypt.BlockSize = option.BLOCK_SIZE;
			//KeySize: Gets or sets the size, in bits, of the secret key  
			dataencrypt.KeySize = option.KEY_SIZE;
			//Key: Gets or sets the symmetric key that is used for encryption and decryption.  
			dataencrypt.Key = System.Text.Encoding.UTF8.GetBytes(option.secureKey);
			//IV : Gets or sets the initialization vector (IV) for the symmetric algorithm  
			dataencrypt.IV = System.Text.Encoding.UTF8.GetBytes(option.viKey);
			//Padding: Gets or sets the padding mode used in the symmetric algorithm  
			dataencrypt.Padding = PaddingMode.PKCS7;
			//Mode: Gets or sets the mode for operation of the symmetric algorithm  
			dataencrypt.Mode = CipherMode.CBC;
			//Creates a symmetric AES encryptor object using the current key and initialization vector (IV).  
			ICryptoTransform crypto1 = dataencrypt.CreateEncryptor(dataencrypt.Key, dataencrypt.IV);
			//TransformFinalBlock is a special function for transforming the last block or a partial block in the stream.   
			//It returns a new array that contains the remaining transformed bytes. A new array is returned, because the amount of   
			//information returned at the end might be larger than a single block when padding is added.  
			byte[] encrypteddata = crypto1.TransformFinalBlock(bytearraytoencrypt, 0, bytearraytoencrypt.Length);
			crypto1.Dispose();
			//return the encrypted data  
			string encryptedString = Encoding.UTF8.GetString(encrypteddata);
			return encryptedString;

		}

		public static string AESDecrypt(string encryptedData) {
			return AESDecrypt(encryptedData, new EncOption());
		}
		public static string AESDecrypt(string encryptedData, EncOption option) {
			AesCryptoServiceProvider keydecrypt = new AesCryptoServiceProvider();
			byte[] bytearraytodecrypt = Encoding.UTF8.GetBytes(encryptedData);
			keydecrypt.BlockSize = option.BLOCK_SIZE;
			keydecrypt.KeySize = option.KEY_SIZE;
			keydecrypt.Key = Encoding.UTF8.GetBytes(option.secureKey);
			keydecrypt.IV = Encoding.UTF8.GetBytes(option.viKey);
			keydecrypt.Padding = option.pMode;
			keydecrypt.Mode = option.cMode;
			ICryptoTransform crypto1 = keydecrypt.CreateDecryptor(keydecrypt.Key, keydecrypt.IV);
			byte[] returnbytearray = crypto1.TransformFinalBlock(bytearraytodecrypt, 0, bytearraytodecrypt.Length);
			crypto1.Dispose();
			string plainData = Encoding.UTF8.GetString(returnbytearray);
			return plainData;
		}

		#endregion
	}

	public enum EncryptionMode {
		RFC2898,
		AES
	}
}