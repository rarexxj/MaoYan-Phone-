using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BntWeb.Data;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.MemberBase;
using BntWeb.MemberBase.Models;
using BntWeb.MemberBase.Services;
using BntWeb.MemberCenter.ViewModels;
using BntWeb.Mvc;
using BntWeb.Security;
using BntWeb.Security.Identity;
using BntWeb.Utility.Extensions;
using BntWeb.Validation;
using BntWeb.Wallet.Models;
using BntWeb.Wallet.Services;
using BntWeb.Web.Extensions;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;




namespace BntWeb.MemberCenter.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IStorageFileService _storageFileService;
        private readonly ICurrencyService _currencyService;
        private readonly IWalletService _walletService;
        public AdminController(IWalletService walletService, ICurrencyService currencyService, IMemberService memberService, IStorageFileService storageFileService)
        {
            _currencyService = currencyService;
            _memberService = memberService;
            _storageFileService = storageFileService;
            _walletService = walletService;
        }


        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditMemberKey })]
        public ActionResult Edit(string id)
        {
            Argument.ThrowIfNullOrEmpty(id, "会员Id不能为空");
            var member = _memberService.FindMemberById(id);

            ViewBag.AvatarFile =
                _storageFileService.GetFiles(member.Id.ToGuid(), MemberBaseModule.Key, "Avatar").FirstOrDefault();
            return View(member);
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditMemberKey })]
        public ActionResult EditOnPost(EditMemberViewModel editMember)
        {
            var result = new DataJsonResult();
            Member oldMember = null;
            if (!string.IsNullOrWhiteSpace(editMember.MemberId))
                oldMember = _memberService.FindMemberById(editMember.MemberId);

            if (oldMember == null)
            {
                //新建用户
                User user = _memberService.FindUserByName(editMember.UserName);
                if (user == null)
                {
                    var member = new Member
                    {
                        UserName = editMember.UserName,
                        Email = editMember.Email,
                        PhoneNumber = editMember.PhoneNumber,
                        LockoutEnabled = false,
                        NickName = editMember.NickName,
                        Birthday = editMember.Birthday,
                        Sex = editMember.Sex,
                        Province = editMember.Member_Province,
                        City = editMember.Member_City,
                        District = editMember.Member_District,
                        Street = editMember.Member_Street,
                        Address = editMember.Address
                    };

                    var identityResult = _memberService.CreateMember(member, editMember.Password);

                    if (!identityResult.Succeeded)
                    {
                        result.ErrorMessage = identityResult.Errors.FirstOrDefault();
                    }
                    else
                    {
                        _storageFileService.AssociateFile(member.Id.ToGuid(), MemberBaseModule.Key, MemberBaseModule.DisplayName, editMember.Avatar.ToGuid(), "Avatar");
                    }
                }
                else
                {
                    result.ErrorMessage = "此用户名的账号已经存在！";
                }
            }
            else
            {
                //编辑用户
                oldMember.Email = editMember.Email;
                oldMember.PhoneNumber = editMember.PhoneNumber;

                oldMember.NickName = editMember.NickName;
                oldMember.Birthday = editMember.Birthday;
                oldMember.Sex = editMember.Sex;
                oldMember.Province = editMember.Member_Province;
                oldMember.City = editMember.Member_City;
                oldMember.District = editMember.Member_District;
                oldMember.Street = editMember.Member_Street;
                oldMember.Address = editMember.Address;

                var identityResult = _memberService.UpdateMember(oldMember, editMember.Password, editMember.Password2);
                if (!identityResult.Succeeded)
                {
                    result.ErrorMessage = identityResult.Errors.FirstOrDefault();
                }
                else
                {
                    _storageFileService.ReplaceFile(oldMember.Id.ToGuid(), MemberBaseModule.Key, MemberBaseModule.DisplayName, editMember.Avatar.ToGuid(), "Avatar");
                }
            }

            return Json(result);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewMemberKey })]
        public ActionResult List()
        {
            return View();
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewMemberKey })]
        public ActionResult ListOnPage()
        {
            var result = new DataTableJsonResult();

            //取参数值
            int draw, pageIndex, pageSize, totalCount;
            string sortColumn;
            bool isDesc;
            Request.GetDatatableParameters(out draw, out pageIndex, out pageSize, out sortColumn, out isDesc);
            result.draw = draw;

            //取查询条件
            var userName = Request.Get("extra_search[UserName]");
            var checkUserName = string.IsNullOrWhiteSpace(userName);
            
            var nickName = Request.Get("extra_search[NickName]");
            var checkNickName = string.IsNullOrWhiteSpace(nickName);

            var sex = Request.Get("extra_search[Sex]");
            var checkSex = string.IsNullOrWhiteSpace(sex);
            var sexInt = sex.To<int>();

             var createTimeBegin = Request.Get("extra_search[CreateTimeBegin]");
            var checkCreateTimeBegin = string.IsNullOrWhiteSpace(createTimeBegin);
            var createTimeBeginTime = createTimeBegin.To<DateTime>();

            var createTimeEnd = Request.Get("extra_search[CreateTimeEnd]");
            var checkCreateTimeEnd = string.IsNullOrWhiteSpace(createTimeEnd);
            var createTimeEndTime = createTimeEnd.To<DateTime>();

            Expression<Func<Member, bool>> expression =
                l => (checkUserName || l.UserName.Contains(userName)) &&
                     (checkNickName || l.NickName.Equals(nickName, StringComparison.OrdinalIgnoreCase)) &&
                     (checkSex || (int)l.Sex == sexInt) &&
                     (checkCreateTimeBegin || l.CreateTime >= createTimeBeginTime) &&
                     (checkCreateTimeEnd || l.CreateTime <= createTimeEndTime) &&
                     l.UserType == UserType.Member;

            Expression<Func<Member, object>> orderByExpression;
            //设置排序
            switch (sortColumn)
            {
                case "Birthday":
                    orderByExpression = u => new { u.Birthday };
                    break;
                case "Sex":
                    orderByExpression = u => new { u.Sex };
                    break;
                case "NickName":
                    orderByExpression = u => new { u.NickName };
                    break;
                case "CreateTime":
                    orderByExpression = u => new { u.CreateTime };
                    break;
                case "PhoneNumber":
                    orderByExpression = u => new { u.PhoneNumber };
                    break;
                case "Email":
                    orderByExpression = u => new { u.Email };
                    break;
                case "LockoutEnabled":
                    orderByExpression = u => new { u.LockoutEnabled };
                    break;
                default:
                    orderByExpression = u => new { u.UserName };
                    break;
            }

            //分页查询
            var members = _memberService.GetListPaged(pageIndex, pageSize, expression, orderByExpression, isDesc, out totalCount);

            result.data = members;
            result.recordsTotal = totalCount;
            result.recordsFiltered = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditMemberKey })]
        public ActionResult Switch(SwitchMemberViewModel switchUser)
        {
            var result = new DataJsonResult();
            Member member = _memberService.FindMemberById(switchUser.MemberId);

            if (member != null)
            {
                if (member.UserName.Equals("bocadmin", StringComparison.OrdinalIgnoreCase))
                {
                    result.ErrorMessage = "内置账号不可以禁用！";
                }
                else
                {
                    var identityResult = _memberService.SetLockoutEnabled(member.Id, switchUser.Enabled);

                    if (!identityResult.Succeeded)
                    {
                        result.ErrorMessage = identityResult.Errors.FirstOrDefault();
                    }
                }
            }

            else
            {
                result.ErrorMessage = "此用户名的账号不存在！";
            }

            return Json(result);
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.DeleteMemberKey })]
        public async Task<ActionResult> Delete(string memberId)
        {
            var result = new DataJsonResult();
            Member member = _memberService.FindMemberById(memberId);

            if (member != null)
            {
                if (member.UserName.Equals("bocadmin", StringComparison.OrdinalIgnoreCase))
                {
                    result.ErrorMessage = "内置账号不可以删除！";
                }
                else
                {
                    var identityResult = await _memberService.Delete(member);

                    if (!identityResult.Succeeded)
                    {
                        result.ErrorMessage = identityResult.Errors.FirstOrDefault();
                    }
                }
            }
            else
            {
                result.ErrorMessage = "此用户名的账号不存在！";
            }

            return Json(result);
        }


        #region 导入会员
      

        [HttpGet]
        [AdminAuthorize]
        public ActionResult DownloadTemplate()
        {
            var filePath = Server.MapPath("~/Modules/BntWeb.MemberCenter/Content/Excels/会员模板.xlsx");
            return base.File(filePath, "application/ms-excel", "会员模板.xlsx");
        }

        [AdminAuthorize]
        public ActionResult ImportExcel()
        {
            ViewBag.EditMode = false;
            return View();
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditMemberKey })]
        [HttpPost]
        public ActionResult ExeclToMemberOnPost()
        {
            var result = new DataJsonResult();
            var fileExplorerId = Request.Params["ImportExcel"].ToString();
            if (fileExplorerId != "")
            {
                var file = _currencyService.GetSingleById<StorageFile>(fileExplorerId.ToGuid());
                if (file != null)
                {
                    //添加会员
                    var addMember = new List<Member>();
                    //添加会员
                    var editMember = new List<Member>();
                    int countAdd = 0;
                    int countEdit= 0;
                    var noUsers = new List<string>();
                    try
                    {
                        var filePath = Request.MapPath("/") + file.RelativePath.Replace("/", "\\");

                        using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            var workbook = WorkbookFactory.Create(stream); //使用接口，自动识别excel2003/2007格式
                            var sheet = workbook.GetSheetAt(0);
                          
                            if (workbook.GetSheetAt(0).LastRowNum>5000)
                            {
                                result.ErrorMessage = "导入的数据不能超过1000条";

                                return Json(result);
                            }
                        
                            //得到里面第一个sheet
                            var data = new DataTable();
                            try
                            {
                                if (sheet != null)
                                {
                                    var firstRow = sheet.GetRow(1);
                                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数
                                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                    {
                                        var cell = firstRow.GetCell(i);
                                        if (cell != null)
                                        {
                                            string cellValue = cell.ToString();
                                            if (cellValue != "")
                                            {
                                                DataColumn column = new DataColumn(cellValue);
                                                data.Columns.Add(column);
                                            }
                                        }
                                    }
                                    //最后一列的标号
                                    int rowCount = sheet.LastRowNum;
                                    for (int i = 1; i <= rowCount; ++i)
                                    {
                                        IRow row = sheet.GetRow(i);
                                        if (row == null || row.Cells.Count == 0 || row.FirstCellNum > 2)
                                            continue; //没有数据的行默认是null　　　　　　　
                                        DataRow dataRow = data.NewRow();
                                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                                        {
                                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                                dataRow[j] = GetCellValue(row.GetCell(j));
                                        }
                                        data.Rows.Add(dataRow);
                                      
                                    }
                                }
                             
                            }
                            catch (Exception ex)
                            {
                                result.ErrorMessage = ex.Message;
                            }
                            if (data.Columns.Count != 2)
                            {
                                result.ErrorMessage = "格式不正确：上传Excel列数不对！";
                            }
                            else
                            {
                                for (int i = 0; i < data.Rows.Count; i++)
                                {
                                    var memberAdd = new Member();
                                    var row = data.Rows[i];
                                    var importUser = _memberService.FindUserByName(row[0].ToString());
                                    if (importUser == null)
                                    {
                                        memberAdd.UserName = row[0].ToString();
                                        memberAdd.PhoneNumber = row[0].ToString();
                                        memberAdd.NickName = row[0].ToString();
                                        memberAdd.LockoutEnabled = false;
                                        memberAdd.Sex = SexType.UnKonw;
                                        memberAdd.Birthday = DateTime.Parse("1949-10-1");
                                        memberAdd.CreateTime = DateTime.Now;
                                        addMember.Add(memberAdd);
                                        using (TransactionScope scope = new TransactionScope())
                                        {
                                            var identityResult = _memberService.CreateMember(memberAdd, "123456");

                                            if (!identityResult.Succeeded)
                                            {
                                                throw new Exception(identityResult.Errors.FirstOrDefault());

                                            }
                                            else
                                            {
                                                if (row[1].ToString().To<Decimal>() > 0)
                                                {
                                                    string erro;
                                                    _walletService.Deposit(memberAdd.Id, WalletType.Integral,
                                                        row[1].ToString().To<Decimal>(), "会员导入", out erro);
                                                }
                                                countAdd++;
                                            }
                                            scope.Complete();
                                        }
                                    }
                                    else
                                    {

                                        //获得当前总积分
                                        var totalIntegral = _walletService.GetWalletByMemberId(importUser.Id,
                                            WalletType.Integral).Available;
                                        //与传入的积分比较
                                        if (row[1].ToString().To<Decimal>() > 0)
                                        {
                                            var min = totalIntegral - row[1].ToString().To<Decimal>();
                                            //计算是增加还是减少
                                            if (min > 0)
                                            {
                                                string erro;
                                                _walletService.Draw(importUser.Id, WalletType.Integral,
                                                   Math.Abs(min), "会员导入支出", out erro);
                                            }
                                            else
                                            {
                                                string erro;
                                                _walletService.Deposit(importUser.Id, WalletType.Integral,
                                                     Math.Abs(min), "会员导入收入", out erro);
                                            }
                                          
                                        }
                                     
                                    }
                                   
                              
                        }
                          
                                List<string> listErrorMessage = new List<string>();
                                if (addMember.Count > 0)
                                {
                                    if (countAdd > 0)
                                    {
                                        if (countAdd != addMember.Count)
                                        {
                                            listErrorMessage.Add($"{countAdd}条数据添加成功，{addMember.Count - countAdd}条数据未添加成功！");
                                        }
                                    }
                                    else
                                    {
                                        listErrorMessage.Add("添加失败");
                                    }
                                }
                                if (editMember.Count > 0)
                                {
                                    if (countEdit == 0)
                                    {
                                        listErrorMessage.Add("修改失败");
                                    }
                                    if (countEdit != editMember.Count)
                                    {
                                        listErrorMessage.Add($"{countEdit}条数据修改成功，{editMember.Count - countEdit}条数据未修改成功！");
                                    }
                                }
                                if (noUsers.Count > 0)
                                {
                                    listErrorMessage.Add($"{string.Join(",", noUsers)}已经存在");
                                }
                             
                                if (listErrorMessage.Count > 0)
                                {
                                    result.ErrorMessage = string.Join(",", listErrorMessage);
                                }
                            }
                            }
                        }
                    catch (Exception ex)
                    {
                        result.ErrorMessage = ex.Message;
                    }
                }
                else
                {
                    result.Success = false;
                    result.ErrorMessage = "文件读取不成功！";
                }
            }
            else
            {
                result.Success = false;
                result.ErrorMessage = "文件上传不成功！";
            }
            return Json(result);
        }
        /// <summary>
        /// 根据Excel列类型获取列的值
        /// </summary>
        /// <param name="cell">Excel列</param>
        /// <returns></returns>
        private static string GetCellValue(ICell cell)
        {
            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.Blank:
                    return string.Empty;
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Numeric:
                case CellType.Unknown:
                default:
                    return cell.ToString();
                //This is a trick to get the correct value of the cell. NumericCellValue will return a numeric value no matter the cell value is a date or a number
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Formula:
                    try
                    {
                        //HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        //e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }
        
        #endregion

    }

}