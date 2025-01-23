using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CyberSource;
using CyberSource.Clients;

namespace Client.Samples
{
    public static class Util
    {
        #region constants
        public const String ICS_PAYPAL_CREATE_AGREEMENT = "ics_paypal_create_agreement";
        public const String ICS_PAYPAL_EC_DO_PAYMENT = "ics_paypal_ec_do_payment";
        public const String ICS_PAYPAL_EC_GET_DETAILS = "ics_paypal_ec_get_details";
        public const String ICS_PAYPAL_EC_ORDER_SETUP = "ics_paypal_ec_order_setup";
        public const String ICS_PAYPAL_EC_SET = "ics_paypal_ec_set";
        public const String SO_PAYPAL_CREATE_AGREEMENT_SERVICE_PAYPAL_TOKEN = "payPalCreateAgreementService_paypalToken";
        public const String SO_PAYPAL_EC_DO_PAYMENT_SERVICE_PAYPAL_TOKEN = "payPalEcDoPaymentService_paypalToken";
        public const String SO_PAYPAL_EC_GET_DETAILS_SERVICE_PAYPAL_TOKEN = "payPalEcGetDetailsService_paypalToken";
        public const String SO_PAYPAL_EC_ORDER_SETUP_SERVICE_PAYPAL_TOKEN = "payPalEcOrderSetupService_paypalToken";
        public const String SO_PAYPAL_EC_SET_SERVICE_PAYPAL_TOKEN = "payPalEcSetService_paypalToken";


        // Keys for Simple Order API request
        public const String SO_REQUEST_CC_AUTH_SERVICE_RUN = "ccAuthService_run";
        public const String SO_REQUEST_CC_CREDIT_SERVICE_RUN = "ccCreditService_run";
        public const String SO_REQUEST_MERCHANT_REFERENCE_CODE = "merchantReferenceCode";
        public const String SO_REQUEST_BILL_TO_FIRST_NAME = "billTo_firstName";
        public const String SO_REQUEST_BILL_TO_LAST_NAME = "billTo_lastName";
        public const String SO_REQUEST_BILL_TO_EMAIL = "billTo_email";
        public const String SO_REQUEST_BILL_TO_PHONE_NUMBER = "billTo_phoneNumber";
        public const String SO_REQUEST_BILL_TO_STREET1 = "billTo_street1";
        public const String SO_REQUEST_BILL_TO_STREET2 = "billTo_street2";//SCMP:bill_address2
        public const String SO_REQUEST_BILL_TO_CITY = "billTo_city";
        public const String SO_REQUEST_BILL_TO_STATE = "billTo_state";
        public const String SO_REQUEST_BILL_TO_POSTAL_CODE = "billTo_postalCode";
        public const String SO_REQUEST_BILL_TO_COUNTRY = "billTo_country";
        public const String SO_REQUEST_BILL_TO_IP_ADDRESS = "billTo_ipAddress"; // SCMP: customer_ipaddress
        public const String SO_REQUEST_SHIP_TO_FIRST_NAME = "shipTo_firstName";//SCMP: ship_to_firstname
        public const String SO_REQUEST_SHIP_TO_LAST_NAME = "shipTo_lastName";//SCMP: ship_to_lasttname
        public const String SO_REQUEST_SHIP_TO_STREET1 = "shipTo_street1";
        public const String SO_REQUEST_SHIP_TO_STREET2 = "shipTo_street2";
        public const String SO_REQUEST_SHIP_TO_CITY = "shipTo_city";
        public const String SO_REQUEST_SHIP_TO_STATE = "shipTo_state";
        public const String SO_REQUEST_SHIP_TO_POSTAL_CODE = "shipTo_postalCode";
        public const String SO_REQUEST_SHIP_TO_COUNTRY = "shipTo_country";
        public const String SO_REQUEST_CARD_ACCOUNT_NUMBER = "card_accountNumber";
        public const String SO_REQUEST_CARD_EXPIRATION_MONTH = "card_expirationMonth";
        public const String SO_REQUEST_CARD_EXPIRATION_YEAR = "card_expirationYear";
        public const String SO_REQUEST_PURCHASE_TOTALS_CURRENCY = "purchaseTotals_currency";
        public const String SO_REQUEST_CHECK_ACCOUNT_ENCODER_ID = "check_accountEncoderID";
        public const String SO_REQUEST_CARD_ACCOUNT_ENCODER_ID = "card_accountEncoderID";
        public const String SO_REQUEST_AUTH_SERVICE_AGGREGATOR_ID = "ccAuthService_aggregatorID";
        public const String SO_REQUEST_CREDIT_SERVICE_AGGREGATOR_ID = "ccCreditService_aggregatorID";
        public const String SO_REQUEST_AUTH_SERVICE_AGGREGATOR_NAME = "ccAuthService_aggregatorName";
        public const String SO_REQUEST_CREDIT_SERVICE_AGGREGATOR_NAME = "ccCreditService_aggregatorName";
        public const String SO_REQUEST_AP_AUTH_REVERSAL_SERVICE_AUTH_REQUEST_ID = "apAuthReversalService_authRequestID";
        public const String SO_REQUEST_AP_CAPTURE_SERVICE_AUTH_REQUEST_ID = "apCaptureService_authRequestID";
        public const String SO_REQUEST_AP_CHECK_STATUS_SERVICE_INITIATE_REQUEST_ID = "apCheckStatusService_apInitiateRequestID";
        public const String SO_REQUEST_AP_REFUND_SERVICE_INITIATE_REQUEST_ID = "apRefundService_apInitiateRequestID";
        public const String SO_REQUEST_AUTO_AUTH_REVERSAL_SERVICE_AUTH_CODE = "ccAutoAuthReversalService_authCode";
        public const String SO_REQUEST_CREDIT_SERVICE_AUTH_CODE = "ccCreditService_authCode";
        public const String SO_REQUEST_AUTH_REVERSAL_SERVICE_AUTH_REQUEST_ID = "ccAuthReversalService_authRequestID";
        public const String SO_REQUEST_AUTO_AUTH_REVERSAL_SERVICE_AUTH_REQUEST_ID = "ccAutoAuthReversalService_authRequestID";
        public const String SO_REQUEST_CAPTURE_SERVICE_AUTH_REQUEST_ID = "ccCaptureService_authRequestID";
        public const String SO_REQUEST_AUTH_REVERSAL_SERVICE_AUTH_REQUEST_TOKEN = "ccAuthReversalService_authRequestToken";
        public const String SO_REQUEST_CAPTURE_SERVICE_AUTH_REQUEST_TOKEN = "ccCaptureService_authRequestToken";
        public const String SO_REQUEST_AUTH_SERVICE_RECONCILIATION_ID = "ccAuthService_reconciliationID";
        public const String SO_REQUEST_AUTO_AUTH_REVERSAL_SERVICE_RECONCILIATION_ID = "ccAutoAuthReversalService_reconciliationID";
        public const String SO_REQUEST_AUTH_SERVICE_AUTH_TYPE = "ccAuthService_authType";
        public const String SO_REQUEST_CAPTURE_SERVICE_AUTH_TYPE = "ccCaptureService_authType";
        public const String SO_REQUEST_AUTH_SERVICE_BILL_PAYMENT = "ccAuthService_billPayment";
        public const String SO_REQUEST_AUTO_AUTH_REVERSAL_SERVICE_BILL_PAYMENT = "ccAutoAuthReversalService_billPayment";
        public const String SO_REQUEST_CREDIT_SERVICE_BILL_PAYMENT = "ccCreditService_billPayment";
        public const String SO_REQUEST_CREDIT_SERVICE_CAPTURE_REQUEST_ID = "ccCreditService_captureRequestID";
        public const String SO_REQUEST_DCC_UPDATE_SERVICE_CAPTURE_REQUEST_ID = "ccDCCUpdateService_captureRequestID";
        public const String SO_REQUEST_AUTH_SERVICE_CHECKSUM_KEY = "ccAuthService_checksumKey";
        public const String SO_REQUEST_CAPTURE_SERVICE_CHECKSUM_KEY = "ccCaptureService_checksumKey";
        public const String SO_REQUEST_CREDIT_SERVICE_CHECKSUM_KEY = "ccCreditService_checksumKey";
        public const String SO_REQUEST_PAYPAL_TRANSACTION_SEARCH_SERVICE_CURRENCY = "payPalTransactionSearchService_currency";
        public const String SO_REQUEST_DIRECT_DEBIT_REFUND_SERVICE_MANDATE_AUTHENTICATION_DATE = "directDebitRefundService_mandateAuthenticationDate";
        public const String SO_REQUEST_DIRECT_DEBIT_SERVICE_MANDATE_AUTHENTICATION_DATE = "directDebitService_mandateAuthenticationDate";
        public const String SO_REQUEST_DIRECT_DEBIT_REFUND_SERVICE_RECURRING_TYPE = "directDebitRefundService_recurringType";
        public const String SO_REQUEST_DIRECT_DEBIT_SERVICE_RECURRING_TYPE = "directDebitService_recurringType";
        public const String SO_REQUEST_DIRECT_DEBIT_REFUND_SERVICE_DIRECT_DEBIT_TYPE = "directDebitRefundService_directDebitType";
        public const String SO_REQUEST_DIRECT_DEBIT_SERVICE_DIRECT_DEBIT_TYPE = "directDebitService_directDebitType";
        public const String SO_REQUEST_AUTH_SERVICE_COMMERCE_INDICATOR = "ccAuthService_commerceIndicator";
        public const String SO_REQUEST_AUTO_AUTH_REVERSAL_SERVICE_COMMERCE_INDICATOR = "ccAutoAuthReversalService_commerceIndicator";
        public const String SO_REQUEST_CREDIT_SERVICE_COMMERCE_INDICATOR = "ccCreditService_commerceIndicator";
        public const String SO_REQUEST_ECP_CREDIT_SERVICE_COMMERCE_INDICATOR = "ecCreditService_commerceIndicator";
        public const String SO_REQUEST_ECP_DEBIT_SERVICE_COMMERCE_INDICATOR = "ecDebitService_commerceIndicator";
        public const String SO_REQUEST_PIN_DEBIT_CREDIT_SERVICE_COMMERCE_INDICATOR = "pinDebitCreditService_commerceIndicator";
        public const String SO_REQUEST_PIN_DEBIT_SERVICE_COMMERCE_INDICATOR = "pinDebitPurchaseService_commerceIndicator";
        public const String SO_REQUEST_PIN_LESS_DEBIT_SERVICE_COMMERCE_INDICATOR = "pinlessDebitService_commerceIndicator";

        public const String SO_REQUEST_ECP_CREDIT_SERVICE_DEBIT_REQUEST_ID = "ecCreditService_debitRequestID";
        public const String SO_REQUEST_ECP_DEBIT_SERVICE_DEBIT_REQUEST_ID = "ecDebitService_debitRequestID";
        public const String SO_REQUEST_ECP_CREDIT_SERVICE_TRANSACTION_TOKEN = "ecCreditService_transactionToken";
        public const String SO_REQUEST_ECP_DEBIT_SERVICE_TRANSACTION_TOKEN = "ecDebitService_transactionToken";

        public const String SO_REQUEST_ECP_AUTHENTICATE_SERVICE_REFERENCE_NUMBER = "ecAuthenticateService_referenceNumber";
        public const String SO_REQUEST_ECP_CREDIT_SERVICE_REFERENCE_NUMBER = "ecCreditService_referenceNumber";
        public const String SO_REQUEST_ECP_DEBIT_SERVICE_REFERENCE_NUMBER = "ecDebitService_referenceNumber";

        public const String SO_REQUEST_ECP_CREDIT_SERVICE_SETTLEMENT_METHOD = "ecCreditService_settlementMethod";
        public const String SO_REQUEST_ECP_DEBIT_SERVICE_SETTLEMENT_METHOD = "ecDebitService_settlementMethod";

        public const String SO_REQUEST_PAYPAL_TRANSACTION_SEARCH_SERVICE_GRAND_TOTAL_AMOUNT = "payPalTransactionSearchService_grandTotalAmount";
        public const String SO_REQUEST_PURCHASE_TOTALS_GRAND_TOTAL_AMOUNT = "purchaseTotals_grandTotalAmount";

        public const String SO_REQUEST_AUTH_SERVICE_INDUSTRY_DATA_TYPE = "ccAuthService_industryDatatype";
        public const String SO_REQUEST_CAPTURE_SERVICE_INDUSTRY_DATA_TYPE = "ccCaptureService_industryDatatype";
        public const String SO_REQUEST_CREDIT_SERVICE_INDUSTRY_DATA_TYPE = "ccCreditService_industryDatatype";

        public const String SO_REQUEST_DIRECT_DEBIT_REFUND_SERVICE_MANDATE_ID = "directDebitRefundService_mandateID";
        public const String SO_REQUEST_DIRECT_DEBIT_SERVICE_MANDATE_ID = "directDebitService_mandateID";

        public const String SO_REQUEST_FRAUD_UPDATE_SERVICE_MARKING_NOTES = "fraudUpdateService_markingNotes";
        public const String SO_REQUEST_RISK_UPDATE_SERVICE_MARKING_NOTES = "riskUpdateService_markingNotes";

        public const String SO_REQUEST_FRAUD_UPDATE_SERVICE_MARKING_REASON = "fraudUpdateService_markingReason";
        public const String SO_REQUEST_RISK_UPDATE_SERVICE_MARKING_REASON = "riskUpdateService_markingReason";

        public const String SO_REQUEST_CAPTURE_SERVICE_MERCHANT_RECEIPT_NUMBER = "ccCaptureService_merchantReceiptNumber";
        public const String SO_REQUEST_CREDIT_SERVICE_MERCHANT_RECEIPT_NUMBER = "ccCreditService_merchantReceiptNumber";
        public const String SO_REQUEST_PIN_DEBIT_CREDIT_SERVICE_NETWORK_ORDER = "pinDebitCreditService_networkOrder";
        public const String SO_REQUEST_PIN_DEBIT_SERVICE_NETWORK_ORDER = "pinDebitPurchaseService_networkOrder";

        public const String SO_REQUEST_CAPTURE_SERVICE_PARTIAL_PAYMENT_ID = "ccCaptureService_partialPaymentID";
        public const String SO_REQUEST_CREDIT_SERVICE_PARTIAL_PAYMENT_ID = "ccCreditService_partialPaymentID";
        public const String SO_REQUEST_ECP_CREDIT_SERVICE_PARTIAL_PAYMENT_ID = "ecCreditService_partialPaymentID";
        public const String SO_REQUEST_ECP_DEBIT_SERVICE_PARTIAL_PAYMENT_ID = "ecDebitService_partialPaymentID";

        public const String SO_REQUEST_PAYPAL_AUTH_REVERSAL_SERVICE_PAYPAL_AUTHORIZATION_ID = "payPalAuthReversalService_paypalAuthorizationId";
        public const String SO_REQUEST_PAYPAL_DO_CAPTURE_SERVICE_PAYPAL_AUTHORIZATION_ID = "payPalDoCaptureService_paypalAuthorizationId";

        public const String SO_REQUEST_PAYPAL_AUTH_REVERSAL_SERVICE_PAYPAL_AUTHORIZATION_REQUEST_ID = "payPalAuthReversalService_paypalAuthorizationRequestID";
        public const String SO_REQUEST_PAYPAL_DO_CAPTURE_SERVICE_PAYPAL_AUTHORIZATION_REQUEST_ID = "payPalDoCaptureService_paypalAuthorizationRequestID";

        public const String SO_REQUEST_PAYPAL_AUTH_REVERSAL_SERVICE_PAYPAL_AUTHORIZATION_REQUEST_TOKEN = "payPalAuthReversalService_paypalAuthorizationRequestToken";
        public const String SO_REQUEST_PAYPAL_DO_CAPTURE_SERVICE_PAYPAL_AUTHORIZATION_REQUEST_TOKEN = "payPalDoCaptureService_paypalAuthorizationRequestToken";
        public const String SO_REQUEST_PAYPAL_EC_SET_SERVICE_PAYPAL_BILLING_AGREEMENT_CUSTOM = "payPalEcSetService_paypalBillingAgreementCustom";
        public const String SO_REQUEST_PAYPAL_UPDATE_AGREEMENT_SERVICE_PAYPAL_BILLING_AGREEMENT_CUSTOM = "payPalUpdateAgreementService_paypalBillingAgreementCustom";

        public const String SO_REQUEST_PAYPAL_DO_REF_TRANSACTION_SERVICE_PAYPAL_BILLING_AGREEMENT_ID = "payPalDoRefTransactionService_paypalBillingAgreementId";
        public const String SO_REQUEST_PAYPAL_UPDATE_AGREEMENT_SERVICE_PAYPAL_BILLING_AGREEMENT_ID = "payPalUpdateAgreementService_paypalBillingAgreementId";

        public const String SO_REQUEST_PAYPAL_AUTHORIZATION_SERVICE_PAYPAL_CUSTOMER_EMAIL = "payPalAuthorizationService_paypalCustomerEmail";
        public const String SO_REQUEST_PAYPAL_EC_DO_PAYMENT_SERVICE_PAYPAL_CUSTOMER_EMAIL = "payPalEcDoPaymentService_paypalCustomerEmail";
        public const String SO_REQUEST_PAYPAL_EC_ORDER_SETUP_SERVICE_PAYPAL_CUSTOMER_EMAIL = "payPalEcOrderSetupService_paypalCustomerEmail";
        public const String SO_REQUEST_PAYPAL_EC_SET_SERVICE_PAYPAL_CUSTOMER_EMAIL = "payPalEcSetService_paypalCustomerEmail";
        public const String SO_REQUEST_PAYPAL_TRANSACTION_SEARCH_SERVICE_PAYPAL_CUSTOMER_EMAIL = "payPalTransactionSearchService_paypalCustomerEmail";

        public const String SO_REQUEST_PAYPAL_DO_REF_TRANSACTION_SERVICE_PAYPAL_DESC = "payPalDoRefTransactionService_paypalDesc";
        public const String SO_REQUEST_PAYPAL_EC_DO_PAYMENT_SERVICE_PAYPAL_DESC = "payPalEcDoPaymentService_paypalDesc";
        public const String SO_REQUEST_PAYPAL_EC_ORDER_SETUP_SERVICE_PAYPAL_DESC = "payPalEcOrderSetupService_paypalDesc";
        public const String SO_REQUEST_PAYPAL_EC_SET_SERVICE_PAYPAL_DESC = "payPalEcSetService_paypalDesc";

        public const String SO_REQUEST_PAYPAL_AUTH_REVERSAL_SERVICE_PAYPAL_EC_DO_PAYMENT_REQUEST_ID = "payPalAuthReversalService_paypalEcDoPaymentRequestID";
        public const String SO_REQUEST_PAYPAL_DO_CAPTURE_SERVICE_PAYPAL_EC_DO_PAYMENT_REQUEST_ID = "payPalDoCaptureService_paypalEcDoPaymentRequestID";

        public const String SO_REQUEST_PAYPAL_AUTH_REVERSAL_SERVICE_PAYPAL_EC_DO_PAYMENT_REQUEST_TOKEN = "payPalAuthReversalService_paypalEcDoPaymentRequestToken";
        public const String SO_REQUEST_PAYPAL_DO_CAPTURE_SERVICE_PAYPAL_EC_DO_PAYMENT_REQUEST_TOKEN = "payPalDoCaptureService_paypalEcDoPaymentRequestToken";

        public const String SO_REQUEST_PAYPAL_AUTH_SERVICE_PAYPAL_EC_ORDER_SETUP_REQUEST_ID = "payPalAuthorizationService_paypalEcOrderSetupRequestID";
        public const String SO_REQUEST_PAYPAL_AUTH_REVERSAL_SERVICE_PAYPAL_EC_ORDER_SETUP_REQUEST_ID = "payPalAuthReversalService_paypalEcOrderSetupRequestID";

        public const String SO_REQUEST_PAYPAL_AUTHORIZATION_SERVICE_PAYPAL_EC_ORDER_SETUP_REQUEST_TOKEN = "payPalAuthorizationService_paypalEcOrderSetupRequestToken";
        public const String SO_REQUEST_PAYPAL_AUTH_REVERSAL_SERVICE_PAYPAL_EC_ORDER_SETUP_REQUEST_TOKEN = "payPalAuthReversalService_paypalEcOrderSetupRequestToken";

        public const String SO_REQUEST_PAYPAL_CREATE_AGREEMENT_SERVICE_PAYPAL_EC_SET_REQUEST_ID = "payPalCreateAgreementService_paypalEcSetRequestID";
        public const String SO_REQUEST_PAYPAL_EC_DO_PAYMENT_SERVICE_PAYPAL_EC_SET_REQUEST_ID = "payPalEcDoPaymentService_paypalEcSetRequestID";
        public const String SO_REQUEST_PAYPAL_EC_GET_DETAILS_SERVICE_PAYPAL_EC_SET_REQUEST_ID = "payPalEcGetDetailsService_paypalEcSetRequestID";
        public const String SO_REQUEST_PAYPAL_EC_ORDER_SETUP_SERVICE_PAYPAL_EC_SET_REQUEST_ID = "payPalEcOrderSetupService_paypalEcSetRequestID";
        public const String SO_REQUEST_PAYPAL_EC_SET_SERVICE_PAYPAL_EC_SET_REQUEST_ID = "payPalEcSetService_paypalEcSetRequestID";

        public const String SO_REQUEST_PAYPAL_CREATE_AGREEMENT_SERVICE_PAYPAL_EC_SET_REQUEST_TOKEN = "payPalCreateAgreementService_paypalEcSetRequestToken";
        public const String SO_REQUEST_PAYPAL_EC_DO_PAYMENT_SERVICE_PAYPAL_EC_SET_REQUEST_TOKEN = "payPalEcDoPaymentService_paypalEcSetRequestToken";
        public const String SO_REQUEST_PAYPAL_EC_GET_DETAILS_SERVICE_PAYPAL_EC_SET_REQUEST_TOKEN = "payPalEcGetDetailsService_paypalEcSetRequestToken";
        public const String SO_REQUEST_PAYPAL_EC_ORDER_SETUP_SERVICE_PAYPAL_EC_SET_REQUEST_TOKEN = "payPalEcOrderSetupService_paypalEcSetRequestToken";
        public const String SO_REQUEST_PAYPAL_EC_SET_SERVICE_PAYPAL_EC_SET_REQUEST_TOKEN = "payPalEcSetService_paypalEcSetRequestToken";

        public const String SO_REQUEST_PAYPAL_DO_CAPTURE_SERVICE_INVOICE_NUMBER = "payPalDoCaptureService_invoiceNumber";
        public const String SO_REQUEST_PAYPAL_DO_REF_TRANSACTION_SERVICE_INVOICE_NUMBER = "payPalDoRefTransactionService_invoiceNumber";
        public const String SO_REQUEST_PAYPAL_EC_DO_PAYMENT_SERVICE_INVOICE_NUMBER = "payPalEcDoPaymentService_invoiceNumber";
        public const String SO_REQUEST_PAYPAL_EC_ORDER_SETUP_SERVICE_INVOICE_NUMBER = "payPalEcOrderSetupService_invoiceNumber";
        public const String SO_REQUEST_PAYPAL_EC_SET_SERVICE_INVOICE_NUMBER = "payPalEcSetService_invoiceNumber";
        public const String SO_REQUEST_PAYPAL_TRANSACTION_SEARCH_SERVICE_INVOICE_NUMBER = "payPalTransactionSearchService_invoiceNumber";
        public const String SO_REQUEST_PAYPAL_PRE_APPROVED_PAYMENT_SERVICE_MP_ID = "payPalPreapprovedPaymentService_mpID";
        public const String SO_REQUEST_PAYPAL_PRE_APPROVED_UPDATE_SERVICE_MP_ID = "payPalPreapprovedUpdateService_mpID";

        public const String SO_REQUEST_PAYPAL_EC_DO_PAYMENT_SERVICE_PAYPAL_PAYER_ID = "payPalEcDoPaymentService_paypalPayerId";
        public const String SO_REQUEST_PAYPAL_EC_ORDER_SETUP_SERVICE_PAYPAL_PAYER_ID = "payPalEcOrderSetupService_paypalPayerId";

        public const String SO_REQUEST_PAYPAL_DO_REF_TRANSACTION_SERVICE_PAYPAL_PAYMENT_TYPE = "payPalDoRefTransactionService_paypalPaymentType";
        public const String SO_REQUEST_PAYPAL_EC_SET_SERVICE_PAYPAL_PAYMENT_TYPE = "payPalEcSetService_paypalPaymentType";

        public const String SO_REQUEST_PAYPAL_DO_REF_TRANSACTION_SERVICE_PAYPAL_REQ_CONFIRM_SHIPPING = "payPalDoRefTransactionService_paypalReqconfirmshipping";
        public const String SO_REQUEST_PAYPAL_EC_SET_SERVICE_PAYPAL_REQ_CONFIRM_SHIPPING = "payPalEcSetService_paypalReqconfirmshipping";

        public const String SO_REQUEST_PAYPAL_GET_TRANSACTION_DETAILS_SERVICE_TRANSACTION_ID = "payPalGetTxnDetailsService_transactionID";
        public const String SO_REQUEST_PAYPAL_TRANSACTION_SEARCH_SERVICE_TRANSACTION_ID = "payPalTransactionSearchService_transactionID";

        public const String SO_REQUEST_CAPTURE_SERVICE_PURCHASING_LEVEL = "ccCreditService_purchasingLevel";
        public const String SO_REQUEST_CREDIT_SERVICE_PURCHASING_LEVEL = "ccCreditService_purchasingLevel";

        // Keys for line item/product fields
        public const String SO_REQUEST_ITEM_UNIT_PRICE = "unitPrice";
        public const String SO_REQUEST_ITEM_ITEM = "item";
        public const String SO_REQUEST_ITEM_PRODUCT_SKU = "productSKU";
        public const String SO_REQUEST_ITEM_PRODUCT_CODE = "productCode";
        public const String SO_REQUEST_ITEM_PRODUCT_DESCRIPTION = "productDescription";
        public const String SO_REQUEST_ITEM_PRODUCT_NAME = "productName";
        public const String SO_REQUEST_ITEM_PRODUCT_RISK = "productRisk";
        public const String SO_REQUEST_ITEM_PRODUCT_TAX_AMOUNT = "taxAmount";
        public const String SO_REQUEST_ITEM_PRODUCT_CITY_OVERRIDE_AMOUNT = "cityOverrideAmount";
        public const String SO_REQUEST_ITEM_CITY_OVERRIDE_RATE = "cityOverrideRate";
        public const String SO_REQUEST_ITEM_COUNTY_OVERRIDE_AMOUNT = "countyOverrideAmount";
        public const String SO_REQUEST_ITEM_COUNTY_OVERRIDE_RATE = "countyOverrideRate";
        public const String SO_REQUEST_ITEM_DISTRICT_OVERRIDE_AMOUNT = "districtOverrideAmount";
        public const String SO_REQUEST_ITEM_DISTRICT_OVERRIDE_RATE = "districtOverrideRate";
        public const String SO_REQUEST_ITEM_STATE_OVERRIDE_AMOUNT = "stateOverrideAmount";
        public const String SO_REQUEST_ITEM_STATE_OVERRIDE_RATE = "stateOverrideRate";
        public const String SO_REQUEST_ITEM_COUNTRY_OVERRIDE_AMOUNT = "countryOverrideAmount";
        public const String SO_REQUEST_ITEM_COUNTRY_OVERRIDE_RATE = "countryOverrideRate";

        //Keys for Order acceptance items
        public const String SO_REQUEST_ORDER_ACCEPTANCE_CITY = "orderAcceptanceCity";
        public const String SO_REQUEST_ORDER_ACCEPTANCE_COUNTY = "orderAcceptanceCounty";
        public const String SO_REQUEST_ORDER_ACCEPTANCE_COUNTRY = "orderAcceptanceCountry";
        public const String SO_REQUEST_ORDER_ACCEPTANCE_STATE = "orderAcceptanceState";
        public const String SO_REQUEST_ORDER_ACCEPTANCE_POSTAL_CODE = "orderAcceptancePostalCode";

        public const String SO_REQUEST_ORDER_ORIGIN_CITY = "orderOriginCity";
        public const String SO_REQUEST_ORDER_ORIGIN_COUNTY = "orderOriginCounty";
        public const String SO_REQUEST_ORDER_ORIGIN_COUNTRY = "orderOriginCountry";
        public const String SO_REQUEST_ORDER_ORIGIN_STATE = "orderOriginState";
        public const String SO_REQUEST_ORDER_ORIGIN_POSTAL_CODE = "orderOriginPostalCode";

        //Keys for ship from
        public const String SO_REQUEST_ORDER_SHIP_FROM_CITY = "shipFromCity";
        public const String SO_REQUEST_ORDER_SHIP_FROM_COUNTY = "shipFromCounty";
        public const String SO_REQUEST_ORDER_SHIP_FROM_COUNTRY = "shipFromCountry";
        public const String SO_REQUEST_ORDER_SHIP_FROM_STATE = "shipFromState";
        public const String SO_REQUEST_ORDER_SHIP_FROM_POSTAL_CODE = "shipFromPostalCode";

        ///Keys for export item details
        public const String SO_REQUEST_ITEM_EXPORT = "export";
        public const String SO_REQUEST_ITEM_NO_EXPORT = "noExport";
        public const String SO_REQUEST_ITEM_NATIONAL_TAX = "nationalTax";
        public const String SO_REQUEST_ITEM_VAT_RATE = "vatRate";

        // Key for buyer registration list of items
        public const String SO_REQUEST_ITEM_BUYER_REGISTRATION = "buyerRegistration";
        public const String SO_REQUEST_ITEM_MIDDLEMAN_REGISTRATION = "middlemanRegistration";

        public const String SO_REQUEST_ITEM_SCORE_GIFT_CATEGORY = "giftCategory";
        public const String SO_REQUEST_ITEM_SCORE_TIME_CATEGORY = "timeCategory";
        public const String SO_REQUEST_ITEM_SCORE_HOST_HEDGE = "hostHedge";
        public const String SO_REQUEST_ITEM_SCORE_TIME_HEDGE = "timeHedge";
        public const String SO_REQUEST_ITEM_SCORE_VELOCITY_HEDGE = "velocityHedge";
        public const String SO_REQUEST_ITEM_SCORE_NONSENSICAL_HEDGE = "nonsensicalHedge";
        public const String SO_REQUEST_ITEM_SCORE_PHONE_HEDGE = "phoneHedge";
        public const String SO_REQUEST_ITEM_SCORE_OBSCENITIES_HEDGE = "obscenitiesHedge";
        public const String SO_REQUEST_ITEM_UNIT_OF_MEASURE = "unitOfMeasure";
        public const String SO_REQUEST_ITEM_TAX_RATE = "taxRate";
        public const String SO_REQUEST_ITEM_TOTAL_AMOUNT = "totalAmount";
        public const String SO_REQUEST_ITEM_DISCOUNT_AMOUNT = "discountAmount";
        public const String SO_REQUEST_ITEM_DISCOUNT_RATE = "discountRate";
        public const String SO_REQUEST_ITEM_COMMODITY_CODE = "commodityCode";
        public const String SO_REQUEST_ITEM_GROSS_NET_INDICATOR = "grossNetIndicator";
        public const String SO_REQUEST_ITEM_TAX_TYPE_APPLIED = "taxTypeApplied";
        public const String SO_REQUEST_ITEM_DISCOUNT_INDICATOR = "discountIndicator";
        public const String SO_REQUEST_ITEM_ALTERNATE_TAX_ID = "alternateTaxID";
        public const String SO_REQUEST_ITEM_ALTERNATE_TAX_AMOUNT = "alternateTaxAmount";
        public const String SO_REQUEST_ITEM_ALTERNATE_TAX_TYPE_APPLIED = "alternateTaxTypeApplied";
        public const String SO_REQUEST_ITEM_ALTERNATE_TAX_RATE = "alternateTaxRate";
        public const String SO_REQUEST_ITEM_ALTERNATE_TAX_TYPE_IDENTIFIER = "alternateTaxType";
        public const String SO_REQUEST_ITEM_LOCAL_TAX = "localTax";
        public const String SO_REQUEST_ITEM_ZERO_COST_TO_CUSTOMER_INDICATOR = "zeroCostToCustomerIndicator";
        public const String SO_REQUEST_ITEM_PASSENGER_FIRST_NAME = "passengerFirstName";
        public const String SO_REQUEST_ITEM_PASSENGER_LAST_NAME = "passengerLastName";
        public const String SO_REQUEST_ITEM_PASSENGER_ID = "passengerID";
        public const String SO_REQUEST_ITEM_PASSENGER_STATUS = "passengerStatus";
        public const String SO_REQUEST_ITEM_PASSENGER_TYPE = "passengerType";
        public const String SO_REQUEST_ITEM_PASSENGER_EMAIL = "passengerEmail";
        public const String SO_REQUEST_ITEM_PASSENGER_PHONE = "passengerPhone";
        public const String SO_REQUEST_ITEM_INVOICE_NUMBER = "invoiceNumber";
        public const String SO_REQUEST_ITEM_SELLER_REGISTRATION = "sellerRegistration";
        public const String SO_REQUEST_ITEM_POINT_OF_TITLE_TRANSFER = "pointOfTitleTransfer";


        // Keys for SCMP request
        public const String SCMP_REQUEST_ICS_APPLICATIONS = "ics_applications";
        public const String SCM_REQUEST_PAYPAL_TOKEN = "paypal_token";
        public const String SCMP_REQUEST_MERCHANT_REF_NUMBER = "merchant_ref_number";
        public const String SCMP_REQUEST_CUSTOMER_FIRST_NAME = "customer_firstname";
        public const String SCMP_REQUEST_CUSTOMER_LAST_NAME = "customer_lastname";
        public const String SCMP_REQUEST_CUSTOMER_EMAIL = "customer_email";
        public const String SCMP_REQUEST_CUSTOMER_PHONE = "customer_phone";
        public const String SCMP_REQUEST_BILL_ADDRESS1 = "bill_address1";
        public const String SCMP_REQUEST_BILL_ADDRESS2 = "bill_address2";
        public const String SCMP_REQUEST_BILL_CITY = "bill_city";
        public const String SCMP_REQUEST_BILL_STATE = "bill_state";
        public const String SCMP_REQUEST_BILL_ZIP = "bill_zip";
        public const String SCMP_REQUEST_BILL_COUNTRY = "bill_country";
        public const String SCMP_REQUEST_BILL_TO_IP_ADDRESS = "customer_ipaddress";
        public const String SCMP_REQUEST_SHIP_TO_FIRST_NAME = "ship_to_firstname";
        public const String SCMP_REQUEST_SHIP_TO_LAST_NAME = "ship_to_lastname";
        public const String SCMP_REQUEST_SHIP_TO_ADDRESS1 = "ship_to_address1";
        public const String SCMP_REQUEST_SHIP_TO_ADDRESS2 = "ship_to_address2";
        public const String SCMP_REQUEST_SHIP_TO_CITY = "ship_to_city";
        public const String SCMP_REQUEST_SHIP_TO_STATE = "ship_to_state";
        public const String SCMP_REQUEST_SHIP_TO_COUNTRY = "ship_to_country";
        public const String SCMP_REQUEST_SHIP_TO_ZIP = "ship_to_zip";
        public const String SCMP_REQUEST_CUSTOMER_CC_NUMBER = "customer_cc_number";
        public const String SCMP_REQUEST_CUSTOMER_CC_EXPIRATION_MONTH = "customer_cc_expmo";
        public const String SCMP_REQUEST_CUSTOMER_CC_EXPIRATION_YEAR = "customer_cc_expyr";
        public const String SCMP_REQUEST_CURRENCY = "currency";
        public const String SCMP_REQUEST_PAYPAL_TRANSACTION_ID = "paypal_transaction_id";
        public const String SCMP_REQUEST_ACCOUNT_ENCODER_ID = "account_encoder_id";
        public const String SCMP_REQUEST_BANK_TRANSIT_NUMBER = "ecp_rdfi";
        public const String SCMP_REQUEST_AGGREGATOR_ID = "aggregator_id";
        public const String SCMP_REQUEST_AGGREGATOR_NAME = "aggregator_name";
        public const String SCMP_REQUEST_ICS_AUTH = "ics_auth";
        public const String SCMP_REQUEST_ICS_CREDIT = "ics_credit";
        public const String SCMP_REQUEST_AP_AUTH_REQUEST_ID = "ap_auth_request_id";
        public const String SCMP_REQUEST_ICS_AP_AUTH_REVERSAL = "ics_ap_auth_reversal";
        public const String SCMP_REQUEST_ICS_AP_CAPTURE = "ics_ap_capture";
        public const String SCMP_REQUEST_AP_INITIATE_REQUEST_ID = "ap_initiate_request_id";
        public const String SCMP_REQUEST_ICS_AP_CHECK_STATUS = "ics_ap_check_status";
        public const String SCMP_REQUEST_ICS_AP_REFUND = "ics_ap_refund";
        public const String SCMP_REQUEST_AUTH_CODE = "auth_code";
        public const String SCMP_REQUEST_ICS_AUTO_AUTH_REVERSAL = "ics_auto_auth_reversal";
        public const String SCMP_REQUEST_AUTH_REQUEST_ID = "auth_request_id";
        public const String SCMP_REQUEST_ICS_AUTH_REVERSAL = "ics_auth_reversal";
        public const String SCMP_REQUEST_ICS_CAPTURE = "ics_bill";
        public const String SCMP_REQUEST_AUTH_REQUEST_TOKEN = "auth_request_token";
        public const String SCMP_REQUEST_AUTH_TRANS_REF_NO = "auth_trans_ref_no";
        public const String SCMP_REQUEST_AUTH_TYPE = "auth_type";
        public const String SCMP_REQUEST_BILL_PAYMENT = "bill_payment";
        public const String SCMP_REQUEST_BILL_REQUEST_ID = "bill_request_id";
        public const String SCMP_REQUEST_ICS_DCC_UPDATE = "ics_dcc_update";
        public const String SCMP_REQUEST_CHECKSUM_KEY = "checksum_key";
        public const String SCMP_REQUEST_ICS_PAYPAL_TRANSACTION_SEARCH = "ics_paypal_transaction_search";
        public const String SCMP_REQUEST_ICS_PAYPAL_AUTH_REVERSAL = "ics_paypal_auth_reversal";
        public const String SCMP_REQUEST_ICS_PAYPAL_EC_SET = "ics_paypal_ec_set";
        public const String SCMP_REQUEST_ICS_PAYPAL_UPDATE_AGREEMENT = "ics_paypal_update_agreement";
        public const String SCMP_REQUEST_ICS_PAYPAL_CREATE_AGREEMENT = "ics_paypal_create_agreement";
        public const String SCMP_REQUEST_ICS_PAYPAL_DO_CAPTURE = "ics_paypal_do_capture";
        public const String SCMP_REQUEST_ICS_PAYPAL_AUTH = "ics_paypal_authorization";
        public const String SCMP_REQUEST_ICS_PAYPAL_EC_DO_PAYMENT = "ics_paypal_ec_do_payment";
        public const String SCMP_REQUEST_ICS_PAYPAL_EC_ORDER_SETUP = "ics_paypal_ec_order_setup";
        public const String SCMP_REQUEST_ICS_PAYPAL_DO_REF_TRANSACTION = "ics_paypal_do_ref_transaction";
        public const String SCMP_REQUEST_ICS_PAYPAL_EC_GET_DETAILS = "ics_paypal_ec_get_details";
        public const String SCMP_REQUEST_ICS_PAYPAL_PRE_APPROVED_PAYMENT = "ics_paypal_preapproved_payment";
        public const String SCMP_REQUEST_ICS_PAYPAL_PRE_APPROVED_UPDATE = "ics_paypal_preapproved_update";
        public const String SCMP_REQUEST_ICS_PAYPAL_GET_TRANSACTION_DETAILS = "ics_paypal_get_txn_details";
        public const String SCMP_REQUEST_PURCHASING_LEVEL = "purchasing_level";


        public const String SCMP_REQUEST_DIRECT_DEBIT_MANDATE_AUTHENTICATION_DATE = "direct_debit_mandate_authentication_date";
        public const String SCMP_REQUEST_ICS_DIRECT_DEBIT_REFUND = "ics_direct_debit_refund";
        public const String SCMP_REQUEST_ICS_DIRECT_DEBIT = "SCMP_REQUEST_ICS_DIRECT_DEBIT";
        public const String SCMP_REQUEST_DIRECT_DEBIT_RECURRING_TYPE = "direct_debit_recurring_type";
        public const String SCMP_REQUEST_DIRECT_DEBIT_TYPE = "direct_debit_type";
        public const String SCMP_REQUEST_ECOMMERCE_INDICATOR = "e_commerce_indicator";

        public const String SCMP_REQUEST_ICS_ECP_CREDIT = "ics_ecp_credit";
        public const String SCMP_REQUEST_ICS_ECP_AUTHENTICATE = "ics_ecp_authenticate";
        public const String SCMP_REQUEST_ICS_ECP_DEBIT = "ics_ecp_debit";
        public const String SCMP_REQUEST_ICS_PIN_DEBIT_CREDIT = "ics_pin_debit_credit";
        public const String SCMP_REQUEST_ICS_PIN_DEBIT = "ics_pin_debit";
        public const String SCMP_REQUEST_ICS_PIN_LESS_DEBIT = "ics_pinless_debit";
        public const String SCMP_REQUEST_ICS_FRAUD_UPDATE = "ics_ifs_update";
        public const String SCMP_REQUEST_ICS_RISK_UPDATE = "ics_risk_update";
        public const String SCMP_REQUEST_DEBIT_REQUEST_ID = "ecp_debit_request_id";
        public const String SCMP_REQUEST_ECP_PAYMENT_KEY = "ecp_payment_key";
        public const String SCMP_REQUEST_ECP_REF_NO = "ecp_ref_no";
        public const String SCMP_REQUEST_ECP_SETTLEMENT_METHOD = "ecp_settlement_method";
        public const String SCMP_REQUEST_GRAND_TOTAL_AMOUNT = "grand_total_amount";
        public const String SCMP_REQUEST_INDUSTRY_DATA_TYPE = "industry_datatype";
        public const String SCMP_REQUEST_MANDATE_ID = "mandate_id";
        public const String SCMP_REQUEST_MARKING_NOTES = "marking_notes";
        public const String SCMP_REQUEST_MARKING_REASON = "marking_reason";
        public const String SCMP_REQUEST_MERCHANT_RECEIPT_NUMBER = "merchant_receipt_number";

        public const String SCMP_REQUEST_NETWORK_ORDER = "network_order";
        public const String SCMP_REQUEST_PARTIAL_PAYMENT_ID = "partial_payment_id";

        public const String SCMP_REQUEST_PAYPAL_AUTHORIZATION_ID = "paypal_authorization_id";
        public const String SCMP_REQUEST_PAYPAL_AUTHORIZATION_REQUEST_ID = "paypal_authorization_request_id";
        public const String SCMP_REQUEST_PAYPAL_AUTHORIZATION_REQUEST_TOKEN = "paypal_authorization_request_token";
        public const String SCMP_REQUEST_PAYPAL_BILLING_AGREEMENT_CUSTOM = "paypal_billing_agreement_custom";
        public const String SCMP_REQUEST_PAYPAL_BILLING_AGREEMENT_ID = "paypal_billing_agreement_id";
        public const String SCMP_REQUEST_PAYPAL_CUSTOMER_EMAIL = "paypal_customer_email";
        public const String SCMP_REQUEST_PAYPAL_DESC = "paypal_desc";
        public const String SCMP_REQUEST_PAYPAL_EC_DO_PAYMENT_REQUEST_ID = "paypal_ec_do_payment_request_id";
        public const String SCMP_REQUEST_PAYPAL_EC_DO_PAYMENT_REQUEST_TOKEN = "paypal_ec_do_payment_request_token";
        public const String SCMP_REQUEST_PAYPAL_EC_ORDER_SETUP_REQUEST_ID = "paypal_ec_order_setup_request_id";
        public const String SCMP_REQUEST_PAYPAL_EC_ORDER_SETUP_REQUEST_TOKEN = "paypal_ec_order_setup_request_token";

        public const String SCMP_REQUEST_PAYPAL_EC_SET_REQUEST_ID = "paypal_ec_set_request_id";
        public const String SCMP_REQUEST_PAYPAL_EC_SET_REQUEST_TOKEN = "paypal_ec_set_request_token";

        public const String SCMP_REQUEST_PAYPAL_INVOICE_NUMBER = "paypal_invoice_number";
        public const String SCMP_REQUEST_PAYPAL_MP_ID = "paypal_mp_id";
        public const String SCMP_REQUEST_PAYPAL_PAYER_ID = "paypal_payer_id";

        public const String SCMP_REQUEST_PAYPAL_PAYMENT_TYPE = "paypal_payer_type";

        public const String SCMP_REQUEST_PAYPAL_REQ_CONFIRM_SHIPPING = "paypal_reqconfirmshipping";
        // Fields for ICSOffer item
        public const String SCMP_REQUEST_PRODUCT_NAME = "product_name";
        public const String SCMP_REQUEST_ITEM_PRODUCT_DESCRIPTION = "product_description";
        public const String SCMP_REQUEST_PRODUCT_CODE = "product_code";
        public const String SCMP_REQUEST_MERCHANT_PRODUCT_SKU = "merchant_product_sku";
        public const String SCMP_REQUEST_AMOUNT = "amount";
        public const String SCMP_REQUEST_QUANTITY = "quantity";
        public const String SCMP_REQUEST_PRODUCT_RISK = "product_risk";
        public const String SCMP_REQUEST_TAX_AMOUNT = "tax_amount";
        public const String SCMP_REQUEST_CITY_OVERRIDE_AMOUNT = "city_override_amount";
        public const String SCMP_REQUEST_CITY_OVERRIDE_RATE = "city_override_rate";
        public const String SCMP_REQUEST_COUNTY_OVERRIDE_AMOUNT = "county_override_amount";
        public const String SCMP_REQUEST_COUNTY_OVERRIDE_RATE = "county_override_rate";
        public const String SCMP_REQUEST_DISTRICT_OVERRIDE_AMOUNT = "district_override_amount";
        public const String SCMP_REQUEST_DISTRICT_OVERRIDE_RATE = "district_override_rate";
        public const String SCMP_REQUEST_STATE_OVERRIDE_AMOUNT = "state_override_amount";
        public const String SCMP_REQUEST_STATE_OVERRIDE_RATE = "state_override_rate";
        public const String SCMP_REQUEST_COUNTRY_OVERRIDE_AMOUNT = "country_override_amount";
        public const String SCMP_REQUEST_COUNTRY_OVERRIDE_RATE = "country_override_rate";

        //Fields for Acceptance Order items
        public const String SCMP_REQUEST_ORDER_ACCEPTANCE_CITY = "order_acceptance_city";
        public const String SCMP_REQUEST_ORDER_ACCEPTANCE_COUNTY = "order_acceptance_county";
        public const String SCMP_REQUEST_ORDER_ACCEPTANCE_COUNTRY = "order_acceptance_country";
        public const String SCMP_REQUEST_ORDER_ACCEPTANCE_STATE = "order_acceptance_state";
        public const String SCMP_REQUEST_ORDER_ACCEPTANCE_ZIP = "order_acceptance_zip";

        //Keys for order origin items
        public const String SCMP_REQUEST_ORDER_ORIGIN_CITY = "order_origin_city";
        public const String SCMP_REQUEST_ORDER_ORIGIN_COUNTY = "order_origin_county";
        public const String SCMP_REQUEST_ORDER_ORIGIN_COUNTRY = "order_origin_country";
        public const String SCMP_REQUEST_ORDER_ORIGIN_STATE = "order_origin_state";
        public const String SCMP_REQUEST_ORDER_ORIGIN_ZIP = "order_origin_zip";

        //Keys for ship from items
        public const String SCMP_REQUEST_SHIP_FROM_CITY = "ship_from_city";
        public const String SCMP_REQUEST_SHIP_FROM_COUNTY = "ship_from_county";
        public const String SCMP_REQUEST_SHIP_FROM_COUNTRY = "ship_from_country";
        public const String SCMP_REQUEST_SHIP_FROM_STATE = "ship_from_state";
        public const String SCMP_REQUEST_SHIP_FROM_ZIP = "ship_from_zip";

        // Keys for export items
        public const String SCMP_REQUEST_ITEM_EXPORT = "export";
        public const String SCMP_REQUEST_ITEM_NO_EXPORT = "no_export";
        public const String SCMP_REQUEST_ITEM_NATIONAL_TAX = "national_tax";
        public const String SCMP_REQUEST_ITEM_VAT_RATE = "vat_rate";

        //Key for buyer registration items
        public const String SCMP_REQUEST_ITEM_BUYER_REGISTRATION = "buyer_registration";
        public const String SCMP_REQUEST_ITEM_MIDDLEMAN_REGISTRATION = "middleman_registration";

        public const String SCMP_REQUEST_ITEM_SCORE_CATEGORY_GIFT = "score_category_gift";
        public const String SCMP_REQUEST_ITEM_SCORE_CATEGORY_TIME = "score_category_time";
        public const String SCMP_REQUEST_ITEM_SCORE_HOST_HEDGE = "score_host_hedge";
        public const String SCMP_REQUEST_ITEM_SCORE_TIME_HEDGE = "score_time_hedge";
        public const String SCMP_REQUEST_ITEM_SCORE_VELOCITY_HEDGE = "score_velocity_hedge";
        public const String SCMP_REQUEST_ITEM_SCORE_NONSENSICAL_HEDGE = "score_nonsensical_hedge";
        public const String SCMP_REQUEST_ITEM_SCORE_PHONE_HEDGE = "score_phone_hedge";
        public const String SCMP_REQUEST_ITEM_SCORE_OBSCENITIES_HEDGE = "score_obscenities_hedge";
        public const String SCMP_REQUEST_ITEM_UNIT_OF_MEASURE = "unit_of_measure";
        public const String SCMP_REQUEST_ITEM_TAX_RATE = "tax_rate";
        public const String SCMP_REQUEST_ITEM_TOTAL_AMOUNT = "total_amount";
        public const String SCMP_REQUEST_ITEM_DISCOUNT_AMOUNT = "discount_amount";
        public const String SCMP_REQUEST_ITEM_DISCOUNT_RATE = "discount_rate";
        public const String SCMP_REQUEST_ITEM_COMMODITY_CODE = "commodity_code";
        public const String SCMP_REQUEST_ITEM_GROSS_NET_INDICATOR = "gross_net_indicator";
        public const String SCMP_REQUEST_ITEM_TAX_TYPE_APPLIED = "tax_type_applied";
        public const String SCMP_REQUEST_ITEM_DISCOUNT_INDICATOR = "discount_indicator";
        public const String SCMP_REQUEST_ITEM_ALTERNATE_TAX_ID = "alternate_tax_id";
        public const String SCMP_REQUEST_ITEM_ALTERNATE_TAX_AMOUNT = "alternate_tax_amount";
        public const String SCMP_REQUEST_ITEM_ALTERNATE_TAX_TYPE_APPLIED = "alternate_tax_type_applied";
        public const String SCMP_REQUEST_ITEM_ALTERNATE_TAX_RATE = "alternate_tax_rate";
        public const String SCMP_REQUEST_ITEM_ALTERNATE_TAX_TYPE_IDENTIFIER = "alternate_tax_type_identifier";
        public const String SCMP_REQUEST_ITEM_LOCAL_TAX = "local_tax";
        public const String SCMP_REQUEST_ITEM_ZERO_COST_TO_CUSTOMER_INDICATOR = "zero_cost_to_customer_indicator";
        public const String SCMP_REQUEST_ITEM_PASSENGER_FIRST_NAME = "passenger_firstname";
        public const String SCMP_REQUEST_ITEM_PASSENGER_LAST_NAME = "passenger_lastname";
        public const String SCMP_REQUEST_ITEM_PASSENGER_ID = "passenger_id";
        public const String SCMP_REQUEST_ITEM_PASSENGER_STATUS = "passenger_status";
        public const String SCMP_REQUEST_ITEM_PASSENGER_TYPE = "passenger_type";
        public const String SCMP_REQUEST_ITEM_PASSENGER_EMAIL = "passenger_email";
        public const String SCMP_REQUEST_ITEM_PASSENGER_PHONE = "passenger_phone";
        public const String SCMP_REQUEST_ITEM_INVOICE_NUMBER = "invoice_number";
        public const String SCMP_REQUEST_ITEM_SELLER_REGISTRATION = "seller_registration_";
        public const String SCMP_REQUEST_ITEM_POINT_OF_TITLE_TRANSFER = "point_of_title_transfer";


        //Keys for Simple Order Response
        public const String SO_RESPONSE_AVS_CODE = "ccAuthReply_avsCode";
        public const String SO_RESPONSE_REQUEST_TOKEN = "requestToken";
        public const String SO_RESPONSE_TERMINAL_ID = "pos_terminalID";
        public const String SO_RESPONSE_CARD_REGULATED = "ccAuthReply_cardRegulated";
        public const String SO_RESPONSE_CARD_HEALTHCARE = "ccAuthReply_cardHealthcare";
        public const String SO_RESPONSE_PAYMENT_NETWORK_TRANSACTION_ID = "ccAuthReply_paymentNetworkTransactionID";
        public const String SO_RESPONSE_CURRENCY = "purchaseTotals_currency";
        public const String SO_RESPONSE_REQUEST_ID = "requestID";
        public const String SO_RESPONSE_CARD_LEVEL3_ELIGIBLE = "ccAuthReply_cardLevel3Eligible";
        public const String SO_RESPONSE_AUTH_AMOUNT = "ccAuthReply_amount";
        public const String SO_RESPONSE_CARD_COMMERCIAL = "ccAuthReply_cardCommercial";
        public const String SO_RESPONSE_DECISION_EARLY_REPLY_CODE = "decisionEarlyReply_rcode";
        public const String SO_RESPONSE_RECONCILIATION_ID = "ccAuthReply_reconciliationID";
        public const String SO_RESPONSE_AUTH_CODE = "ccAuthReply_authorizationCode";
        public const String SO_RESPONSE_CARD_SIGNATURE_DEBIT = "ccAuthReply_cardSignatureDebit";
        public const String SO_RESPONSE_CARD_TYPE = "card_cardType";
        public const String SO_RESPONSE_CARD_PIN_LESS_DEBIT = "ccAuthReply_cardPINlessDebit";
        public const String SO_RESPONSE_REASON_CODE = "ccAuthReply_reasonCode";
        public const String SO_RESPONSE_CARD_ISSUER_COUNTRY = "ccAuthReply_cardIssuerCountry";
        public const String SO_RESPONSE_CARD_PREPAID = "ccAuthReply_cardPrepaid";
        public const String SO_RESPONSE_AFFLUENCE_INDICATOR = "ccAuthReply_affluenceIndicator";
        public const String SO_RESPONSE_AVS_RAW_CODE = "ccAuthReply_avsCodeRaw";
        public const String SO_RESPONSE_CARD_PAYROLL = "ccAuthReply_cardPayroll";
        public const String SO_RESPONSE_MERCHANT_REFERENCE_CODE = "merchantReferenceCode";
        public const String SO_RESPONSE_AUTHORIZE_DATE_TIME = "ccAuthReply_authorizedDateTime";

        //Keys for SCMP Response
        public const String SCMP_RESPONSE_AVS_CODE = "auth_auth_avs";
        public const String SCMP_RESPONSE_REQUEST_TOKEN = "request_token";
        public const String SCMP_RESPONSE_TERMINAL_ID = "terminal_id";
        public const String SCMP_RESPONSE_CARD_REGULATED = "auth_card_regulated";
        public const String SCMP_RESPONSE_CARD_HEALTHCARE = "auth_card_healthcare";
        public const String SCMP_RESPONSE_PAYMENT_NETWORK_TRANSACTION_ID = "auth_payment_network_transaction_id";
        public const String SCMP_RESPONSE_CURRENCY = "currency";
        public const String SCMP_RESPONSE_REQUEST_ID = "request_id";
        public const String SCMP_RESPONSE_CARD_LEVEL3_ELIGIBLE = "auth_card_level_3_eligible";
        public const String SCMP_RESPONSE_AUTH_AUTH_AMOUNT = "auth_auth_amount";
        public const String SCMP_RESPONSE_CARD_COMMERCIAL = "auth_card_commercial";
        public const String SCMP_RESPONSE_DECISION_EARLY_REPLY_CODE = "decision_early_rcode";
        public const String SCMP_RESPONSE_RECONCILIATION_ID = "auth_trans_ref_no";
        public const String SCMP_RESPONSE_AUTH_CODE = "auth_auth_code";
        public const String SCMP_RESPONSE_CARD_SIGNATURE_DEBIT = "auth_card_signature_debit";
        public const String SCMP_RESPONSE_CARD_TYPE = "card_type";
        public const String SCMP_RESPONSE_CARD_PIN_LESS_DEBIT = "auth_card_pinless_debit";
        public const String SCMP_RESPONSE_REASON_CODE = "auth_auth_response";
        public const String SCMP_RESPONSE_CARD_ISSUER_COUNTRY = "auth_card_issuer_country";
        public const String SCMP_RESPONSE_CARD_PREPAID = "auth_card_prepaid";
        public const String SCMP_RESPONSE_AFFLUENCE_INDICATOR = "auth_affluence_indicator";
        public const String SCMP_RESPONSE_AVS_RAW_CODE = "auth_avs_raw";
        public const String SCMP_RESPONSE_CARD_PAYROLL = "auth_card_payroll";
        public const String SCMP_RESPONSE_MERCHANT_REFERENCE_CODE = "merchant_ref_number";
        public const String SCMP_RESPONSE_AUTHORIZE_DATE_TIME = "auth_auth_time";
        public const String SCMP_RESPONSE_ICS_REPLY_CODE = "ics_rcode";

        public const String SCMP_REQUEST_ICS_PA_ENROLL = "ics_pa_enroll";
        public const String SCMP_REQUEST_ICS_PA_VALIDATE = "ics_pa_validate";
        public const String SCMP_REQUEST_ICS_TAX = "ics_tax";
        public const string SCMP_REQUEST_ICS_SCORE = "ics_score";
        public const string SCMP_REQUEST_ICS_DAV = "ics_dav";
        public const string SCMP_REQUEST_ICS_EXPORT = "ics_export";
        public const string SCMP_REQUEST_ICS_FXRATES = "ics_fxrates";
        public const string SCMP_REQUEST_ICS_BANK_TRANSFER = "ics_bank_transfer";
        public const string SCMP_REQUEST_ICS_BANK_TRANSFER_REAL_TIME = "ics_bank_transfer_real_time";
        public const string SCMP_REQUEST_ICS_BANK_TRANSFER_REFUND = "ics_bank_transfer_refund";

        public const string SCMP_REQUEST_ICS_DIRECT_DEBIT_VALIDATE = "ics_direct_debit_validate";
        public const string SCMP_REQUEST_ICS_DIRECT_DEBIT_MANDATE = "ics_direct_debit_mandate";
        public const string SCMP_REQUEST_ICS_PAY_SUBSCRIPTION_CREATE = "ics_pay_subscription_create";
        public const string SCMP_REQUEST_ICS_PAY_SUBSCRIPTION_UPDATE = "ics_pay_subscription_update";
        public const string SCMP_REQUEST_ICS_PAY_SUBSCRIPTION_EVENT_UPDATE = "ics_pay_subscription_event_update";
        public const string SCMP_REQUEST_ICS_PAY_SUBSCRIPTION_RETRIEVE = "ics_pay_subscription_retrieve";
        public const string SCMP_REQUEST_ICS_PAY_SUBSCRIPTION_DELETE = "ics_pay_subscription_delete";
        public const string SCMP_REQUEST_ICS_PAYPAL_PAYMENT = "ics_paypal_payment";
        public const string SCMP_REQUEST_ICS_PAYPAL_CREDIT = "ics_paypal_credit";
        public const string SCMP_REQUEST_ICS_VOID = "ics_void";
        public const string SCMP_REQUEST_ICS_PINLESS_DEBIT = "ics_pinless_debit";
        public const string SCMP_REQUEST_ICS_PINLESS_DEBIT_VALIDATE = "ics_pinless_debit_validate";
        public const string SCMP_REQUEST_ICS_PINLESS_DEBIT_REVERSAL = "ics_pinless_debit_reversal";
        public const string SCMP_REQUEST_ICS_PAYPAL_BUTTON_CREATE = "ics_paypal_button_create";

        public const string SCMP_REQUEST_ICS_IFS_UPDATE = "ics_ifs_update";
        public const string SCMP_REQUEST_ICS_CM_ACTION = "ics_cm_action";
        public const string SCMP_REQUEST_ICS_PAYPAL_AUTHORIZATION = "ics_paypal_authorization";
        public const string SCMP_REQUEST_ICS_PAYPAL_REFUND = "ics_paypal_refund";

        public const string SCMP_REQUEST_ICS_CHINA_PAYMENT = "ics_china_payment";
        public const string SCMP_REQUEST_ICS_CHINA_REFUND = "ics_china_refund";
        public const string SCMP_REQUEST_ICS_BOLETO_PAYMENT = "ics_boleto_payment";
        public const string SCMP_REQUEST_ICS_PIN_DEBIT_PURCHASE = "ics_pin_debit_purchase";
        public const string SCMP_REQUEST_ICS_AP_CHECKOUT_DETAILS = "ics_ap_checkout_details";
        public const string SCMP_REQUEST_ICS_AP_TRANSACTION_DETAILS = "ics_ap_transaction_details";
        public const string SCMP_REQUEST_ICS_AP_CONFIRM_PURCHASE = "ics_ap_confirm_purchase";
        public const string SCMP_REQUEST_ICS_PAYPAL_GET_TXN_DETAILS = "ics_paypal_get_txn_details";
        public const string SCMP_REQUEST_ICS_PIN_DEBIT_REVERSAL = "ics_pin_debit_reversal";
        public const string SCMP_REQUEST_ICS_AP_INITIATE = "ics_ap_initiate";
        public const string SCMP_REQUEST_ICS_AP_AUTH = "ics_ap_auth";

        public const string SO_REQUEST_PAYPAL_AUTHORIZATION_SERVICE_PAYPAL_EC_ORDER_SETUP_REQUEST_ID = "";
        public const string SO_REQUEST_PAYPAL_UPDATE_AGREEMENT_SERVICE_PAYPAL_EC_SET_REQUEST_ID = "";

        private static readonly Dictionary<string, string> icsApplicationsLookupTable = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> requestConversionTable = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> responseConversionTable = new Dictionary<string, string>();
        #endregion




        static Util()
        {
            
            icsApplicationsLookupTable = LoadPropertiesToDictionary("ics_applications.properties");
            requestConversionTable = LoadPropertiesToDictionary("scmp_so_mapping.properties");
            Util.FindDuplicateKeys("so_scmp_response_mapping.properties");
            responseConversionTable = LoadPropertiesToDictionary("so_scmp_response_mapping.properties");
            

        }

        public static void FindDuplicateKeys(string filePath)
        {
            var keyValuePairs = new Dictionary<string, string>();
            var duplicateKeys = new List<string>();
            try
            {
                foreach (var line in File.ReadLines(filePath))
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    {
                        continue;
                    }

                    var keyValue = line.Split(new[] { '=' }, 2);
                    if (keyValue.Length != 2)
                    {
                        continue;
                    }

                    var key = keyValue[0].Trim();
                    var value = keyValue[1].Trim();

                    if (keyValuePairs.ContainsKey(key))
                    {
                        duplicateKeys.Add(key);
                    }
                    else
                    {
                        keyValuePairs[key] = value;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Reading file {filePath} threw an exception: " + ex.Message);
                throw ex;
            }

            if (duplicateKeys.Count > 0)
            {
                
                foreach (var key in duplicateKeys)
                {
                    Console.WriteLine(key);
                }
            }
            else
            {
                Console.WriteLine("No duplicate keys found.");
            }
        }

        private static Dictionary<string, string> LoadPropertiesToDictionary(string PATH_TO_FILE)
        {
            var dictionaryResult = new Dictionary<string, string>();
            try
            {
            foreach (var line in File.ReadAllLines(PATH_TO_FILE))
            {
                if ((!string.IsNullOrEmpty(line)) &&
                    (!line.StartsWith(";")) &&
                    (!line.StartsWith("#")) &&
                    (!line.StartsWith("'")) &&
                    (line.Contains('=')))
                {
                    dictionaryResult.Add(line.Split('=')[0], string.Join("=", line.Split('=').Skip(1).ToArray()));
                }
            }
            } catch (Exception ex)
            {
                Console.WriteLine($"Reading file {PATH_TO_FILE} threw an exception: " + ex.Message);
                throw ex;
            }


            return dictionaryResult;
            
        }

        public static ICSReply ConvertNVPResponseToICSReply(KeyValuePair<string, string> nvpResponse)
        {
            ICSReply reply = null;

            if (nvpResponse.Equals(null))
            {
                return null;
            }

            return null;
        }

        public static Hashtable convertICSRequestToNVPRequest(ICSRequest icsClientRequest)
        {
            if (icsClientRequest == null)
            {
                Console.WriteLine("Cannot convert null ICSRequest");
                return null;
            }

            var nvpRequest = new Hashtable();
            string icsApplications = icsClientRequest[SCMP_REQUEST_ICS_APPLICATIONS];
            List<String> icsApplicationsList = new List<String>();

            if (!String.IsNullOrEmpty(icsApplications))
            {
                if (icsApplications.Contains(","))
                {
                    icsApplicationsList.AddRange(icsApplications.Split(','));

                    foreach (string icsApplication in icsApplicationsList)
                    {
                        if (icsApplicationsLookupTable.ContainsKey(icsApplication))
                        {
                            string soRequestKey = icsApplicationsLookupTable[icsApplication];
                            if (!String.IsNullOrEmpty(icsClientRequest[soRequestKey]))
                            {
                                nvpRequest.Add(soRequestKey, "true");
                            }
                            else
                            {
                                Console.WriteLine("ics_applications: " + icsApplication + " has no mapping. Check the ics_applications.properties file");
                            }
                        }
                    }
                }
                else
                {
                    string icsApplication = icsApplicationsLookupTable[icsApplications.Trim()];
                    if (!String.IsNullOrEmpty(icsApplication))
                    {
                        nvpRequest.Add(icsApplication, "true");
                        icsApplicationsList.Add(icsApplications.Trim());
                    }
                    else
                    {
                        Console.WriteLine("ics_applications: " + icsApplication + " has no mapping. Check the ics_applications.properties file");
                    }
                }
            } else
            {
                Console.WriteLine(SCMP_REQUEST_ICS_APPLICATIONS + "=null");
            }

            //Need to map offer items
            mapSCMPOffersToSimpleOrderItem(icsClientRequest, nvpRequest);

            //Map Rest of the fields
            foreach (NameValuePair nvp in icsClientRequest)
            {
                string nvpSOKey = "";
                Console.WriteLine(nvp.Name);
                string scmpValue = nvp.Value;
                if (nvp.Name.StartsWith("offer"))
                {
                    continue;
                } else if (requestConversionTable.ContainsKey(nvp.Name))
                {
                    nvpSOKey = requestConversionTable[nvp.Name];
                } 
                if (String.IsNullOrEmpty(nvp.Value))
                {
                    Console.WriteLine("SCMP Key=" + nvp.Name + " has null value");
                }
                else if (String.IsNullOrEmpty(nvpSOKey))
                {
                    //May need to do some mapping
                    MapNonDistinctSCMPFields(icsClientRequest, nvp.Name, icsApplicationsList, nvpRequest, scmpValue);
                }
                else
                {
                    if (nvpRequest.ContainsKey(nvpSOKey))
                    {
                        continue;
                    }
                    nvpRequest.Add(nvpSOKey, scmpValue);
                }
            }

            

            return nvpRequest;
        }

        public static List<KeyValuePair<string, string>> GetOffers(ICSRequest icsClientRequest)
        {
            var offers = new List<KeyValuePair<string, string>>();

            foreach (NameValuePair nvp in icsClientRequest)
            {
                if (nvp.Name.StartsWith("offer"))
                {
                    offers.Add(new KeyValuePair<string, string>(nvp.Name, icsClientRequest[nvp.Name]));
                }
            }

            return offers;
        }

        public static void mapSCMPOffersToSimpleOrderItem(ICSRequest icsClientRequest, Hashtable nvpRequest)
        {
            var offers = GetOffers(icsClientRequest);
            var properties = new Dictionary<string, string>
    {
        { SCMP_REQUEST_AMOUNT, SO_REQUEST_ITEM_UNIT_PRICE },
        { SCMP_REQUEST_QUANTITY, SCMP_REQUEST_QUANTITY },
        { SCMP_REQUEST_PRODUCT_NAME, SO_REQUEST_ITEM_PRODUCT_NAME },
        { SCMP_REQUEST_ITEM_PRODUCT_DESCRIPTION, SO_REQUEST_ITEM_PRODUCT_DESCRIPTION },
        { SCMP_REQUEST_PRODUCT_CODE, SO_REQUEST_ITEM_PRODUCT_CODE },
        { SCMP_REQUEST_MERCHANT_PRODUCT_SKU, SO_REQUEST_ITEM_PRODUCT_SKU },
        { SCMP_REQUEST_PRODUCT_RISK, SO_REQUEST_ITEM_PRODUCT_RISK },
        { SCMP_REQUEST_TAX_AMOUNT, SO_REQUEST_ITEM_PRODUCT_TAX_AMOUNT },
        { SCMP_REQUEST_CITY_OVERRIDE_AMOUNT, SO_REQUEST_ITEM_PRODUCT_CITY_OVERRIDE_AMOUNT },
        { SCMP_REQUEST_CITY_OVERRIDE_RATE, SO_REQUEST_ITEM_CITY_OVERRIDE_RATE },
        { SCMP_REQUEST_COUNTY_OVERRIDE_AMOUNT, SO_REQUEST_ITEM_COUNTY_OVERRIDE_AMOUNT },
        { SCMP_REQUEST_COUNTY_OVERRIDE_RATE, SO_REQUEST_ITEM_COUNTY_OVERRIDE_RATE },
        { SCMP_REQUEST_DISTRICT_OVERRIDE_AMOUNT, SO_REQUEST_ITEM_DISTRICT_OVERRIDE_AMOUNT },
        { SCMP_REQUEST_DISTRICT_OVERRIDE_RATE, SO_REQUEST_ITEM_DISTRICT_OVERRIDE_RATE },
        { SCMP_REQUEST_STATE_OVERRIDE_AMOUNT, SO_REQUEST_ITEM_STATE_OVERRIDE_AMOUNT },
        { SCMP_REQUEST_STATE_OVERRIDE_RATE, SO_REQUEST_ITEM_STATE_OVERRIDE_RATE },
        { SCMP_REQUEST_COUNTRY_OVERRIDE_AMOUNT, SO_REQUEST_ITEM_COUNTRY_OVERRIDE_AMOUNT },
        { SCMP_REQUEST_COUNTRY_OVERRIDE_RATE, SO_REQUEST_ITEM_COUNTRY_OVERRIDE_RATE },
        { SCMP_REQUEST_ORDER_ACCEPTANCE_CITY, SO_REQUEST_ORDER_ACCEPTANCE_CITY },
        { SCMP_REQUEST_ORDER_ACCEPTANCE_COUNTY, SO_REQUEST_ORDER_ACCEPTANCE_COUNTY },
        { SCMP_REQUEST_ORDER_ACCEPTANCE_COUNTRY, SO_REQUEST_ORDER_ACCEPTANCE_COUNTRY },
        { SCMP_REQUEST_ORDER_ACCEPTANCE_STATE, SO_REQUEST_ORDER_ACCEPTANCE_STATE },
        { SCMP_REQUEST_ORDER_ACCEPTANCE_ZIP, SO_REQUEST_ORDER_ACCEPTANCE_POSTAL_CODE },
        { SCMP_REQUEST_ORDER_ORIGIN_CITY, SO_REQUEST_ORDER_ORIGIN_CITY },
        { SCMP_REQUEST_ORDER_ORIGIN_COUNTY, SO_REQUEST_ORDER_ORIGIN_COUNTY },
        { SCMP_REQUEST_ORDER_ORIGIN_COUNTRY, SO_REQUEST_ORDER_ORIGIN_COUNTRY },
        { SCMP_REQUEST_ORDER_ORIGIN_STATE, SO_REQUEST_ORDER_ORIGIN_STATE },
        { SCMP_REQUEST_ORDER_ORIGIN_ZIP, SO_REQUEST_ORDER_ORIGIN_POSTAL_CODE },
        { SCMP_REQUEST_SHIP_FROM_CITY, SO_REQUEST_ORDER_SHIP_FROM_CITY },
        { SCMP_REQUEST_SHIP_FROM_COUNTY, SO_REQUEST_ORDER_SHIP_FROM_COUNTY },
        { SCMP_REQUEST_SHIP_FROM_COUNTRY, SO_REQUEST_ORDER_SHIP_FROM_COUNTRY },
        { SCMP_REQUEST_SHIP_FROM_STATE, SO_REQUEST_ORDER_SHIP_FROM_STATE },
        { SCMP_REQUEST_SHIP_FROM_ZIP, SO_REQUEST_ORDER_SHIP_FROM_POSTAL_CODE },
        { SCMP_REQUEST_ITEM_EXPORT, SO_REQUEST_ITEM_EXPORT },
        { SCMP_REQUEST_ITEM_NO_EXPORT, SO_REQUEST_ITEM_NO_EXPORT },
        { SCMP_REQUEST_ITEM_NATIONAL_TAX, SO_REQUEST_ITEM_NATIONAL_TAX },
        { SCMP_REQUEST_ITEM_VAT_RATE, SO_REQUEST_ITEM_VAT_RATE },
        { SCMP_REQUEST_ITEM_BUYER_REGISTRATION, SO_REQUEST_ITEM_BUYER_REGISTRATION },
        { SCMP_REQUEST_ITEM_MIDDLEMAN_REGISTRATION, SO_REQUEST_ITEM_MIDDLEMAN_REGISTRATION },
        { SCMP_REQUEST_ITEM_SCORE_CATEGORY_GIFT, SO_REQUEST_ITEM_SCORE_GIFT_CATEGORY },
        { SCMP_REQUEST_ITEM_SCORE_CATEGORY_TIME, SO_REQUEST_ITEM_SCORE_TIME_CATEGORY },
        { SCMP_REQUEST_ITEM_SCORE_HOST_HEDGE, SO_REQUEST_ITEM_SCORE_HOST_HEDGE },
        { SCMP_REQUEST_ITEM_SCORE_TIME_HEDGE, SO_REQUEST_ITEM_SCORE_TIME_HEDGE },
        { SCMP_REQUEST_ITEM_SCORE_VELOCITY_HEDGE, SO_REQUEST_ITEM_SCORE_VELOCITY_HEDGE },
        { SCMP_REQUEST_ITEM_SCORE_NONSENSICAL_HEDGE, SO_REQUEST_ITEM_SCORE_NONSENSICAL_HEDGE },
        { SCMP_REQUEST_ITEM_SCORE_PHONE_HEDGE, SO_REQUEST_ITEM_SCORE_PHONE_HEDGE },
        { SCMP_REQUEST_ITEM_SCORE_OBSCENITIES_HEDGE, SO_REQUEST_ITEM_SCORE_OBSCENITIES_HEDGE },
        { SCMP_REQUEST_ITEM_UNIT_OF_MEASURE, SO_REQUEST_ITEM_UNIT_OF_MEASURE },
        { SCMP_REQUEST_ITEM_TAX_RATE, SO_REQUEST_ITEM_TAX_RATE },
        { SCMP_REQUEST_ITEM_TOTAL_AMOUNT, SO_REQUEST_ITEM_TOTAL_AMOUNT },
        { SCMP_REQUEST_ITEM_DISCOUNT_AMOUNT, SO_REQUEST_ITEM_DISCOUNT_AMOUNT },
        { SCMP_REQUEST_ITEM_DISCOUNT_RATE, SO_REQUEST_ITEM_DISCOUNT_RATE },
        { SCMP_REQUEST_ITEM_COMMODITY_CODE, SO_REQUEST_ITEM_COMMODITY_CODE },
        { SCMP_REQUEST_ITEM_GROSS_NET_INDICATOR, SO_REQUEST_ITEM_GROSS_NET_INDICATOR },
        { SCMP_REQUEST_ITEM_TAX_TYPE_APPLIED, SO_REQUEST_ITEM_TAX_TYPE_APPLIED },
        { SCMP_REQUEST_ITEM_DISCOUNT_INDICATOR, SO_REQUEST_ITEM_DISCOUNT_INDICATOR },
        { SCMP_REQUEST_ITEM_ALTERNATE_TAX_ID, SO_REQUEST_ITEM_ALTERNATE_TAX_ID },
        { SCMP_REQUEST_ITEM_ALTERNATE_TAX_AMOUNT, SO_REQUEST_ITEM_ALTERNATE_TAX_AMOUNT },
        { SCMP_REQUEST_ITEM_ALTERNATE_TAX_TYPE_APPLIED, SO_REQUEST_ITEM_ALTERNATE_TAX_TYPE_APPLIED },
        { SCMP_REQUEST_ITEM_ALTERNATE_TAX_RATE, SO_REQUEST_ITEM_ALTERNATE_TAX_RATE },
        { SCMP_REQUEST_ITEM_ALTERNATE_TAX_TYPE_IDENTIFIER, SO_REQUEST_ITEM_ALTERNATE_TAX_TYPE_IDENTIFIER },
        { SCMP_REQUEST_ITEM_LOCAL_TAX, SO_REQUEST_ITEM_LOCAL_TAX },
        { SCMP_REQUEST_ITEM_ZERO_COST_TO_CUSTOMER_INDICATOR, SO_REQUEST_ITEM_ZERO_COST_TO_CUSTOMER_INDICATOR },
        { SCMP_REQUEST_ITEM_PASSENGER_FIRST_NAME, SO_REQUEST_ITEM_PASSENGER_FIRST_NAME },
        { SCMP_REQUEST_ITEM_PASSENGER_LAST_NAME, SO_REQUEST_ITEM_PASSENGER_LAST_NAME },
        { SCMP_REQUEST_ITEM_PASSENGER_ID, SO_REQUEST_ITEM_PASSENGER_ID },
        { SCMP_REQUEST_ITEM_PASSENGER_STATUS, SO_REQUEST_ITEM_PASSENGER_STATUS },
        { SCMP_REQUEST_ITEM_PASSENGER_TYPE, SO_REQUEST_ITEM_PASSENGER_TYPE },
        { SCMP_REQUEST_ITEM_PASSENGER_EMAIL, SO_REQUEST_ITEM_PASSENGER_EMAIL },
        { SCMP_REQUEST_ITEM_PASSENGER_PHONE, SO_REQUEST_ITEM_PASSENGER_PHONE },
        { SCMP_REQUEST_ITEM_INVOICE_NUMBER, SO_REQUEST_ITEM_INVOICE_NUMBER },
        { SCMP_REQUEST_ITEM_SELLER_REGISTRATION, SO_REQUEST_ITEM_SELLER_REGISTRATION },
        { SCMP_REQUEST_ITEM_POINT_OF_TITLE_TRANSFER, SO_REQUEST_ITEM_POINT_OF_TITLE_TRANSFER }
    };

            for (int i = 0; i < offers.Count; i++)
            {
                var offer = offers[i];
                var offerDetails = offer.Value.Split('^');
                var offerProperties = new Dictionary<string, string>();
                foreach (var offerDetail in offerDetails)
                {
                    var keyValue = offerDetail.Split(':');
                    if (keyValue.Length == 2)
                    {
                        offerProperties[keyValue[0]] = keyValue[1];
                    }
                    nvpRequest.Add($"{SO_REQUEST_ITEM_ITEM}_{i}_{properties[keyValue[0]]}", keyValue[1]);
                }
            }
        }


        public static Hashtable processRequest(ICSRequest icsClientRequest, Configuration cybsProperties = null)
        {
            var nvpRequest = Util.convertICSRequestToNVPRequest(icsClientRequest);
            Hashtable nvpHashTable;
            ICSReply icsReply = null;
            try
            {
                Hashtable nvpResponse = NVPClient.RunTransaction(cybsProperties, nvpRequest);
                nvpHashTable = convertNVPResponseToICSReply(nvpResponse, icsClientRequest);
            }
            catch (Exception e) {
                Console.WriteLine("Error processing Simple Order request" + e.Message);
                throw e;
            }
            return nvpHashTable;

            }

        public static void MapNonDistinctSCMPFields(ICSRequest icsClientRequest, string scmpKey, List<string> icsApplications, Hashtable nvpRequest, string scmpValue)
        {
            switch (scmpKey)
            {
                case SCMP_REQUEST_ACCOUNT_ENCODER_ID:
                    if (icsClientRequest[SCMP_REQUEST_BANK_TRANSIT_NUMBER] != null)
                    {
                        nvpRequest[SO_REQUEST_CHECK_ACCOUNT_ENCODER_ID] = scmpValue;
                    }
                    else
                    {
                        nvpRequest[SO_REQUEST_CARD_ACCOUNT_ENCODER_ID] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_AGGREGATOR_ID:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_AUTH))
                    {
                        nvpRequest[SO_REQUEST_AUTH_SERVICE_AGGREGATOR_ID] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_CREDIT))
                    {
                        nvpRequest[SO_REQUEST_CREDIT_SERVICE_AGGREGATOR_ID] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_AGGREGATOR_NAME:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_AUTH))
                    {
                        nvpRequest[SO_REQUEST_AUTH_SERVICE_AGGREGATOR_NAME] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_CREDIT))
                    {
                        nvpRequest[SO_REQUEST_CREDIT_SERVICE_AGGREGATOR_NAME] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_AP_AUTH_REQUEST_ID:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_AP_AUTH_REVERSAL))
                    {
                        nvpRequest[SO_REQUEST_AP_AUTH_REVERSAL_SERVICE_AUTH_REQUEST_ID] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_AP_CAPTURE))
                    {
                        nvpRequest[SO_REQUEST_AP_CAPTURE_SERVICE_AUTH_REQUEST_ID] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_AP_INITIATE_REQUEST_ID:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_AP_CHECK_STATUS))
                    {
                        nvpRequest[SO_REQUEST_AP_CHECK_STATUS_SERVICE_INITIATE_REQUEST_ID] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_AP_REFUND))
                    {
                        nvpRequest[SO_REQUEST_AP_REFUND_SERVICE_INITIATE_REQUEST_ID] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_AUTH_CODE:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_AUTO_AUTH_REVERSAL))
                    {
                        nvpRequest[SO_REQUEST_AUTO_AUTH_REVERSAL_SERVICE_AUTH_CODE] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_CREDIT))
                    {
                        nvpRequest[SO_REQUEST_CREDIT_SERVICE_AUTH_CODE] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_AUTH_REQUEST_ID:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_AUTH_REVERSAL))
                    {
                        nvpRequest[SO_REQUEST_AUTH_REVERSAL_SERVICE_AUTH_REQUEST_ID] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_AUTO_AUTH_REVERSAL))
                    {
                        nvpRequest[SO_REQUEST_AUTO_AUTH_REVERSAL_SERVICE_AUTH_REQUEST_ID] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_CAPTURE))
                    {
                        nvpRequest[SO_REQUEST_CAPTURE_SERVICE_AUTH_REQUEST_ID] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_AUTH_REQUEST_TOKEN:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_AUTH_REVERSAL))
                    {
                        nvpRequest[SO_REQUEST_AUTH_REVERSAL_SERVICE_AUTH_REQUEST_TOKEN] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_CAPTURE))
                    {
                        nvpRequest[SO_REQUEST_CAPTURE_SERVICE_AUTH_REQUEST_TOKEN] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_AUTH_TRANS_REF_NO:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_AUTH))
                    {
                        nvpRequest[SO_REQUEST_AUTH_SERVICE_RECONCILIATION_ID] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_AUTO_AUTH_REVERSAL))
                    {
                        nvpRequest[SO_REQUEST_AUTO_AUTH_REVERSAL_SERVICE_RECONCILIATION_ID] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_AUTH_TYPE:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_AUTH))
                    {
                        nvpRequest[SO_REQUEST_AUTH_SERVICE_AUTH_TYPE] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_CAPTURE))
                    {
                        nvpRequest[SO_REQUEST_CAPTURE_SERVICE_AUTH_TYPE] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_BILL_PAYMENT:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_AUTH))
                    {
                        nvpRequest[SO_REQUEST_AUTH_SERVICE_BILL_PAYMENT] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_AUTO_AUTH_REVERSAL))
                    {
                        nvpRequest[SO_REQUEST_AUTO_AUTH_REVERSAL_SERVICE_BILL_PAYMENT] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_CREDIT))
                    {
                        nvpRequest[SO_REQUEST_CREDIT_SERVICE_BILL_PAYMENT] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_BILL_REQUEST_ID:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_CREDIT))
                    {
                        nvpRequest[SO_REQUEST_CREDIT_SERVICE_CAPTURE_REQUEST_ID] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_AUTO_AUTH_REVERSAL))
                    {
                        nvpRequest[SO_REQUEST_AUTO_AUTH_REVERSAL_SERVICE_BILL_PAYMENT] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_DCC_UPDATE))
                    {
                        nvpRequest[SO_REQUEST_DCC_UPDATE_SERVICE_CAPTURE_REQUEST_ID] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_CHECKSUM_KEY:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_AUTH))
                    {
                        nvpRequest[SO_REQUEST_AUTH_SERVICE_CHECKSUM_KEY] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_CAPTURE))
                    {
                        nvpRequest[SO_REQUEST_CAPTURE_SERVICE_CHECKSUM_KEY] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_CREDIT))
                    {
                        nvpRequest[SO_REQUEST_CREDIT_SERVICE_CHECKSUM_KEY] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_CURRENCY:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_TRANSACTION_SEARCH))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_TRANSACTION_SEARCH_SERVICE_CURRENCY] = scmpValue;
                    }
                    else
                    {
                        nvpRequest[SO_REQUEST_PURCHASE_TOTALS_CURRENCY] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_DIRECT_DEBIT_MANDATE_AUTHENTICATION_DATE:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_DIRECT_DEBIT_REFUND))
                    {
                        nvpRequest[SO_REQUEST_DIRECT_DEBIT_REFUND_SERVICE_MANDATE_AUTHENTICATION_DATE] = scmpValue;
                    }
                    else
                    {
                        nvpRequest[SO_REQUEST_DIRECT_DEBIT_SERVICE_MANDATE_AUTHENTICATION_DATE] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_DIRECT_DEBIT_RECURRING_TYPE:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_DIRECT_DEBIT_REFUND))
                    {
                        nvpRequest[SO_REQUEST_DIRECT_DEBIT_REFUND_SERVICE_RECURRING_TYPE] = scmpValue;
                    }
                    else
                    {
                        nvpRequest[SO_REQUEST_DIRECT_DEBIT_SERVICE_RECURRING_TYPE] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_DIRECT_DEBIT_TYPE:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_DIRECT_DEBIT_REFUND))
                    {
                        nvpRequest[SO_REQUEST_DIRECT_DEBIT_REFUND_SERVICE_DIRECT_DEBIT_TYPE] = scmpValue;
                    }
                    else
                    {
                        nvpRequest[SO_REQUEST_DIRECT_DEBIT_SERVICE_DIRECT_DEBIT_TYPE] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_ECOMMERCE_INDICATOR:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_AUTH))
                    {
                        nvpRequest[SO_REQUEST_AUTH_SERVICE_COMMERCE_INDICATOR] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_AUTO_AUTH_REVERSAL))
                    {
                        nvpRequest[SO_REQUEST_AUTO_AUTH_REVERSAL_SERVICE_COMMERCE_INDICATOR] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_CREDIT))
                    {
                        nvpRequest[SO_REQUEST_CREDIT_SERVICE_COMMERCE_INDICATOR] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_ECP_CREDIT))
                    {
                        nvpRequest[SO_REQUEST_ECP_CREDIT_SERVICE_COMMERCE_INDICATOR] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_ECP_DEBIT))
                    {
                        nvpRequest[SO_REQUEST_ECP_DEBIT_SERVICE_COMMERCE_INDICATOR] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PIN_DEBIT_CREDIT))
                    {
                        nvpRequest[SO_REQUEST_PIN_DEBIT_CREDIT_SERVICE_COMMERCE_INDICATOR] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PIN_DEBIT))
                    {
                        nvpRequest[SO_REQUEST_PIN_DEBIT_SERVICE_COMMERCE_INDICATOR] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PIN_LESS_DEBIT))
                    {
                        nvpRequest[SO_REQUEST_PIN_LESS_DEBIT_SERVICE_COMMERCE_INDICATOR] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_DEBIT_REQUEST_ID:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_ECP_CREDIT))
                    {
                        nvpRequest[SO_REQUEST_ECP_CREDIT_SERVICE_DEBIT_REQUEST_ID] = scmpValue;
                    }
                    else
                    {
                        nvpRequest[SO_REQUEST_ECP_DEBIT_SERVICE_DEBIT_REQUEST_ID] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_ECP_PAYMENT_KEY:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_ECP_CREDIT))
                    {
                        nvpRequest[SO_REQUEST_ECP_CREDIT_SERVICE_TRANSACTION_TOKEN] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_ECP_DEBIT))
                    {
                        nvpRequest[SO_REQUEST_ECP_DEBIT_SERVICE_TRANSACTION_TOKEN] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_ECP_REF_NO:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_ECP_AUTHENTICATE))
                    {
                        nvpRequest[SO_REQUEST_ECP_AUTHENTICATE_SERVICE_REFERENCE_NUMBER] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_ECP_CREDIT))
                    {
                        nvpRequest[SO_REQUEST_ECP_CREDIT_SERVICE_REFERENCE_NUMBER] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_ECP_DEBIT))
                    {
                        nvpRequest[SO_REQUEST_ECP_DEBIT_SERVICE_REFERENCE_NUMBER] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_ECP_SETTLEMENT_METHOD:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_ECP_CREDIT))
                    {
                        nvpRequest[SO_REQUEST_ECP_CREDIT_SERVICE_SETTLEMENT_METHOD] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_ECP_DEBIT))
                    {
                        nvpRequest[SO_REQUEST_ECP_DEBIT_SERVICE_SETTLEMENT_METHOD] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_GRAND_TOTAL_AMOUNT:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_TRANSACTION_SEARCH))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_TRANSACTION_SEARCH_SERVICE_GRAND_TOTAL_AMOUNT] = scmpValue;
                    }
                    else
                    {
                        nvpRequest[SO_REQUEST_PURCHASE_TOTALS_GRAND_TOTAL_AMOUNT] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_INDUSTRY_DATA_TYPE:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_AUTH))
                    {
                        nvpRequest[SO_REQUEST_AUTH_SERVICE_INDUSTRY_DATA_TYPE] = scmpValue;
                    }
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_CAPTURE))
                    {
                        nvpRequest[SO_REQUEST_CAPTURE_SERVICE_INDUSTRY_DATA_TYPE] = scmpValue;
                    }
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_CREDIT))
                    {
                        nvpRequest[SO_REQUEST_CREDIT_SERVICE_INDUSTRY_DATA_TYPE] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_MANDATE_ID:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_DIRECT_DEBIT_REFUND))
                    {
                        nvpRequest[SO_REQUEST_DIRECT_DEBIT_REFUND_SERVICE_MANDATE_ID] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_DIRECT_DEBIT))
                    {
                        nvpRequest[SO_REQUEST_DIRECT_DEBIT_SERVICE_MANDATE_ID] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_MARKING_NOTES:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_FRAUD_UPDATE))
                    {
                        nvpRequest[SO_REQUEST_FRAUD_UPDATE_SERVICE_MARKING_NOTES] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_RISK_UPDATE))
                    {
                        nvpRequest[SO_REQUEST_RISK_UPDATE_SERVICE_MARKING_NOTES] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_MARKING_REASON:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_FRAUD_UPDATE))
                    {
                        nvpRequest[SO_REQUEST_FRAUD_UPDATE_SERVICE_MARKING_REASON] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_RISK_UPDATE))
                    {
                        nvpRequest[SO_REQUEST_RISK_UPDATE_SERVICE_MARKING_REASON] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_MERCHANT_RECEIPT_NUMBER:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_CAPTURE))
                    {
                        nvpRequest[SO_REQUEST_CAPTURE_SERVICE_MERCHANT_RECEIPT_NUMBER] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_CREDIT))
                    {
                        nvpRequest[SO_REQUEST_CREDIT_SERVICE_MERCHANT_RECEIPT_NUMBER] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_NETWORK_ORDER:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_PIN_DEBIT_CREDIT))
                    {
                        nvpRequest[SO_REQUEST_PIN_DEBIT_CREDIT_SERVICE_NETWORK_ORDER] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PIN_DEBIT))
                    {
                        nvpRequest[SO_REQUEST_PIN_DEBIT_SERVICE_NETWORK_ORDER] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_PARTIAL_PAYMENT_ID:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_CAPTURE))
                    {
                        nvpRequest[SO_REQUEST_CAPTURE_SERVICE_PARTIAL_PAYMENT_ID] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_CREDIT))
                    {
                        nvpRequest[SO_REQUEST_CREDIT_SERVICE_PARTIAL_PAYMENT_ID] = scmpValue;
                    }
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_ECP_CREDIT))
                    {
                        nvpRequest[SO_REQUEST_ECP_CREDIT_SERVICE_PARTIAL_PAYMENT_ID] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_ECP_DEBIT))
                    {
                        nvpRequest[SO_REQUEST_ECP_DEBIT_SERVICE_PARTIAL_PAYMENT_ID] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_PAYPAL_AUTHORIZATION_ID:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_AUTH_REVERSAL))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_AUTH_REVERSAL_SERVICE_PAYPAL_AUTHORIZATION_ID] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_DO_CAPTURE))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_DO_CAPTURE_SERVICE_PAYPAL_AUTHORIZATION_ID] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_PAYPAL_AUTHORIZATION_REQUEST_ID:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_AUTH_REVERSAL))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_AUTH_REVERSAL_SERVICE_PAYPAL_AUTHORIZATION_REQUEST_ID] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_DO_CAPTURE))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_DO_CAPTURE_SERVICE_PAYPAL_AUTHORIZATION_REQUEST_ID] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_PAYPAL_AUTHORIZATION_REQUEST_TOKEN:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_AUTH_REVERSAL))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_AUTH_REVERSAL_SERVICE_PAYPAL_AUTHORIZATION_REQUEST_TOKEN] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_DO_CAPTURE))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_DO_CAPTURE_SERVICE_PAYPAL_AUTHORIZATION_REQUEST_TOKEN] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_PAYPAL_BILLING_AGREEMENT_CUSTOM:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_EC_SET))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_EC_SET_SERVICE_PAYPAL_BILLING_AGREEMENT_CUSTOM] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_UPDATE_AGREEMENT))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_UPDATE_AGREEMENT_SERVICE_PAYPAL_BILLING_AGREEMENT_CUSTOM] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_PAYPAL_BILLING_AGREEMENT_ID:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_EC_SET))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_DO_REF_TRANSACTION_SERVICE_PAYPAL_BILLING_AGREEMENT_ID] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_UPDATE_AGREEMENT))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_UPDATE_AGREEMENT_SERVICE_PAYPAL_BILLING_AGREEMENT_ID] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_PAYPAL_CUSTOMER_EMAIL:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_AUTH))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_AUTHORIZATION_SERVICE_PAYPAL_CUSTOMER_EMAIL] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_EC_DO_PAYMENT))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_EC_DO_PAYMENT_SERVICE_PAYPAL_CUSTOMER_EMAIL] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_EC_ORDER_SETUP))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_EC_ORDER_SETUP_SERVICE_PAYPAL_CUSTOMER_EMAIL] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_EC_SET))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_EC_SET_SERVICE_PAYPAL_CUSTOMER_EMAIL] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_TRANSACTION_SEARCH))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_TRANSACTION_SEARCH_SERVICE_PAYPAL_CUSTOMER_EMAIL] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_PAYPAL_DESC:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_DO_REF_TRANSACTION))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_DO_REF_TRANSACTION_SERVICE_PAYPAL_DESC] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_EC_DO_PAYMENT))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_EC_DO_PAYMENT_SERVICE_PAYPAL_DESC] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_EC_ORDER_SETUP))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_EC_ORDER_SETUP_SERVICE_PAYPAL_DESC] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_EC_SET))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_EC_SET_SERVICE_PAYPAL_DESC] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_PAYPAL_EC_DO_PAYMENT_REQUEST_ID:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_AUTH_REVERSAL))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_AUTH_REVERSAL_SERVICE_PAYPAL_EC_DO_PAYMENT_REQUEST_ID] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_DO_CAPTURE))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_DO_CAPTURE_SERVICE_PAYPAL_EC_DO_PAYMENT_REQUEST_ID] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_PAYPAL_EC_DO_PAYMENT_REQUEST_TOKEN:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_AUTH_REVERSAL))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_AUTH_REVERSAL_SERVICE_PAYPAL_EC_DO_PAYMENT_REQUEST_TOKEN] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_DO_CAPTURE))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_DO_CAPTURE_SERVICE_PAYPAL_EC_DO_PAYMENT_REQUEST_TOKEN] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_PAYPAL_EC_ORDER_SETUP_REQUEST_ID:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_AUTH))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_AUTH_SERVICE_PAYPAL_EC_ORDER_SETUP_REQUEST_ID] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_AUTH_REVERSAL))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_AUTH_REVERSAL_SERVICE_PAYPAL_EC_ORDER_SETUP_REQUEST_ID] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_PAYPAL_EC_ORDER_SETUP_REQUEST_TOKEN:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_AUTH))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_AUTHORIZATION_SERVICE_PAYPAL_EC_ORDER_SETUP_REQUEST_TOKEN] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_AUTH_REVERSAL))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_AUTH_REVERSAL_SERVICE_PAYPAL_EC_ORDER_SETUP_REQUEST_TOKEN] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_PAYPAL_EC_SET_REQUEST_ID:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_CREATE_AGREEMENT))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_CREATE_AGREEMENT_SERVICE_PAYPAL_EC_SET_REQUEST_ID] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_EC_DO_PAYMENT))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_EC_DO_PAYMENT_SERVICE_PAYPAL_EC_SET_REQUEST_ID] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_EC_GET_DETAILS))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_EC_GET_DETAILS_SERVICE_PAYPAL_EC_SET_REQUEST_ID] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_EC_ORDER_SETUP))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_EC_ORDER_SETUP_SERVICE_PAYPAL_EC_SET_REQUEST_ID] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_EC_SET))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_EC_SET_SERVICE_PAYPAL_EC_SET_REQUEST_ID] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_PAYPAL_EC_SET_REQUEST_TOKEN:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_CREATE_AGREEMENT))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_CREATE_AGREEMENT_SERVICE_PAYPAL_EC_SET_REQUEST_TOKEN] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_EC_DO_PAYMENT))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_EC_DO_PAYMENT_SERVICE_PAYPAL_EC_SET_REQUEST_TOKEN] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_EC_GET_DETAILS))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_EC_GET_DETAILS_SERVICE_PAYPAL_EC_SET_REQUEST_TOKEN] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_EC_ORDER_SETUP))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_EC_ORDER_SETUP_SERVICE_PAYPAL_EC_SET_REQUEST_TOKEN] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_EC_SET))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_EC_SET_SERVICE_PAYPAL_EC_SET_REQUEST_TOKEN] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_PAYPAL_INVOICE_NUMBER:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_DO_CAPTURE))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_DO_CAPTURE_SERVICE_INVOICE_NUMBER] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_DO_REF_TRANSACTION))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_DO_REF_TRANSACTION_SERVICE_INVOICE_NUMBER] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_EC_DO_PAYMENT))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_EC_DO_PAYMENT_SERVICE_INVOICE_NUMBER] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_EC_ORDER_SETUP))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_EC_ORDER_SETUP_SERVICE_INVOICE_NUMBER] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_EC_SET))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_EC_SET_SERVICE_INVOICE_NUMBER] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_TRANSACTION_SEARCH))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_TRANSACTION_SEARCH_SERVICE_INVOICE_NUMBER] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_PAYPAL_MP_ID:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_PRE_APPROVED_PAYMENT))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_PRE_APPROVED_PAYMENT_SERVICE_MP_ID] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_PRE_APPROVED_UPDATE))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_PRE_APPROVED_UPDATE_SERVICE_MP_ID] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_PAYPAL_PAYER_ID:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_EC_DO_PAYMENT))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_EC_DO_PAYMENT_SERVICE_PAYPAL_PAYER_ID] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_EC_ORDER_SETUP))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_EC_ORDER_SETUP_SERVICE_PAYPAL_PAYER_ID] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_PAYPAL_PAYMENT_TYPE:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_DO_REF_TRANSACTION))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_DO_REF_TRANSACTION_SERVICE_PAYPAL_PAYMENT_TYPE] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_EC_SET))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_EC_SET_SERVICE_PAYPAL_PAYMENT_TYPE] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_PAYPAL_REQ_CONFIRM_SHIPPING:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_DO_REF_TRANSACTION))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_DO_REF_TRANSACTION_SERVICE_PAYPAL_REQ_CONFIRM_SHIPPING] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_EC_SET))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_EC_SET_SERVICE_PAYPAL_REQ_CONFIRM_SHIPPING] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_PAYPAL_TRANSACTION_ID:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_GET_TRANSACTION_DETAILS))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_GET_TRANSACTION_DETAILS_SERVICE_TRANSACTION_ID] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_PAYPAL_TRANSACTION_SEARCH))
                    {
                        nvpRequest[SO_REQUEST_PAYPAL_TRANSACTION_SEARCH_SERVICE_TRANSACTION_ID] = scmpValue;
                    }
                    break;

                case SCMP_REQUEST_PURCHASING_LEVEL:
                    if (icsApplications.Contains(SCMP_REQUEST_ICS_CAPTURE))
                    {
                        nvpRequest[SO_REQUEST_CAPTURE_SERVICE_PURCHASING_LEVEL] = scmpValue;
                    }
                    else if (icsApplications.Contains(SCMP_REQUEST_ICS_CREDIT))
                    {
                        nvpRequest[SO_REQUEST_CREDIT_SERVICE_PURCHASING_LEVEL] = scmpValue;
                    }
                    break;

                case SCM_REQUEST_PAYPAL_TOKEN:
                    MapSCMPRequestPayPalToken(icsApplications, nvpRequest);
                    break;

                default:
                    Console.WriteLine("SCMP Key=" + scmpKey + " is unknown or not handled yet.");
                    break;
            }
        }

        private static void MapSCMPRequestPayPalToken(List<string> icsApplications, Hashtable nvpRequest)
        {
            if (icsApplications != null && icsApplications.Any())
            {
                if (icsApplications.Contains(ICS_PAYPAL_CREATE_AGREEMENT))
                {
                    nvpRequest[SCM_REQUEST_PAYPAL_TOKEN] = SO_PAYPAL_CREATE_AGREEMENT_SERVICE_PAYPAL_TOKEN;
                }
                else if (icsApplications.Contains(ICS_PAYPAL_EC_DO_PAYMENT))
                {
                    nvpRequest[SCM_REQUEST_PAYPAL_TOKEN] = SO_PAYPAL_EC_DO_PAYMENT_SERVICE_PAYPAL_TOKEN;
                }
                else if (icsApplications.Contains(ICS_PAYPAL_EC_GET_DETAILS))
                {
                    nvpRequest[SCM_REQUEST_PAYPAL_TOKEN] = SO_PAYPAL_EC_GET_DETAILS_SERVICE_PAYPAL_TOKEN;
                }
                else if (icsApplications.Contains(ICS_PAYPAL_EC_ORDER_SETUP))
                {
                    nvpRequest[SCM_REQUEST_PAYPAL_TOKEN] = SO_PAYPAL_EC_ORDER_SETUP_SERVICE_PAYPAL_TOKEN;
                }
                else if (icsApplications.Contains(ICS_PAYPAL_EC_SET))
                {
                    nvpRequest[SCM_REQUEST_PAYPAL_TOKEN] = SO_PAYPAL_EC_SET_SERVICE_PAYPAL_TOKEN;
                }
            }
        }


        public static void MapNonDistinctSOResponseFields(Hashtable icsReply, string value, ICSRequest icsClientRequest)
        {
            string icsApplications = icsClientRequest[SCMP_REQUEST_ICS_APPLICATIONS];
            List<string> icsApplicationsList = new List<string>();

            if (!string.IsNullOrEmpty(icsApplications))
            {
                if (icsApplications.Contains(","))
                {
                    foreach (string icsApp in icsApplications.Split(','))
                    {
                        string icsApplication = icsApplicationsLookupTable.ContainsKey(icsApp.Trim()) ? icsApplicationsLookupTable[icsApp.Trim()] : null;
                        if (!string.IsNullOrEmpty(icsApplication))
                        {
                            icsApplicationsList.Add(icsApplication);
                        }
                        else
                        {
                            Console.WriteLine("ics_applications: " + icsApp + " is not mapped. Check the ics_applications.properties file");
                        }
                    }
                }
                else
                {
                    string icsApplication = icsApplicationsLookupTable.ContainsKey(icsApplications.Trim()) ? icsApplicationsLookupTable[icsApplications.Trim()] : null;
                    if (!string.IsNullOrEmpty(icsApplication))
                    {
                        icsApplicationsList.Add(icsApplication);
                    }
                    else
                    {
                        Console.WriteLine("ICS applications: " + icsApplications + " not found in ics_applications.properties mapping file");
                    }
                }
            }
        }

        private static Hashtable convertNVPResponseToICSReply(Hashtable nvpResponse, ICSRequest icsClientRequest)
        {

            Hashtable hashTable = new Hashtable();


             if (nvpResponse == null || nvpResponse.Count == 0)
            {
                Console.WriteLine("NVP Response is null or empty");
                return hashTable;
            }
            foreach (DictionaryEntry dictionaryEntry in nvpResponse)
            {

                string value = dictionaryEntry.Value.ToString();
                string key = dictionaryEntry.Key.ToString();
                string scmpKey = "";
                if (responseConversionTable.ContainsKey(key))
                {
                    scmpKey = responseConversionTable[key];
                    hashTable[scmpKey] = value;
                }
                else
                {
                    //MapNonDistinctFields
                    MapNonDistinctSOResponseFields(nvpResponse, value, icsClientRequest);
                }
            }
            return hashTable;
        }
    
    }
    
}

