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

function ValidateNumber(e) {
    var evt = (e) ? e : window.event;
    var charCode = (evt.keyCode) ? evt.keyCode : evt.which;
    //alert(charCode)
    if (charCode != 8 && charCode != 0 && (charCode < 48 || charCode > 57) && (charCode < 96 || charCode > 105)) {
        return false;
    }
    return true;
};



$(".number").keypress(function (e) {
    //if not number display error message   
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57) ) {       
        return false;
    }
    return true;
});