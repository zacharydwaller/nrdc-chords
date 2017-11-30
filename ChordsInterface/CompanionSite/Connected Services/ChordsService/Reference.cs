﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CompanionSite.ChordsService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ChordsType", Namespace="http://schemas.datacontract.org/2004/07/ChordsInterface.Chords")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CompanionSite.ChordsService.Measurement))]
    public partial class ChordsType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Measurement", Namespace="http://schemas.datacontract.org/2004/07/ChordsInterface.Chords")]
    [System.SerializableAttribute()]
    public partial class Measurement : CompanionSite.ChordsService.ChordsType {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private uint InstrumentIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TimeStampField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal ValueField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public uint InstrumentID {
            get {
                return this.InstrumentIDField;
            }
            set {
                if ((this.InstrumentIDField.Equals(value) != true)) {
                    this.InstrumentIDField = value;
                    this.RaisePropertyChanged("InstrumentID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TimeStamp {
            get {
                return this.TimeStampField;
            }
            set {
                if ((object.ReferenceEquals(this.TimeStampField, value) != true)) {
                    this.TimeStampField = value;
                    this.RaisePropertyChanged("TimeStamp");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal Value {
            get {
                return this.ValueField;
            }
            set {
                if ((this.ValueField.Equals(value) != true)) {
                    this.ValueField = value;
                    this.RaisePropertyChanged("Value");
                }
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ChordsService.IService")]
    public interface IService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetSiteList", ReplyAction="http://tempuri.org/IService/GetSiteListResponse")]
        string GetSiteList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetSiteList", ReplyAction="http://tempuri.org/IService/GetSiteListResponse")]
        System.Threading.Tasks.Task<string> GetSiteListAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetSite", ReplyAction="http://tempuri.org/IService/GetSiteResponse")]
        string GetSite(int siteID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetSite", ReplyAction="http://tempuri.org/IService/GetSiteResponse")]
        System.Threading.Tasks.Task<string> GetSiteAsync(int siteID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetSystemList", ReplyAction="http://tempuri.org/IService/GetSystemListResponse")]
        string GetSystemList(int siteID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetSystemList", ReplyAction="http://tempuri.org/IService/GetSystemListResponse")]
        System.Threading.Tasks.Task<string> GetSystemListAsync(int siteID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetInstrumentList", ReplyAction="http://tempuri.org/IService/GetInstrumentListResponse")]
        string GetInstrumentList(int systemID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetInstrumentList", ReplyAction="http://tempuri.org/IService/GetInstrumentListResponse")]
        System.Threading.Tasks.Task<string> GetInstrumentListAsync(int systemID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetMeasurements", ReplyAction="http://tempuri.org/IService/GetMeasurementsResponse")]
        string GetMeasurements(int siteID, int streamIndex, int hoursBack);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetMeasurements", ReplyAction="http://tempuri.org/IService/GetMeasurementsResponse")]
        System.Threading.Tasks.Task<string> GetMeasurementsAsync(int siteID, int streamIndex, int hoursBack);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/CreateMeasurement", ReplyAction="http://tempuri.org/IService/CreateMeasurementResponse")]
        string CreateMeasurement(CompanionSite.ChordsService.Measurement measurement);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/CreateMeasurement", ReplyAction="http://tempuri.org/IService/CreateMeasurementResponse")]
        System.Threading.Tasks.Task<string> CreateMeasurementAsync(CompanionSite.ChordsService.Measurement measurement);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceChannel : CompanionSite.ChordsService.IService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceClient : System.ServiceModel.ClientBase<CompanionSite.ChordsService.IService>, CompanionSite.ChordsService.IService {
        
        public ServiceClient() {
        }
        
        public ServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string GetSiteList() {
            return base.Channel.GetSiteList();
        }
        
        public System.Threading.Tasks.Task<string> GetSiteListAsync() {
            return base.Channel.GetSiteListAsync();
        }
        
        public string GetSite(int siteID) {
            return base.Channel.GetSite(siteID);
        }
        
        public System.Threading.Tasks.Task<string> GetSiteAsync(int siteID) {
            return base.Channel.GetSiteAsync(siteID);
        }
        
        public string GetSystemList(int siteID) {
            return base.Channel.GetSystemList(siteID);
        }
        
        public System.Threading.Tasks.Task<string> GetSystemListAsync(int siteID) {
            return base.Channel.GetSystemListAsync(siteID);
        }
        
        public string GetInstrumentList(int systemID) {
            return base.Channel.GetInstrumentList(systemID);
        }
        
        public System.Threading.Tasks.Task<string> GetInstrumentListAsync(int systemID) {
            return base.Channel.GetInstrumentListAsync(systemID);
        }
        
        public string GetMeasurements(int siteID, int streamIndex, int hoursBack) {
            return base.Channel.GetMeasurements(siteID, streamIndex, hoursBack);
        }
        
        public System.Threading.Tasks.Task<string> GetMeasurementsAsync(int siteID, int streamIndex, int hoursBack) {
            return base.Channel.GetMeasurementsAsync(siteID, streamIndex, hoursBack);
        }
        
        public string CreateMeasurement(CompanionSite.ChordsService.Measurement measurement) {
            return base.Channel.CreateMeasurement(measurement);
        }
        
        public System.Threading.Tasks.Task<string> CreateMeasurementAsync(CompanionSite.ChordsService.Measurement measurement) {
            return base.Channel.CreateMeasurementAsync(measurement);
        }
    }
}
