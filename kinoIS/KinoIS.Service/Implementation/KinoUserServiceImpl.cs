﻿using KinoIs.Repository.Interface;
using KinoIS.Domain.Models;
using KinoIS.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinoIS.Service.Implementation
{
    public class KinoUserServiceImpl : KinoUserService
    {
        private readonly KinoUserRepository kinoUserRepository;
        public KinoUserServiceImpl(KinoUserRepository kinoUserRepository)
        {
            this.kinoUserRepository = kinoUserRepository;
        }
        public KinoUser findById(string id)
        {
            return this.kinoUserRepository.findById(id);
        }
    }
}