
var table;

function LoadLatestOffersData() {
    debugger;
    table = $('#myTable').DataTable({
        "ajax": {
            "url": "/Home/LoadLatestOffersData",
            "type": "GET",
            "datatype": "json",
        },
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.13/i18n/Arabic.json"
        },
        "columns": [
                { "data": "RestaurantName", "autoWidth": true, "className": "text-center" },
                { "data": "FeeValues", "autoWidth": true, "className": "text-center" },
                { "data": "StartDate", "autoWidth": true, "className": "text-center" },
                { "data": "EndDate", "autoWidth": true, "className": "text-center" },
                

        ]
    });
}


function LoadOrdersData() {
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


