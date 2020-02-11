var table;
function Change(ID) {
    var URL = '/Orders/ChangeStatus/' + ID;
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
    if ($('#roleName').val() == 0) {
table = $('#OrderTable').DataTable({
       
        "ajax": {
            "url": "/Orders/LoadData",
            "type": "GET",
            "datatype": "json",
        },
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.13/i18n/Arabic.json"
        },
        "columns": [
             //{
             //    "className": 'details-control',
             //    "orderable": false,
             //    "data": null,
             //    "defaultContent": '+',
                
             //},
               { "data": "Id", "autoWidth": true, "className": "hidden id" },
                { "data": "ContactName", "autoWidth": true, "className": "text-center" ,"orderable": true, "searchable": true },
                { "data": "RestaurantName", "autoWidth": true, "className": "text-center" },
                { "data": "OrderStatus_ar", "autoWidth": true, "className": "text-center" },
                //{ "data": "TotalPrice", "autoWidth": true, "className": "text-center" },
                //{ "data": "PayType", "autoWidth": true, "className": "text-center" },
                { "data": "OrderDate", "autoWidth": true, "className": "text-center" },
                //{ "data": "IsActive", "autoWidth": true, "className": "text-center" },
                //{ "data": "Edit", "className": "text-center", "sortable": false },
                //{ "data": "Deleted", "className": "text-center", "sortable": false },
                { "data": "Details", "className": "text-center", "sortable": false }

        ]
    });
    }
    else
    {
        table = $('#OrderTable').DataTable({

            "ajax": {
                "url": "/Orders/LoadData",
                "type": "GET",
                "datatype": "json",
            },
            "language": {
                "url": "//cdn.datatables.net/plug-ins/1.10.13/i18n/Arabic.json"
            },
            "columns": [
                 //{
                 //    "className": 'details-control',
                 //    "orderable": false,
                 //    "data": null,
                 //    "defaultContent": '+',

                 //},
                   { "data": "Id", "autoWidth": true, "className": "hidden id" },
                    { "data": "ContactName", "autoWidth": true, "className": "text-center", "orderable": true, "searchable": true },
                    { "data": "RestaurantName", "autoWidth": true, "className": "text-center" },
                    { "data": "OrderStatus_ar", "autoWidth": true, "className": "text-center" },
                    //{ "data": "TotalPrice", "autoWidth": true, "className": "text-center" },
                    //{ "data": "PayType", "autoWidth": true, "className": "text-center" },
                    { "data": "OrderDate", "autoWidth": true, "className": "text-center" },
                    { "data": "IsActive", "autoWidth": true, "className": "text-center" },
                    { "data": "Edit", "className": "text-center", "sortable": false },
                    { "data": "Deleted", "className": "text-center", "sortable": false },
                    { "data": "Details", "className": "text-center", "sortable": false }

            ]
        });
    }
}



//function format(d) {
//    // `d` is the original data object for the row

//    var sOut = '';
//    sOut += '<table id="tblQueueMessageDetails" class="table table-striped table-bordered table-hover Details">';
//    sOut += '<thead>';
//    sOut += '<tr class="heading">';
//    sOut += '<th class="text-center">اسم الوجبه</th>';
//    sOut += '<th class="text-center">السعر</th>';
//    sOut += '<th class="text-center">العدد</th>';
//    sOut += '</tr>';

//    sOut += '</thead>';
//    sOut += '<tbody>';
//    for (var i = 0; i < d.length; i++) {
//        sOut += '<tr role="row" >';
//        sOut += '<td class="text-center">' + d[i].FoodName + '</td>';
//        sOut += '<td class="text-center">' + d[i].ItemPrice + ' </td>';
//        sOut += '<td class="text-center">' + d[i].ItemCount + ' </td>';

//        sOut += '</tr>';
//    }
//    sOut += '</tbody>';
//    sOut += '</table>';
//    return sOut;
//}