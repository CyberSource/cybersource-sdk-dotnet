﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CyberSource.Clients.NVPServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="urn:schemas-cybersource-com:transaction-data:TransactionProcessor", ConfigurationName="NVPServiceReference.INVPTransactionProcessor")]
    public interface INVPTransactionProcessor {
        
        // CODEGEN: Generating message contract since the operation runTransaction is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="runTransaction", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        CyberSource.Clients.NVPServiceReference.outputNVPMessageOut runTransaction(CyberSource.Clients.NVPServiceReference.inputNVPMessageIn request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class inputNVPMessageIn {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:schemas-cybersource-com:transaction-data-1.211", Order=0)]
        public string nvpRequest;
        
        public inputNVPMessageIn() {
        }
        
        public inputNVPMessageIn(string nvpRequest) {
            this.nvpRequest = nvpRequest;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class outputNVPMessageOut {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:schemas-cybersource-com:transaction-data-1.211", Order=0)]
        public string nvpReply;
        
        public outputNVPMessageOut() {
        }
        
        public outputNVPMessageOut(string nvpReply) {
            this.nvpReply = nvpReply;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface INVPTransactionProcessorChannel : CyberSource.Clients.NVPServiceReference.INVPTransactionProcessor, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class NVPTransactionProcessorClient : System.ServiceModel.ClientBase<CyberSource.Clients.NVPServiceReference.INVPTransactionProcessor>, CyberSource.Clients.NVPServiceReference.INVPTransactionProcessor {
        
        public NVPTransactionProcessorClient() {
        }
        
        public NVPTransactionProcessorClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public NVPTransactionProcessorClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NVPTransactionProcessorClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NVPTransactionProcessorClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        CyberSource.Clients.NVPServiceReference.outputNVPMessageOut CyberSource.Clients.NVPServiceReference.INVPTransactionProcessor.runTransaction(CyberSource.Clients.NVPServiceReference.inputNVPMessageIn request) {
            return base.Channel.runTransaction(request);
        }
        
        public string runTransaction(string nvpRequest) {
            CyberSource.Clients.NVPServiceReference.inputNVPMessageIn inValue = new CyberSource.Clients.NVPServiceReference.inputNVPMessageIn();
            inValue.nvpRequest = nvpRequest;
            CyberSource.Clients.NVPServiceReference.outputNVPMessageOut retVal = ((CyberSource.Clients.NVPServiceReference.INVPTransactionProcessor)(this)).runTransaction(inValue);
            return retVal.nvpReply;
        }
    }
}
