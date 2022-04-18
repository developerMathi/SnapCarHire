using SnapCarHireModel;
using SnapCarHireModel.AccessModels;
using SnapCarHireServices.ApiService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapCarHireController
{
    public class RegisterController
    {
        RegisterService registerservice;
        public RegisterController()
        {
            registerservice = new RegisterService();
        }

        public int registerUser(CustomerReview customer, string _token)
        {
            return registerservice.registerUser(customer, _token);
        }

        public RegistrationDBModel getRegistrationDBModel(int customerId, string _token)
        {
            return registerservice.getRegistrationDBModel(customerId, _token);
        }


        public bool checkBookable(int customerId, string _token)
        {
            RegistrationDBModel registrationDB = registerservice.getRegistrationDBModel(customerId, _token);
            if (registrationDB.Reservations != null)
            {
                if (registrationDB.Reservations.Count == 0)
                {
                    return true;
                }
                else
                {
                    if (registrationDB.Reservations[0].Status == "Open" || registrationDB.Reservations[0].Status == "New" || registrationDB.Reservations[0].Status == "Quote")
                    {
                        return false;
                    }
                    else if (registrationDB.Reservations[0].Status == "CheckOut")
                    {
                        if (registrationDB.Agreements[0].Status == "Open")
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                return false;
            }

        }


        public List<CustomerReservationModel> getReservations(int customerId, string token)
        {
            List<CustomerReservationModel> reservationModels = null;
            try
            {
                reservationModels = registerservice.getReservations(customerId, token);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return reservationModels;
        }

        public List<CustomerAgreementModel> getAgreements(int customerId, string token)
        {
            List<CustomerAgreementModel> agreementModels = null;
            try
            {
                agreementModels = registerservice.getAgreements(customerId, token);


                if (agreementModels != null)
                {
                    if (agreementModels.Count > 0)
                    {

                        GetVehicleDetailsMobileListResponse getVehicleDetailsMobile = null;
                        VehicleService vehicleService = new VehicleService();
                        getVehicleDetailsMobile = vehicleService.getVehicleTypesMobile(token);
                        AgreementService agreementService = new AgreementService();
                        foreach (CustomerAgreementModel cam in agreementModels)
                        {
                            if(cam.Status != null)
                            {
                                int agreeId = cam.AgreementId;
                                int vehId = cam.VehicleId;
                                int vehicleTypeID = 0;
                                GetAgreementByAgreementIdMobileResponse agreementByAgreementIdMobileResponse = null;
                                GetAgreementByAgreementIdMobileRequest agreementIdMobileRequest = new GetAgreementByAgreementIdMobileRequest();
                                agreementIdMobileRequest.agreementId = agreeId;
                                agreementByAgreementIdMobileResponse = agreementService.getAgreement(agreementIdMobileRequest, token);
                                cam.custAgreement = agreementByAgreementIdMobileResponse.custAgreement;
                                foreach (VehicleTypeMobileResult vtmr in getVehicleDetailsMobile.listVehicle)
                                {
                                    if (vtmr.VehicleType == agreementByAgreementIdMobileResponse.custAgreement.AgreementDetail.VehicleType)
                                    {
                                        vehicleTypeID = vtmr.VehicleTypeId;
                                    }
                                }
                                if (vehId > 0 && vehicleTypeID > 0)
                                {
                                    cam.agreementVehicle = vehicleService.Getvehicle(vehicleTypeID, vehId, token);
                                }
                            }
                           
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return agreementModels;
        }

        public CustomerReservationModel getReservation(int reservationId, string token)
        {
            CustomerReservationModel model = null;
            try
            {
                model = registerservice.getReservation(reservationId, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public GetReservationByIDMobileResponse getReservationByID(GetReservationByIDMobileRequest reservationByIDMobileRequest, string token)
        {
            GetReservationByIDMobileResponse getReservationByID = null;
            try
            {
                getReservationByID = registerservice.getReservationByID(reservationByIDMobileRequest, token);
                if (getReservationByID.reservationData.Reservationview.VehicleId > 0)
                {
                    getReservationByID.vehicleModel = registerservice.GetVehicleWithRates(getReservationByID.reservationData.Reservationview.VehicleTypeID, getReservationByID.reservationData.Reservationview.VehicleId, token);
                }
                else if (getReservationByID.reservationData.Reservationview.VehicleTypeID > 0)
                {
                    getReservationByID.vehicleTypeModel = registerservice.GetVehicleTypesWithRates(getReservationByID.reservationData.Reservationview.VehicleTypeID, token);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return getReservationByID;
        }

        public UpdateCustomerProfileDetailsMobileResponse updateUser(UpdateCustomerProfileDetailsMobileRequest profileDetailsMobileRequest, string token)
        {
            UpdateCustomerProfileDetailsMobileResponse response = null;

            try
            {
                response = registerservice.updateUser(profileDetailsMobileRequest, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public GetReservationAgreementMobileResponse getMobileRegistrationDBModel(GetReservationAgreementMobileRequest registrationDBModelRequest, string token)
        {
            GetReservationAgreementMobileResponse response = null;

            try
            {
                response = registerservice.getMobileRegistrationDBModel(registrationDBModelRequest, token);

                //if(response.regDB != null)
                //{
                //    if(response.regDB.Agreements != null)
                //    {
                //        if (response.regDB.Agreements.Count > 0)
                //        {

                //            GetVehicleDetailsMobileListResponse getVehicleDetailsMobile = null;
                //            VehicleService vehicleService = new VehicleService();
                //            getVehicleDetailsMobile = vehicleService.getVehicleTypesMobile(token);
                //            AgreementService agreementService = new AgreementService();
                //            foreach (CustomerAgreementModel cam in response.regDB.Agreements)
                //            {
                //                int agreeId = cam.AgreementId;
                //                int vehId = cam.VehicleId;
                //                int vehicleTypeID = 0;
                //                GetAgreementByAgreementIdMobileResponse agreementByAgreementIdMobileResponse = null;
                //                GetAgreementByAgreementIdMobileRequest agreementIdMobileRequest = new GetAgreementByAgreementIdMobileRequest();
                //                agreementIdMobileRequest.agreementId = agreeId;
                //                agreementByAgreementIdMobileResponse = agreementService.getAgreement(agreementIdMobileRequest, token);
                //                cam.custAgreement = agreementByAgreementIdMobileResponse.custAgreement;
                //                foreach (VehicleTypeMobileResult vtmr in getVehicleDetailsMobile.listVehicle)
                //                {
                //                    if (vtmr.VehicleType == agreementByAgreementIdMobileResponse.custAgreement.AgreementDetail.VehicleType)
                //                    {
                //                        vehicleTypeID = vtmr.VehicleTypeId;
                //                    }
                //                }
                //                if (vehId > 0 && vehicleTypeID > 0)
                //                {
                //                    cam.agreementVehicle = vehicleService.Getvehicle(vehicleTypeID, vehId, token);
                //                }
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public AddLicenceImageResponse addLicenceImage(AddLicenceImagesRequest licenceImagesRequest, string token)
        {
            AddLicenceImageResponse response = null;

            try
            {
                response = registerservice.addLicenceImage(licenceImagesRequest, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public UploadCustomerImageMobileResponse addCustomerImage(UploadCustomerImageMobileRequest imageMobileRequest, string _token)
        {
            UploadCustomerImageMobileResponse response = null;

            try
            {
                response = registerservice.addCustomerImage(imageMobileRequest, _token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }
}
