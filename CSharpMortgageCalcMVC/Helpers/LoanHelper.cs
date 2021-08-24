using CSharpMortgageCalcMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpMortgageCalcMVC.Helpers
{
    public class LoanHelper
    {
        public Loan GetPayment(Loan loan)
        {

            //Calculate my monthly payment
            loan.Payment = CalcPayment(loan.Amount, loan.Rate, loan.Term);
            //Calculate from one to the term
            var balance = loan.Amount;
            var totalInterest = 0.0m;
            var monthlyInterest = 0.0m;
            var monthlyPrincipal = 0.0m;
            var monthlyRate = CalcMonthlyRate(loan.Rate);

            for (int month = 1; month <= loan.Term; month++)
            {
                monthlyInterest = CalcMonthlyInterest(balance, monthlyRate);
                totalInterest += monthlyInterest;
                monthlyPrincipal = loan.Payment - monthlyInterest;
                balance -= monthlyPrincipal;

                LoanPayment loanpayment = new LoanPayment();
                loanpayment.Month = month;
                loanpayment.Payment = loan.Payment;
                loanpayment.MonthlyPrincipal = monthlyPrincipal;
                loanpayment.MonthlyInterest = monthlyInterest;
                loanpayment.TotalInterest = totalInterest;
                loanpayment.Balance = balance;

                //push the object into the loan model
                loan.Payments.Add(loanpayment);
            }
            loan.TotalInterest = totalInterest;
            loan.TotalCost = loan.Amount + totalInterest;
            //Calculate a payment schedule


            //push payments in the loan

            //return the loan to the view
            return loan;
        }

        private decimal CalcPayment(decimal amount, decimal rate, int term)
        {
            
            var monthlyRate = CalcMonthlyRate(rate);
            var rateD = Convert.ToDouble(monthlyRate);
            var amountD = Convert.ToDouble(amount);
            var paymentD = (amountD * rateD) / (1 - Math.Pow(1 + rateD, -term));
            return Convert.ToDecimal(paymentD);
        }
        private decimal CalcMonthlyRate(decimal rate)
        {
            return rate / 1200;
        }
        private decimal CalcMonthlyInterest(decimal balance, decimal monthlyRate)
        {
            return balance * monthlyRate;
        }
    }
}
