var table;

function getOrdersbyRestaurant(ID) {
    debugger;
    table = $('#OrderTable').DataTable({
        "ajax": {
            "url": "/Orders/LoadOrdersbyRestaurant?ID=" + ID,
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
               { "data": "Id", "autoWidth": true, "className": "hidden id" },
                { "data": "ContactName", "autoWidth": true, "className": "text-center", "orderable": true, "searchable": true },
                { "data": "RestaurantName", "autoWidth": true, "className": "text-center" },
                { "data": "OrderStatus_ar", "autoWidth": true, "className": "text-center" },
                { "data": "TotalPrice", "autoWidth": true, "className": "text-center" },
                //{ "data": "PayType", "autoWidth": true, "className": "text-center" },
                { "data": "OrderDate", "autoWidth": true, "className": "text-center" }


        ]
    });
}
function getTotalSalesbyRestaurant(ID) {

    $.ajax({
        url: "/Orders/LoadTotalSalesbyRestaurant?ID=" + ID,
        type: "POST",
        dataType: "JSON",
        async: false,
        contentType: "application/json",
        success: function (d) {
            if (d.data!=0)
            {
                $('#TotalSales').text(d.data);
            }
            else
            {
                $('#TotalSales').text("");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {

            
        }

    });

}
function getTotalSalesChart(ID) {

    $.ajax({
        url: "/Orders/LoadChart?ID=" + ID,
        type: "POST",
        dataType: "JSON",
        async: false,
        contentType: "application/json",
        success: function (d) {
            drawChart(d.data);
           
        },
        error: function (xhr, ajaxOptions, thrownError) {
            debugger;

        }

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
    sOut += '<th class="text-center">العدد</th>';
    sOut += '</tr>';

    sOut += '</thead>';
    sOut += '<tbody>';
    for (var i = 0; i < d.length; i++) {
        sOut += '<tr role="row" >';
        sOut += '<td class="text-center">' + d[i].FoodName + '</td>';
        sOut += '<td class="text-center">' + d[i].ItemPrice + ' </td>';
        sOut += '<td class="text-center">' + d[i].ItemCount + ' </td>';

        sOut += '</tr>';
    }
    sOut += '</tbody>';
    sOut += '</table>';
    return sOut;
}



