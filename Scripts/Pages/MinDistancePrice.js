var table;
var UserData = [];
var ErrorArray = [];
function Change(ID) {
    var URL = '/MinDistancePrice/ChangeStatus/' + ID;
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
    debugger;
    table = $('#myTable').DataTable({
        "ajax": {
            "url": "/MinDistancePrice/LoadData",
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
             { "data": "RestaurantID", "autoWidth": true, "className": "text-center", "className": "hidden id" },
                { "data": "RestaurantName", "autoWidth": true, "className": "text-center" },
                { "data": "UserName", "autoWidth": true, "className": "text-center" },
                { "data": "Address", "autoWidth": true, "className": "text-center" },
               // { "data": "deliveryFeeValue", "autoWidth": true, "className": "text-center" },
                 
                 { "data": "IsActive", "autoWidth": true, "className": "text-center" },
                 { "data": "Edit", "autoWidth": true, "className": "text-center" },
                { "data": "Delete", "className": "text-center", "sortable": false },
              //   { "data": "Deleted", "className": "text-center", "sortable": false }

        ]
    });
}

function format(d) {
    // `d` is the original data object for the row

    var sOut = '';
    sOut += '<table id="tblQueueMessageDetails" class="table table-striped table-bordered table-hover Details">';
    sOut += '<thead>';
    sOut += '<tr class="heading">';
    sOut += '<th class="text-center">المدينه</th>';
    sOut += '<th class="text-center">المنطقه</th>';
    sOut += '<th class="text-center">حد الأدنى للمطعم</th>';
    sOut += '<th class="text-center">قيمة التوصيل</th>';
    sOut += '<th class="text-center">تعديل</th>';
    sOut += '</tr>';

    sOut += '</thead>';
    sOut += '<tbody>';
    for (var i = 0; i < d.length; i++) {
        sOut += '<tr role="row" >';
        sOut += '<td class="text-center">' + d[i].City + '</td>';
        sOut += '<td class="text-center">' + d[i].Region + '</td>';
        sOut += '<td class="text-center">' + d[i].minPrice + ' </td>';
        sOut += '<td class="text-center">' + d[i].deliveryFeeValue + ' </td>';
        sOut += "<td class='text-center'> <a  id='btn_Edit' class='btn default btn-xs green' href='/MinDistancePrice/Edit/" + d[i].ID + "'>تعديل</a> </td>";
        sOut += '</tr>';
    }
    sOut += '</tbody>';
    sOut += '</table>';
    return sOut;
}

function DeleteUserData(ID) {

    ConfirmMsg('Are you sure want to delete ?', function (flag) {

        if (flag == true) {

            var ItemIndex = $(this).closest('tr').index();
            var realindex = ItemIndex - 1;
            var index = UserData.findIndex(x => x.count === ID);

            UserData.splice(index, 1);
            fillsMMSMessageTable();
            ShowMsg('تم الحذف بنجاح', MessageType.Success, 5);
            $.ajax({
                url: "/MinDistancePrice/addtempDistance",
                type: "POST",
                data: JSON.stringify(UserData),
                // dataType: "JSON",
                contentType: "application/json; charset=utf-8",
                success: function (isclient) {
                    debugger;
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    debugger;
                    console.warn(xhr.responseText);
                }
            });
        }
        else {

        }
    })


}

function fillsMMSMessageTable() {
    $("#tableUserData").remove();

    var myTable = "<table id='tableUserData' class='table table-striped table-bordered table-hover' style='width:96%;margin-right:2%;'>";
    myTable += "<tr class='heading'><th style='width: 100px; color: red; text-align: center;display:none'>الرقم</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;display:none;'>كود المطعم</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;'>المطعم</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;display:none;'>كودالمنطقه</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;'>المنطقه</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;'>تكلفه التوصيل</th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;'>الحد الادنى للمطعم </th>";
    myTable += "<th style='width: 100px; color: red; text-align: center;'>حذف</th></tr>";


    for (var i = 0; i < UserData.length; i++) {
        myTable += "<tr><td style='width: 100px; text-align: center;display:none'>" + UserData[i].count + "</td>";
        myTable += "<td style='width: 100px; text-align: center;display:none;'>" + UserData[i].Restaurant_id + "</td>";
        myTable += "<td style='width: 100px; text-align: center;'>" + UserData[i].Restaurant_Name + "</td>";
        myTable += "<td style='width: 100px; text-align: center;display:none;'>" + UserData[i].Region_id + "</td>";
        myTable += "<td style='width: 100px; text-align: center;'>" + UserData[i].Region_Name + "</td>";
        myTable += "<td style='width: 100px; text-align: center;'>" + UserData[i].deliveryFeeValue + "</td>";
        myTable += "<td style='width: 100px; text-align: center;'>" + UserData[i].minPrice + "</td>";
        myTable += "<td style='width: 100px; text-align: center;'><input type='button' id='btnDeleteUserData' name='btnDeleteUserData' class='removebutton btn default btn-xs black' value='حذف' onclick='DeleteUserData(" + UserData[i].count + ")'/></td></tr>";
    }
    myTable += "</table>";

    $("#tableholderUserData").append(myTable);
};

$("#btnAddDistance").click(function () {
    debugger;
    var isUserDataValid = true;
    if ($('#RestaurantID option:selected').text().trim() == 'اختر') {
        isUserDataValid = false;
        ErrorArray.push({ code: "2", ErrorName: "اختر المطعم" })
    }

    if ($('#RestaurantDataID option:selected').text().trim() == 'اختر') {
        isUserDataValid = false;
        ErrorArray.push({ code: "2", ErrorName: "اختر عنوان المطعم" })
    }

    if ($('#RegionID option:selected').text().trim() == 'اختر') {
        isUserDataValid = false;
        ErrorArray.push({ code: "2", ErrorName: "اختر المنطقه" })
    }

    if ($('#deliveryFeeValue').val().trim() == '') {
        isUserDataValid = false;
        ErrorArray.push({ code: "2", ErrorName: "ادخل تكلفه التوصيل" })
    }

    if ($('#minDistance').val().trim() == '') {
        isUserDataValid = false;
        ErrorArray.push({ code: "2", ErrorName: "ادخل الحد الادنى للمطعم" })
    }
    

    if (isUserDataValid) {
        UserData.push({
            count: UserData.length + 1,
            Region_id: parseInt($('#RegionID option:selected').val().trim()),
            Region_Name: $('#RegionID option:selected').text().trim(),
            Restaurant_id: parseInt($('#RestaurantID option:selected').val().trim()),
            RestaurantData_id: parseInt($('#RestaurantDataID option:selected').val().trim()),
            Restaurant_Name: $('#RestaurantID option:selected').text().trim(),
            deliveryFeeValue: $('#deliveryFeeValue').val().trim(),
            minPrice: $('#minDistance').val().trim()
        });
        fillsMMSMessageTable();
        
        $.ajax({
            url: "/MinDistancePrice/addtempDistance",
            type: "POST",
            data: JSON.stringify(UserData),
            // dataType: "JSON",
            contentType: "application/json; charset=utf-8",
            success: function (isclient) {
                debugger;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                debugger;
                console.warn(xhr.responseText);
            }
        });

        $('#deliveryFeeValue,#minDistance').val('');
       

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

function getRestaurantBranch(restaurantID)
{
    debugger;
    $.ajax({
        url: "/MinDistancePrice/GetRestaurantBranches?restaurantID="+restaurantID,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            debugger;
            UserData = response;
            fillsMMSMessageTable();
            $.ajax({
                url: "/MinDistancePrice/addtempDistance",
                type: "POST",
                data: JSON.stringify(UserData),
                // dataType: "JSON",
                contentType: "application/json; charset=utf-8",
                success: function (isclient) {
                    debugger;
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    debugger;
                    console.warn(xhr.responseText);
                }
            });
        },
        error: function (xhr, ajaxOptions, thrownError) {
            debugger;
            console.warn(xhr.responseText);
        }
    });
}
