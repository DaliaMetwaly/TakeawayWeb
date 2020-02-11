var table;
var UserData = [];
var ErrorArray = [];

function LoadData() {
    debugger;
    table = $('#myTable').DataTable({
        "ajax": {
            "url": "/Manage/LoadData",
            "type": "Get",
            "datatype": "json"
        },
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.13/i18n/Arabic.json"
        },
        "columns": [
            { "data": "ID", "autoWidth": true, "className": "text-center", "visible": false },
            //{ "data": "Description", "autoWidth": true, "className": "text-center", "visible": true },
            { "data": "UserName", "autoWidth": true, "className": "text-center", "visible": true },
            { "data": "OrderDate", "autoWidth": true, "className": "text-center", "visible": true },
            { "data": "RestaurantName", "autoWidth": true, "className": "text-center", "visible": true },
            { "data": "OrderStatus", "autoWidth": true, "className": "text-center", "visible": true },
            //{ "data": "OrderDetails", "autoWidth": true, "className": "text-center", "visible": true },
            { "data": "Delivery_estimation", "autoWidth": true, "className": "text-center", "visible": true },
            //{ "data": "PayType", "autoWidth": true, "className": "text-center", "visible": true },
            { "data": "TotalPrice", "autoWidth": true, "className": "text-center", "visible": true }
            
        ]
    });
}
function LoadUserData(usrID) {
    debugger;
    $.ajax({
        url: "/Manage/LoadUserData?usrID=" + usrID,
        type: "POST",
        dataType: "JSON",
        async: false,
        contentType: "application/json",
        success: function (d) {

            var lst = d.data;
            for (var a = 0; a < lst.length ; a++) {

                UserData.push({
                    count: UserData.length + 1,
                    Address: lst[a].Address,
                    Region_id: parseInt(lst[a].Region_id),
                    Region_Name: lst[a].Region_Name,
                    MapLatitude: lst[a].MapLatitude,
                    MapLongitude: lst[a].MapLongitude,
                    IsActive: true
                });
            }
            fillsMMSMessageTable();
        },
        error: function (xhr, ajaxOptions, thrownError) {


        }

    });

}
function fillsMMSMessageTable() {
    $("#tableUserData").remove();

    var myTable = "<table id='tableUserData' class='table table-striped table-bordered table-hover'>";
    myTable += "<tr class='heading'><th style='width: 100px; color: red; text-align: center;display:none'>الرقم</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;display:none;'>كودالمنطقه</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;'>المنطقه</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;'>العنوان</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;'>خط العرض</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;'>خط الطول</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;'>حذف</th></tr>";


    for (var i = 0; i < UserData.length; i++) {
        myTable += "<tr><td style='width: 100px; text-align: center;display:none'>" + UserData[i].count + "</td>";
        myTable += "<td style='width: 100px; text-align: center;display:none;'>" + UserData[i].Region_id + "</td>";
        myTable += "<td style='width: 100px; text-align: center;'>" + UserData[i].Region_Name + "</td>";
        myTable += "<td style='width: 100px; text-align: center;'>" + UserData[i].Address + "</td>";
        myTable += "<td style='width: 100px; text-align: center;'>" + UserData[i].MapLatitude + "</td>";
        myTable += "<td style='width: 100px; text-align: center;'>" + UserData[i].MapLongitude + "</td>";
        myTable += "<td style='width: 100px; text-align: center;'><input type='button' id='btnDeleteUserData' name='btnDeleteUserData' class='removebutton btn default btn-xs black' value='حذف' onclick='DeleteUserData(" + UserData[i].count + ")'/></td></tr>";
    }
    myTable += "</table>";

    $("#tableholderUserData").append(myTable);
};
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
$("#btnAddEditUserData").click(function () {
    debugger;
    var isUserDataValid = true;
    if ($('#RegionN').text().trim() == 'اختر') {
        isUserDataValid = false;
        ErrorArray.push({ code: "2", ErrorName: "اختر المنطقه" })
    }

    if ($('#Address').val().trim() == '') {
        isUserDataValid = false;
        ErrorArray.push({ code: "2", ErrorName: "ادخل العنوان" })
    }

    if ($('#GMapLutitude').val().trim() == '') {
        isUserDataValid = false;
        ErrorArray.push({ code: "2", ErrorName: "ادخل GMapLutitude" })
    }

    if ($('#GMapLogitude').val().trim() == '') {
        isUserDataValid = false;
        ErrorArray.push({ code: "2", ErrorName: "ادخل GMapLogitude" })
    }

    if (isUserDataValid) {
        UserData = [];
        $("#tableUserData").find('tr').each(function (i, el) {
            if (i > 0) {



                var $tds = $(this).find('td'),
               
               Region_idVal = $tds.eq(1).text(),
               Region_NameVal = $tds.eq(2).text(),
               AddressVal = $tds.eq(3).text(),
               MapLatitudeVal = $tds.eq(4).text(),
               MapLongitudeVal = $tds.eq(5).text();

                UserData.push({
                    count: UserData.length + 1,
                    Address: AddressVal.trim(),
                    Region_id: parseInt(Region_idVal),
                    Region_Name: Region_NameVal,
                    MapLatitude: MapLatitudeVal.trim(),
                    MapLongitude: MapLongitudeVal.trim(),
                    IsActive: true
                });
            }


        });

        UserData.push({
            count: UserData.length + 1,
            Address: $('#Address').val().trim(),
            Region_id: parseInt($('#RegionN option:selected').val().trim()),
            Region_Name: $('#RegionN option:selected').text().trim(),
            MapLatitude: $('#GMapLutitude').val().trim(),
            MapLongitude: $('#GMapLogitude').val().trim(),
            IsActive: true
        });
        fillsMMSMessageTable();
        $('#Address,#GMapLutitude,#GMapLogitude').val('');
        $('#RegionN').text = 'اختر';
    }
});
$("#btnSaveUserData").click(function () {  
        var ChangePasswordVM = {
            ChangePasswordVM: {
                UserDataList: UserData
            }
        }
        $.ajax({
            url: "/Manage/EditAddress",
            type: "POST",
            data: JSON.stringify(ChangePasswordVM),
            dataType: "JSON",
            async: false,
            contentType: "application/json",
            success: function (d) {

                if (d.status == true) {

                    window.location.href = '/Manage/Index'
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {

                window.location.href = '/Manage/Index'
            }

        });
   
});


   