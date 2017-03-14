using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BntWeb.Data;
using BntWeb.Data.Services;
using BntWeb.MemberBase.Models;
using BntWeb.MemberBase.Services;
using BntWeb.MemberCenter.ApiModels;
using BntWeb.MemberCenter.ViewModels;
using BntWeb.Mvc;
using BntWeb.Security;
using BntWeb.Validation;
using BntWeb.WebApi.Models;

namespace BntWeb.MemberCenter.Controllers
{
    public class WebMemberAddressController : Controller
    {
        private readonly ICurrencyService _currencyService;
        private readonly IMemberContainer _memberContainer;
        private readonly IMemberService _memberService;
        public WebMemberAddressController(IMemberService memberService, ICurrencyService currencyService, IMemberContainer memberContainer)
        {
            _currencyService = currencyService;
            _memberContainer = memberContainer;
            _memberService = memberService;
        }
        // GET: WebMemberAddress
        //获得我的收货地址列表
        [MemberAuthorize]
        public ActionResult WebMemberAddressList()
        {
            var currentUser = _memberContainer.CurrentMember;
            ViewBag.Id = currentUser.Id;
            var addresses = _currencyService.GetList<MemberAddress>(me => me.MemberId == currentUser.Id);
            ViewBag.AddressList = addresses;
            return View();
        }


        //添加我的收货地址
        [MemberAuthorize]
        public ActionResult WebCreateAddress(WebAddMemberAddressModels addAddressModel)
        {
            var result = new DataJsonResult();
            if (string.IsNullOrWhiteSpace(addAddressModel.Address))
                throw new Exception("详细地址不能空！");
            if (string.IsNullOrWhiteSpace(addAddressModel.Contacts))
                throw new Exception("收货人不能为空！");
            if (string.IsNullOrWhiteSpace(addAddressModel.Phone))
                throw new Exception("手机号码不能为空！");
            if (string.IsNullOrWhiteSpace(addAddressModel.Province))
                throw new Exception("省不能为空");
            if (string.IsNullOrWhiteSpace(addAddressModel.City))
                throw new Exception("市是不能为空！");
            //获得当前用户
            var currentUser = _memberContainer.CurrentMember;

            var myAddress = _currencyService.GetList<MemberAddress>(me => me.MemberId == currentUser.Id);
            var model = new MemberAddress
            {
                Id = KeyGenerator.GetGuidKey(),
                MemberId = currentUser.Id,
                Address = addAddressModel.Address,
                Contacts = addAddressModel.Contacts,
                Phone = addAddressModel.Phone,
                Province = addAddressModel.Province,
                City = addAddressModel.City,
                District = addAddressModel.District,
                Street = addAddressModel.Street,
                RegionName = addAddressModel.RegionName,
                IsDefault = addAddressModel.IsDefault,
                Postcode = addAddressModel.Postcode
            };

            if (myAddress == null || myAddress.Count == 0)
                model.IsDefault = true;
            if (!_currencyService.Create(model))
                throw new WebApiInnerException("3001", "添加失败,内部执出错");

            var member = _currencyService.GetSingleById<MemberExtension>(currentUser.Id);
            if (string.IsNullOrWhiteSpace(member.Address) && (myAddress == null || myAddress.Count == 0))
            {
                member.Address = model.Address;
                _currencyService.Update(member);
            }

            if (model.IsDefault)
            {
                _memberService.SetDefaultAddress(model.MemberId, model.Id);
            }
            result.Data = model.Id;
            return Json(result);
        }

        //编辑我的收货地址 

        public ActionResult WebEditAddress(WebEditMemberAddressModels editModel)
        {
            var errorMessage = "";
            var success = false;
            try
            {

                Argument.ThrowIfNullOrEmpty(editModel.Contacts, "收件人");
                Argument.ThrowIfNullOrEmpty(editModel.Phone, "联系电话");
                Argument.ThrowIfNullOrEmpty(editModel.Province, "省");
                Argument.ThrowIfNullOrEmpty(editModel.City, "市");
                Argument.ThrowIfNullOrEmpty(editModel.District, "区");
                Argument.ThrowIfNullOrEmpty(editModel.Address, "地址");

                var address = _currencyService.GetSingleById<MemberAddress>(editModel.AddressId);

                if (address == null)
                    throw new WebApiInnerException("3004", "地址不存在");

                address.Address = editModel.Address;
                address.Contacts = editModel.Contacts;
                address.Phone = editModel.Phone;
                address.Province = editModel.Province;
                address.City = editModel.City;
                address.District = editModel.District;
                address.Street = editModel.Street;
                address.RegionName = editModel.Province + "," + editModel.City + "," + editModel.District + "," +
                                     editModel.Address;
                address.IsDefault = editModel.IsDefault;

                if (!_currencyService.Update(address))
                {
                    throw new WebApiInnerException("3005", "编辑失败，内部执行错误");
                }
                if (address.IsDefault)
                {
                    _memberService.SetDefaultAddress(address.MemberId, address.Id);
                }

                success = true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            return Json(new { Success = success, ErrorMessage = errorMessage }, JsonRequestBehavior.AllowGet);

        }

        //设为默认地址
        public ActionResult WebDeleteAddress(Guid addressId)
        {
            var errorMessage = "";
            var success = false;
            try
            {
                //var result = new DataTableJsonResult();
                var address = _currencyService.GetSingleById<MemberAddress>(addressId);

                if (address == null)
                    throw new Exception("地址不存在");
                if (_currencyService.DeleteByConditon<MemberAddress>(me => me.Id == addressId) < 1)
                    throw new Exception("删除失败内部执出错");
                success = true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            return Json(new { Success = success, ErrorMessage = errorMessage }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult SetDefaultAddress(string memberId, Guid addressId)
        {
            var errorMessage = "";
            var success = false;
            try
            {
                //var result = new DataTableJsonResult();
                var address = _currencyService.GetSingleById<MemberAddress>(addressId);

                if (address == null)
                    throw new Exception("地址不存在");
                _memberService.SetDefaultAddress(memberId, addressId);
                success = true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            return Json(new { Success = success, ErrorMessage = errorMessage }, JsonRequestBehavior.AllowGet);
        }
    }
}