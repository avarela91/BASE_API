using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ET.Objetos
{
    public class PaymentsModel
    {
        public string Session_Id { get; set; }
        private string U_PagoCreditoField;
        private double U_childTransferSumField;
        private string U_isChildPaymentField;
        private string U_RB_docNumField;
        private string U_isTransferPuntosField;
        private string U_DocNum_PrincipalField;
        private string U_ClientePuntosField;

        private string ObjTypeField;

        private string docTypeField;

        private string docDateField;

        private string cardCodeField;

        private string cardNameField;

        private string addressField;

        private double cashSumField;

        private string cashAccountField;

        private double CreditSumField;

        private double CheckSumField;

        private string checkAccountField;

        private string transferAccountField;

        private double transferSumField;

        private string transferDateField;

        private string transferReferenceField;

        private string reference1Field;

        private string reference2Field;

        private string counterReferenceField;

        private string paymentTypeField;

        private string CommentsField;

        private double SeriesField;

        private double U_RB_TotalPagosPuField;

        private PaymentPaymentCheck[] paymentChecksField;

        // private PaymentPaymentInvoice[] paymentInvoicesField;

        private PaymentPaymentAccount[] paymentAccountsField;

        public string ObjType
        {
            get
            {
                return this.ObjTypeField;
            }
            set
            {
                this.ObjTypeField = value;
            }
        }

        /// <remarks/>
        public string DocType
        {
            get
            {
                return this.docTypeField;
            }
            set
            {
                this.docTypeField = value;
            }
        }

        //kavx U_childTransferSumFieldSpecified
        public string U_PagoCredito
        {
            get
            {
                return this.U_PagoCreditoField;
            }
            set
            {
                this.U_PagoCreditoField = value;
            }
        }
        public double U_childTransferSum
        {
            get
            {
                return this.U_childTransferSumField;
            }
            set
            {
                this.U_childTransferSumField = value;
            }
        }

        public string U_IsChildPayment
        {
            get
            {
                return this.U_isChildPaymentField;
            }
            set
            {
                this.U_isChildPaymentField = value;
            }
        }


        //kavx 
        public string U_DocNum_Principal
        {
            get
            {
                return this.U_DocNum_PrincipalField;
            }
            set
            {
                this.U_DocNum_PrincipalField = value;
            }
        }

        public string U_RB_DocNum
        {
            get
            {
                return this.U_RB_docNumField;
            }
            set
            {
                this.U_RB_docNumField = value;
            }
        }

        public string U_isTransferPuntos
        {
            get
            {
                return this.U_isTransferPuntosField;
            }
            set
            {
                this.U_isTransferPuntosField = value;
            }
        }

        //U_ClientePuntosField
        public string U_ClientePuntos
        {
            get
            {
                return this.U_ClientePuntosField;
            }
            set
            {
                this.U_ClientePuntosField = value;
            }
        }
        /// <remarks/>
        public string DocDate
        {
            get
            {
                return this.docDateField;
            }
            set
            {
                this.docDateField = value;
            }
        }

        /// <remarks/>
        public string CardCode
        {
            get
            {
                return this.cardCodeField;
            }
            set
            {
                this.cardCodeField = value;
            }
        }

        /// <remarks/>
        public string CardName
        {
            get
            {
                return this.cardNameField;
            }
            set
            {
                this.cardNameField = value;
            }
        }

        public string Address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        /// <remarks/>
        public double CashSum
        {
            get
            {
                return this.cashSumField;
            }
            set
            {
                this.cashSumField = value;
            }
        }
        /// <remarks/> 
        /// 
        //Almacena el total sobre el cual se calcularan los puntos 
        public double U_RB_TotalPagosPu
        {
            get
            {
                return this.U_RB_TotalPagosPuField;
            }
            set
            {
                this.U_RB_TotalPagosPuField = value;
            }
        }

        public double CheckSum
        {
            get
            {
                return this.CheckSumField;
            }
            set
            {
                this.CheckSumField = value;
            }
        }

        public double CreditSum
        {
            get
            {
                return this.CreditSumField;
            }
            set
            {
                this.CreditSumField = value;
            }
        }

        /// <remarks/>
        public string CashAccount
        {
            get
            {
                return this.cashAccountField;
            }
            set
            {
                this.cashAccountField = value;
            }
        }

        /// <remarks/>
        public string CheckAccount
        {
            get
            {
                return this.checkAccountField;
            }
            set
            {
                this.checkAccountField = value;
            }
        }

        /// <remarks/>
        public string TransferAccount
        {
            get
            {
                return this.transferAccountField;
            }
            set
            {
                this.transferAccountField = value;
            }
        }

        /// <remarks/>
        public double TransferSum
        {
            get
            {
                return this.transferSumField;
            }
            set
            {
                this.transferSumField = value;
            }
        }

        /// <remarks/>
        public string TransferDate
        {
            get
            {
                return this.transferDateField;
            }
            set
            {
                this.transferDateField = value;
            }
        }

        /// <remarks/>
        public string TransferReference
        {
            get
            {
                return this.transferReferenceField;
            }
            set
            {
                this.transferReferenceField = value;
            }
        }

        /// <remarks/>
        public string Reference1
        {
            get
            {
                return this.reference1Field;
            }
            set
            {
                this.reference1Field = value;
            }
        }

        /// <remarks/>
        public string Reference2
        {
            get
            {
                return this.reference2Field;
            }
            set
            {
                this.reference2Field = value;
            }
        }

        public string CounterReference
        {
            get
            {
                return this.counterReferenceField;
            }
            set
            {
                this.counterReferenceField = value;
               
            }
        }

        /// <remarks/>
        public string PaymentType
        {
            get
            {
                return this.paymentTypeField;
            }
            set
            {
                this.paymentTypeField = value;
            }
        }

        public string Comments
        {
            get
            {
                return this.CommentsField;
            }
            set
            {
                this.CommentsField = value;
            }
        }

        public double Series
        {
            get
            {
                return SeriesField;
            }
            set
            {
                this.SeriesField = value;
            }
        }

        public PaymentPaymentCheck[] PaymentChecks
        {
            get
            {
                return this.paymentChecksField;
            }
            set
            {
                this.paymentChecksField = value;
            }
        }

        /* public PaymentPaymentInvoice[] PaymentInvoices
         {
             get
             {
                 return this.paymentInvoicesField;
             }
             set
             {
                 this.paymentInvoicesField = value;
             }
         }*/

        public PaymentPaymentAccount[] PaymentAccounts
        {
            get
            {
                return this.paymentAccountsField;
            }
            set
            {
                this.paymentAccountsField = value;
            }

        }

       
    }
    public partial class PaymentPaymentCheck
    {

        private int lineNumField;

        private string dueDateField;

        private long checkNumberField;

        private double checkSumField;

        private string bankCodeField;

        private string checkAccountField;

        private string accounttNumField;



        /// <remarks/>
        public int LineNum
        {
            get
            {
                return this.lineNumField;
            }
            set
            {
                this.lineNumField = value;
            }
        }

        public string DueDate
        {
            get
            {
                return this.dueDateField;
            }
            set
            {
                this.dueDateField = value;
            }
        }

        /// <remarks/>
        public long CheckNumber
        {
            get
            {
                return this.checkNumberField;
            }
            set
            {
                this.checkNumberField = value;
            }
        }

        /// <remarks/>
        public double CheckSum
        {
            get
            {
                return this.checkSumField;
            }
            set
            {
                this.checkSumField = value;
            }
        }

        public string BankCode
        {
            get
            {
                return this.bankCodeField;
            }
            set
            {
                this.bankCodeField = value;
            }
        }

        public string CheckAccount
        {
            get
            {
                return this.checkAccountField;
            }
            set
            {
                this.checkAccountField = value;
               
            }
        }

        public string AccounttNum
        {
            get
            {
                return this.accounttNumField;
            }
            set
            {
                this.accounttNumField = value;
            }
        }

    }
    public partial class PaymentPaymentAccount : object, System.ComponentModel.INotifyPropertyChanged
    {

        private long lineNumField;

        private bool lineNumFieldSpecified;

        private string accountCodeField;

        private double sumPaidField;

        private bool sumPaidFieldSpecified;

        private double sumPaidFCField;

        private bool sumPaidFCFieldSpecified;

        private string decriptionField;

        private string vatGroupField;

        private string accountNameField;

        private double grossAmountField;

        private bool grossAmountFieldSpecified;

        private string profitCenterField;

        private string projectCodeField;

        private double vatAmountField;

        private bool vatAmountFieldSpecified;

        private string profitCenter2Field;

        private string profitCenter3Field;

        private string profitCenter4Field;

        private string profitCenter5Field;

        private long locationCodeField;

        private bool locationCodeFieldSpecified;

        private double equalizationVatAmountField;

        private bool equalizationVatAmountFieldSpecified;

        /// <remarks/>
        public long LineNum
        {
            get
            {
                return this.lineNumField;
            }
            set
            {
                this.lineNumField = value;
                this.RaisePropertyChanged("LineNum");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LineNumSpecified
        {
            get
            {
                return this.lineNumFieldSpecified;
            }
            set
            {
                this.lineNumFieldSpecified = value;
                this.RaisePropertyChanged("LineNumSpecified");
            }
        }

        /// <remarks/>
        public string AccountCode
        {
            get
            {
                return this.accountCodeField;
            }
            set
            {
                this.accountCodeField = value;
                this.RaisePropertyChanged("AccountCode");
            }
        }

        /// <remarks/>
        public double SumPaid
        {
            get
            {
                return this.sumPaidField;
            }
            set
            {
                this.sumPaidField = value;
                this.RaisePropertyChanged("SumPaid");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SumPaidSpecified
        {
            get
            {
                return this.sumPaidFieldSpecified;
            }
            set
            {
                this.sumPaidFieldSpecified = value;
                this.RaisePropertyChanged("SumPaidSpecified");
            }
        }

        /// <remarks/>
        public double SumPaidFC
        {
            get
            {
                return this.sumPaidFCField;
            }
            set
            {
                this.sumPaidFCField = value;
                this.RaisePropertyChanged("SumPaidFC");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SumPaidFCSpecified
        {
            get
            {
                return this.sumPaidFCFieldSpecified;
            }
            set
            {
                this.sumPaidFCFieldSpecified = value;
                this.RaisePropertyChanged("SumPaidFCSpecified");
            }
        }

        /// <remarks/>
        public string Decription
        {
            get
            {
                return this.decriptionField;
            }
            set
            {
                this.decriptionField = value;
                this.RaisePropertyChanged("Decription");
            }
        }

        /// <remarks/>
        public string VatGroup
        {
            get
            {
                return this.vatGroupField;
            }
            set
            {
                this.vatGroupField = value;
                this.RaisePropertyChanged("VatGroup");
            }
        }

        /// <remarks/>
        public string AccountName
        {
            get
            {
                return this.accountNameField;
            }
            set
            {
                this.accountNameField = value;
                this.RaisePropertyChanged("AccountName");
            }
        }

        /// <remarks/>
        public double GrossAmount
        {
            get
            {
                return this.grossAmountField;
            }
            set
            {
                this.grossAmountField = value;
                this.RaisePropertyChanged("GrossAmount");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool GrossAmountSpecified
        {
            get
            {
                return this.grossAmountFieldSpecified;
            }
            set
            {
                this.grossAmountFieldSpecified = value;
                this.RaisePropertyChanged("GrossAmountSpecified");
            }
        }

        /// <remarks/>
        public string ProfitCenter
        {
            get
            {
                return this.profitCenterField;
            }
            set
            {
                this.profitCenterField = value;
                this.RaisePropertyChanged("ProfitCenter");
            }
        }

        /// <remarks/>
        public string ProjectCode
        {
            get
            {
                return this.projectCodeField;
            }
            set
            {
                this.projectCodeField = value;
                this.RaisePropertyChanged("ProjectCode");
            }
        }

        /// <remarks/>
        public double VatAmount
        {
            get
            {
                return this.vatAmountField;
            }
            set
            {
                this.vatAmountField = value;
                this.RaisePropertyChanged("VatAmount");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VatAmountSpecified
        {
            get
            {
                return this.vatAmountFieldSpecified;
            }
            set
            {
                this.vatAmountFieldSpecified = value;
                this.RaisePropertyChanged("VatAmountSpecified");
            }
        }

        /// <remarks/>
        public string ProfitCenter2
        {
            get
            {
                return this.profitCenter2Field;
            }
            set
            {
                this.profitCenter2Field = value;
                this.RaisePropertyChanged("ProfitCenter2");
            }
        }

        /// <remarks/>
        public string ProfitCenter3
        {
            get
            {
                return this.profitCenter3Field;
            }
            set
            {
                this.profitCenter3Field = value;
                this.RaisePropertyChanged("ProfitCenter3");
            }
        }

        /// <remarks/>
        public string ProfitCenter4
        {
            get
            {
                return this.profitCenter4Field;
            }
            set
            {
                this.profitCenter4Field = value;
                this.RaisePropertyChanged("ProfitCenter4");
            }
        }

        /// <remarks/>
        public string ProfitCenter5
        {
            get
            {
                return this.profitCenter5Field;
            }
            set
            {
                this.profitCenter5Field = value;
                this.RaisePropertyChanged("ProfitCenter5");
            }
        }

        /// <remarks/>
        public long LocationCode
        {
            get
            {
                return this.locationCodeField;
            }
            set
            {
                this.locationCodeField = value;
                this.RaisePropertyChanged("LocationCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LocationCodeSpecified
        {
            get
            {
                return this.locationCodeFieldSpecified;
            }
            set
            {
                this.locationCodeFieldSpecified = value;
                this.RaisePropertyChanged("LocationCodeSpecified");
            }
        }

        /// <remarks/>
        public double EqualizationVatAmount
        {
            get
            {
                return this.equalizationVatAmountField;
            }
            set
            {
                this.equalizationVatAmountField = value;
                this.RaisePropertyChanged("EqualizationVatAmount");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EqualizationVatAmountSpecified
        {
            get
            {
                return this.equalizationVatAmountFieldSpecified;
            }
            set
            {
                this.equalizationVatAmountFieldSpecified = value;
                this.RaisePropertyChanged("EqualizationVatAmountSpecified");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /* public partial class PaymentPaymentInvoice
     {    

         private int lineNumField;


         private double sumAppliedField;

         private string invoiceTypeField;

         private int DocLineField;

         private int SAP_DocEntryField;
         //kavx
         private int DocNumField;

         public int LineNum
         {
             get
             {
                 return this.lineNumField;
             }
             set
             {
                 this.lineNumField = value;
             }
         }

         public int DocLine
         {
             get
             {
                 return this.DocLineField;
             }
             set
             {
                 this.DocLineField = value;
             }
         }                          

         public int SAP_DocEntry
         {
             get
             {
                 return this.SAP_DocEntryField;
             }
             set
             {
                 this.SAP_DocEntryField = value;
             }
         }

         public int DocNum
         {
             get
             {
                 return this.DocNumField;
             }
             set
             {
                 this.DocNumField = value;
             }
         }                    

         /// <remarks/>
         public double SumApplied
         {
             get
             {
                 return this.sumAppliedField;
             }
             set
             {
                 this.sumAppliedField = value;
             }
         }

         public string InvoiceType
         {
             get
             {
                 return this.invoiceTypeField;
             }
             set
             {
                 this.invoiceTypeField = value;
             }
         }                

     }*/

}
