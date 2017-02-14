var isInfoAboutYouValid = false;
// email regex
var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

$(function () {
    // To basket
    $("#parts .tableOfProductsBtn").click(function () {
        $("#paymentAndTransportation").addClass("hidden");
        $("#orderFinish").addClass("hidden");
        $("#infoAboutCustomer").addClass("hidden");
        $("#tableOfProducts").removeClass("hidden"); // hide 1part
    });

    // To paymentAndTransportation
    $("#parts .paymentAndTransportBtn").click(function () {
        if ($("#basketProductsTable tbody tr").length > 1) {
            $("#paymentAndTransportation").removeClass("hidden");
            $("#orderFinish").addClass("hidden");
            $("#infoAboutCustomer").addClass("hidden");
            $("#tableOfProducts").addClass("hidden"); // hide 1part
            $("#parts .paymentAndTransportBtn").addClass("active");
        }
    });

    // To infoAboutCustomer
    $("#parts .infoAboutCustomerBtn").click(function () {
        
        $("#parts .infoAboutCustomerBtn").addClass("active");
            $("#paymentAndTransportation").addClass("hidden");
            $("#orderFinish").addClass("hidden");
            $("#infoAboutCustomer").removeClass("hidden");
            $("#tableOfProducts").addClass("hidden"); // hide 1part


    });

    // To orderFinish
    $("#parts .orderFinish").click(function () {
        if (isInfoAboutYouValid) {
            $("#paymentAndTransportation").addClass("hidden");
            $("#orderFinish").removeClass("hidden");
            $("#infoAboutCustomer").addClass("hidden");
            $("#tableOfProducts").addClass("hidden"); // hide 1part
        }
    });

    // From table of products to Payment and Transportation
    $("#tableOfProducts .paymentAndTransportBtn").click(function () {
        if ($("#basketProductsTable tbody tr").length > 1) {
            $("#tableOfProducts").addClass("hidden"); // hide 1part
            $("#paymentAndTransportation").removeClass("hidden"); // show 2part
            $("#parts .paymentAndTransportBtn").addClass("active");
        }
    });

    // From Payment and Transportation to Info about customer
    $("#paymentAndTransportation .infoAboutCustomerBtn").click(function () {
        $("#paymentAndTransportation").addClass("hidden"); // hide 1part
        $("#infoAboutCustomer").removeClass("hidden"); // show 2part
        $("#parts .infoAboutCustomerBtn").addClass("active");
    });

    // From info about you to Order finish
    $("#infoAboutCustomer .orderFinish").click(function () {
        var email = $("#email").val();
        $("#customerEmail").empty();
        $("#customerEmail").append(email);
        if (re.test(email)) {
            $("#infoAboutCustomer").addClass("hidden"); // hide 1part
            $("#orderFinish").removeClass("hidden"); // show 2part
            $("#parts .orderFinish").addClass("active");
            isInfoAboutYouValid = true;
        }
        else {
            $("#email").css("border", "solid red 4px");
        }

    });


    // Product count
    $(".productCountInput").change(function () {
        var priceOfOneProduct = parseFloat($(this).parent().parent().find(".salePrice").attr("data-salePrice"));
        var count = parseFloat($(this).val());
        var fullPrice = count * priceOfOneProduct;
        var productId = $(this).attr("data-productId");

        $(this).parent().parent().find(".salePrice").empty();
        $(this).parent().parent().find(".salePrice").append("<span class='fullSalePrice'>" + fullPrice + "</span>" + ",00 Kč");

        var sumPrice = 0.00;
        // Sum all prices
        $.each($("#basketProductsTable .fullSalePrice"), function (i, d) {
            sumPrice += parseInt($(d).text());
        });

        $(".fullOrderPrice").empty();
        $(".fullOrderPrice").append(sumPrice + ",00");

        // Product recapitulation update
        $(".count[data-productId='" + productId + "']").empty();
        $(".count[data-productId='" + productId + "']").append(count);

        $(".salePriceRecap[data-productId='" + productId + "']").empty();
        $(".salePriceRecap[data-productId='" + productId + "']").append("<span class='fullSalePriceRecap'>" + fullPrice + "</span>" + ",00 Kč");


    });
});