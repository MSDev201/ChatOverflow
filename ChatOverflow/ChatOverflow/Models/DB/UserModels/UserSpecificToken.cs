﻿using ChatOverflow.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatOverflow.Models.DB.UserModels
{
    public enum UserSpecificTokenType
    {
        RefreshToken = 0,
        SocketToken = 1
    }

    public class UserSpecificToken
    {
        public ulong Id { get; set; }

        [MaxLength(100)]
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public User User { get; set; }


        public UserSpecificTokenType Type { get; set; }


        public bool Active { get { return Expires != null && Expires > DateTime.Now; } }


        public static UserSpecificToken Generate(UserSpecificTokenType type, DateTime expireDate)
        {
            var token = RandomString.Generate(70);
            return new UserSpecificToken
            {
                Expires = expireDate,
                Type = type,
                Token = token
            };
        }
    }
}
