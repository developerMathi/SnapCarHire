﻿using SnapCarHireModel;
using SnapCarHireModel.AccessModels;
using SnapCarHireServices.ApiService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapCarHireController
{
    public class ApiController
    {
        LoginService loginservice;
        public ApiController()
        {
            loginservice = new LoginService();
        }

        public  GetClientSecretTokenResponse GetClientSecretToken(GetClientSecretTokenRequest getClientSecretTokenRequest)
        {
            return loginservice.GetClientSecretToken(getClientSecretTokenRequest);
        }

        public ApiToken GetAccessToken(GetAccessTokenRequest tokenRequest)
        {
            return loginservice.GetAccessToken(tokenRequest);
        }
    }
}
