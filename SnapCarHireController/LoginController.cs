using MaxVonGrafKftMobileModel.AccessModels;
using SnapCarHireModel;
using SnapCarHireServices.ApiService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapCarHireController
{
    public class LoginController
    {
        LoginService loginservice;
        public LoginController()
        {
            loginservice = new LoginService();
        }

        public CutomerAuthContext CheckLogin(CustomerLogin loginCustomer, string token)
        {
            return loginservice.CheckLogin(loginCustomer,token);
        }

        public ConfirmEmailAddressResponse ConfirmEmailAddress(ConfirmEmailAddressRequest confirmEmailAddressRequest, string token)
        {
            ConfirmEmailAddressResponse response = new ConfirmEmailAddressResponse();
            try
            {
                response = loginservice.ConfirmEmailAddress(confirmEmailAddressRequest, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public CheckConfirmEmailAddressResponse checkConfirmEmailAddress(ConfirmEmailAddressRequest request, string token)
        {
            CheckConfirmEmailAddressResponse response = new CheckConfirmEmailAddressResponse();
            try
            {
                response = loginservice.checkConfirmEmailAddress(request, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }
}
