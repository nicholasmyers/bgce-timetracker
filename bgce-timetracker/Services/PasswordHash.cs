﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace bgce_timetracker.Services
{
    public class PasswordHash
    {
        private string GHash;
        private string _pass;
        private byte[] _salt;
        public PasswordHash()
        {
            Pass = null;
            Salt = null;
        }
        public string GetHash(string password, byte[] salt)
        {

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            /*string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));*/
            //Console.WriteLine($"Hashed: {hashed}");

            var hashed = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = hashed.GetBytes(20);
            string hashedString = Convert.ToBase64String(hash);
            return hashedString;  
        }
        public byte[] GenerateSalt()
        {
            // generate a 128-bit salt using a secure PRNG
            byte[] s;//= new byte[128 / 8];
            //Encoding enc = Encoding.UTF8;
            new RNGCryptoServiceProvider().GetBytes(s = new byte[128 / 8]);
            /*using (var rng = RandomNumberGenerator.Create())
            {
               s = enc.GetBytes(rng);
            }*/
            return s;
        }
        public byte[] Salt
        {
            get { return _salt; }
            set { _salt = value; }
        }
        public string Pass
        {
            get { return _pass; }
            set { _pass = value; }
        }


    }
}
