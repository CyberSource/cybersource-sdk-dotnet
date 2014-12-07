using System;
using System.Collections;
using System.Text;

namespace CyberSource.Base
{
    public class SafeFields
    {
        public enum MessageType
        {
            REQUEST, REPLY
        }

	    private enum FieldNameState {
		    Boundary, NonIndex, PossibleIndex, Done
	    }

        internal const string REQUEST_MESSAGE = "requestMessage";
        internal const string REPLY_MESSAGE = "replyMessage";
        private const char UNDERSCORE = '_';

        private static Hashtable safeTable;

        static SafeFields()
        {
            safeTable = new Hashtable();

	        safeTable.Add( "item", "unitPrice quantity productCode productName productSKU productRisk taxAmount cityOverrideAmount cityOverrideRate countyOverrideAmount countyOverrideRate districtOverrideAmount districtOverrideRate stateOverrideAmount stateOverrideRate countryOverrideAmount countryOverrideRate orderAcceptanceCity orderAcceptanceCounty orderAcceptanceCountry orderAcceptanceState orderAcceptancePostalCode orderOriginCity orderOriginCounty orderOeriginCountry orderOriginState orderOriginPostalCode shipFromCity shipFromCounty shipFromCountry shipFromState shipFromPostalCode export noExport nationalTax vatRate sellerRegistration buyerRegistration middlemanRegistration pointOfTitleTransfer giftCategory timeCategory hostHedge timeHedge velocityHedge unitOfMeasure taxRate totalAmount discountAmount discountRate commodityCode grossNetIndicator taxTypeApplied discountIndicator alternateTaxID" );

	        safeTable.Add( "ccAuthService", "run cavv commerceIndicator eciRaw xid reconcilationID avsLevel fxQuoteID returnAuthRecord authType verbalAuthCode billPayment" );

	        safeTable.Add( "ccCaptureService", "run authType verbalAuthCode authRequestID transactionToken reconciliationID partialPaymentID purchasingLevel industryDataType" );

	        safeTable.Add( "ccCreditService", "run captureRequestID transactionToken reconciliationID partialPaymentID purchasingLevel industryDataType commerceIndicator billPayment" );

	        safeTable.Add( "ccAuthReversalService", "run authRequestID transactionToken" );

	        safeTable.Add( "ecDebitService", "run paymentMode referenceNumber settlementMethod transactionToken verificationLevel partialPaymentID commerceIndicator" );

	        safeTable.Add( "ecCreditService", "run referenceNumber settlementMethod transactionToken debitRequestID partialPaymentID commerceIndicator" );

	        safeTable.Add( "payerAuthEnrollService", "run httpAccept httpUserAgent merchantName merchantURL purchaseDescription purchaseTime countryCode acquirerBin merchantID" );

	        safeTable.Add( "payerAuthValidateService", "run signedPARes" );

	        safeTable.Add( "taxService", "run nexus noNexus orderAcceptanceCity orderAcceptanceCounty orderAcceptanceCountry orderAcceptanceState orderAcceptancePostalCode orderOriginCity orderOriginCounty orderOriginCountry orderOriginState orderOriginPostalCode sellerRegistration buyerRegistration middlemanRegistration pointOfTitleTransfer" );

	        safeTable.Add( "afsService", "run avsCode cvCode disableAVSScoring" );

	        safeTable.Add( "davService", "run" );

	        safeTable.Add( "exportService", "run addressOperator addressWeight companyWeight nameWeight" );

	        safeTable.Add( "fxRatesService", "run" );

	        safeTable.Add( "bankTransferService", "run" );

	        safeTable.Add( "bankTransferRefundService", "run bankTransferRequestID reconciliationID" );

	        safeTable.Add( "directDebitService", "run dateCollect directDebitText authorizationID transactionType directDebitType validateRequestID" );

	        safeTable.Add( "directDebitRefundService", "run directDebitRequestID reconciliationID" );

	        safeTable.Add( "directDebitValidateService", "run directDebitValidateText" );

	        safeTable.Add( "paySubscriptionCreateService", "run paymentRequestID disableAutoAuth" );

	        safeTable.Add( "paySubscriptionUpdateService", "run" );

	        safeTable.Add( "paySubscriptionEventUpdateService", "run action" );

	        safeTable.Add( "paySubscriptionRetrieveService", "run" );

	        safeTable.Add( "payPalPaymentService", "run cancelURL successURL reconciliationID" );

	        safeTable.Add( "payPalCreditService", "run payPalPaymentRequestID reconciliationID" );

	        safeTable.Add( "voidService", "run voidRequestID" );

	        safeTable.Add( "pinlessDebitService", "run reconciliationID commerceIndicator" );

	        safeTable.Add( "pinlessDebitValidateService", "run" );

	        safeTable.Add( "payPalButtonCreateService", "run buttonType reconciliationID" );

	        safeTable.Add( "payPalPreapprovedPaymentService", "run reconciliationID" );

	        safeTable.Add( "payPalPreapprovedUpdateService", "run reconciliationID" );

	        safeTable.Add( "riskUpdateService", "run actionCode recordID negativeAddress_city negativeAddress_state negativeAddress_postalCode negativeAddress_country" );

	        safeTable.Add( "invoiceHeader", "merchantDescriptor merchantDescriptorContact isGift returnsAccepted tenderType merchantVATRegistrationNumber purchaserOrderDate purchaserVATRegistrationNumber vatInvoiceReferenceNumber summaryCommodityCode supplierOrderReference userPO costCenter purchaserCode taxable amexDataTAA1 amexDataTAA2 amexDataTAA3 amexDataTAA4 invoiceDate" );

	        safeTable.Add( "businessRules", "ignoreAVSResult ignoreCVResult ignoreDAVResult ignoreExportResult ignoreValidateResult declineAVSFlags scoreThreshold" );

	        safeTable.Add( "billTo", "title suffix city county state postalCode country company ipAddress ipNetworkAddress hostname domainName driversLicenseState customerID httpBrowserType httpBrowserCookiesAccepted" );

	        safeTable.Add( "shipTo", "title suffix city county state postalCode country company shippingMethod" );

	        safeTable.Add( "shipFrom", "title suffix city county state postalCode country company" );

	        safeTable.Add( "card", "bin" );	

	        safeTable.Add( "check", "" );
        	
	        safeTable.Add( "bml", "customerBillingAddressChange customerEmailChange customerHasCheckingAccount CustomerHasSavingsAccount customerPasswordChange customerPhoneChange customerRegistrationDate customerTypeFlag grossHouseholdIncome householdIncomeCurrency itemCategory merchantPromotionCode preapprovalNumber productDeliveryTypeIndicator residenceStatus tcVersion yearsAtCurrentResidence yearsWithCurrentEmployer employerCity employerCompanyName employerCountry employerPhoneType employerState employerPostalCode shipToPhoneType billToPhoneType" ); 

	        safeTable.Add( "otherTax", "vatTaxAmount vatTaxRate alternateTaxAmount alternateTaxIndicator alternateTaxID localTaxAmount localTaxIndicator nationalTaxAmount nationalTaxIndicator" );
        	
	        safeTable.Add( "purchaseTotals", "currency discountAmount taxAmount dutyAmount grandTotalAmount freightAmount" );

	        safeTable.Add( "fundingTotals", "currency grandTotalAmount" );

	        safeTable.Add( "gecc", "saleType planNumber sequenceNumber promotionEndDate promotionPlan line" );

	        safeTable.Add( "ucaf", "authenticationData collectionIndicator" );

	        safeTable.Add( "fundTransfer", "" );

	        safeTable.Add( "bankInfo", "bankCode name address city country branchCode swiftCode sortCode" );

	        safeTable.Add( "recurringSubscriptionInfo", "status amount numberOfPayments numberOfPaymentsToAdd automaticRenew frequency startDate endDate approvalRequired event_amount event_approvedBy event_number billPayment" );

	        safeTable.Add( "subscription", "title paymentMethod" );

	        safeTable.Add( "decisionManager", "enabled profile" );

	        safeTable.Add( "batch", "batchID recordID" );

	        safeTable.Add( "payPal", "" );

	        safeTable.Add( "jpo", "paymentMethod bonusAmount bonuses installments" );

	        safeTable.Add( REQUEST_MESSAGE, "merchantID merchantReferenceCode clientLibrary clientLibraryVersion clientEnvironment clientSecurityLibraryVersion clientApplication clientApplicationVersion clientApplicationUser comments" );

	        safeTable.Add( "ccAuthReply", "reasonCode amount avsCode avsCodeRaw cvCode cvCodeRaw authorizedDateTime processorResponse authFactorCode reconciliationID transactionToken fundingTotals_currency fundingTotals_grandTotalAmount fxQuoteID fxQuoteRate fxQuoteType fxQuoteExpirationDateTime" );

	        safeTable.Add( "ccCaptureReply", "reasonCode requestDateTime amount reconciliationID transactionToken fundingTotals_currency fundingTotals_grandTotalAmount fxQuoteID fxQuoteRate fxQuoteType fxQuoteExpirationDateTime purchasingLevel3Enabled enhancedDataEnabled" );

	        safeTable.Add( "ccCreditReply", "reasonCode requestDateTime amount reconciliationID transactionToken purchasingLevel3Enabled enhancedDataEnabled" );

	        safeTable.Add( "ccAuthReversalReply", "reasonCode amount processorResponse requestDateTime transactionToken" );

	        safeTable.Add( "ecDebitReply", "reasonCode settlementMethod requestDateTime amount verificationLevel reconciliationID processorResponse transactionToken avsCode avsCodeRaw" );

	        safeTable.Add( "ecCreditReply", "reasonCode settlementMethod requestDateTime amount reconciliationID processorResponse transactionToken" );

	        safeTable.Add( "payerAuthEnrollReply", "reasonCode acsURL commerceIndicator paReq proxyPAN xid proofXML ucafCollectionIndicator" );

	        safeTable.Add( "payerAuthValidateReply", "reasonCode authenticationResult authenticationStatusMessage cavv commerceIndicator eci eciRaw xid ucafAuthenticationData ucafCollectionIndicator" );

	        safeTable.Add( "taxReply", "reasonCode currency grandTotalAmount totalCityTaxAmount city totalCountyTaxAmount county totalDistrictTaxAmount totalStateTaxAmount state totalTaxAmount postalCode geocode item_cityTaxAmount item_countyTaxAmount item_districtTaxAmount item_stateTaxAmount item_totalTaxAmount" );

	        safeTable.Add( "afsReply", "reasonCode afsResult hostSeverity consumerLocalTime afsFactorCode addressInfoCode hotlistInfoCode internetInfoCode phoneInfoCode suspiciousInfoCode velocityInfoCode" );

	        safeTable.Add( "davReply", "reasonCode addressType apartmentInfo barCode barCodeCheckDigit cityInfo countryInfo directionalInfo lvrInfo matchScore standardizedCity standardizedCounty standardizedCSP standardizedState standardizedPostalCode standardizedCountry standardizedISOCountry stateInfo streetInfo suffixInfo postalCodeInfo overallInfo usInfo caInfo intlInfo usErrorInfo caErrorInfo intlErrorInfo" );

	        safeTable.Add( "deniedPartiesMatch", "list" );
        	
	        safeTable.Add( "exportReply", "reasonCode ipCountryConfidence" );

	        safeTable.Add( "fxRatesReply", "reasonCode quote_id quote_rate quote_type quote_expirationDateTime quote_currency quote_fundingCurrency quote_receivedDateTime" );

	        safeTable.Add( "bankTransferReply", "reasonCode amount bankName bankCity bankCountry paymentReference processorResponse bankSwiftCode bankSpecialID requestDateTime reconciliationID" );

	        safeTable.Add( "bankTransferRefundReply", "reasonCode amount requestDateTime reconciliationID processorResponse" );

	        safeTable.Add( "directDebitReply", "reasonCode amount requestDateTime reconciliationID processorResponse" );

	        safeTable.Add( "directDebitValidateReply", "reasonCode amount requestDateTime reconciliationID processorResponse" );

	        safeTable.Add( "directDebitRefundReply", "reasonCode amount requestDateTime reconciliationID processorResponse" );

	        safeTable.Add( "paySubscriptionCreateReply", "reasonCode" );

	        safeTable.Add( "paySubscriptionUpdateReply", "reasonCode" );

	        safeTable.Add( "paySubscriptionEventUpdateReply", "reasonCode" );

	        safeTable.Add( "paySubscriptionRetrieveReply", "reasonCode approvalRequired automaticRenew cardType checkAccountType city comments companyName country currency customerAccountID endDate frequency merchantReferenceCode paymentMethod paymentsRemaining postalCode recurringAmount setupAmount startDate state status title totalPayments shipToCity shipToState shipToCompany shipToCountry billPayment merchantDefinedDataField1 merchantDefinedDataField2 merchantDefinedDateField3 merchantDefinedDataField4" );

	        safeTable.Add( "payPalPaymentReply", "reasonCode amount requestDateTime reconciliationID" );

	        safeTable.Add( "payPalCreditReply", "reasonCode amount requestDateTime reconciliationID processorResponse" );

	        safeTable.Add( "voidReply", "reasonCode requestDateTime amount currency" );

	        safeTable.Add( "pinlessDebitReply", "reasonCode amount requestDateTime processorResponse receiptNumber reconciliationID" );

	        safeTable.Add( "pinlessDebitValidateReply", "reasonCode status requestDateTime" );

	        safeTable.Add( "payPalButtonCreateReply", "reasonCode encryptedFormData requestDateTime reconciliationID buttonType" );

	        safeTable.Add( "payPalPreapprovedPaymentReply", "reasonCode requestDateTime reconciliationID payerStatus transactionType feeAmount payerCountry pendingReason paymentStatus mpStatus payerBusiness desc mpMax paymentType paymentDate paymentGrossAmount settleAmount taxAmount exchangeRate paymentSourceID" );

	        safeTable.Add( "payPalPreapprovedUpdateReply", "reasonCode requestDateTime reconciliationID payerStatus payerCountry mpStatus payerBusiness desc mpMax paymentSourceID" );

	        safeTable.Add( "riskUpdateReply", "reasonCode" );

	        safeTable.Add( "decisionReply", "activeProfileReply_selectedBy activeProfileReply_name activeProfileReply_destinationQueue activeProfileReply_rulesTriggered_ruleResultItem_name activeProfileReply_rulesTriggered_ruleResultItem_decision activeProfileReply_rulesTriggered_ruleResultItem_evaluation activeProfileReply_rulesTriggered_ruleResultItem_ruleID" );

	        safeTable.Add( REPLY_MESSAGE, "merchantReferenceCode requestID decision reasonCode missingField invalidField requestToken" );

	        safeTable.Add( "airlineData", "agentCode agentName ticketIssuerCity ticketIssuerState ticketIssuerPostalCode ticketIssuerCountry ticketIssuerCode ticketIssuerName ticketNumber checkDigit restrictedTicketIndicator transactionType extendedPaymentCode carrierName customerCode documentType documentNumber documentNumberOfParts chargeDetails bookingReference leg_carrierCode leg_flightNumber leg_originatingAirportCode leg_class leg_stopoverCode leg_departureDate leg_destination leg_fareBasis leg_departTax" );

	        safeTable.Add( "pos", "entryMode cardPresent terminalCapability terminalID terminalType terminalLocation transactionSecurity catLevel conditionCode" );

	        safeTable.Add( "merchantDefinedData", "field1 field2 field3 field4" );
        }

        public static bool IsSafe(MessageType type, string field)
        {
            field = RemoveIndices(field);

	        string parent, child;
            int pos = field.IndexOf( UNDERSCORE );
            if (pos != -1) {
                parent = field.Substring( 0, pos );
                child  = field.Substring( pos + 1 );
                return( IsSafe( parent, child ) );
            }
            else {
                return (
                    IsSafe(
                        type == MessageType.REQUEST
                            ? REQUEST_MESSAGE : REPLY_MESSAGE,
                        field));
            }
        }

        public static bool IsSafe( string parent, string child )
        {	
            string list = (string) safeTable[parent];

            // if none, then this field is definitely not safe
            if (list == null) return( false );

            // return whether or not this child is on the list
            return( list.Contains( child ) );
        }

        // removes indices, e.g. item_0_unitPrice becomes item_unitPrice.
        public static string RemoveIndices(string field)
        {
            int len = field != null ? field.Length : 0;
            if (len == 0) return( field );

            char ch;
            bool isDigit, isPastEnd;
            int indexStart = 0;
            FieldNameState state = FieldNameState.Boundary;
            StringBuilder sb = new StringBuilder();

            for (int src = 0;
                 state != FieldNameState.Done;
                 ++src)
            {
                isPastEnd = src >= len;
                ch = !isPastEnd ? field[src] : UNDERSCORE;
                isDigit = Char.IsDigit( ch );

                switch (state)
                {
                    case FieldNameState.Boundary:
                        if (isDigit)
                        {
                            state = FieldNameState.PossibleIndex;
                            indexStart = sb.Length;
                        }
                        else
                        {
                            state = FieldNameState.NonIndex;
                        }
                        break;
                    case FieldNameState.NonIndex:
                        if (ch == UNDERSCORE)
                        {
                            state = FieldNameState.Boundary;
                        }
                        break;
                    case FieldNameState.PossibleIndex:

                        if (ch == UNDERSCORE)
                        {
                            if (indexStart == 0)
                            {
                                // we found an index at the start of
                                // the string; let's remove it and the
                                // underscore after it.
                                sb.Remove(0, sb.Length);
                                if (!isPastEnd) continue;
                            }
                            // we found an index either in the middle
                            // or at the end of the string; let's
                            // remove it and the underscore before
                            // it.
                            else
                            {
                                sb.Remove(
                                    indexStart - 1,
                                    sb.Length - indexStart + 1 ); 
                            }
                            state = FieldNameState.Boundary;

                        }
                        else if (!isDigit)
                        {
                            // it wasn't an index after all
                            state = FieldNameState.NonIndex;
                        }
                        // else if still a digit, then
                        // it's still a possible index
                        break;
                } // switch

                if (!isPastEnd)
                {
                    sb.Append(ch);
                }
                else
                {
                    state = FieldNameState.Done;
                }

            } // for

            return (sb.ToString());

        } // RemoveIndices
    }
}
