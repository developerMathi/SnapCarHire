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
    public class AgreementController
    {
        AgreementService agreementService;
        public AgreementController()
        {
            agreementService = new AgreementService();
        }

        public GetAgreementByCustomerIdMobileResponse getAgreementMobile(getAgreementByCustomerIdMobileRequest getAgreementByCustomerIdMobileRequest, string token)
        {
            throw new NotImplementedException();
        }

        public GetAgreementByAgreementIdMobileResponse getAgreement(GetAgreementByAgreementIdMobileRequest agreementByAgreementIdMobileRequest, string token, int vehicleId)
        {
            GetAgreementByAgreementIdMobileResponse response = null;
            GetVehicleDetailsMobileListResponse getVehicleDetailsMobile = null;
            VehicleService vehicleService = new VehicleService();
            int vehicleTypeID = 0;
            int vehicleID = vehicleId;

            try
            {
                response = agreementService.getAgreement(agreementByAgreementIdMobileRequest, token);
                getVehicleDetailsMobile = vehicleService.getVehicleTypesMobile(token);

                foreach(VehicleTypeMobileResult vtmr in getVehicleDetailsMobile.listVehicle)
                {
                    if (vtmr.VehicleType == response.custAgreement.AgreementDetail.VehicleType)
                    {
                        vehicleTypeID = vtmr.VehicleTypeId;
                    }
                }
                if(vehicleId>0 && vehicleTypeID > 0)
                {
                    response.agreementVehicle = vehicleService.Getvehicle(vehicleTypeID, vehicleId, token);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        public AddAgreementSignMobileResponse saveSignature(AddAgreementSignMobileRequest signMobileRequest, string token)
        {
            AddAgreementSignMobileResponse response = null;
            try
            {
                response = agreementService.saveSignature(signMobileRequest, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        public AddDamageSignMobileResponse saveDamageSignature(AddDamageSignMobileRequest signMobileRequest, string token)
        {
            AddDamageSignMobileResponse response = null;
            try
            {
                response = agreementService.saveDamageSignature(signMobileRequest, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        public ReloadSignatureImageURLMobileResponse getDamageSignature(ReloadSignatureImageURLMobileRequest imageURLMobileRequest, string token)
        {
            ReloadSignatureImageURLMobileResponse response = null;
            try
            {
                response = agreementService.getDamageSignature(imageURLMobileRequest, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        public AddfourTypeVehicleImagesResponse addfourTypeVehicleImages(VehicleImage vehicleImage, string token)
        {
            AddfourTypeVehicleImagesResponse response = null;
            try
            {
                response = agreementService.addfourTypeVehicleImages(vehicleImage, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        public LoadvehicleViewImagesResponse loadvehicleViewImages(LoadvehicleViewImagesReq loadvehicleViewImagesReq, string token)
        {
            LoadvehicleViewImagesResponse response = null;
            try
            {
                response = agreementService.loadvehicleViewImages(loadvehicleViewImagesReq, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        public LoadvehicleViewImagesResponse loadvehicleViewImagesCheckIn(LoadvehicleViewImagesReq loadvehicleViewImagesReq, string token)
        {
            LoadvehicleViewImagesResponse response = null;
            try
            {
                response = agreementService.loadvehicleViewImagesCheckIn(loadvehicleViewImagesReq, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        public int updateAgreement(AgreementReview agreementReview, string _token)
        {
            int agreementId = 0;
            try
            {
                agreementId = agreementService.updateAgreement(agreementReview, _token);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return agreementId;
        }

        public ExtendAgreementResponse extendAgreement(ExtendAgreementRequest request, string token)
        {
            ExtendAgreementResponse response = null;
            try
            {
                response = agreementService.extendAgreement(request, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }
    }
}
