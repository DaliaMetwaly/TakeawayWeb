var table;
function Change(ID) {
    var URL = '/Regions/ChangeStatus/' + ID;
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
            "url": "/Regions/LoadData",
            "type": "GET",
            "datatype": "json",
        },
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.13/i18n/Arabic.json"
        },
        "columns": [
             { "data": "RegionAr", "autoWidth": true, "className": "text-center" },
                { "data": "RegionEn", "autoWidth": true, "className": "text-center" },
                { "data": "CityAr", "autoWidth": true, "className": "text-center" },
                { "data": "CityEn", "autoWidth": true, "className": "text-center" },
                { "data": "PostalCode", "autoWidth": true, "className": "text-center" },
                { "data": "IsActive", "autoWidth": true, "className": "text-center" },
                { "data": "Edit", "className": "text-center", "sortable": false },
                { "data": "Deleted", "className": "text-center", "sortable": false }

        ]
    });
}