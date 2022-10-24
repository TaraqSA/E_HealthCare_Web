$(document).ready(function () {
    CheckSupportForInputTypeDate();
    $(".CustomDatePicker").datepicker({        
        changeMonth: true,
        changeYear: true,        
        yearRange: "1950:2010",
        defaultDate: "01/01/2000",
        dateFormat: "dd/mm/yy"     
    });   
});

function CheckSupportForInputTypeDate() {
    jQuery.validator.methods.date = function (value, element) {
        var isChrome = /Chrome/.test(navigator.userAgent) && /Google Inc/.test(navigator.vendor);
        if (isChrome) {
            var d = new Date();
            return this.optional(element) || !/Invalid|NaN/.test(new Date(d.toLocaleDateString(value)));
        } else {
            return this.optional(element) || !/Invalid|NaN/.test(new Date(value));
        }
    };
}

