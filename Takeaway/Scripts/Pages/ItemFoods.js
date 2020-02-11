var table;
function Change(ID) {
    var URL = '/ItemFoods/ChangeStatus/' + ID;
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

function ChangeCooking(ID) {
    var URL = '/ItemFoods/ChangeCooking/' + ID;
    ChangeStatus(URL, function (flag) {
        if (parseInt(ResultType.Success) == flag) {
            ShowMsg('Status Changed Successfully', MessageType.Success, 5);
            table.destroy();
            LoadData();
        }
        else if (parseInt(ResultType.Error) == flag) {
            ShowMsg('Error. Please try again.', MessageType.Error, 5);
        }
    });
}


function LoadData() {
    table = $('#myTable').DataTable({
        "ajax": {
            "url": "/ItemFoods/LoadData",
            "type": "GET",
            "datatype": "json",
        },
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.13/i18n/Arabic.json"
        },
        "columns": [
                { "data": "FoodName", "autoWidth": true, "className": "text-center" },
                { "data": "FoodNameEn", "autoWidth": true, "className": "text-center" },
                { "data": "RestaurantName", "autoWidth": true, "className": "text-center" },
                { "data": "CategoryName", "autoWidth": true, "className": "text-center" },
                { "data": "CategoryTypeName", "autoWidth": true, "className": "text-center" },
                { "data": "Price", "autoWidth": true, "className": "text-center" },
                { "data": "Size", "autoWidth": true, "className": "text-center" },
                { "data": "IsCooking", "autoWidth": true, "className": "text-center" },
                { "data": "IsActive", "autoWidth": true, "className": "text-center" },
                { "data": "Edit", "className": "text-center", "sortable": false },
                { "data": "Deleted", "className": "text-center", "sortable": false }

        ]
    });
}