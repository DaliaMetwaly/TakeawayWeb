var table;
var UserData = [];
var ErrorArray = [];
function Change(ID) {
    var URL = '/Restaurant/ChangeStatus/' + ID;
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
            "url": "/Restaurant/LoadData",
            "type": "GET",
            "datatype": "json",
        },
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.13/i18n/Arabic.json"
        },
        "columns": [
             {
                 "className": 'details-control',
                 "orderable": false,
                 "data": null,
                 "defaultContent": '+',

             },
                { "data": "ID", "autoWidth": true, "className": "text-center", "className": "hidden id" },
                { "data": "RestaurantName", "autoWidth": true, "className": "text-center" },
                { "data": "RestaurantNameEn", "autoWidth": true, "className": "text-center" },
                //{ "data": "RestaurantStatus", "autoWidth": true, "className": "text-center" },
                { "data": "DeliveryName", "autoWidth": true, "className": "text-center" },
                //{ "data": "feetype", "autoWidth": true, "className": "text-center" },
                { "data": "owner", "autoWidth": true, "className": "text-center" },
                //{ "data": "Country", "autoWidth": true, "className": "text-center" },
                { "data": "City", "autoWidth": true, "className": "text-center" },
                { "data": "IsActive", "autoWidth": true, "className": "text-center" },
                { "data": "Edit", "className": "text-center", "sortable": false },
                { "data": "Orders", "className": "text-center", "sortable": false },
                { "data": "Addresses", "className": "text-center", "sortable": false },

        ]
    });
}


function format(d) {
    // `d` is the original data object for the row

    var sOut = '';
    sOut += '<table id="tblQueueMessageDetails" class="table table-striped table-bordered table-hover Details">';
    sOut += '<thead>';
    sOut += '<tr class="heading">';
    sOut += '<th class="text-center">اسم الوجبه</th>';
    sOut += '<th class="text-center">السعر</th>';
    sOut += '<th class="text-center">الحجم</th>';
    sOut += '</tr>';

    sOut += '</thead>';
    sOut += '<tbody>';
    for (var i = 0; i < d.length; i++) {
        sOut += '<tr role="row" >';
        sOut += '<td class="text-center">' + d[i].FoodName + '</td>';
        sOut += '<td class="text-center">' + d[i].ItemPrice + ' </td>';
        sOut += '<td class="text-center">' + d[i].ItemSize + ' </td>';

        sOut += '</tr>';
    }
    sOut += '</tbody>';
    sOut += '</table>';
    return sOut;
}

//=======================
//DM 13-8-2017
function DeleteUserData(ID) {

    ConfirmMsg('Are you sure want to delete ?', function (flag) {

        if (flag == true) {

            var ItemIndex = $(this).closest('tr').index();
            var realindex = ItemIndex - 1;
            var index = UserData.findIndex(x => x.count === ID);

            UserData.splice(index, 1);
            fillsMMSMessageTable();
            debugger;
            $.ajax({
                url: '/Restaurant/addtempAddress',
                contentType: "application/json; charset=utf-8",
                type: "Post",
                async: false,
                data: JSON.stringify(UserData),
                success: function (response) {
                    debugger;
                },
                error: function () {
                    debugger;

                }
            });
            ShowMsg('تم حذف بانات المستخدم بنجاح', MessageType.Success, 5);
        }
        else {

        }
    })


}


function fillsMMSMessageTable() {
    $("#tableRestaurantData").remove();

    var myTable = "<table id='tableRestaurantData' class='table table-striped table-bordered table-hover'>";
    myTable += "<tr class='heading'><th style='width: 100px; color: red; text-align: center;display:none'>الرقم</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;display:none;'>كودالمنطقه</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;'>المنطقه</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;'>العنوان</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;'>خط الطول</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;'>خط العرض</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;'>التليفون</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;'>الموبايل</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;'>حذف</th></tr>";


    for (var i = 0; i < UserData.length; i++) {
        myTable += "<tr><td style='width: 100px; text-align: center;display:none'>" + UserData[i].count + "</td>";
        myTable += "<td style='width: 100px; text-align: center;display:none;'>" + UserData[i].Region_id + "</td>";
        myTable += "<td style='width: 100px; text-align: center;'>" + UserData[i].Region_Name + "</td>";
        myTable += "<td style='width: 100px; text-align: center;'>" + UserData[i].Address + "</td>";
        myTable += "<td style='width: 100px; text-align: center;'>" + UserData[i].MapLatitude + "</td>";
        myTable += "<td style='width: 100px; text-align: center;'>" + UserData[i].MapLongitude + "</td>";
        myTable += "<td style='width: 100px; text-align: center;'>" + UserData[i].Phone + "</td>";
        myTable += "<td style='width: 100px; text-align: center;'>" + UserData[i].Mobile + "</td>";
        myTable += "<td style='width: 100px; text-align: center;'><input type='button' id='btnDeleteUserData' name='btnDeleteUserData' class='removebutton btn default btn-xs black' value='حذف' onclick='DeleteUserData(" + UserData[i].count + ")'/></td></tr>";
    }
    myTable += "</table>";

    $("#tableholderRestauarntData").append(myTable);
}


$("#btnAddRestaurantData").click(function () {
    debugger;
    var isUserDataValid = true;
    if ($('#RegionN option:selected').text().trim() == 'اختر') {
        isUserDataValid = false;
        ErrorArray.push({ code: "2", ErrorName: "اختر المنطقه" })
    }

    if ($('#AddressMap').val().trim() == '') {
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
    if ($('#Phone').val().trim() == '') {
        isUserDataValid = false;
        ErrorArray.push({ code: "2", ErrorName: "ادخل التليفون" })
    }
    if ($('#Mobile').val().trim() == '') {
        isUserDataValid = false;
        ErrorArray.push({ code: "2", ErrorName: "ادخل الموبايل" })
    }

    if (isUserDataValid) {
        UserData.push({
            count: UserData.length + 1,
            Address: $('#AddressMap').val().trim(),
            Region_id: parseInt($('#RegionN option:selected').val().trim()),
            Region_Name: $('#RegionN option:selected').text().trim(),
            MapLatitude: $('#GMapLutitude').val().trim(),
            MapLongitude: $('#GMapLogitude').val().trim(),
            Phone: $('#Phone').val().trim(),
            Mobile: $('#Mobile').val().trim()
        });
        fillsMMSMessageTable();
        $('#AddressMap,#GMapLutitude,#GMapLogitude,#Phone,#Mobile').val('');

        $.ajax({
            url: '/Restaurant/addtempAddress',
            contentType: "application/json; charset=utf-8",
            type: "Post",
            async: false,
            data: JSON.stringify(UserData),
            success: function (response) {
               
            },
            error: function () {
               

            }
        });


    }
    else {
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
$("#btnEditRestaurantData").click(function () {
    debugger;
    var isUserDataValid = true;
    if ($('#RegionN option:selected').text().trim() == 'اختر') {
        isUserDataValid = false;
        ErrorArray.push({ code: "2", ErrorName: "اختر المنطقه" })
    }

    if ($('#AddressMap').val().trim() == '') {
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
    if ($('#Phone').val().trim() == '') {
        isUserDataValid = false;
        ErrorArray.push({ code: "2", ErrorName: "ادخل التليفون" })
    }
    if ($('#Mobile').val().trim() == '') {
        isUserDataValid = false;
        ErrorArray.push({ code: "2", ErrorName: "ادخل الموبايل" })
    }

    if (isUserDataValid) {
        UserData.push({
            count: UserData.length + 1,
            Address: $('#AddressMap').val().trim(),
            Region_id: parseInt($('#RegionN option:selected').val().trim()),
            Region_Name: $('#RegionN option:selected').text().trim(),
            MapLatitude: $('#GMapLutitude').val().trim(),
            MapLongitude: $('#GMapLogitude').val().trim(),
            Phone: $('#Phone').val().trim(),
            Mobile: $('#Mobile').val().trim()
        });
        fillsMMSMessageTable();
        $('#AddressMap,#GMapLutitude,#GMapLogitude,#Phone,#Mobile').val('');

        $.ajax({
            url: '/Restaurant/addtempAddress',
            contentType: "application/json; charset=utf-8",
            type: "Post",
            async: false,
            data: JSON.stringify(UserData),
            success: function (response) {

            },
            error: function () {


            }
        });


    }
    else {
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

function LoadRestauantData(restaurantID) {
    debugger;
    $.ajax({
        url: "/Restaurant/LoadRestauantData?restaurantID=" + restaurantID,
        type: "POST",
        dataType: "JSON",
        async: false,
        contentType: "application/json",
        success: function (d) {
            debugger;
            var lst = d.data;
            for (var a = 0; a < lst.length ; a++) {

                UserData.push({
                    count: UserData.length + 1,
                    Address: lst[a].Address,
                    Region_id: parseInt(lst[a].Region_id),
                    Region_Name: lst[a].Region_Name,
                    MapLatitude: lst[a].MapLatitude,
                    MapLongitude: lst[a].MapLongitude,
                    Phone: lst[a].Phone,
                    Mobile: lst[a].Mobile,
                    IsActive: true
                });

            }
            
           
            fillsMMSMessageTable();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            debugger;

        }

    });

}