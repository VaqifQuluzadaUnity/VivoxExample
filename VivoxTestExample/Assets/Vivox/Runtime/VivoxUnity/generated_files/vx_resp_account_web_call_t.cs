//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


public class vx_resp_account_web_call_t : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal vx_resp_account_web_call_t(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(vx_resp_account_web_call_t obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~vx_resp_account_web_call_t() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          VivoxCoreInstancePINVOKE.delete_vx_resp_account_web_call_t(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public vx_resp_base_t base_ {
    set {
      VivoxCoreInstancePINVOKE.vx_resp_account_web_call_t_base__set(swigCPtr, vx_resp_base_t.getCPtr(value));
    } 
    get {
      global::System.IntPtr cPtr = VivoxCoreInstancePINVOKE.vx_resp_account_web_call_t_base__get(swigCPtr);
      vx_resp_base_t ret = (cPtr == global::System.IntPtr.Zero) ? null : new vx_resp_base_t(cPtr, false);
      return ret;
    } 
  }

  public string content_type {
    set {
      VivoxCoreInstancePINVOKE.vx_resp_account_web_call_t_content_type_set(swigCPtr, value);
    } 
    get {
      string ret = VivoxCoreInstancePINVOKE.vx_resp_account_web_call_t_content_type_get(swigCPtr);
      return ret;
    } 
  }

  public int content_length {
    set {
      VivoxCoreInstancePINVOKE.vx_resp_account_web_call_t_content_length_set(swigCPtr, value);
    } 
    get {
      int ret = VivoxCoreInstancePINVOKE.vx_resp_account_web_call_t_content_length_get(swigCPtr);
      return ret;
    } 
  }

  public string content {
    set {
      VivoxCoreInstancePINVOKE.vx_resp_account_web_call_t_content_set(swigCPtr, value);
    } 
    get {
      string ret = VivoxCoreInstancePINVOKE.vx_resp_account_web_call_t_content_get(swigCPtr);
      return ret;
    } 
  }

  public vx_resp_account_web_call_t() : this(VivoxCoreInstancePINVOKE.new_vx_resp_account_web_call_t(), true) {
  }

}
