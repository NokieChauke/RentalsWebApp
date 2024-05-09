﻿using RentalsWebApp.Models;

namespace RentalsWebApp.Interfaces
{
    public interface IProofOfPaymentRepository
    {
        bool UploadProofOfPayment(ProofOfPayment proof);
        Task<ProofOfPayment> DownloadProofOfPayment(string userId);
        Task<ProofOfPayment> GetPOPByBillId(int billId);
        Task<ProofOfPayment> GetPOPMonth(string userId);
        bool UpdateProofOfPayment(ProofOfPayment proof);
        bool DeleteProofOfPayment(ProofOfPayment proof);
        bool Save();
    }
}
