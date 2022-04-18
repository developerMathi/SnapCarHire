using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnapCarHireModel;
using SnapCarHireModel.AccessModels;
using SnapCarHireServices.ApiService;

namespace SnapCarHireController
{

    public class CustomerController
    {
        CustomerService customerService;
        public CustomerController()
        {
            customerService = new CustomerService();
        }

        public CustomerReview getCustomerById(int customerId, string token)
        {
            CustomerReview customerReview = new CustomerReview();
            try
            {
                customerReview = customerService.getCustomerById(customerId, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return customerReview;
        }

        public GetMobileCustomerByIDResponse getMobileCustomerById(GetMobileCustomerByIDRequest getMobileCustomerByIDRequest, string token)
        {
            GetMobileCustomerByIDResponse response = new GetMobileCustomerByIDResponse();
            try
            {
                response = customerService.getMobileCustomerById(getMobileCustomerByIDRequest, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public GetForgetPasswordMobileResponse getForgetPasswordMobileRequest(GetForgetPasswordMobileRequest forgetPasswordMobileRequest, string token)
        {
            GetForgetPasswordMobileResponse response;
            try
            {
                response = customerService.getForgetPasswordMobileRequest(forgetPasswordMobileRequest, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public List<CustomerSeachResult> getCustomerByFilter(CustomerSerach customerSerach, string token)
        {
            List<CustomerSeachResult> response;
            try
            {
                response = customerService.getCustomerByFilter(customerSerach, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public GetCustomerPortalDetailsMobileResponse getCustomerDetailsWithProfilePic(GetCustomerPortalDetailsMobileRequest portalDetailsMobileRequest, string token)
        {
            GetCustomerPortalDetailsMobileResponse response = new GetCustomerPortalDetailsMobileResponse();
            try
            {
                response = customerService.getCustomerDetailsWithProfilePic(portalDetailsMobileRequest, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public GetCustomerAgreementReservationCountResponse GetCustomerAgreementReservationCount(GetCustomerAgreementReservationCountRequest getCustomerAgreementReservationCountRequest, string token)
        {
            GetCustomerAgreementReservationCountResponse response = new GetCustomerAgreementReservationCountResponse();
            try
            {
                response = customerService.GetCustomerAgreementReservationCount(getCustomerAgreementReservationCountRequest, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public UpdateCustomerMobileCreditCardResponse AddMobileCustomerCreditCard(CustomerCreditCards customer,string token)
        {
            UpdateCustomerMobileCreditCardResponse response = new UpdateCustomerMobileCreditCardResponse();
            try
            {
                response = customerService.AddMobileCustomerCreditCard(customer, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public UpdateCustomerMobileCreditCardResponse UpdateMobileCustomerCreditCard(CreditCards cards, string token)
        {
            UpdateCustomerMobileCreditCardResponse response = new UpdateCustomerMobileCreditCardResponse();
            try
            {
                response = customerService.UpdateMobileCustomerCreditCard(cards, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public int changePassword(int customerId, string oldPassword, string newPassword, string token)
        {
            int response;
            try
            {
                response = customerService.changePassword(customerId, oldPassword, newPassword, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public List<CreditCards> GetAllMobileCustomerCreditCard(GetAllCustomerMobileCreditCardRequest cards, string token)
        {
            List<CreditCards> response = new List<CreditCards>();
            try
            {
                response = customerService.GetAllMobileCustomerCreditCard(cards, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }
    
}
