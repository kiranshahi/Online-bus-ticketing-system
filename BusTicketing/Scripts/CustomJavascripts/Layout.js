toastr.options = {
    "closeButton": false,
    "debug": false,
    "positionClass": "toast-bottom-right",
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
   "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}



$(document).ready(function () {
    if ($("#hdn-isSuccessMsg").val().trim().toUpperCase() == "TRUE") {
        if ($("#hdn-successMsg").val().trim() != "") {
            toastr.success($("#hdn-successMsg").val());
            $("#hdn-isSuccessMsg").val("");
        }
    }
    else if ($("#hdn-isSuccessMsg").val().trim().toUpperCase() == "FALSE") {
        if ($("#hdn-successMsg").val().trim() != "") {
            toastr.error($("#hdn-successMsg").val());
            $("#hdn-isSuccessMsg").val("");
        }
    }
});



