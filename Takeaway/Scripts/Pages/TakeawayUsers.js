
var table;
var UserData = [];
var ErrorArray = [];

function Change(ID) {
    var URL = '/TakeawayUsers/ChangeStatus/' + ID;
    ChangeStatus(URL, function (flag) {
        if (parseInt(ResultType.Success) == flag) {
            ShowMsg('حسنا تم التفعيل  وكل شيء يبدو جيدا', MessageType.Success, 5);
            table.destroy();
            LoadData();
        }
        else if (parseInt(ResultType.Error) == flag) {
            ShowMsg('خطأ حاول مرة اخرى', MessageType.Error, 5);
        }
    });
}

function LoadData() {
    table = $('#myTable').DataTable({
        "ajax": {
            "url": "/TakeawayUsers/LoadData",
            "type": "GET",
            "datatype": "json",
        },
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.13/i18n/Arabic.json"
        },
        "columns": [
             { "data": "Name", "autoWidth": true, "className": "text-center" },
                { "data": "UserName", "autoWidth": true, "className": "text-center" },
                { "data": "Phone", "autoWidth": true, "className": "text-center" },
                { "data": "Email", "autoWidth": true, "className": "text-center" },
                { "data": "RoleName", "autoWidth": true, "className": "text-center" },
                { "data": "UserCategory", "autoWidth": true, "className": "text-center" },
                { "data": "ChangePassword", "autoWidth": true, "className": "text-center" },
                { "data": "ChangeEmail", "autoWidth": true, "className": "text-center" },
                { "data": "IsActive", "autoWidth": true, "className": "text-center" },
                { "data": "Edit", "className": "text-center", "sortable": false },
                { "data": "Deleted", "className": "text-center", "sortable": false }

        ]
    });
}
//DM 9-8-2017
function LoadCustomerData() {
    table = $('#myTable').DataTable({
        "ajax": {
            "url": "/TakeawayUsers/LoadCustomerData",
            "type": "GET",
            "datatype": "json",
        },
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.13/i18n/Arabic.json"
        },
        "columns": [
             { "data": "Name", "autoWidth": true, "className": "text-center" },
                { "data": "UserName", "autoWidth": true, "className": "text-center" },
                { "data": "Phone", "autoWidth": true, "className": "text-center" },
                { "data": "Email", "autoWidth": true, "className": "text-center" },
                //{ "data": "RoleName", "autoWidth": true, "className": "text-center" },
                { "data": "CreationDate", "autoWidth": true, "className": "text-center" },
                { "data": "UserCategory", "autoWidth": true, "className": "text-center" },
                { "data": "ChangePassword", "autoWidth": true, "className": "text-center" },
                { "data": "ChangeEmail", "autoWidth": true, "className": "text-center" },
                { "data": "IsActive", "autoWidth": true, "className": "text-center" },
                { "data": "Edit", "className": "text-center", "sortable": false },
                { "data": "Deleted", "className": "text-center", "sortable": false }

        ]
    });
}

$("#CountryN").change(function () {
    if ($("#CountryN option:selected").val().trim() != '') {

        $.ajax({
            url: '/TakeawayUsers/FillCities',
            type: 'GET',
            async: false,
            data: { CountryID: $("#CountryN option:selected").val().trim() },
            dataType: 'JSON',
            contentType: 'application/json',
            success: function (response) {
                if (response.length > 0) {
                    $("#CityN").empty();
                    $("#RegionN").empty();

                    $.each(response, function (i) {
                        $("#CityN").append('<option value="' + response[i].value + '">' + response[i].text + '</option>');
                    });
                }
            },
            error: function () {
                //   alert('Error. Please try again.');
                ShowMsg('Error. Please try again.', MessageType.Error, 5);
            }
        });

    }
});


$("#CityN").change(function () {
    if ($("#CityN option:selected").val().trim() != '') {

        $.ajax({
            url: '/TakeawayUsers/FillRegions',
            type: 'GET',
            async: false,
            data: { CityID: $("#CityN option:selected").val().trim() },
            dataType: 'JSON',
            contentType: 'application/json',
            success: function (response) {
                if (response.length > 0) {
                    $("#RegionN").empty();

                    $.each(response, function (i) {
                        $("#RegionN").append('<option value="' + response[i].value + '">' + response[i].text + '</option>');
                    });
                }
            },
            error: function () {
                //   alert('Error. Please try again.');
                ShowMsg('Error. Please try again.', MessageType.Error, 5);
            }
        });

    }
});

//////////////////////////// tab 5 Messages

//$("#btnAddUserData").click(function () {
//    if ($("#Messagetypetab5 option:selected").val() != "" &&
//        $("#Languagetab5 option:selected").val() != "" &&
//        $("#Typetab5 option:selected").val() != "") {
//        fillmessagetemplatetab5data();
//       // fillmessagetemplatetab5table();

//    }
//});

//function fillmessagetemplatetab5data() {
//    //messagetemplatetabList5.push({
//    //    ID: $("#Messagetypetab5 option:selected").val(),
//    //    Name: $("#Messagetypetab5 option:selected").text(),
//    //    SecondID: $("#Languagetab5 option:selected").val(),
//    //    SecondName: $("#Languagetab5 option:selected").text(),
//    //    ThirdID: $("#Typetab5 option:selected").val(),
//    //    ThirdName: $("#Typetab5 option:selected").text(),
//    //    Description: $("#txtMessagetab5").val()
//    //});
//}

//function fill() {
//    $("#tblmessagetemplatetab5").remove();
//    var myTableFive = "<table id='tblmessagetemplatetab5' class='table table-striped table-bordered table-hover'>";
//    myTableFive += "<tr class='heading'><th style='width: 100px; color: red; text-align: center;'>Message Type</th>";
//    myTableFive += "<th style='width: 100px; color: red; text-align: center;'>Language Type</th>";
//    myTableFive += "<th style='width: 100px; color: red; text-align: center;'>Message</th>";
//    myTableFive += "<th style='width: 100px; color: red; text-align: center;'>Delete</th></tr>";


//    //for (var i = 0; i < messagetemplatetabList5.length; i++) {
//    //    myTableFive += "<tr><td style='width: 100px; text-align: center;'>" + messagetemplatetabList5[i].ThirdName + "</td>";
//    //    myTableFive += "<td style='width: 100px; text-align: center;'>" + messagetemplatetabList5[i].SecondName + "</td>";
//    //    myTableFive += "<td style='width: 100px; text-align: center;'>" + messagetemplatetabList5[i].Description + "</td>";
//    //    myTableFive += "<td style='width: 100px; text-align: center;'>";
//    //    myTableFive += "<input type='button' id='deletetab5' class='removebutton btn default btn-xs black' value='Delete'/></td></tr>";
//    //}
//    myTableFive += "</table>";
//    // alert(myTableFive.length);
//    $("#tableholdertblMessagestab5").html();
//    $("#tableholdertblMessagestab5").append(myTableFive);
//};

//$(document).on('click', '#deletetab5', function () {
//    var ItemIndex = $(this).closest('tr').index();

//    ConfirmMsg('Are you sure want to delete ?', function (flag) {
//        if (flag == true) {
//            var realindex = ItemIndex - 1;
//            //messagetemplatetabList5.splice(realindex, 1);
//           // fillmessagetemplatetab5table();
//            ShowMsg('Message Deleted Successfully', MessageType.Success, 5);
//        }
//        else {
//            //   alert('cancel');
//        }
//    })
//});

//$("#Messagetemplatetab5").change(function () {
//    if ($("#Messagetemplatetab5 option:selected").val() != "") {
//        $("#txtMessagetab5").val($("#Messagetemplatetab5 option:selected").text());
//    }
//});

//==================================================
//DM 20-7-2017
//function LoadUserData(usrID) {
    
//    $.ajax({
//        url: "/TakeawayUsers/LoadUserData?usrID="+usrID,
//        type: "POST",
//        dataType: "JSON",
//        async: false,
//        contentType: "application/json",
//        success: function (d) {

//            var lst = d.data;
//            for (var a = 0; a < lst.length ; a++) {
              
//                UserData.push({
//                    count:UserData.length+1,
//                    Address: lst[a].Address,
//                    Region_id: parseInt(lst[a].Region_id),
//                    Region_Name: lst[a].Region_Name,
//                    MapLatitude: lst[a].MapLatitude,
//                    MapLongitude: lst[a].MapLongitude,
//                    IsActive:true
//                });
//            }
//            fillsMMSMessageTable();
//        },
//        error: function (xhr, ajaxOptions, thrownError) {
           
         
//        }

//    });

//}

function DeleteUserData(ID) {
    
    ConfirmMsg('Are you sure want to delete ?', function (flag) {
       
        if (flag == true) {
           
            var ItemIndex = $(this).closest('tr').index();
            var realindex = ItemIndex - 1;
            var index = UserData.findIndex(x => x.count === ID);
          
            UserData.splice(index, 1);
            fillsMMSMessageTable();
            ShowMsg('تم حذف بانات المستخدم بنجاح', MessageType.Success, 5);
        }
        else {

        }
    })


}

function ChangeRole() {
  
    if ($('#Roleddl option:selected').text() == "User") {
        $('#UserDataDiv').css('display', 'table');
        //DM 8-8-2017
        //after display Userdata div render map again
        google.maps.event.trigger(map, 'resize', function () {
            initMap();
        });
    }
    else {
        $('#UserDataDiv').css('display', 'none');
    }
}

function fillsMMSMessageTable() {
    $("#tableUserData").remove();
   
    var myTable = "<table id='tableUserData' class='table table-striped table-bordered table-hover'>";
    myTable += "<tr class='heading'><th style='width: 100px; color: red; text-align: center;display:none'>الرقم</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;display:none;'>كودالمنطقه</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;'>المنطقه</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;'>العنوان</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;'>خط الطول</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;'>خط العرض</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;'>حذف</th></tr>";


    for (var i = 0; i < UserData.length; i++) {
        myTable += "<tr><td style='width: 100px; text-align: center;display:none'>" + UserData[i].count + "</td>";
        myTable += "<td style='width: 100px; text-align: center;display:none;'><input type='text' name='Region_id' class='Region_id' value='" + UserData[i].Region_id + "' readonly></input></td>";
        myTable += "<td style='width: 100px; text-align: center;'>" + UserData[i].Region_Name + "</td>";
        myTable += "<td style='width: 100px; text-align: center;'><input type='text' name='Address' class='Address'  value='" + UserData[i].Address + "' readonly></input></td>";
        myTable += "<td style='width: 100px; text-align: center;'><input type='text' name='MapLatitude' class='MapLatitude' value='" + UserData[i].MapLatitude + "' readonly></input></td>";
        myTable += "<td style='width: 100px; text-align: center;'><input type='text' name='MapLongitude' class='MapLongitude' value='" + UserData[i].MapLongitude + "' readonly></input></td>";
        myTable += "<td style='width: 100px; text-align: center;'><input type='button' id='btnDeleteUserData' name='btnDeleteUserData' class='removebutton btn default btn-xs black' value='حذف' onclick='DeleteUserData(" + UserData[i].count + ")'/></td></tr>";
    }
    myTable += "</table>";

    $("#tableholderUserData").append(myTable);
};

//ADD
//$("#submit").click(function () {
   
//    var isValid = true;
//    if ($('#ContactName').val().trim() == '') {
//        isValid = false;
//        ErrorArray.push({ code: '1', ErrorName: 'من فضلك ادخل الاسم' });
//    }
//    if ($('#UserName').val().trim() == '') {
//        isValid = false;
//        ErrorArray.push({ code: "1", ErrorName: "من فضلك ادخل اسم المستخدم" })

//    }

//    if ($('#Password').val().trim() == '') {
//        isValid = false;
//        ErrorArray.push({ code: "1", ErrorName: "من فضلك ادخل كلمه المرور" })

//    }
//    else if ($('#Password').val().trim() != '') {
//        if ($('#Password').val().length < 6) {
//            isValid = false;
//            ErrorArray.push({ code: "1", ErrorName: "كلمه المرور يجب الا تقل عن 6 حروف" })

//        }
//        //var passNumFilter = /[0-9]/;
//        //if (!passNumFilter.test($('#Password').val().trim())) {
//        //    isValid = false;
//        //    ErrorArray.push({ code: "1", ErrorName: "كلمه المرور يجب ان تحتوى على رقم من 0-9" })
//        //}
//        //var passCharFilter = /[a-z]/;
//        //if (!passCharFilter.test($('#Password').val().trim())) {
//        //    isValid = false;
//        //    ErrorArray.push({ code: "1", ErrorName: "كلمه المرور يجب ان تحتوى على حروف من a-z" })
//        //}

//        if ($('#ConfirmPassword').val().trim() != $('#Password').val().trim()) {
//            isValid = false;
//            ErrorArray.push({ code: "1", ErrorName: "كلمه المرور غير متطابقه" })

//        }
//    }
   
    
   
    
//    if ($('#ContactPhone').val().trim() == '') {
//        isValid = false;
//        ErrorArray.push({ code: "1", ErrorName: "من فضلك ادخل رقم التليفون" })

//    }

//    if ($('#ContactEmail').val().trim() == '') {
//        isValid = false;
//        ErrorArray.push({ code: "1", ErrorName: "من فضلك ادخل البريد الالكترونى" })
//    }
//    else if ($('#ContactEmail').val().trim() != '') {
//        var emailfilter = /^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$/i
//        var returnval = emailfilter.test($('#ContactEmail').val().trim());
//        if (returnval == false) {
//            isValid = false;
//            ErrorArray.push({ code: "1", ErrorName: "من فضلك ادخل بريد الكترونى صحيح" })
//        }
//    }


//    //if ($('#PayTypeID option:selected').text().trim() == 'اختر') {
//    //    isValid = false;
//    //    ErrorArray.push({ code: "1", ErrorName: "من فضلك اختر طريقه الدفع" })

//   // }
//    if ($('#Roleddl option:selected').text().trim() == 'اختر') {
//        isValid = false;
//        ErrorArray.push({ code: "1", ErrorName: "من فضلك اختر الصلاحيه " })

//    }
//    if (isValid) {
//        var TakeawayUser = {
//            TakeawayUser: {
//                ContactName: ($('#ContactName').val().trim()),
//                UserName: ($('#UserName').val().trim()),
//                Password: ($('#Password').val().trim()),
//                ConfirmPassword: ($('#ConfirmPassword').val().trim()),
//                ContactEmail: ($('#ContactEmail').val().trim()),
//                ContactPhone: ($('#ContactPhone').val().trim()),
//                //PayTypeID: parseInt($('#PayTypeID').val().trim()),
//                RoleID: ($('#Roleddl').val().trim()),
//                //UserCategory: ($('#UserCategory').val().trim()),
//                //IsActive: ($('#IsActive').is(':checked')),
//                IsDetete: false,
//                isClient: $('#isClient').val(),
//                UserDataList: UserData

               
//            }
//        }

//        $.ajax({
//            url: "/TakeawayUsers/Create",
//            type: "POST",
//            data: JSON.stringify(TakeawayUser),
//           // dataType: "JSON",
            
//            // contentType: "application/json",
//            contentType: "application/json; charset=utf-8",
//            success: function (isclient) {
//                debugger;
//                if (isclient == 1) {
//                    url = "/TakeawayUsers/Index2?Msg=6";
//                    window.location.href = url;

//                }
//                if (isclient == 0) {
//                    url = "/TakeawayUsers/Index?Msg=6";
//                    window.location.href = url;

//                }
                
//            },
//            error: function (xhr, ajaxOptions, thrownError) {

//                debugger;
//                console.warn(xhr.responseText);
//            }



//        });

//    }
//    else {
        
//        document.getElementById("ValidationMsg").innerHTML = "";
//        $("#ValidationMsg").append("<ul>");
//        //Show Validation Msg
//        for (var i = 0; i < ErrorArray.length; i++) {
//            if (ErrorArray[i].code == "1") {
//                $("#ValidationMsg").append("<li>" + ErrorArray[i].ErrorName + "</li>")
//            }

//        }
//        $("#ValidationMsg").append("</ul>");
//        $("#ValidationMsg").removeClass("validation-summary-valid");
//        ErrorArray = [];
//    }


//});
$("#btnAddUserData").click(function () {
   
    var isUserDataValid = true;
    if ($('#RegionN option:selected').text().trim() == 'اختر') {
        isUserDataValid = false;
        ErrorArray.push({ code: "2", ErrorName: "اختر المنطقه" })
    }

    if ($('#GAddress').val().trim() == '') {
        isUserDataValid = false;
        ErrorArray.push({ code: "2", ErrorName: "ادخل العنوان" })
    }

    if ($('#GMapLutitude').val().trim() == '') {
        isUserDataValid = false;
        ErrorArray.push({ code: "2", ErrorName: "ادخل خط العرض" })
    }

    if ($('#GMapLogitude').val().trim() == '') {
        isUserDataValid = false;
        ErrorArray.push({ code: "2", ErrorName: "ادخل خط الطول" })
    }

    if (isUserDataValid) {
        UserData.push({
            count:UserData.length+1,
            Address: $('#GAddress').val().trim(),
            Region_id: parseInt($('#RegionN option:selected').val().trim()),
            Region_Name: $('#RegionN option:selected').text().trim(),
            MapLatitude: $('#GMapLutitude').val().trim(),
            MapLongitude: $('#GMapLogitude').val().trim()
        });
        fillsMMSMessageTable();
        $('#GAddress,#GMapLutitude,#GMapLogitude').val('');
       //$('#RegionN').text('');
        //$('#RegionN').attr('placeholder','اختر')
        // $('#RegionN option:first-child').attr("selected", "selected");
        //   $("#RegionN option:first").attr('selected', true);
        //$('#RegionN option:selected').attr("selected", null);

       // $("#RegionN option[value='']").attr("selected", true);
      //  $("#RegionN option[value='0']").attr("selected", true);
       
    }
    else
    {
        document.getElementById("ValidationMsg").innerHTML = "";
        $("#ValidationMsg").append("<ul>");
        //Show Validation Msg
        for (var i = 0; i < ErrorArray.length; i++) {
            if (ErrorArray[i].code == "2") {
                $("#ValidationMsg").append("<li>" + ErrorArray[i].ErrorName + "</li>")
            }

        }
        $("#ValidationMsg").append("</ul>");
        $("#ValidationMsg").removeClass("validation-summary-valid");
        ErrorArray = [];
    }
});


//Edit 
//$("#submitEdit").click(function () {
   
//    var isValid = true;
//    if ($('#ContactName').val().trim() == '') {
//        isValid = false;
//        ErrorArray.push({ code: '1', ErrorName: 'من فضلك ادخل الاسم' });
//    }
//    if ($('#UserName').val().trim() == '') {
//        isValid = false;
//        ErrorArray.push({ code: "1", ErrorName: "من فضلك ادخل اسم المستخدم" })

//    }
    
    
//    if ($('#ContactPhone').val().trim() == '') {
//        isValid = false;
//        ErrorArray.push({ code: "1", ErrorName: "من فضلك ادخل رقم التليفون" })

//    }

//    if ($('#ContactEmail').val().trim() == '') {
//        isValid = false;
//        ErrorArray.push({ code: "1", ErrorName: "من فضلك ادخل البريد الالكترونى" })

//    }
//    else if ($('#ContactEmail').val().trim() != '')
//    {
//    var emailfilter = /^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$/i
//    var returnval = emailfilter.test($('#ContactEmail').val().trim());
//    if (returnval == false)
//    {
//        isValid = false;
//        ErrorArray.push({ code: "1", ErrorName: "من فضلك ادخل بريد الكترونى صحيح" })
//    }
//   }
//    //if ($('#IP_Address').val().trim() == '') {
//    //    isValid = false;
//    //    ErrorArray.push({ code: "1", ErrorName: "من فضلك ادخل IP Address" })

//    //}


//    //if ($('#PayTypeID option:selected').text().trim() == 'اختر') {
//    //    isValid = false;
//    //    ErrorArray.push({ code: "1", ErrorName: "من فضلك اختر طريقه الدفع" })

//    //}
//    if ($('#Roleddl option:selected').text().trim() == 'اختر') {
//        isValid = false;
//        ErrorArray.push({ code: "1", ErrorName: "من فضلك اختر الصلاحيه " })

//    }
//    if (isValid) {
//        var TakeawayUser = {
//            TakeawayUser: {
//                ID: ($('#HiddenUserID').val().trim()),
//                ContactName: ($('#ContactName').val().trim()),
//                UserName: ($('#UserName').val().trim()),
//                ContactEmail: ($('#ContactEmail').val().trim()),
//                ContactPhone: ($('#ContactPhone').val().trim()),
//                //PayTypeID: parseInt($('#PayTypeID').val().trim()),
//                RoleID: ($('#Roleddl').val().trim()),
//               // IsActive: ($('#IsActive').is(':checked')),
//                IsDetete: false,
//                UserDataList: UserData


//            }
//        }
//        $.ajax({
//            url: "/TakeawayUsers/Edit",
//            type: "POST",
//            data: JSON.stringify(TakeawayUser),
//            dataType: "JSON",
//            async: false,
//            contentType: "application/json",
//            success: function (d) {

//                if (d.status == true) {
                   
//                    window.location.href = '/TakeawayUsers/Index?Msg=6'


//                }
//            },
//            error: function (xhr, ajaxOptions, thrownError) {
               
//                window.location.href = '/TakeawayUsers/Index?Msg=2'
               
//            }



//        });
//    }
//    else {
//        document.getElementById("ValidationMsg").innerHTML = "";
//        $("#ValidationMsg").append("<ul>");
//        //Show Validation Msg
//        for (var i = 0; i < ErrorArray.length; i++) {
//            if (ErrorArray[i].code == "1") {
//                $("#ValidationMsg").append("<li>" + ErrorArray[i].ErrorName + "</li>")
//            }

//        }
//        $("#ValidationMsg").append("</ul>");
//        $("#ValidationMsg").removeClass("validation-summary-valid");
//        ErrorArray = [];
//    }


//});
$("#btnAddEditUserData").click(function () {
    debugger;
    var isUserDataValid = true;
    if ($('#RegionN option:selected').text().trim() == 'اختر')  {
        isUserDataValid = false;
        ErrorArray.push({ code: "2", ErrorName: "اختر المنطقه" })
    }

    if ($('#GAddress').val().trim() == '') {
        isUserDataValid = false;
        ErrorArray.push({ code: "2", ErrorName: "ادخل العنوان" })
    }

    if ($('#GMapLutitude').val().trim() == '') {
        isUserDataValid = false;
        ErrorArray.push({ code: "2", ErrorName: "ادخل خط العرض" })
    }

    if ($('#GMapLogitude').val().trim() == '') {
        isUserDataValid = false;
        ErrorArray.push({ code: "2", ErrorName: "ادخل خط الطول" })
    }

    if (isUserDataValid) {
        UserData = [];
        $("#tableUserData").find('tr').each(function (i, el) {
            debugger;
            if (i > 0) {
               


                var $tds = $(this).find('td'),
               Region_idVal = $tds.find('.Region_id').val(),
               Region_NameVal = $tds.eq(2).text(),
               AddressVal = $tds.find('.Address').val(),
               MapLatitudeVal = $tds.find('.MapLatitude').val(),
               MapLongitudeVal = $tds.find('.MapLongitude').val();

                UserData.push({
                    count: UserData.length + 1,
                    Address: AddressVal.trim(),
                    Region_id: parseInt(Region_idVal),
                    Region_Name: Region_NameVal,
                    MapLatitude: MapLatitudeVal.trim(),
                    MapLongitude: MapLongitudeVal.trim(),
                    IsActive:true
                });
            }


        });

        UserData.push({
            count: UserData.length + 1,
            Address: $('#GAddress').val().trim(),
            Region_id: parseInt($('#RegionN option:selected').val().trim()),
            Region_Name: $('#RegionN option:selected').text().trim(),
            MapLatitude: $('#GMapLutitude').val().trim(),
            MapLongitude: $('#GMapLogitude').val().trim(),
            IsActive: true
        });
        fillsMMSMessageTable();
        $('#GAddress,#GMapLutitude,#GMapLogitude').val('');
        $('#RegionN').text = 'اختر';
    }
    else
    {
        document.getElementById("ValidationMsg").innerHTML = "";
        $("#ValidationMsg").append("<ul>");
        //Show Validation Msg
        for (var i = 0; i < ErrorArray.length; i++) {
            if (ErrorArray[i].code == "2") {
                $("#ValidationMsg").append("<li>" + ErrorArray[i].ErrorName + "</li>")
            }

        }
        $("#ValidationMsg").append("</ul>");
        $("#ValidationMsg").removeClass("validation-summary-valid");
        ErrorArray = [];
    }
});

function Delete(Id) {
   
    var URL = '/TakeawayUsers/DeleteConfirmed/' + Id;
    DeleteRecord(URL, function (flag) {

        if (parseInt(ResultType.Success) == flag) {
            ShowMsg('حسنا تم الحذف  وكل شيء يبدو جيدا. ', MessageType.Success, 5);
            table.destroy();
            LoadData();
        }
        else if (parseInt(ResultType.Error) == flag) {
            ShowMsg('لم يتم الحذف,الرجاء المحاولة مرة اخري.', MessageType.Error, 5);
        }
    });



}




