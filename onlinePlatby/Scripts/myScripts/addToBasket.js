$(function () {
    $(".buyButton").click(function (e) {
        var productVariantId = $(this).attr("data-productVariantId");
        $.ajax({
            url: "/Basket/AddBasketProduct?ProductVariantId=" + productVariantId,
            method: "get",
            success: function (data) {
                //alert(data);
            }
        })
    });
});