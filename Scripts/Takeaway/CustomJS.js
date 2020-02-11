var MessageType = {
    Success: 'success',
    Error: 'danger',
    Warning: 'warning',
    Info: 'info'
};

var ResultType = {
    Success: '1',
    Error: '2',
    Already: '3'
};

function DeleteConfirm() {
    if (confirm("Are you sure want to delete record"))
        return true;
    else
        return false;
}

function ChangeConfirm() {
    if (confirm("Are you sure want to Change It"))
        return true;
    else
        return false;
}

function DeleteRecord(URL, cb) {
    var Result;
    bootbox.confirm("Are you sure want to delete ?", function (result) {
        if (result == true) {
            $.ajax({
                url: URL, //'/LanguageTemplate/Delete/' + ID,
                type: "GET",
                dataType: "JSON",
                contentType: "application/json",
                success: function (response) {
                    if (response == "1") {
                        cb(ResultType.Success);
                    }
                    else if (response == "2") {
                        cb(ResultType.Already);
                    }

                },
                error: function () {
                    cb(ResultType.Error);
                }
            });
        }

    })


}

function ChangeStatus(URL, cb) {
    var Result;
    bootbox.confirm("Are you sure want to Change It ?", function (result) {
        if (result == true) {
            $.ajax({
                url: URL, //'/LanguageTemplate/Delete/' + ID,
                type: "GET",
                dataType: "JSON",
                contentType: "application/json",
                success: function (response) {
                    if (response == "1") {
                        cb(ResultType.Success);
                    }
                },
                error: function () {
                    cb(ResultType.Error);
                }
            });
        }

    })


}

function ConfirmMsg(Message, cb) {
    bootbox.confirm(Message, function (result) {
        cb(result);
    })
}






//"Are you sure want to delete ?"




